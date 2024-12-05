using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

using Verse.Noise;
using UnityEngine.Diagnostics;

namespace VanillaChristmasExpanded
{
    public class FestivePresent : Building, IOpenable
    {


   

        public float PriceByQuality(QualityCategory quality)
        {
            switch(quality){

                case QualityCategory.Awful:
                    return 50;
                case QualityCategory.Poor:
                    return 150;
                case QualityCategory.Normal:
                    return 300;
                case QualityCategory.Good:
                    return 500;
                case QualityCategory.Excellent:
                    return 800;
                case QualityCategory.Masterwork:
                    return 1500;
                case QualityCategory.Legendary:
                    return 5000;
 
            }
            return 300;


        }
        public IntRange AmountByQuality(QualityCategory quality)
        {
            switch (quality)
            {

                case QualityCategory.Awful:
                    return new IntRange(1, 5);
                case QualityCategory.Poor:
                    return new IntRange(1, 5);
                case QualityCategory.Normal:
                    return new IntRange(1, 5);
                case QualityCategory.Good:
                    return new IntRange(1, 5);
                case QualityCategory.Excellent:
                    return new IntRange(1, 5);
                case QualityCategory.Masterwork:
                    return new IntRange(1, 1);
                case QualityCategory.Legendary:
                    return new IntRange(1,1);

            }
            return  new IntRange(1, 5); 


        }

        public virtual int OpenTicks => 300;

        public virtual bool CanOpen => true;

        public virtual void Open()
        {
            QualityCategory quality = this.GetComp<CompQuality>().Quality;
            ThingSetMakerParams parms = default(ThingSetMakerParams);
            float price = PriceByQuality(quality);
            parms.totalMarketValueRange = new FloatRange(price*0.9f,price);
            parms.minSingleItemMarketValuePct = 0;
            parms.allowNonStackableDuplicates = true;
            parms.countRange = AmountByQuality(quality);
            List<Thing> list2 = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);

          
            if (list2 != null)
            {
                Messages.Message("VCE_OpenedPresent".Translate(list2.ToStringSafeEnumerable()), MessageTypeDefOf.PositiveEvent, true);


                foreach (Thing thing in list2)
                {
                   
                  
                    GenPlace.TryPlaceThing(thing, Position, Map, ThingPlaceMode.Near);


                }
                
                if (this.Spawned)
                {
                    this.Destroy();
                }

            }


        }



    }
}