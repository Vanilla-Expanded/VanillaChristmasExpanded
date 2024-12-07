using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(LordJob_Joinable_MarriageCeremony))]
    [HarmonyPatch("WeddingSucceeded")]
    public static class VanillaChristmasExpanded_LordJob_Joinable_MarriageCeremony_WeddingSucceeded_Patch
    {


        public static void Postfix()
        {


            FestiveFavorManager.Instance.AddFestiveFavor(20);


        }
    }
}