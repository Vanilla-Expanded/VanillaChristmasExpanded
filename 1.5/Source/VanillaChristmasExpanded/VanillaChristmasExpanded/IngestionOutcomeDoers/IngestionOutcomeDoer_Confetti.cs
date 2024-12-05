using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaChristmasExpanded
{
    public class IngestionOutcomeDoer_OffsetHemogen : IngestionOutcomeDoer
    {
      

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            if (pawn.Map != null)
            {
                FleckMaker.Static(pawn.Position, pawn.Map, Confetti(Rand.RangeInclusive(0, 3)));
                FleckMaker.Static(pawn.Position, pawn.Map, Confetti(Rand.RangeInclusive(0, 3)));
                InternalDefOf.VCE_ConfettiExplosion.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));

            }
        }

        public static FleckDef Confetti(int index)
        {
            return index switch
            {
                0 => InternalDefOf.VCE_ConfettiA,
                1 => InternalDefOf.VCE_ConfettiB,
                2 => InternalDefOf.VCE_ConfettiC,
                3 => InternalDefOf.VCE_ConfettiD,
                _ => InternalDefOf.VCE_ConfettiA
            };
        }

    }
}