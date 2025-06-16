
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace VanillaChristmasExpanded
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {
        public static HashSet<(InteractionDef, int)> interactionsWithNull = new HashSet<(InteractionDef, int)>() { ( InteractionDefOf.Chitchat, 1 ),
            ( InteractionDefOf.DeepTalk,1),( InternalDefOf.KindWords,2),( InteractionDefOf.Nuzzle,3),( InteractionDefOf.BabyPlay,2)
            ,( InternalDefOf.VSIE_Vent,3),( InternalDefOf.VFEE_RoyalGossip,1)
        };

        public static HashSet<(PreceptDef, int)> preceptsWithNull = new HashSet<(PreceptDef, int)>() { ( PreceptDefOf.ThroneSpeech, 10 ),
            ( PreceptDefOf.AnimaTreeLinking,10),( PreceptDefOf.Funeral,10),( InternalDefOf.FuneralNoCorpse,10),( InternalDefOf.Festival,10)
            ,( InternalDefOf.Classic_DanceParty,10),( InternalDefOf.DateRitualConsumable,10),( InternalDefOf.LeaderSpeech,10),( InternalDefOf.TreeConnection,20)
            ,( InternalDefOf.VFEE_RoyalAddress,10)
        };

        public static Dictionary<InteractionDef, int> interactions = new Dictionary<InteractionDef, int>();
        public static Dictionary<PreceptDef, int> precepts = new Dictionary<PreceptDef, int>();



        static StaticCollections()
        {

            foreach((InteractionDef, int) item in interactionsWithNull)
            {
                if (item.Item1 != null)
                {
                    interactions[item.Item1] = item.Item2;
                }

            }
            foreach ((PreceptDef, int) item in preceptsWithNull)
            {
                if (item.Item1 != null)
                {
                    precepts[item.Item1] = item.Item2;
                }

            }

        }




    }
}
