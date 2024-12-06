using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaChristmasExpanded
{

    public class GraphicsByQualityExtension : DefModExtension
    {
        public List<GraphicsByQuality> graphics = null;

    }

    public class GraphicsByQuality
    {
        public List<string> texturePaths;
        public QualityCategory quality;

    }
}
