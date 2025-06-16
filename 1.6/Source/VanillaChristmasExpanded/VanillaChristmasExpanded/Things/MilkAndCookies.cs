using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

using Verse.Noise;
using UnityEngine.Diagnostics;
using System;
using System.Linq;

namespace VanillaChristmasExpanded
{
    public class MilkAndCookies : ThingWithComps
    {




        protected override void Tick()
        {
            base.Tick();

            if (this.IsHashIntervalTick(1800) && this.Map != null)
            {

                if (Utils.IsDecembary(Tile))
                {
                    if (GenLocalDate.DayOfQuadrum(Tile) == 9)
                    {
                       
                        if (GenLocalDate.HourOfDay(this.Map) == 7)
                        {
                            Utils.PopUpConfetti(Position, Map, false);
                            FestiveFavorManager.Instance.AddFestiveFavor(10 * stackCount);
                            this.Destroy();
                        }

                    }



                }


            }



        }




    }
}