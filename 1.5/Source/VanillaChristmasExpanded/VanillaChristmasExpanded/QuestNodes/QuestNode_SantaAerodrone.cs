
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;
using VanillaChristmasExpanded;
using Verse;

namespace VanillaChristmasExpanded
{
	public class QuestNode_SantaAerodrone : QuestNode
	{
		private const string QuestTag = "VCE_SantaAerodrone";

		private const int TicksToShuttleArrival = 180;

		private static IntRange RaidDelayTicksRange = new IntRange(18000, 30000);

		private static IntRange RaidIntervalTicksRange = new IntRange(17500, 22500);

		private static float MinRaidThreatPointsFactor = 0.3f;

		private static float MaxRaidThreatPointsFactor = 0.55f;

		private static int AerodroneDestroyDelayTicks = 120000;

		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap();
			float num = slate.Get("points", 0f);
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			TryFindShuttleCrashPosition(map, InternalDefOf.VCE_SantaAerodrone_Crashed.size, out var landingSpot);
			var aeroDrone = ThingMaker.MakeThing(InternalDefOf.VCE_SantaAerodrone_Crashed) as Aerodrone;
			switch (quest.challengeRating)
			{
				case 1:
					aeroDrone.presentsToEject = new FloatRange(3, 4);
					break;
				case 2:
					aeroDrone.presentsToEject = new FloatRange(3, 6);
					break;
				case 3:
					aeroDrone.presentsToEject = new FloatRange(4, 8);
					break;
				case 4:
					aeroDrone.presentsToEject = new FloatRange(5, 10);
					break;
				case 5:
					aeroDrone.presentsToEject = new FloatRange(6, 12);
					break;
			}

			string text = QuestGen.GenerateNewSignal("RaidsDelay");
			string aeroDroneDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID("aeroDrone.Destroyed");
			string triggerRaidSignal = QuestGen.GenerateNewSignal("TriggerRaid");
			string text2 = QuestGen.GenerateNewSignal("QuestEndSuccess");
			string text3 = QuestGen.GenerateNewSignal("QuestEndFailure");
			string text4 = QuestGen.GenerateNewSignal("AerodroneDelayDestroy");
			quest.Delay(120, delegate
			{
				quest.Letter(LetterDefOf.NeutralEvent, null, null, null, null, useColonistsFromCaravanArg: false, QuestPart.SignalListenMode.OngoingOnly, label: "VCE_LetterLabelAerodroneIncoming".Translate(), text: "VCE_LetterTextAerodroneIncoming".Translate(), lookTargets: Gen.YieldSingle(aeroDrone));
				SpawnAerodrone(quest, map, InternalDefOf.VCE_SantaAerodrone_Crashing, Gen.YieldSingle(aeroDrone), null, landingSpot);
			}, null, null, null, reactivatable: false, null, null, isQuestTimeout: false, null, null, null, tickHistorically: false, QuestPart.SignalListenMode.OngoingOnly, waitUntilPlayerHasHomeMap: true);
			if (allowViolentQuests)
			{
				TryFindEnemyFaction(out var enemyFaction);
				QuestPart_FactionGoodwillLocked questPart_FactionGoodwillLocked = new QuestPart_FactionGoodwillLocked();
				questPart_FactionGoodwillLocked.faction1 = Faction.OfPlayer;
				questPart_FactionGoodwillLocked.faction2 = enemyFaction;
				questPart_FactionGoodwillLocked.inSignalEnable = QuestGen.slate.Get<string>("inSignal");
				quest.AddPart(questPart_FactionGoodwillLocked);
				QuestPart_Delay questPart_Delay = new QuestPart_Delay();
				questPart_Delay.delayTicks = RaidDelayTicksRange.RandomInRange;
				questPart_Delay.alertLabel = "QuestPartRaidsDelay".Translate();
				questPart_Delay.alertExplanation = "QuestPartRaidsDelayDesc".Translate();
				questPart_Delay.ticksLeftAlertCritical = 60000;
				questPart_Delay.inSignalEnable = QuestGen.slate.Get<string>("inSignal");
				questPart_Delay.alertCulprits.Add(aeroDrone);
				questPart_Delay.isBad = true;
				questPart_Delay.outSignalsCompleted.Add(text);
				questPart_Delay.waitUntilPlayerHasHomeMap = true;
				quest.AddPart(questPart_Delay);
				quest.Signal(text, delegate
				{
					QuestPart_PassOutInterval part = new QuestPart_PassOutInterval
					{
						inSignalEnable = QuestGen.slate.Get<string>("inSignal"),
						outSignals = { triggerRaidSignal },
						inSignalsDisable = { aeroDroneDestroyedSignal },
						ticksInterval = RaidIntervalTicksRange
					};
					quest.AddPart(part);
				});
				QuestPart_RandomRaid questPart_RandomRaid = new QuestPart_RandomRaid();
				questPart_RandomRaid.inSignal = triggerRaidSignal;
				questPart_RandomRaid.mapParent = map.Parent;
				questPart_RandomRaid.pointsRange = new FloatRange(num * MinRaidThreatPointsFactor, num * MaxRaidThreatPointsFactor);
				questPart_RandomRaid.faction = enemyFaction;
				questPart_RandomRaid.arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				questPart_RandomRaid.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				questPart_RandomRaid.attackTargets = new List<Thing> { aeroDrone };
				questPart_RandomRaid.generateFightersOnly = true;
				questPart_RandomRaid.customLetterLabel = "Raid".Translate() + ": " + enemyFaction.Name;
				questPart_RandomRaid.customLetterText = "VCE_LetterTextRaidSpacedrone".Translate(enemyFaction.NameColored, aeroDrone.LabelCap).Resolve();
				questPart_RandomRaid.fallbackToPlayerHomeMap = true;
				quest.AddPart(questPart_RandomRaid);
				slate.Set("enemyFaction", enemyFaction);
				slate.Set("enemyFactionNeolithic", enemyFaction.def.techLevel == TechLevel.Neolithic);
			}
			Quest quest2 = quest;
			int aeroDroneDestroyDelayTicks = AerodroneDestroyDelayTicks;
			Action inner = delegate
			{
			};
			string expiryInfoPart = "VCE_AerodroneSelfDestructsIn".Translate();
			quest2.Delay(aeroDroneDestroyDelayTicks, inner, null, null, text4, reactivatable: false, null, null, isQuestTimeout: false, expiryInfoPart);
			quest.AnySignal(new List<string> { text4 }, delegate
			{
				quest.StartWick(aeroDrone);
			});

			quest.End(QuestEndOutcome.Fail, 0, null, text3, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			quest.End(QuestEndOutcome.Success, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
			slate.Set("map", map);
			slate.Set("aeroDrone", aeroDrone);
			slate.Set("raidIntervalAvg", RaidIntervalTicksRange.Average);
			slate.Set("allowViolence", allowViolentQuests);
			slate.Set("destroyDelayTicks", AerodroneDestroyDelayTicks);
		}

		private QuestPart_SpawnThing SpawnAerodrone(Quest quest, Map map, ThingDef skyfallerDef, IEnumerable<Thing> innerThings, Faction factionForSafeSpot = null, IntVec3? cell = null, string inSignal = null, bool lookForSafeSpot = false, bool tryLandInShipLandingZone = false, Thing tryLandNearThing = null, Pawn mapParentOfPawn = null)
		{
			Skyfaller thing = SkyfallerMaker.MakeSkyfaller(skyfallerDef, innerThings);
			QuestPart_SpawnAeroDrone questPart_SpawnAerodrone = new QuestPart_SpawnAeroDrone();
			questPart_SpawnAerodrone.thing = thing;
			questPart_SpawnAerodrone.mapParent = map?.Parent;
			questPart_SpawnAerodrone.mapParentOfPawn = mapParentOfPawn;
			questPart_SpawnAerodrone.factionForFindingSpot = factionForSafeSpot;
			if (cell.HasValue)
			{
				questPart_SpawnAerodrone.cell = cell.Value;
			}
			questPart_SpawnAerodrone.inSignal = inSignal ?? QuestGen.slate.Get<string>("inSignal");
			questPart_SpawnAerodrone.lookForSafeSpot = lookForSafeSpot;
			questPart_SpawnAerodrone.tryLandInShipLandingZone = tryLandInShipLandingZone;
			questPart_SpawnAerodrone.tryLandNearThing = tryLandNearThing;
			quest.AddPart(questPart_SpawnAerodrone);
			return questPart_SpawnAerodrone;
		}
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap();
			if (map == null)
			{
				return false;
			}

			if (!TryFindShuttleCrashPosition(map, InternalDefOf.VCE_SantaAerodrone_Crashed.size, out var _))
			{
				return false;
			}
			return true;
		}

		private bool TryFindShuttleCrashPosition(Map map, IntVec2 size, out IntVec3 spot)
		{
			if (DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, size))
			{
				return true;
			}
			return false;
		}

		private bool CanUseFaction(Faction f)
		{
			if (!f.temporary && !f.defeated && !f.IsPlayer && (f.def.humanlikeFaction || f == Faction.OfMechanoids) && (!f.Hidden || f == Faction.OfMechanoids))
			{
				return f.HostileTo(Faction.OfPlayer);
			}
			return false;
		}

		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			return Find.FactionManager.AllFactions.Where(CanUseFaction).TryRandomElement(out enemyFaction);
		}
	}
}