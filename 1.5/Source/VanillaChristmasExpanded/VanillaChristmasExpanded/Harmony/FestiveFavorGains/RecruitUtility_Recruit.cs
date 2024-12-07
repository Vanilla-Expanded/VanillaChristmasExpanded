using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(RecruitUtility), "Recruit")]
    public static class VanillaChristmasExpanded_RecruitUtility_Recruit_Patch
    {
      

        public static void Postfix(Pawn pawn)
        {
            if (pawn.RaceProps.Humanlike)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(20);
            }
        }
    }
}