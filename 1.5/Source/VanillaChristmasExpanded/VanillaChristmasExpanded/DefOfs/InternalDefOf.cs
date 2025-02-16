﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaChristmasExpanded
{

	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

		public static EffecterDef VCE_Confetti;
	
		public static SoundDef VCE_ConfettiExplosion;
        [MayRequireIdeology]
        public static SoundDef VCE_RitualSustainer_Carols;

        [MayRequireIdeology]
        public static HistoryEventDef CharityFulfilled_HospitalityRefugees;

        [MayRequireIdeology]
        public static JobDef VCE_EatAtFestiveFeast;

        public static ThoughtDef AteLavishMeal;

        public static ThingDef VCE_FestivePresent;
        public static ThingDef VCE_FestiveCracker;
        public static ThingDef VCE_GingerRoot;
        public static ThingDef VCE_Gingerbread;
        public static ThingDef VCE_Snow;
        public static ThingDef VCE_Filth_Water;
        public static ThingDef VCE_SantaAerodrone_Crashed;
        public static ThingDef VCE_FestiveJukebox;
        public static ThingDef VCE_SnowSculpture;

        public static StorytellerDef VCE_SantaSeasonal;

		public static ThingSetMakerDef VCE_Reward_Resources;
        public static ThingSetMakerDef VCE_Reward_Anything;

		public static InteractionDef KindWords;
        [MayRequire("VanillaExpanded.VanillaSocialInteractionsExpanded")]
        public static InteractionDef VSIE_Vent;
        [MayRequire("OskarPotocki.VFE.Empire")]
        public static InteractionDef VFEE_RoyalGossip;

        [MayRequireIdeology]
        public static PreceptDef FuneralNoCorpse;
        [MayRequireIdeology]
        public static PreceptDef Festival;
        [MayRequireIdeology]
        public static PreceptDef Classic_DanceParty;
        [MayRequireIdeology]
        public static PreceptDef DateRitualConsumable;
        [MayRequireIdeology]
        public static PreceptDef LeaderSpeech;
        [MayRequireIdeology]
        public static PreceptDef TreeConnection;
        [MayRequire("OskarPotocki.VFE.Empire")]
        public static PreceptDef VFEE_RoyalAddress;

        public static WeatherDef VCE_FestiveSnow;

        public static GameConditionDef VCE_ColdSnap;

        public static TransportShipDef VCE_SantaAerodroneTransportShipDef;
        public static ThingDef VCE_SantaAerodrone_Crashing;
        public static ThingDef VCE_FestivePresentProjectile;
    }
}