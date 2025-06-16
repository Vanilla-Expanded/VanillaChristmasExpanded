using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Grammar;


namespace VanillaChristmasExpanded
{
    public class Toy : ThingWithComps
    {

        public QualityCategory cachedQuality;

        public string description ="";

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.description, "description", "", false);
            Scribe_Values.Look(ref this.cachedQuality, "cachedQuality", QualityCategory.Normal, false);

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            cachedQuality = this.TryGetComp<CompQuality>().Quality;
        }

        public override string DescriptionFlavor => DescriptionDetailed;

        public override string DescriptionDetailed {
            get
            {
                return GetDescription();
            }
        
        }

        protected override void Tick()
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