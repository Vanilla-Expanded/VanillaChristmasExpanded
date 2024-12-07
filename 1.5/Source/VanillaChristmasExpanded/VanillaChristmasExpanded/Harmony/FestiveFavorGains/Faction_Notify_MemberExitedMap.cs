using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(Faction), "Notify_MemberExitedMap")]
    public static class VanillaChristmasExpanded_Faction_Notify_MemberExitedMap_Patch
    {

        public static void Postfix(Pawn member)
        {
            if (member.guest?.Released==true)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(35);
            }
        }
    }
}