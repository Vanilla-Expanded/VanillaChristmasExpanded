using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace VanillaChristmasExpanded
{
	[HarmonyPatch(typeof(QuestGen_Rewards), "GiveRewards")]
	public static class QuestGen_Rewards_GiveRewards_Patch
	{
		public static void Prefix(out float __state, RewardsGeneratorParams parms, bool? useDifficultyFactor = null)
		{
			Slate slate = QuestGen.slate;
			RewardsGeneratorParams parmsResolved = parms;
			__state = ((parmsResolved.rewardValue == 0f) ? slate.Get("rewardValue", 0f) : parmsResolved.rewardValue);
			if (useDifficultyFactor ?? true)
			{
				parmsResolved.rewardValue *= Find.Storyteller.difficulty.EffectiveQuestRewardValueFactor;
				parmsResolved.rewardValue = Math.Max(1f, parmsResolved.rewardValue);
			}
		}
		
		public static void Postfix(QuestPart_Choice __result, RewardsGeneratorParams parms, float __state)
		{
			if (FestiveFavorManager.Active && __state > 0)
			{
				var reward = new Reward_FestiveFavor();
				RewardsGeneratorParams parmsResolved = parms;
				parmsResolved.rewardValue = __state;
				reward.InitFromValue(parmsResolved.rewardValue, parms, out var value);
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.rewards.Add(reward);
				__result.choices.Add(choice);
				foreach (QuestPart item in reward.GenerateQuestParts(0, parms, null, null, null, null))
				{
					QuestGen.quest.AddPart(item);
					choice.questParts.Add(item);
				}
			}
		}
	}
}
