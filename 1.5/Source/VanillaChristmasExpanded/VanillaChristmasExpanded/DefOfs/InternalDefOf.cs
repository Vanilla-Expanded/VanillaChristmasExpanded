using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaChristmasExpanded
{

    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        public static FleckDef VCE_ConfettiA;
        public static FleckDef VCE_ConfettiB;
        public static FleckDef VCE_ConfettiC;
        public static FleckDef VCE_ConfettiD;

        public static SoundDef VCE_ConfettiExplosion;
    }
}