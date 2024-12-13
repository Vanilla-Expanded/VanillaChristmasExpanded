using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using Mono.Unix.Native;
using System.Linq;



namespace VanillaChristmasExpanded
{
    public class MapComponent_SnowMelter : MapComponent
    {



        public int tickCounter = 0;
        public int tickInterval = 2500;
        private const float MeltPerIntervalPer10Degrees = 1.5f;


        public MapComponent_SnowMelter(Map map) : base(map)
        {

        }

        public override void MapComponentTick()
        {

            tickCounter++;
            if ((tickCounter > tickInterval))
            {

                List<Thing> colonyThings = this.map.listerThings.AllThings.Where(x => x.Stuff == InternalDefOf.VCE_Snow).ToList();




                foreach (Thing thing in colonyThings)
                {
                    if(thing.Map!=null&& thing.Position.InBounds(thing.Map))
                    {
                        float ambientTemperature = thing.AmbientTemperature;
                        if (ambientTemperature > 0f)
                        {
                            int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
                            if (num > 0)
                            {
                                thing.TakeDamage(new DamageInfo(DamageDefOf.Rotting, num));
                                IntVec3 c;
                                CellFinder.TryFindRandomReachableCellNearPosition(thing.Position, thing.Position, map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                                if (c.InBounds(map))
                                {
                                    FilthMaker.TryMakeFilth(c, map, InternalDefOf.VCE_Filth_Water);
                                }
                            }
                        }
                    }
                        

                    
                }

               


                tickCounter = 0;
            }



        }




    }


}



