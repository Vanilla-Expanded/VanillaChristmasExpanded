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

        public CompQuality compQuality;

        public Graphic_Single cachedGraphic = null;
        public GraphicsByQualityExtension cachedGraphicsExtension;
        public string cachedGraphicPath = "";


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.cachedGraphicPath, "cachedGraphicPath", "", false);

        }

        public override void Tick()
        {
            base.Tick();

            if (this.IsHashIntervalTick(60000))
            {
                FestiveFavorManager.Instance.AddFestiveFavor(FestiveFavorByQuality(compQuality.Quality));
            }

        }

        public int FestiveFavorByQuality(QualityCategory quality)
        {
            switch (quality)
            {

                case QualityCategory.Awful:
                    return 1;
                case QualityCategory.Poor:
                    return 1;
                case QualityCategory.Normal:
                    return 2;
                case QualityCategory.Good:
                    return 2;
                case QualityCategory.Excellent:
                    return 3;
                case QualityCategory.Masterwork:
                    return 4;
                case QualityCategory.Legendary:
                    return 5;

            }
            return 1;


        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            compQuality = this.TryGetComp<CompQuality>();

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
                cachedGraphicPath = cachedGraphicsExtension.graphics.Where(x => x.quality == quality).First().texturePaths.First();
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