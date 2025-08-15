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

            if (__instance is JobDriver_ClearSnowAndSand && !IsSand(__instance,__instance.job.targetA.Cell))
            {

                Thing thing = ThingMaker.MakeThing(InternalDefOf.VCE_Snow);
                thing.stackCount = 4;
                GenPlace.TryPlaceThing(thing, __instance.job.targetA.Cell, __instance.pawn.Map, ThingPlaceMode.Near);

            }


        }

        private static bool IsSand(JobDriver instance,IntVec3 cell)
        {
            float? num = instance.pawn?.MapHeld?.sandGrid?.GetDepth(cell);
            if (num.HasValue)
            {
                return num.GetValueOrDefault() > instance.pawn?.MapHeld?.snowGrid.GetDepth(cell);
            }
            return false;
        }
    }
}