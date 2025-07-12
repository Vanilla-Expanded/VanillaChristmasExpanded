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
            Scribe_Values.Look(ref this.tickCounter, "tickCounter", 0, false);
            Scribe_Values.Look(ref this.onDayCounter, "onDayCounter", false, false);


        }

        protected override void TickInterval(int delta)
        {
            base.TickInterval(delta);

            if(!onDayCounter&&this.Map!=null&& this.IsHashIntervalTick(60, delta) && compPower is not { PowerOn: not true })
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
                tickCounter += delta;
                // Allow for 15 ticks less due to tick interval.
                // Not that it matters much, as the effect cannot trigger unless it's midnight
                if (tickCounter > GenDate.TicksPerDay - 15)
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
                cachedGraphicPath = cachedGraphicsExtension.graphics.First(x => x.quality == compQuality.Quality).texturePaths.ElementAt(cachedGraphicIndex);

                cachedGraphic = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(cachedGraphicPath, ShaderDatabase.Cutout,
                         this.def.graphicData.drawSize, Color.white);
            };

            yield return command_Action;

        }




    }
}