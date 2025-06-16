using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using System.Security.Cryptography;
using Verse.Noise;

namespace VanillaChristmasExpanded
{
    public class IngestionOutcomeDoer_GainFavor : IngestionOutcomeDoer
    {
        public int amount = 1;
        public bool needsDecembary = false;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {

            if (!needsDecembary)
            {
                FestiveFavorManager.Instance.AddFestiveFavor(amount);
            }
            else
            {
               
                if (Utils.IsDecembary(pawn.Tile))
                {
                    FestiveFavorManager.Instance.AddFestiveFavor(amount);
                }
            }

        }

       

    }
}