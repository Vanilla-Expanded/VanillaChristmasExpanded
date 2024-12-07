using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaChristmasExpanded
{
	[HarmonyPatch(typeof(RewardsGenerator), "DoGenerate")]
	public static class RewardsGenerator_GenerateRewards_Patch
	{
		public static void Prefix(ref RewardsGeneratorParams parms, out Reward_FestiveFavor __state)
		{
			if (FestiveFavorManager.Active && !parms.thingRewardDisallowed && !parms.thingRewardItemsOnly)
			{
				__state = new Reward_FestiveFavor();
				__state.InitFromValue(parms.rewardValue, parms, out var value);
				parms.rewardValue -= value;
				if (value == 0f) __state = null;
			}
			else __state = null;
		}

		public static void Postfix(ref List<Reward> __result, Reward_FestiveFavor __state)
		{
			if (__state != null) __result.Add(__state);
		}
	}
}
