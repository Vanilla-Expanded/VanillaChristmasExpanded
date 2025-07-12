using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaChristmasExpanded
{
	[HotSwappable]
	public class Aerodrone : Building
	{
		public FloatRange presentsToEject;
		private int ticksSinceSpawn;
		private const float MAX_RADIUS = 100f;
		private const int TOTAL_HOURS = 24;
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				ticksSinceSpawn = 0;
				var amount = presentsToEject.RandomInRange;
				for (int i = 0; i < amount; i++)
				{
					IntVec3 randomCell = this.OccupiedRect().RandomCell;
					if (RCellFinder.TryFindRandomCellNearWith(Position, (IntVec3 c) => !c.Fogged(Map) && c.Walkable(Map) && !c.Impassable(Map), Map, out IntVec3 result, 13, 60))
					{
						var projectile = (Projectile_SpawnPresent)GenSpawn.Spawn(InternalDefOf.VCE_FestivePresentProjectile, randomCell, Map);
						projectile.present = (FestivePresent)ThingMaker.MakeThing(InternalDefOf.VCE_FestivePresent);
						QualityCategory qualityPresent = QualityUtility.GenerateQualityRandomEqualChance();
						projectile.present.compQuality.SetQuality(qualityPresent, null);
						projectile.Launch(this, result, result, ProjectileHitFlags.None, false, null);
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref presentsToEject, "presentsToEject");
			Scribe_Values.Look(ref ticksSinceSpawn, "ticksSinceSpawn", 0);
		}

		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (Spawned)
			{
				ticksSinceSpawn += delta;
				if (this.IsHashIntervalTick(60, delta))
				{
					SpreadSnow();
				}
			}
		}

		private void SpreadSnow()
		{
			float hoursElapsed = (float)ticksSinceSpawn / 2500f;
			float currentRadius = Mathf.Min(MAX_RADIUS, (hoursElapsed / (float)TOTAL_HOURS) * MAX_RADIUS);

			IEnumerable<IntVec3> cellsInRadius = GenRadial.RadialCellsAround(Position, currentRadius, true);
			foreach (IntVec3 cell in cellsInRadius)
			{
				if (cell.InBounds(Map))
				{
					float distanceFromCenter = cell.DistanceTo(Position);
					if (distanceFromCenter <= currentRadius)
					{
						Map.snowGrid.AddDepth(cell, 1f);
					}
				}
			}
		}

		public override string GetInspectString()
		{
			var baseString = new StringBuilder(base.GetInspectString());
			baseString.AppendLine("VCE_SnowSpreadRadius".Translate(MAX_RADIUS.ToString()));
			return baseString.ToString().TrimEndNewlines();
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