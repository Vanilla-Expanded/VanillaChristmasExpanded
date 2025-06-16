using System.Linq;
using RimWorld;
using Verse;
namespace VanillaChristmasExpanded
{
    public class GameCondition_FestiveCold : GameCondition
    {
        private const float MaxTempOffset = -20f;

        public override int TransitionTicks => 300000;

        public override float TemperatureOffset()
        {
            return GameConditionUtility.LerpInOutValue(this, TransitionTicks, MaxTempOffset);
        }

        public override WeatherDef ForcedWeather()
        {
            Map map = base.AffectedMaps.FirstOrDefault();
            if (map != null)
            {
               
                    return InternalDefOf.VCE_FestiveSnow;
         
        
            }
            return null;
        }
    }
}
