using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using Mono.Unix.Native;



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

                List<Building> colonyBuildings = this.map.listerBuildings.allBuildingsColonist;




                foreach (Building building in colonyBuildings)
                {
                    if (building.Stuff == InternalDefOf.VCE_Snow)
                    {

                        float ambientTemperature = building.AmbientTemperature;
                        if (!(ambientTemperature < 0f))
                        {
                            int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
                            if (num > 0)
                            {
                                building.TakeDamage(new DamageInfo(DamageDefOf.Rotting, num));
                                IntVec3 c;
                                CellFinder.TryFindRandomReachableCellNearPosition(building.Position, building.Position, map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                                FilthMaker.TryMakeFilth(c, map, InternalDefOf.VCE_Filth_Water);
                            }
                        }

                    }
                }


                tickCounter = 0;
            }



        }




    }


}



