
using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;
using VanillaChristmasExpanded;
using Verse;

namespace VanillaChristmasExpanded
{
    public class QuestNode_SantaAerodrone : QuestNode
    {
        private const string QuestTag = "VCE_SantaAerodrone";

        private const int TicksToShuttleArrival = 180;

      
     
        protected override void RunInt()
        {
         


        }

        protected override bool TestRunInt(Slate slate)
        {
            Map map = QuestGen_Get.GetMap();
            if (map == null)
            {
                return false;
            }

            if (!TryFindShuttleCrashPosition(map, InternalDefOf.VCE_SantaAerodrone_Crashed.size, out var _))
            {
                return false;
            }
            return true;
        }

        private bool TryFindShuttleCrashPosition(Map map, IntVec2 size, out IntVec3 spot)
        {
            if (DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, size))
            {
                return true;
            }
            return false;
        }
    }
}