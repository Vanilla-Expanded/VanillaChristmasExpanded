using System;
using Verse;
using UnityEngine;
using RimWorld;

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
				int currentDay = GenLocalDate.DayOfQuadrum(tile);
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
			var debugText = "(DEBUG) BadEventsMultiplier: " + BadEventsMultiplier;
			var debugSize = Text.CalcSize(debugText);
			var debugRect = new Rect(
				UI.screenWidth - rightMargin - debugSize.x,
				countdownRect.y + countdownRect.height + 5f,
				debugSize.x,
				iconSize
			);
			Widgets.Label(debugRect, debugText);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		public void AddFestiveFavor(int amount)
		{
			festiveFavor += amount;
		}

		public void ConvertFavorToPresents()
		{

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
