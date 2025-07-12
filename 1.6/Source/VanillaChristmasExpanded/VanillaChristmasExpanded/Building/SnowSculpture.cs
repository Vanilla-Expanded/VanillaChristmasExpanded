using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

using Verse.Noise;
using UnityEngine.Diagnostics;
using System;
using System.Linq;

namespace VanillaChristmasExpanded
{
    public class SnowSculpture : Building
    {

        public Graphic_Single cachedGraphic = null;
        public GraphicsByQualityExtension cachedGraphicsExtension;
        public string cachedGraphicPath = "";


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.cachedGraphicPath, "cachedGraphicPath", "", false);

        }

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);

            if (this.IsHashIntervalTick(60000, delta))
            {
                FestiveFavorManager.Instance.AddFestiveFavor(FestiveFavorByQuality(compQuality.Quality));
            }

        }

        public int FestiveFavorByQuality(QualityCategory quality)
        {
            return quality switch
            {
                QualityCategory.Normal or QualityCategory.Good => 2,
                QualityCategory.Excellent => 3,
                QualityCategory.Masterwork => 4,
                QualityCategory.Legendary => 5,
                _ => 1 // Awful/poor/incorrect value
            };
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            cachedGraphicsExtension = this.def.GetModExtension<GraphicsByQualityExtension>();
            if (cachedGraphicsExtension != null)
            {
                LongEventHandler.ExecuteWhenFinished(delegate { StoreGraphics(); });
            }
            base.SpawnSetup(map, respawningAfterLoad);
        }

        public void StoreGraphics()
        {
            QualityCategory quality = compQuality.Quality;

            if (cachedGraphicPath == "")
            {
                cachedGraphicPath = cachedGraphicsExtension.graphics.First(x => x.quality == quality).texturePaths.First();
            }
            cachedGraphic = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(cachedGraphicPath, ShaderDatabase.Cutout,
                     this.def.graphicData.drawSize, Color.white);

        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            base.DrawAt(drawLoc, flip);

            if (cachedGraphicPath != "")
            {
                var vector = DrawPos + Altitudes.AltIncVect;

                vector.y += Altitudes.AltInc;
                Vector3 vectorOffset = Vector3.zero;

                cachedGraphic.DrawFromDef(vector + vectorOffset, Rot4.North, null);

            }
        }






    }
}