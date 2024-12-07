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

        public static Dictionary<InteractionDef,int> interactions = new Dictionary<InteractionDef, int>() { { InteractionDefOf.Chitchat, 1 },
            { InteractionDefOf.DeepTalk,1},{ InternalDefOf.KindWords,2},{ InteractionDefOf.Nuzzle,3},{ InteractionDefOf.BabyPlay,2}
            ,{ InternalDefOf.VSIE_Vent,3},{ InternalDefOf.VFEE_RoyalGossip,1}
        };

        [HarmonyPostfix]
        static void PostFix(InteractionWorker __instance)
        {
           if(__instance.interaction!=null&&interactions.ContainsKey(__instance.interaction))
            {
                FestiveFavorManager.Instance.AddFestiveFavor(interactions[__instance.interaction]);
            }
          

        }
    }








}
