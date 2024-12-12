using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(JobDriver_ClearSnow), "MakeNewToils")]
    public static class VanillaChristmasExpanded_JobDriver_ClearSnow_MakeNewToils_Patch
    {


        public static void Postfix(JobDriver_ClearSnow __instance)
        {
          

           
                Thing thing = ThingMaker.MakeThing(InternalDefOf.VCE_Snow);
                thing.stackCount = 4;
                GenPlace.TryPlaceThing(thing, __instance.job.targetA.Cell, __instance.pawn.Map, ThingPlaceMode.Near);

          


        }
    }
}