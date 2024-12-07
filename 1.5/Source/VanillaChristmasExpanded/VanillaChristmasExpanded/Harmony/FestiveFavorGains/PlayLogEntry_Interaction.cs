using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(PlayLogEntry_Interaction), MethodType.Constructor)]
    public static class VanillaChristmasExpanded_PlayLogEntry_Interaction_Patch
    {


        public static void Postfix(InteractionDef intDef)
        {
           
            if (intDef == InteractionDefOf.Counsel_Success || intDef == InteractionDefOf.Counsel_Success)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(2);
            }
           
        }
    }
}