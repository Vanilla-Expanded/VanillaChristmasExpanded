using HarmonyLib;
using RimWorld;
using System;
using Verse;
using RimWorld.BaseGen;



namespace VanillaChristmasExpanded
{

    /*This Harmony Postfix tries to remove exotic materials from ruins
*/
    [HarmonyPatch(typeof(BaseGenUtility))]
    [HarmonyPatch("RandomCheapWallStuff")]
    [HarmonyPatch(new Type[] { typeof(TechLevel), typeof(bool) })]


    public static class VanillaChristmasExpanded_BaseGenUtility_RandomCheapWallStuff_Patch
    {
        [HarmonyPostfix]
        public static void RemoveMaterial(ref ThingDef __result)

        {
            if (__result != null)
            {
                if (__result == InternalDefOf.VCE_Gingerbread || __result == InternalDefOf.VCE_Snow)
                {

                    __result = ThingDefOf.BlocksGranite;
                }
            }



        }

    }


}