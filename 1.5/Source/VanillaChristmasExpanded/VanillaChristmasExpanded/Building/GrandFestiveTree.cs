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
        public Graphic_Single cachedGraphic = null;
        public GraphicsByQualityExtension cachedGraphicsExtension;
        public string cachedGraphicPath = "";
        public int cachedGraphicIndex = 0;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref this.cachedGraphicPath, "cachedGraphicPath", "", false);
            Scribe_Values.Look(ref this.cachedGraphicIndex, "cachedGraphicIndex", 1, false);


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