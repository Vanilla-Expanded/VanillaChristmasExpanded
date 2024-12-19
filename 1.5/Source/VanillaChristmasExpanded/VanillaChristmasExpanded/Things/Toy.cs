using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

using Verse.Noise;
using UnityEngine.Diagnostics;
using System;
using System.Linq;
using UnityEngine.PlayerLoop;
using Verse.Grammar;
using static Mono.Security.X509.X520;

namespace VanillaChristmasExpanded
{
    public class Toy : ThingWithComps
    {

        public CompQuality cachedQuality;

        public string description ="";

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.description, "description", "", false);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            cachedQuality = this.TryGetComp<CompQuality>();
        }

        public override string DescriptionFlavor => DescriptionDetailed;

        public override string DescriptionDetailed {
            get
            {
                return GetDescription();
            }
        
        }

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(60000))
            {
                Pawn pawn = (this.ParentHolder as Pawn_InventoryTracker)?.pawn;

                if (pawn?.ageTracker.canGainGrowthPoints==true)
                {
                    pawn.ageTracker.growthPoints += 1;
                }

            }
        }

        public string GetDescription()
        {
            if (description == "")
            {
                ToyDetails extension = this.def.GetModExtension<ToyDetails>();
                GrammarRequest grammarRequest = default(GrammarRequest);

                grammarRequest.Includes.Add(RulePackDefOf.TalelessImages);
                grammarRequest.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
                grammarRequest.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
                grammarRequest.Includes.Add(extension.descRulePack);
                description = GrammarResolver.Resolve("desc", grammarRequest).StripTags();
              
            }
            return this.def.description+"\n\n"+description;


        }

       
}
}