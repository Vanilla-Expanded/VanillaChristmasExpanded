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
	public class FestiveFavorManager : GameComponent
	{
		private int festiveFavor;

		public static FestiveFavorManager Instance;
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

		public void DrawUI()
		{
			Rect rect = new Rect(100, 100, 200, 60);
			Widgets.Label(new Rect(rect.x, rect.y, rect.width, 30), "Festive Favor: " + festiveFavor);
			Widgets.Label(new Rect(rect.x, rect.y + 30, rect.width, 30), "Days until festive presents: " + DaysUntilDecembrary10);
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
			DrawUI();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref this.festiveFavor, "festiveFavor", 0);
			Instance = this;
		}
	}
}
