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
		public static SoundDef VCE_RitualSustainer_Carols;

		public static ThingDef VCE_FestivePresent;

		public static StorytellerDef VCE_SantaSeasonal;
	}
}