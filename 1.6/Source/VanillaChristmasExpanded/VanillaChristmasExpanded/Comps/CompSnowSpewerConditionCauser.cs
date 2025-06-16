
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;
namespace VanillaChristmasExpanded
{

    public class CompSnowSpewerConditionCauser : ThingComp
    {

      
        public SnowSpewer_SteamSprayer sprayer;

        new public CompProperties_SnowSpewerConditionCauser Props => (CompProperties_SnowSpewerConditionCauser)props;



        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            sprayer = new SnowSpewer_SteamSprayer(this.parent);
            SnowSpewerComponent.Instance.active=true;
        }

        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.Map != null)
            {
                sprayer.SteamSprayerTick();
            }
            

        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);
            SnowSpewerComponent.Instance.Deactivate(previousMap);
            previousMap.weatherManager.TransitionTo(WeatherDefOf.Clear);

            foreach (Map map in Find.Maps)
            {
                SnowSpewerComponent.Instance.Deactivate(map);
                map.weatherManager.TransitionTo(WeatherDefOf.Clear);
            }
                

        }





      

    }


}
