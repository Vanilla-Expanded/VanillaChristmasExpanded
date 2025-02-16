﻿using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.Sound;
using VanillaChristmasExpanded;

namespace VanillaChristmasExpanded
{
    public class RitualBehaviorWorker_SingingCarols : RitualBehaviorWorker
    {
        private Sustainer soundPlaying;
        public RitualBehaviorWorker_SingingCarols()
        {
        }

        public RitualBehaviorWorker_SingingCarols(RitualBehaviorDef def) : base(def)
        {
        }



        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);
            if (ritual.StageIndex == 0)
            {
                if (this.soundPlaying == null || this.soundPlaying.Ended)
                {
                    TargetInfo selectedTarget = ritual.selectedTarget;
                    this.soundPlaying = InternalDefOf.VCE_RitualSustainer_Carols.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedTarget.Cell, selectedTarget.Map, false), MaintenanceType.PerTick));
                }
                Sustainer sustainer = this.soundPlaying;
                if (sustainer == null)
                {
                    return;
                }
                sustainer.Maintain();
            }
        }
    }
}
