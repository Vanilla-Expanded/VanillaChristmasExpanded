using RimWorld;
using Verse;

namespace VanillaChristmasExpanded
{
    public class QuestPart_GiveFestiveFavor : QuestPart
    {
        private int favorAmount;
        private string inSignal;

        public QuestPart_GiveFestiveFavor() { }

        public QuestPart_GiveFestiveFavor(string inSignal, int favorAmount)
        {
            this.inSignal = inSignal;
            this.favorAmount = favorAmount;
        }

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag == inSignal)
            {
                if (favorAmount > 0)
                {
                    FestiveFavorManager.Instance.AddFestiveFavor(favorAmount);
                }
                favorAmount = 0;
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            favorAmount = 0;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, nameof(inSignal));
            Scribe_Values.Look(ref favorAmount, nameof(favorAmount));
        }
    }
}
