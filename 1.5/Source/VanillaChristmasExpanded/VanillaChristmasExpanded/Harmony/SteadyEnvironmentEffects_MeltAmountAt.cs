using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(SteadyEnvironmentEffects), "MeltAmountAt")]
    public static class VanillaChristmasExpanded_SteadyEnvironmentEffects_MeltAmountAt_Patch
    {
       

        public static void Postfix(ref float __result)
        {
            if (SnowSpewerComponent.Instance.active) { __result = 0; }
                
            
        }
    }
}