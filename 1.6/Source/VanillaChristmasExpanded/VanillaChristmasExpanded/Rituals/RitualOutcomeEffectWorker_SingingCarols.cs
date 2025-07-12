using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.Sound;
using Verse.AI.Group;
using Verse.AI;
using UnityEngine.UIElements;
using Verse.Noise;


namespace VanillaChristmasExpanded
{
    public class RitualOutcomeEffectWorker_SingingCarols : RitualOutcomeEffectWorker_FromQuality
    {

        public RitualOutcomeEffectWorker_SingingCarols()
        {
        }

        public RitualOutcomeEffectWorker_SingingCarols(RitualOutcomeEffectDef def) : base(def)
        {
        }

        public override bool SupportsAttachableOutcomeEffect
        {
            get
            {
                return false;
            }
        }

        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {

            float quality = base.GetQuality(jobRitual, progress);
            var outcome = this.GetOutcome(quality, jobRitual);
            LookTargets lookTargets = jobRitual.selectedTarget;
            string text = null;
            if (jobRitual.Ritual != null)
            {
                this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
            }

            foreach (Pawn pawn in totalPresence.Keys)
            {

                base.GiveMemoryToPawn(pawn, outcome.memory, jobRitual);

            }


            if (outcome.positivityIndex == -1)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(totalPresence.Keys.Count);
            }

            if (outcome.positivityIndex == 1)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(totalPresence.Keys.Count*3);
            }

            if (outcome.positivityIndex == 2)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(totalPresence.Keys.Count*6);
                if (Rand.Chance(0.2f))
                {
                    InternalDefOf.VCE_ConfettiExplosion.PlayOneShotOnCamera();
                    foreach (Pawn pawn in totalPresence.Keys)
                    {
                        ThingWithComps thingToMake = (ThingWithComps)GenSpawn.Spawn(ThingMaker.MakeThing(InternalDefOf.VCE_FestivePresent), pawn.PositionHeld, pawn.Map);
                        thingToMake.SetFaction(pawn.Faction);
                        QualityCategory qualityPresent = QualityUtility.GenerateQualityRandomEqualChance();
                        thingToMake.compQuality.SetQuality(qualityPresent, null);
                        Utils.PopUpConfetti(pawn.Position, pawn.Map, false);
                    }
                }


            }



            string text2 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            string text3 = this.def.OutcomeMoodBreakdown(outcome);
            if (!text3.NullOrEmpty())
            {
                text2 = text2 + "\n\n" + text3;
            }

            if (text != null)
            {
                text2 = text2 + "\n\n" + text;
            }
            string text4;
            this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
            if (text4 != null)
            {
                text2 = text2 + "\n\n" + text4;
            }
            Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null);




        }


    }
}
