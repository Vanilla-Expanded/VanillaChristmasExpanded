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
        public static Dictionary<PreceptDef, int> precepts = new Dictionary<PreceptDef, int>() { { PreceptDefOf.ThroneSpeech, 10 },
            { PreceptDefOf.AnimaTreeLinking,10},{ PreceptDefOf.Funeral,10},{ InternalDefOf.FuneralNoCorpse,10},{ InternalDefOf.Festival,10}
            ,{ InternalDefOf.Classic_DanceParty,10},{ InternalDefOf.DateRitualConsumable,10},{ InternalDefOf.LeaderSpeech,10},{ InternalDefOf.TreeConnection,20}
            ,{ InternalDefOf.VFEE_RoyalAddress,10}
        };

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

            if (ritual.def!=null && precepts.ContainsKey(ritual.def)) {
                FestiveFavorManager.Instance.AddFestiveFavor(10);
            }
        }
    }
}