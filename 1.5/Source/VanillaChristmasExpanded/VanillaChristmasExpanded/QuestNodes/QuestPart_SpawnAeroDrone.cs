using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace VanillaChristmasExpanded
{
    public class QuestPart_SpawnAeroDrone : QuestPart_SpawnThing
    {
        public override void Notify_QuestSignalReceived(Signal signal)
        {
            if (mapParent == null || !mapParent.HasMap)
            {
                mapParent = QuestGen_Get.GetMap().Parent;
                TryFindAerodronePosition(mapParent.Map, out cell);
            }
            base.Notify_QuestSignalReceived(signal);
        }

        public static bool TryFindAerodronePosition(Map map, out IntVec3 spot)
        {
            CellRect rect = GenAdj.OccupiedRect(IntVec3.Zero, InternalDefOf.VCE_SantaAerodrone_Crashed.defaultPlacingRot, InternalDefOf.VCE_SantaAerodrone_Crashed.size);
            IntVec3 position = rect.CenterCell + InternalDefOf.VCE_SantaAerodrone_Crashed.interactionCellOffset;
            rect = rect.ExpandToFit(position);
            if (DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, new IntVec2(rect.Width, rect.Height), InternalDefOf.VCE_SantaAerodrone_Crashed.interactionCellOffset))
            {
                return true;
            }
            return false;
        }
    }
}