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
    public class GrandFestiveTree : Building
    {

        public CompQuality compQuality;
        public CompPowerTrader compPower;
        public Graphic_Single cachedGraphic = null;
        public GraphicsByQualityExtension cachedGraphicsExtension;
        public string cachedGraphicPath = "";
        public int cachedGraphicIndex = 0;
        public int tickCounter = 0;
        public bool onDayCounter = false;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.cachedGraphicPath, "cachedGraphicPath", "", false);
            Scribe_Values.Look(ref this.cachedGraphicIndex, "cachedGraphicIndex", 1, false);
            Scribe_Values.Look(ref this.cachedGraphicIndex, "tickCounter", 0, false);
            Scribe_Values.Look(ref this.onDayCounter, "onDayCounter", false, false);


        }

        public override void Tick()
        {
            base.Tick();

            if(!onDayCounter&&this.Map!=null&& this.IsHashIntervalTick(60) && compPower?.PowerOn == true)
            {
                if (GenLocalDate.HourOfDay(this.Map) == 0)
                {
                  
                    if (Utils.IsDecembary(Tile))
                    {
                        FestiveFavorManager.Instance.AddFestiveFavor(FestiveFavorByQuality(compQuality.Quality));
                        onDayCounter = true;
                    }
                }              

            }
            if (onDayCounter)
            {
                tickCounter++;
                if (tickCounter > 60000)
                {
                    tickCounter = 0;
                    onDayCounter = false;
                }
            }
            

        }

        public int FestiveFavorByQuality(QualityCategory quality)
        {
            switch (quality)
            {

                case QualityCategory.Awful:
                    return 1;
                case QualityCategory.Poor:
                    return 2;
                case QualityCategory.Normal:
                    return 4;
                case QualityCategory.Good:
                    return 8;
                case QualityCategory.Excellent:
                    return 16;
                case QualityCategory.Masterwork:
                    return 32;
                case QualityCategory.Legendary:
                    return 64;

            }
            return 1;


        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            compQuality = this.TryGetComp<CompQuality>();
            compPower = this.TryGetComp<CompPowerTrader>();
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

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            command_Action.defaultDesc = "VCE_ChangeDecorationsDesc".Translate();
            command_Action.defaultLabel = "VCE_ChangeDecorations".Translate();
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/CycleFestiveColor", true);
            command_Action.hotKey = KeyBindingDefOf.Misc1;
            command_Action.action = delegate
            {
                cachedGraphicIndex++;
                if(cachedGraphicIndex >= 5)
                {
                    cachedGraphicIndex = 0;
                }
                cachedGraphicPath = cachedGraphicsExtension.graphics.Where(x => x.quality == compQuality.Quality).First().texturePaths.ElementAt(cachedGraphicIndex);

                cachedGraphic = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(cachedGraphicPath, ShaderDatabase.Cutout,
                         this.def.graphicData.drawSize, Color.white);
            };

            yield return command_Action;

        }




    }
}