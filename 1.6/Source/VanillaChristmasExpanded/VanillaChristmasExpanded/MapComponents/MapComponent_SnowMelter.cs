using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;

using System.Linq;



namespace VanillaChristmasExpanded
{
    public class MapComponent_SnowMelter : MapComponent
    {



        public int tickInterval = GenDate.TicksPerHour;
        private const float MeltPerIntervalPer10Degrees = 1.5f;


        public MapComponent_SnowMelter(Map map) : base(map)
        {

        }

        public override void MapComponentTick()
        {

            if (map.IsHashIntervalTick(tickInterval))
            {

                List<Thing> colonyThings = this.map.listerThings.AllThings.Where(x => x.def == InternalDefOf.VCE_SnowSculpture || x.def== InternalDefOf.VCE_Snow|| x.Stuff == InternalDefOf.VCE_Snow).ToList();




                foreach (Thing thing in colonyThings)
                {
                    if (thing.Map != null && thing.Position.InBounds(thing.Map))
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



            }



        }




    }


}



