using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(RitualOutcomeEffectWorker_FromQuality), "ApplyDevelopmentPoints")]
    public static class VanillaChristmasExpanded_RitualOutcomeEffectWorker_FromQuality_ApplyDevelopmentPoints_Patch
    {
        

        public static void Postfix(RitualOutcomePossibility outcome, Precept_Ritual ritual)
        {
           

            if (outcome.positivityIndex==2)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(25);
            }
            else if(outcome.positivityIndex == 1)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(15);
            }

            if (ritual.def!=null && StaticCollections.precepts.ContainsKey(ritual.def)) {
                FestiveFavorManager.Instance.AddFestiveFavor(10);
            }
        }
    }
}