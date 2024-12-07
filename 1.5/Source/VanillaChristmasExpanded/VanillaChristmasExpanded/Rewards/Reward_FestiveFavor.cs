using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using Verse;
using UnityEngine;
using Verse.Grammar;

namespace VanillaChristmasExpanded
{
	public class Reward_FestiveFavor : Reward
	{
		public int favorAmount;
		private const float FAVOR_VALUE = 45f;

		private string Label => $"{"VCE_FestiveFavor_Label".Translate()} x{favorAmount}";

		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement(Label, FestiveFavorManager.FestiveFavorIcon, null, null);
			}
		}

		public override float TotalMarketValue => favorAmount * FAVOR_VALUE;

		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			favorAmount = Mathf.RoundToInt(rewardValue / FAVOR_VALUE);
			valueActuallyUsed = favorAmount * FAVOR_VALUE;
			valueActuallyUsed = Mathf.Min(valueActuallyUsed, parms.rewardValue);
			if (valueActuallyUsed < 0)
			{
				valueActuallyUsed = 0f;
			}
		}

		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms,
			string customLetterLabel, string customLetterText,
			RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			yield return new QuestPart_GiveFestiveFavor(QuestGen.slate.Get<string>("inSignal"), favorAmount);
		}

		public override string GetDescription(RewardsGeneratorParams parms) => Label;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref favorAmount, "favorAmount");
		}
	}
}
