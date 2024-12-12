using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(SnowGrid), "SetDepth")]
    public static class VanillaChristmasExpanded_SnowGrid_SetDepth_Patch
    {


        public static void Postfix(IntVec3 c, float newDepth, Map ___map)
        {


            if (newDepth==0)
            {
                Thing thing = ThingMaker.MakeThing(InternalDefOf.VCE_Snow);
                thing.stackCount = 16;
                GenPlace.TryPlaceThing(thing, c, ___map, ThingPlaceMode.Near);

            }


        }
    }
}