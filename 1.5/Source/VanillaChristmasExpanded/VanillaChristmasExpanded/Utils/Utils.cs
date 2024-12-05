using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


    }
}
