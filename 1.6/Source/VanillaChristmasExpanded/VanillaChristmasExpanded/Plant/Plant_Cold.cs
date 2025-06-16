using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace VanillaChristmasExpanded
{
    class Plant_Cold : Plant
    {
        public float GrowthRateFactor_TimeOfYear
        {
            get
            {            
             
                if (Utils.IsDecembary(Tile))
                {
                    return 1f;
                }
                else { return 0.05f; }
            }
        }

        public override float GrowthRate
        {
            get
            {
                if (this.Blighted)
                {
                    return 0f;
                }

                return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Light * GrowthRateFactor_TimeOfYear;
            }
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.LifeStage == PlantLifeStage.Growing)
            {
                stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
                stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
                if (!this.Blighted)
                {
                    if (this.Resting)
                    {
                        stringBuilder.AppendLine("PlantResting".Translate());
                    }
                    if (!this.HasEnoughLightToGrow)
                    {
                        stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + this.def.plant.growMinGlow.ToStringPercent());
                    }
                    float growthRateFactor_Time = this.GrowthRateFactor_TimeOfYear;
                    if (growthRateFactor_Time < 1)
                    {                   
                            stringBuilder.AppendLine("VCE_NotDecembary".Translate());                
                    }
                }
            }
            else if (this.LifeStage == PlantLifeStage.Mature)
            {
                if (this.HarvestableNow)
                {
                    stringBuilder.AppendLine("ReadyToHarvest".Translate());
                }
                else
                {
                    stringBuilder.AppendLine("Mature".Translate());
                }
            }
            if (this.DyingBecauseExposedToLight)
            {
                stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
            }
            if (this.Blighted)
            {
                stringBuilder.AppendLine("Blighted".Translate() + " (" + this.Blight.Severity.ToStringPercent() + ")");
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }


        public override void TickLong()
        {
            CheckMakeLeafless();
            if (base.Destroyed)
            {
                return;
            }

            float num = this.growthInt;
            bool flag = this.LifeStage == PlantLifeStage.Mature;
            this.growthInt += this.GrowthPerTick * 2000f;
            if (this.growthInt > 1f)
            {
                this.growthInt = 1f;
            }
            if (((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f)) && this.CurrentlyCultivated())
            {
                base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
            }

            if (!this.HasEnoughLightToGrow)
            {
                this.unlitTicks += 2000;
            }
            else
            {
                this.unlitTicks = 0;
            }
            this.ageInt += 2000;
            if (this.Dying)
            {
                Map map = base.Map;
                bool isCrop = this.IsCrop;
                bool harvestableNow = this.HarvestableNow;
                bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
                int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
                base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
                if (base.Destroyed)
                {
                    if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
                    {
                        string key;
                        if (harvestableNow)
                        {
                            key = "MessagePlantDiedOfRot_LeftUnharvested";
                        }
                        else if (dyingBecauseExposedToLight)
                        {
                            key = "MessagePlantDiedOfRot_ExposedToLight";
                        }
                        else
                        {
                            key = "MessagePlantDiedOfRot";
                        }
                        Messages.Message(key.Translate(this.GetCustomLabelNoCount(false)).CapitalizeFirst(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
                    }
                    return;
                }
            }

        }
    }
}