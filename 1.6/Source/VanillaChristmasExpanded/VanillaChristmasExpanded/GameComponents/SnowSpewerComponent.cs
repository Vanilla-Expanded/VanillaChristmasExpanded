using System;
using Verse;
using UnityEngine;
using RimWorld;
using System.Linq;
using LudeonTK;

namespace VanillaChristmasExpanded
{

    public class SnowSpewerComponent : GameComponent
    {
        public bool active = false;

        public static SnowSpewerComponent Instance;

        public SnowSpewerComponent(Game game)
        {
            Instance = this;
        }

        public void Activate(Map map)
        {
            if(map!=null) {
                GameCondition gameCondition = GameConditionMaker.MakeCondition(InternalDefOf.VCE_ColdSnap);
                gameCondition.Permanent = true;
                map.GameConditionManager.RegisterCondition(gameCondition);
                active = true;
            }
            
        }

        public void Deactivate(Map map)
        {
            if (map != null)
            {
                GameCondition activeCondition = map.gameConditionManager.GetActiveCondition(InternalDefOf.VCE_ColdSnap);
                if (activeCondition != null)
                {
                    activeCondition.Duration = 0;
                }
                active = false;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref active, "active", false);
            Instance = this;
        }
    }
}
