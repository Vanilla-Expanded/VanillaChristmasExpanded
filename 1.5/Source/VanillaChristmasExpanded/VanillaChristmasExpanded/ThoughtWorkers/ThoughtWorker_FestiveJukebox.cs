using RimWorld;
using Verse;
using System.Collections.Generic;

namespace VanillaChristmasExpanded
{
    internal class ThoughtWorker_FestiveJukebox : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!p.Spawned)
                return false;

            if (!p.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
            {
                return false;
            }

            List<Thing> jukeboxes = p.Map.listerThings.ThingsOfDef(InternalDefOf.VCE_FestiveJukebox);
            for (int i = 0; i < jukeboxes.Count; i++)
            {
                if (ShouldActivateThought(p, jukeboxes[i], 9.9f))
                    return ThoughtState.ActiveAtStage(0);
            }         

            return false;
        }

        private bool ShouldActivateThought(Pawn p, Thing thing, float radius)
        {
            var comp = thing.TryGetComp<CompPowerTrader>();
            if ((comp == null || comp.PowerOn) && p.Position.InHorDistOf(thing.Position, radius))
            {
                return true;
            }

            return false;
        }
    }
}