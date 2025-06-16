using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using RimWorld.QuestGen;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(QuestGenUtility), "HardcodedSignalWithQuestID")]
    public static class VanillaChristmasExpanded_QuestGenUtility_HardcodedSignalWithQuestID_Patch
    {


        public static void Postfix(string signal)
        {
           
            if (signal == "GrandBall.OUTCOME")
            {
                FestiveFavorManager.Instance.AddFestiveFavor(25);
            }
        }
    }
}