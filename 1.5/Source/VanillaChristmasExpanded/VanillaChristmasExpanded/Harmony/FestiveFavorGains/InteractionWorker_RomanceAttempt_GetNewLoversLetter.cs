using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "GetNewLoversLetter")]
    public static class VanillaChristmasExpanded_InteractionWorker_RomanceAttempt_GetNewLoversLetter_Patch
    {


        public static void Postfix()
        {
           
                FestiveFavorManager.Instance.AddFestiveFavor(3);
            
        }
    }
}