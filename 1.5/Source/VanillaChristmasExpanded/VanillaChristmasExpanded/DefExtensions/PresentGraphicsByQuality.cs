using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaChristmasExpanded
{

    public class PresentGraphicsByQuality : DefModExtension
    {
        public List<PresentGraphics> graphics = null;

    }

    public class PresentGraphics
    {
        public List<string> texturePaths;
        public QualityCategory quality;

    }
}
