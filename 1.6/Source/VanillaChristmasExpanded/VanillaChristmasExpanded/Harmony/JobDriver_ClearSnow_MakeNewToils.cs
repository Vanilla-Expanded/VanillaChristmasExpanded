using Verse.AI;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(JobDriver), "Cleanup")]
    public static class VanillaChristmasExpanded_JobDriver_ClearSnow_MakeNewToils_Patch
    {


        public static void Postfix(JobDriver __instance)
        {

            if (__instance is JobDriver_ClearSnowAndSand)
            {

                Thing thing = ThingMaker.MakeThing(InternalDefOf.VCE_Snow);
                thing.stackCount = 4;
                GenPlace.TryPlaceThing(thing, __instance.job.targetA.Cell, __instance.pawn.Map, ThingPlaceMode.Near);

            }


        }
    }
}