using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using VanillaChristmasExpanded;



namespace VanillaChristmasExpanded
{


    [HarmonyPatch(typeof(InteractionWorker))]
    [HarmonyPatch("Interacted")]
    public static class VanillaChristmasExpanded_InteractionWorker_Interacted_Patch
    {
        [HarmonyPostfix]
        static void PostFix(InteractionWorker __instance)
        {
           if(__instance.interaction == InteractionDefOf.Chitchat)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(1);
            }
            if (__instance.interaction == InteractionDefOf.DeepTalk)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(1);
            }
            if (__instance.interaction == InternalDefOf.KindWords)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(2);
            }
            if (__instance.interaction == InteractionDefOf.Nuzzle)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(3);
            }
        }
    }








}
