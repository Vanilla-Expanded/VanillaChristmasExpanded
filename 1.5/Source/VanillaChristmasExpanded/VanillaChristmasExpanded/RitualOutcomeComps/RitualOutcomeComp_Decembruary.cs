﻿
using RimWorld;
using UnityEngine;
using Verse;
namespace VanillaChristmasExpanded
{
    public class RitualOutcomeComp_Decembruary : RitualOutcomeComp_QualitySingleOffset
    {
        public override bool DataRequired => false;

        public override bool Applies(LordJob_Ritual ritual)
        {
            return true;
        }
        [MustTranslate]
        public string labelNotMet;

        public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
        {

            Vector2 vector = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
            Quadrum quadrum = GenDate.Quadrum(Find.TickManager.TicksAbs, vector.x);
            if(quadrum == Quadrum.Decembary)
            {
                return 1;
            }

            return 0f;
        }

        public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
        {
            return ((Count(ritual, data) > 0f) ? label : labelNotMet).CapitalizeFirst().Formatted() + ": " + "OutcomeBonusDesc_QualitySingleOffset".Translate(qualityOffset.ToStringPercent()) + ".";
        }

        public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
        {
            float quality = 0f;
            bool flag = false;
            if (ritualTarget.Map != null)
            {
                Vector2 vector = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
                Quadrum quadrum = GenDate.Quadrum(Find.TickManager.TicksAbs, vector.x);
                flag = quadrum == Quadrum.Decembary;
                quality = (flag ? qualityOffset : 0f);
            }
            return new QualityFactor
            {
                label = label.CapitalizeFirst(),
                qualityChange = ExpectedOffsetDesc(positive: true, quality),
                quality = quality,
                present = flag,
                positive = true,
                priority = 0f
            };
        }
    }
}