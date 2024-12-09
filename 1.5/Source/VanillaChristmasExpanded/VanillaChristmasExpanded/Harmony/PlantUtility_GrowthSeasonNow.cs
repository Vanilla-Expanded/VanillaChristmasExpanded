using System;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace VanillaChristmasExpanded
{
     [HarmonyPatch(typeof(PlantUtility), "GrowthSeasonNow")]
     public static class VanillaChristmasExpanded_PlantUtility_GrowthSeasonNow_Patch
    {


         public static void Postfix(IntVec3 c, Map map,ref bool __result)
         {

           
                if (c.GetPlantToGrowSettable(map)?.GetPlantDefToGrow() == InternalDefOf.VCE_GingerRoot)
                {
                    Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
                    if (roomOrAdjacent != null)
                    {
                        __result =true;
                    }

                }
            

         }
     }
}