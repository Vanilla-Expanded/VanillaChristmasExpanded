using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(WorkGiver_Repair), "HasJobOnThing")]
    public static class VanillaChristmasExpanded_WorkGiver_Repair_HasJobOnThing_Patch
    {


        public static void Postfix(ref bool __result, Thing t)
        {
            if (t.def == InternalDefOf.VCE_SnowSculpture || t.Stuff==InternalDefOf.VCE_Snow) { __result = false; }


        }
    }
}