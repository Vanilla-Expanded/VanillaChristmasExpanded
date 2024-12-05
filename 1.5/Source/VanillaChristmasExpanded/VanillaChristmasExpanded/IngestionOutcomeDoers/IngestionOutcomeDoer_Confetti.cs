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

                Utils.PopUpConfetti(pawn.Position, pawn.Map, true);
                

            }
        }

       

    }
}