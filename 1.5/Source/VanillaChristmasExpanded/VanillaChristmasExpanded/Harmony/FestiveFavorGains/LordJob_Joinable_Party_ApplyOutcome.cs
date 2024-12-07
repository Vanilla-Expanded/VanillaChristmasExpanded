using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(LordJob_Joinable_Party))]
    [HarmonyPatch("ApplyOutcome")]
    public static class VanillaChristmasExpanded_LordJob_Joinable_Party_ApplyOutcome_Patch
    {


        public static void Postfix(InteractionDef intDef)
        {

            
                FestiveFavorManager.Instance.AddFestiveFavor(10);
            

        }
    }
}