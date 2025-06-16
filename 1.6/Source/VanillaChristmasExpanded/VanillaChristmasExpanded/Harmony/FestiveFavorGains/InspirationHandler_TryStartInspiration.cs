using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(InspirationHandler), "TryStartInspiration")]
    public static class VanillaChristmasExpanded_InspirationHandler_TryStartInspiration_Patch
    {


        public static void Postfix(InspirationHandler __instance, bool __result)
        {
            if (__result)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(15);
            }
        }
    }
}