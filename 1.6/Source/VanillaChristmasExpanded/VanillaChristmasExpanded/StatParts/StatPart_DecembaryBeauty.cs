
using RimWorld;
using Verse;

namespace VanillaChristmasExpanded
{
    public class StatPart_DecembaryBeauty : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {          
            if (req.Thing?.Stuff == InternalDefOf.VCE_Gingerbread && Utils.IsDecembary(req.Thing.Tile))
            {
                val *= 2;
            }
                           
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing?.Stuff == InternalDefOf.VCE_Gingerbread && Utils.IsDecembary(req.Thing.Tile))
            {
                return "VCE_GingerbreadInDecembary".Translate();
            }           
            return null;
        }
    }
}
