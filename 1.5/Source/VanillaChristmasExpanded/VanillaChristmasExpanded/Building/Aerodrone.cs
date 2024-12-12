using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaChristmasExpanded
{
	public class Aerodrone : Building
	{
		public FloatRange presentsToEject;
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				var amount = presentsToEject.RandomInRange;
				for (int i = 0; i < amount; i++)
				{
					IntVec3 randomCell = GenAdj.OccupiedRect(this).RandomCell;
					if (RCellFinder.TryFindRandomCellNearWith(Position, (IntVec3 c) => !c.Fogged(Map) && c.Walkable(Map) && !c.Impassable(Map), Map, out IntVec3 result, 13, 60))
					{
						var projectile = (Projectile_SpawnPresent)GenSpawn.Spawn(InternalDefOf.VCE_FestivePresentProjectile, randomCell, Map);
						projectile.present = (FestivePresent)ThingMaker.MakeThing(InternalDefOf.VCE_FestivePresent);
						QualityCategory qualityPresent = QualityUtility.GenerateQualityRandomEqualChance();
						projectile.present.TryGetComp<CompQuality>().SetQuality(qualityPresent, null);
						projectile.Launch(this, result, result, ProjectileHitFlags.None, false, null);
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref presentsToEject, "presentsToEject");
		}
	}

	public class Projectile_SpawnPresent : Projectile
	{
		public override Graphic Graphic => present.Graphic;

        public override Material DrawMat => Graphic.MatSingleFor(this);
        public FestivePresent present;
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = this.Map;
			IntVec3 loc = base.Position;
			if (def.projectile.tryAdjacentFreeSpaces && base.Position.GetFirstBuilding(map) != null)
			{
				foreach (IntVec3 item in GenAdjFast.AdjacentCells8Way(base.Position))
				{
					if (item.GetFirstBuilding(map) == null && item.Standable(map))
					{
						loc = item;
						break;
					}
				}
			}
			GenSpawn.Spawn(present, loc, map);
			present.SetFaction(Faction.OfPlayer);
			Destroy();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look(ref present, "present");
		}
	}
}