using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

using Verse.Noise;
using UnityEngine.Diagnostics;
using System;
using System.Linq;
using Verse.Grammar;

namespace VanillaChristmasExpanded
{
    public class FestiveFeastTable : Building
    {

        public string labelText = "";

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.labelText, "labelText", "", false);
        }

        public override string Label
        {
            get
            {
                if (labelText == "")
                {
                    GrammarRequest request = default(GrammarRequest);
                    request.Includes.Add(def.ideoBuildingNamerBase);


                    labelText = GrammarResolver.Resolve("buildingName", request, null, forceLog: false, null, null, null, capitalizeFirstSentence: false);
                }

                return labelText;


            }
        }




    }
}