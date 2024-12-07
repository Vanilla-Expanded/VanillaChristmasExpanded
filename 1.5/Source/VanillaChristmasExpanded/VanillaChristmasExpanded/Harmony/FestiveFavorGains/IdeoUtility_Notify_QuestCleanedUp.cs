using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(IdeoUtility), "Notify_QuestCleanedUp")]
    public static class VanillaChristmasExpanded_IdeoUtility_Notify_QuestCleanedUp_Patch
    {


        public static void Postfix(Quest quest, QuestState state)
        {
            if (quest.root.successHistoryEvent== InternalDefOf.CharityFulfilled_HospitalityRefugees && state== QuestState.EndedSuccess)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(10);
            }
        }
    }
}