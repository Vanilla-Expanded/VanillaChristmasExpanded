using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace VanillaChristmasExpanded
{
    public static class Utils
    {

        public static void PopUpConfetti(IntVec3 position, Map map, bool sound)
        {
            Effecter effecter = InternalDefOf.VCE_Confetti.Spawn();
            effecter.Trigger(new TargetInfo(position, map), new TargetInfo(position, map));
            effecter.Cleanup();

            if (sound)
            {
                InternalDefOf.VCE_ConfettiExplosion.PlayOneShot(new TargetInfo(position, map, false));
            }

        }

        public static bool IsDecembary(int tile)
        {
            Vector2 vector = Find.WorldGrid.LongLatOf(tile);
            Quadrum quadrum = GenDate.Quadrum(Find.TickManager.TicksAbs, vector.x);
            if (quadrum == Quadrum.Decembary)
            {
                return true;
            }
            return false;
        }

        public static void ThrowExtendedAirPuffUp(Vector3 loc, Map map, float size, float speedMultiplier)
        {
            if (loc.ToIntVec3().ShouldSpawnMotesAt(map))
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f)), map, FleckDefOf.AirPuff, Rand.Range(1.5f, 2.5f) * size);
                dataStatic.rotationRate = Rand.RangeInclusive(-240, 240);
                dataStatic.velocityAngle = Rand.Range(-45, 45);
                dataStatic.velocitySpeed = Rand.Range(1.2f, 1.5f) * speedMultiplier;
                map.flecks.CreateFleck(dataStatic);
            }
        }

    }
}
