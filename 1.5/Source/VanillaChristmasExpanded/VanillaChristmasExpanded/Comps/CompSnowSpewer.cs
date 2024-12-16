
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
namespace VanillaChristmasExpanded
{

    public class CompSnowSpewer : ThingComp
    {

        public bool active = false;
        public SnowSpewer_SteamSprayer sprayer;

        new public CompProperties_SnowSpewer Props => (CompProperties_SnowSpewer)props;



        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref active, "active");

        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            sprayer = new SnowSpewer_SteamSprayer(this.parent);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (active)
            {
                sprayer.SteamSprayerTick();
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            base.PostDestroy(mode, previousMap);

            if (active)
            {
              
                    active = false;
                    SnowSpewerComponent.Instance.Deactivate(previousMap);
                    previousMap.weatherManager.TransitionTo(WeatherDefOf.Clear);
              

            }
        }





        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo c in base.CompGetGizmosExtra())
            {
                yield return c;
            }
            Command_Action command_Action = new Command_Action();

            if (active)
            {
                command_Action.defaultDesc = "VCE_DeactivateSnowSpewerDesc".Translate();
                command_Action.defaultLabel = "VCE_DeactivateSnowSpewer".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Snowspewer_Deactivate", true);

                command_Action.action = delegate
                {
                    active = false;
                    SnowSpewerComponent.Instance.Deactivate(this.parent.Map);
                    this.parent.Map.weatherManager.TransitionTo(WeatherDefOf.Clear);
                };

            }
            else
            {
                command_Action.defaultDesc = "VCE_ActivateSnowSpewerDesc".Translate();
                command_Action.defaultLabel = "VCE_ActivateSnowSpewer".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Snowspewer_Activate", true);

                command_Action.action = delegate
                {
                    active = true;
                    SnowSpewerComponent.Instance.Activate(this.parent.Map);

                };

            }





            yield return command_Action;


        }


    }


}
