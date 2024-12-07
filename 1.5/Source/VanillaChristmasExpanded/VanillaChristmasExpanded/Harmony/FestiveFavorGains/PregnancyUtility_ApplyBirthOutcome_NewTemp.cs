using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(PregnancyUtility), "ApplyBirthOutcome_NewTemp")]
    public static class VanillaChristmasExpanded_PregnancyUtility_ApplyBirthOutcome_NewTemp_Patch
    {


        public static void Postfix(RitualOutcomePossibility outcome)
        {
            int positivityIndex = outcome.positivityIndex;
            if (positivityIndex >= 0)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(50);
            }
        }
    }
}