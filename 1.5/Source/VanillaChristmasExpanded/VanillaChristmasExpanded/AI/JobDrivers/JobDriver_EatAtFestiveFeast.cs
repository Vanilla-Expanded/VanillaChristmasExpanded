﻿
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace VanillaChristmasExpanded
{
    public class JobDriver_EatAtFestiveFeast : JobDriver
    {
      

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.ReserveSittableOrSpot(job.targetB.Cell, job, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
           
                this.EndOnDespawnedOrNull(TargetIndex.A);
                yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
                float totalBuildingNutrition = 12;
                Toil eat = ToilMaker.MakeToil("MakeNewToils");
                eat.tickAction = delegate
                {
                    pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(pawn.Position));
                    pawn.GainComfortFromCellIfPossible();
                    if (pawn.needs.food != null)
                    {
                        pawn.needs.food.CurLevel += totalBuildingNutrition / (float)pawn.GetLord().ownedPawns.Count / (float)eat.defaultDuration;
                    }
                    if (pawn.IsHashIntervalTick(40) && Rand.Value < 0.1f)
                    {
                        IntVec3 c = Rand.Bool ? pawn.Position : pawn.RandomAdjacentCellCardinal();
                        if (c.InBounds(pawn.Map))
                        {
                            FilthMaker.TryMakeFilth(c, pawn.Map, ThingDefOf.Filth_Trash);
                        }
                    }
                };
                eat.AddFinishAction(delegate
                {
                    pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(InternalDefOf.AteLavishMeal);
                });
                eat.WithEffect(EffecterDefOf.EatMeat, TargetIndex.A);
                eat.PlaySustainerOrSound(SoundDefOf.RawMeat_Eat);
                eat.handlingFacing = true;
                eat.defaultCompleteMode = ToilCompleteMode.Delay;
                eat.defaultDuration = (job.doUntilGatheringEnded ? job.expiryInterval : job.def.joyDuration);
                yield return eat;
            
        }
    }
}