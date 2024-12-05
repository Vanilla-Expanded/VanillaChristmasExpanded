using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using System.Security.Cryptography;
using Verse.Noise;

namespace VanillaChristmasExpanded
{
    public class IngestionOutcomeDoer_Confetti : IngestionOutcomeDoer
    {
      

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            if (pawn.Map != null)
            {
                Effecter effecter = InternalDefOf.VCE_Confetti.Spawn();

                //pawn.Map.effecterMaintainer.AddEffecterToMaintain(effecter, pawn.Position, 200);
                effecter.Trigger(new TargetInfo(pawn.Position, pawn.Map), new TargetInfo(pawn.Position, pawn.Map));
                effecter.Cleanup();

                InternalDefOf.VCE_ConfettiExplosion.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));

            }
        }

       

    }
}