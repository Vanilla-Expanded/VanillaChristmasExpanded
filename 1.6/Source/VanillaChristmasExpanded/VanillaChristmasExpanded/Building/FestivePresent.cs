using Verse.Sound;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

using Verse.Noise;
using UnityEngine.Diagnostics;
using System;
using System.Linq;

namespace VanillaChristmasExpanded
{
	public class FestivePresent : Building, IOpenable
	{

		public CompQuality compQuality;
		public Graphic_Single cachedGraphic = null;
		public GraphicsByQualityExtension cachedGraphicsExtension;
		public string cachedGraphicPath="";

		public override Graphic Graphic
		{
			get
			{
				if (cachedGraphicsExtension is null)
				{
					cachedGraphicsExtension = this.def.GetModExtension<GraphicsByQualityExtension>();
				}
				if (cachedGraphic is null)
				{
					StoreGraphics();
				}
				return cachedGraphic;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();

			Scribe_Values.Look(ref this.cachedGraphicPath, "cachedGraphicPath", "", false);
		  

		}

		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			compQuality = this.TryGetComp<CompQuality>();
			cachedGraphicsExtension = this.def.GetModExtension<GraphicsByQualityExtension>();
			if (cachedGraphicsExtension != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate { StoreGraphics(); });
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		public void StoreGraphics()
		{
			if (compQuality is null)
			{
				compQuality = this.TryGetComp<CompQuality>();
			}
			QualityCategory quality = compQuality.Quality;

			if (cachedGraphicPath == "")
			{
				cachedGraphicPath = cachedGraphicsExtension.graphics.Where(x => x.quality == quality).First().texturePaths.RandomElement();
			}
			cachedGraphic = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>(cachedGraphicPath, ShaderDatabase.Cutout,
					 this.def.graphicData.drawSize, Color.white);
			
		}

		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
		   
			if (cachedGraphicPath != "")
			{
				var vector = DrawPos + Altitudes.AltIncVect;
			   
					vector.y += Altitudes.AltInc;
					Vector3 vectorOffset = Vector3.zero;

				cachedGraphic.DrawFromDef(vector + vectorOffset, Rot4.North, null);
				
			}
		}
		

		public float PriceByQuality(QualityCategory quality)
		{
			switch(quality){

				case QualityCategory.Awful:
					return 50;
				case QualityCategory.Poor:
					return 150;
				case QualityCategory.Normal:
					return 300;
				case QualityCategory.Good:
					return 500;
				case QualityCategory.Excellent:
					return 800;
				case QualityCategory.Masterwork:
					return 1500;
				case QualityCategory.Legendary:
					return 5000;
 
			}
			return 300;


		}

		public ThingSetMakerDef ThingSetMakerByQuality(QualityCategory quality)
		{
			switch (quality)
			{

				case QualityCategory.Awful:
					return InternalDefOf.VCE_Reward_Resources;
				case QualityCategory.Poor:
					return InternalDefOf.VCE_Reward_Resources;
				case QualityCategory.Normal:
					return InternalDefOf.VCE_Reward_Anything;
				case QualityCategory.Good:
					return InternalDefOf.VCE_Reward_Anything;
				case QualityCategory.Excellent:
					return InternalDefOf.VCE_Reward_Anything;
				case QualityCategory.Masterwork:
					return InternalDefOf.VCE_Reward_Anything;
				case QualityCategory.Legendary:
					return InternalDefOf.VCE_Reward_Anything;

			}
			return ThingSetMakerDefOf.Reward_ItemsStandard;


		}

		public IntRange AmountByQuality(QualityCategory quality)
		{
			switch (quality)
			{

				case QualityCategory.Awful:
					return new IntRange(1, 5);
				case QualityCategory.Poor:
					return new IntRange(1, 5);
				case QualityCategory.Normal:
					return new IntRange(1, 5);
				case QualityCategory.Good:
					return new IntRange(1, 5);
				case QualityCategory.Excellent:
					return new IntRange(1, 5);
				case QualityCategory.Masterwork:
					return new IntRange(1, 1);
				case QualityCategory.Legendary:
					return new IntRange(1,1);

			}
			return  new IntRange(1, 5); 


		}

		public virtual int OpenTicks => 300;

		public virtual bool CanOpen => true;

		public virtual void Open()
		{
			QualityCategory quality = this.GetComp<CompQuality>().Quality;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			float price = PriceByQuality(quality);
			parms.totalMarketValueRange = new FloatRange(price*0.9f,price);
			parms.minSingleItemMarketValuePct = 0;
			parms.allowNonStackableDuplicates = true;
			parms.countRange = AmountByQuality(quality);
			List<Thing> list2 = ThingSetMakerByQuality(quality).root.Generate(parms);

		  
			if (list2 != null)
			{
				List<string> listToDisplay = new List<string>();

				foreach(Thing t in list2)
				{
					listToDisplay.Add(t.stackCount.ToString() + "x " + t.def.LabelCap);
				}

				Messages.Message("VCE_OpenedPresent".Translate(listToDisplay.ToStringSafeEnumerable()), MessageTypeDefOf.PositiveEvent, true);


				foreach (Thing thing in list2)
				{
				   
				  
					GenPlace.TryPlaceThing(thing, Position, Map, ThingPlaceMode.Near);


				}
				
				if (this.Spawned)
				{
					this.Destroy();
				}

			}


		}



	}
}