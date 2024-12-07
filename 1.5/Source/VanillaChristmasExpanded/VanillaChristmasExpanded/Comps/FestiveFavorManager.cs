using System;
using Verse;
using UnityEngine;
using RimWorld;
using System.Linq;
using LudeonTK;

namespace VanillaChristmasExpanded
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class HotSwappableAttribute : Attribute
	{
	}

	[HotSwappable]
	[StaticConstructorOnStartup]
	public class FestiveFavorManager : GameComponent
	{
		private int festiveFavor;
		private static readonly Texture2D FestiveFavorIcon = ContentFinder<Texture2D>.Get("UI/FestiveFavor");

		public static FestiveFavorManager Instance;
		public static bool Active => Find.Storyteller?.def == InternalDefOf.VCE_SantaSeasonal;
		public FestiveFavorManager(Game game)
		{
			Instance = this;
		}

		public int DaysUntilDecembrary10
		{
			get
			{
				var tile = Find.AnyPlayerHomeMap?.Tile ?? 0;
				int currentDay = GenLocalDate.DayOfQuadrum(tile) + 1;
				var vector = Find.WorldGrid.LongLatOf(tile);
				Quadrum currentQuadrum = GenDate.Quadrum(Find.TickManager.TicksAbs, vector.x);
				int decembrary10 = 10;
				int daysUntil = decembrary10 - currentDay;
				if (currentQuadrum != Quadrum.Decembary)
				{
					int quadrumsUntilDecembary = (Quadrum.Decembary - currentQuadrum + 4) % 4;
					daysUntil += quadrumsUntilDecembary * 15;
				}
				else if (daysUntil < 0)
				{
					daysUntil += 60;
				}
				return daysUntil;
			}
		}
		
		public float BadEventsMultiplier
		{
			get
			{
				int daysUntilDecembrary10 = DaysUntilDecembrary10;
				float multiplier;

				if (daysUntilDecembrary10 >= 0 && daysUntilDecembrary10 <= 10)
				{
					// From 0 to 10 days before Decembrary 10, scale from 100% to 0%
					multiplier = daysUntilDecembrary10 / 10f;
				}
				else if (daysUntilDecembrary10 > 10)
				{
					// From 1 to 30 days after Decembrary 10, scale from 0% to 100%
					// Subtract 10 from daysUntilDecembrary10 to start at Decembrary 11 (when daysUntilDecembrary10 == 59)
					multiplier = Mathf.InverseLerp(59f, 29f, daysUntilDecembrary10);
				}
				else
				{
					// Beyond 40 days after Decembrary 10, multiplier remains at 1
					multiplier = 1f;
				}

				// Ensure multiplier stays between 0 and 1
				multiplier = Mathf.Clamp01(multiplier);
				return multiplier;
			}
		}

		public void DrawUI()
		{
			float iconSize = 24f;
			float rightMargin = 10f;
			float topMargin = 10f;
			var festiveFavorText = "VCE_FestiveFavor".Translate(festiveFavor);
			var festiveFavorTextWidth = Text.CalcSize(festiveFavorText).x;
			var daysUntilPresents = "VCE_DaysUntilPresents".Translate(DaysUntilDecembrary10);
			var textWidth = Text.CalcSize(daysUntilPresents).x;
			
			Rect labelRect = new Rect(
				UI.screenWidth - rightMargin - festiveFavorTextWidth,
				topMargin,
				festiveFavorTextWidth,
				iconSize
			);

			Rect iconRect = new Rect(
				labelRect.x - iconSize - 5,
				topMargin,
				iconSize,
				iconSize
			);

			Rect countdownRect = new Rect(
				UI.screenWidth - rightMargin - textWidth,
				labelRect.y + iconSize + 5f,
				textWidth,
				iconSize
			);

			Text.Anchor = TextAnchor.MiddleRight;
			GUI.DrawTexture(iconRect, FestiveFavorIcon);
			Widgets.Label(labelRect, festiveFavorText);
			Widgets.Label(countdownRect, daysUntilPresents);
			//var debugText = "(DEBUG) BadEventsMultiplier: " + BadEventsMultiplier;
			//var debugSize = Text.CalcSize(debugText);
			//var debugRect = new Rect(
			//	UI.screenWidth - rightMargin - debugSize.x,
			//	countdownRect.y + countdownRect.height + 5f,
			//	debugSize.x,
			//	iconSize
			//);
			//Widgets.Label(debugRect, debugText);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		[DebugAction("Festive Favor", "Festive Favor +10", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddFestiveFavor()
		{
			Instance.AddFestiveFavor(10);
		}
		
		[DebugAction("Festive Favor", "Festive Favor -10", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveFestiveFavor()
		{
			Instance.AddFestiveFavor(-10);
		}
		public void AddFestiveFavor(int amount)
		{
			if (Active)
			{
				festiveFavor += amount;
			}
		}

		public override void GameComponentTick()
		{
			base.GameComponentTick();
			if (Find.TickManager.TicksGame % GenDate.TicksPerHour == 0 && DaysUntilDecembrary10 == 0)
			{
				var map = Find.AnyPlayerHomeMap;
				if (map != null && GenLocalDate.HourOfDay(map) == 19)
				{
					ConvertFavorToPresents(map);
				}
			}
		}

		private static readonly (QualityCategory Quality, float Cost)[] ExpensiveQualities = new[]
		{
			(QualityCategory.Legendary, 400f),
			(QualityCategory.Masterwork, 250f),
			(QualityCategory.Excellent, 145f),
			(QualityCategory.Good, 100f)
		};

		private static readonly (QualityCategory Quality, float Cost)[] NormalQualities = new[]
		{
			(QualityCategory.Normal, 50f),
			(QualityCategory.Poor, 25f),
			(QualityCategory.Awful, 10f)
		};

		private QualityCategory TryGetQuality(float budget, bool expensive)
		{
			var qualities = expensive ? ExpensiveQualities : NormalQualities;
			
			// Filter qualities we can afford
			var affordableQualities = qualities.Where(q => budget >= q.Cost).ToList();
			if (!affordableQualities.Any())
				return expensive ? QualityCategory.Good : QualityCategory.Awful;

			// Randomly select from affordable qualities
			var selected = affordableQualities.RandomElement();
			return selected.Quality;
		}

		private float GetFavorCost(QualityCategory quality)
		{
			var qualities = ExpensiveQualities.Concat(NormalQualities);
			return qualities.First(q => q.Quality == quality).Cost;
		}

		public void ConvertFavorToPresents(Map map)
		{
			if (festiveFavor <= 0) return;

			// Calculate initial distribution
			float expensiveBudget = festiveFavor * 0.5f;
			float normalBudget = festiveFavor * 0.4f;
			float crackerBudget = festiveFavor * 0.1f;

			// Process expensive presents (>100 Festive favor)
			while (expensiveBudget >= 100)
			{
				QualityCategory quality = TryGetQuality(expensiveBudget, true);
				float cost = GetFavorCost(quality);
				SpawnPresent(map, quality);
				expensiveBudget -= cost;
			}

			// Add remaining expensive budget to normal budget
			normalBudget += expensiveBudget;

			// Process normal presents (<100 Festive favor)
			while (normalBudget >= 10)
			{
				QualityCategory quality = TryGetQuality(normalBudget, false);
				float cost = GetFavorCost(quality);
				SpawnPresent(map, quality);
				normalBudget -= cost;
			}

			// Add remaining normal budget to cracker budget
			crackerBudget += normalBudget;

			// Convert remaining favor into festive crackers (1 favor = 1 cracker)
			int crackerCount = Mathf.FloorToInt(crackerBudget);
			if (crackerCount > 0)
			{
				ThingDef crackerDef = InternalDefOf.VCE_FestiveCracker;
				int stackLimit = crackerDef.stackLimit;
				while (crackerCount > 0)
				{
					int stackSize = Mathf.Min(crackerCount, stackLimit);
					SpawnCrackers(map, crackerDef, stackSize);
					crackerCount -= stackSize;
				}
			}

			// Reset favor to 0 after distribution
			festiveFavor = 0;
		}
		
		private IntVec3 GetSpawnLocation(Map map)
		{
			Predicate<IntVec3> validator = (IntVec3 c) => c.Roofed(map) && map.areaManager.Home[c] && BaseCellValidator(map, c);
			if (CellFinder.TryFindRandomCell(map, (IntVec3 c) => validator(c), out IntVec3 cell))
			{
				return cell;
			}
			return CellFinderLoose.RandomCellWith((IntVec3 c) => BaseCellValidator(map, c), map);
		}

		private static bool BaseCellValidator(Map map, IntVec3 c)
		{
			return c.Standable(map) && !c.Fogged(map) && c.GetThingList(map).Any(t => t.def.category == ThingCategory.Building
							|| t.def.category == ThingCategory.Item) is false;
		}

		private void SpawnPresent(Map map, QualityCategory quality)
		{
			Thing present = ThingMaker.MakeThing(InternalDefOf.VCE_FestivePresent);
			present.SetFaction(Faction.OfPlayer);
			CompQuality comp = present.TryGetComp<CompQuality>();
			comp.SetQuality(quality, ArtGenerationContext.Colony);
			IntVec3 spawnLoc = GetSpawnLocation(map);
			GenSpawn.Spawn(present, spawnLoc, map);
			Utils.PopUpConfetti(present.Position, present.Map, false);
			Log.Message("Spawned " + quality.ToString() + " Festive Present");
		}
		
		private void SpawnCrackers(Map map, ThingDef crackerDef, int stackSize)
		{
			Thing crackers = ThingMaker.MakeThing(crackerDef);
			crackers.stackCount = stackSize;
			IntVec3 spawnLoc = GetSpawnLocation(map);
			GenSpawn.Spawn(crackers, spawnLoc, map);
			Utils.PopUpConfetti(crackers.Position, crackers.Map, false);
			Log.Message($"Spawned {stackSize} Festive Crackers");
		}

		public override void GameComponentOnGUI()
		{
			base.GameComponentOnGUI();
			if (Active)
			{
				DrawUI();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref festiveFavor, "festiveFavor", 0);
			Instance = this;
		}
	}
}
