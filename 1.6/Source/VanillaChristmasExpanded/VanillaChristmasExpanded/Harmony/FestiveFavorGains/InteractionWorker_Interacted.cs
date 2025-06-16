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
           if(__instance?.interaction!=null&& StaticCollections.interactions.ContainsKey(__instance.interaction))
            {
                FestiveFavorManager.Instance.AddFestiveFavor(StaticCollections.interactions[__instance.interaction]);
            }
          

        }
    }








}
