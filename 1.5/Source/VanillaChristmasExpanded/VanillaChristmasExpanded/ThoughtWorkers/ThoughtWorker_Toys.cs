using RimWorld;
using Verse;
using System.Collections.Generic;

namespace VanillaChristmasExpanded
{
    public class ThoughtWorker_Toys_Children : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!p.Spawned)
                return false;

            if (p.ageTracker.AgeBiologicalYears<3 || p.ageTracker.AgeBiologicalYears >= 13)
            {
                return false;
            }

            List<Thing> inventory = p.inventory?.innerContainer.InnerListForReading;
            if (inventory.Count > 0) {
                bool flag = false;
                QualityCategory higherQuality = QualityCategory.Awful;
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i] is Toy toy)
                    {
                        flag = true;
                        if(toy.cachedQuality.Quality > higherQuality)
                        {
                            higherQuality=toy.cachedQuality.Quality;
                        }
                    }
                
                }
                if (flag) { 
                    return ThoughtState.ActiveAtStage((int)higherQuality);

                }

                
            }
           

            return false;
        }

       
    }

    public class ThoughtWorker_Toys_Teenager : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!p.Spawned)
                return false;

            if (p.ageTracker.AgeBiologicalYears < 13 || p.ageTracker.AgeBiologicalYears >= 18)
            {
                return false;
            }

            List<Thing> inventory = p.inventory?.innerContainer.InnerListForReading;
            if (inventory.Count > 0)
            {
                bool flag = false;
                QualityCategory higherQuality = QualityCategory.Awful;
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i] is Toy toy)
                    {
                        flag = true;
                        if (toy.cachedQuality.Quality > higherQuality)
                        {
                            higherQuality = toy.cachedQuality.Quality;
                        }
                    }

                }
                if (flag)
                {
                    return ThoughtState.ActiveAtStage((int)higherQuality);

                }


            }


            return false;
        }


    }
}