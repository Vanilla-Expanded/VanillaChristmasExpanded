
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
namespace VanillaChristmasExpanded
{
    public class JobGiver_GiveSilentSpeechFacingTarget : ThinkNode_JobGiver
    {
     

        public bool faceSpectatorsIfPossible;

        public bool showSpeechBubbles = true;

        protected override Job TryGiveJob(Pawn pawn)
        {
            PawnDuty duty = pawn.mindState.duty;
            if (duty == null)
            {
                return null;
            }
            IntVec3 result = pawn.Position;
            if (!pawn.CanReserve(pawn.Position))
            {
                CellFinder.TryRandomClosewalkCellNear(result, pawn.Map, 2, out result, (IntVec3 c) => pawn.CanReserveAndReach(c, PathEndMode.OnCell, pawn.NormalMaxDanger()));
            }
            Rot4? rot = pawn.mindState?.duty?.overrideFacing;
            IntVec3 c2 = (rot.HasValue && rot.Value.IsValid) ? (result + rot.Value.FacingCell) : duty.spectateRect.CenterCell;
            LordJob_Ritual lordJob_Ritual = pawn.GetLord()?.LordJob as LordJob_Ritual;
            Job job = JobMaker.MakeJob(JobDefOf.GiveSpeech, result, c2);
            job.showSpeechBubbles = showSpeechBubbles;
            LordToil_Ritual lordToil_Ritual;
            if (lordJob_Ritual != null && (lordToil_Ritual = (lordJob_Ritual.lord.CurLordToil as LordToil_Ritual)) != null)
            {
                job.interaction = lordToil_Ritual.stage.BehaviorForRole(lordJob_Ritual.RoleFor(pawn).id).speakerInteraction;
            }
         
            job.speechFaceSpectatorsIfPossible = faceSpectatorsIfPossible;
            return job;
        }

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_GiveSilentSpeechFacingTarget obj = (JobGiver_GiveSilentSpeechFacingTarget)base.DeepCopy(resolve);
          
            obj.showSpeechBubbles = showSpeechBubbles;
            obj.faceSpectatorsIfPossible = faceSpectatorsIfPossible;
            return obj;
        }
    }
}