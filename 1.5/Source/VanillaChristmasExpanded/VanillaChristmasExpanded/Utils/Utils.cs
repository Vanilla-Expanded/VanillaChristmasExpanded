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

    }
}
