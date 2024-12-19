using System;
using RimWorld;
using Verse;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace VanillaChristmasExpanded
{
    [HarmonyPatch(typeof(FactionGiftUtility), "GiveGift", new Type[] { typeof(List<Tradeable>), typeof(Faction), typeof(GlobalTargetInfo) })]
    public static class VanillaChristmasExpanded_FactionGiftUtility_GiveGift_Patch
    {
        public static int FestiveFavorByQuality(QualityCategory quality)
        {
            switch (quality)
            {

                case QualityCategory.Awful:
                    return 1;
                case QualityCategory.Poor:
                    return 3;
                case QualityCategory.Normal:
                    return 5;
                case QualityCategory.Good:
                    return 8;
                case QualityCategory.Excellent:
                    return 15;
                case QualityCategory.Masterwork:
                    return 30;
                case QualityCategory.Legendary:
                    return 50;

            }
            return 1;


        }

        public static void Postfix(List<Tradeable> tradeables, Faction giveTo)
        {
           
            float totalGifts = 0;
            float totalFestive = 0;
            foreach (Tradeable tradeable in tradeables)
            {
                foreach (Thing thingTraded in tradeable.thingsColony)
                { 
                    if(thingTraded is Toy toy)
                    {
                        totalGifts++;
                        int favor = FestiveFavorByQuality(toy.cachedQuality.Quality);
                        totalFestive += favor;
                        FestiveFavorManager.Instance.AddFestiveFavor(favor);

                    }

                }
                  

            }
            if (totalGifts>0)
            {
                Messages.Message("VCE_DonatedToys".Translate(totalGifts,giveTo.Name,totalFestive), GlobalTargetInfo.Invalid, MessageTypeDefOf.PositiveEvent);

            }


        }
    }


    [HarmonyPatch(typeof(FactionGiftUtility), "GiveGift", new Type[] { typeof(List<ActiveDropPodInfo>), typeof(Settlement) })]
    public static class VanillaChristmasExpanded_FactionGiftUtility_GiveGift_DropPods_Patch
    {
       

        public static void Prefix(List<ActiveDropPodInfo> pods, Settlement giveTo)
        {
        
            float totalGifts = 0;
            float totalFestive = 0;
            foreach (ActiveDropPodInfo podInfo in pods)
            {
                ThingOwner innerContainer = podInfo.innerContainer;
               
                foreach (Thing thingTraded in innerContainer)
                {
                    Log.Message(thingTraded.def.defName);
                    if (thingTraded is Toy toy)
                    {
                        totalGifts++;
                        int favor = VanillaChristmasExpanded_FactionGiftUtility_GiveGift_Patch.FestiveFavorByQuality(toy.cachedQuality.Quality);
                        totalFestive += favor;
                        FestiveFavorManager.Instance.AddFestiveFavor(favor);

                    }

                }


            }
            if (totalGifts > 0)
            {
                Messages.Message("VCE_DonatedToys".Translate(totalGifts, giveTo.Name, totalFestive), GlobalTargetInfo.Invalid, MessageTypeDefOf.PositiveEvent);

            }


        }
    }
}