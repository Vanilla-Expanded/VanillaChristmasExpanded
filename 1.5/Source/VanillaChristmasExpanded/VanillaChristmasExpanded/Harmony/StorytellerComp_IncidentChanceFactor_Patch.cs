using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
	[HarmonyPatch(typeof(StorytellerComp), "IncidentChanceFinal")]
	public static class StorytellerComp_IncidentChanceFinal_Patch
	{
		private static bool IsBadIncident(IncidentDef def)
		{
			return def.category == IncidentCategoryDefOf.ThreatBig ||
				   def.category == IncidentCategoryDefOf.ThreatSmall ||
				   def.category == IncidentCategoryDefOf.DiseaseHuman ||
				   def.category == IncidentCategoryDefOf.DeepDrillInfestation ||
				   def.letterDef == LetterDefOf.NegativeEvent ||
				   def.letterDef == LetterDefOf.ThreatBig ||
				   def.letterDef == LetterDefOf.ThreatSmall;
		}

		public static void Postfix(IncidentDef def, ref float __result)
		{
			if (FestiveFavorManager.Active && IsBadIncident(def))
			{
				__result *= FestiveFavorManager.Instance.BadEventsMultiplier;
			}
		}
	}
}