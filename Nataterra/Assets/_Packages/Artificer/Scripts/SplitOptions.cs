using UnityEngine;
using UnityEngine.Splines;

namespace Artifice
{
	[System.Serializable]
	public class Overridable<T>
	{
		public bool	overrideState = false;
		public T	value;

		public Overridable(T defaultValue, bool state = false)
		{
			value			= defaultValue;
			overrideState	= state;
		}
	}

	[System.Serializable]
	public class OverridableRange<T>
	{
		public bool overrideState = false;
		public T	value;

		public OverridableRange(T defaultValue, bool state = false)
		{
			value			= defaultValue;
			overrideState	= state;
		}
	}

	[System.Serializable]
	public class SplitOptions
	{
		public OverridableRange<FloatRange>		buildDistRange		= new OverridableRange<FloatRange>(new FloatRange(10.0f));
		public Overridable<Vector3>				origin				= new Overridable<Vector3>(Vector3.zero);
		public Overridable<Vector3>				projection			= new Overridable<Vector3>(Vector3.one);
		public Overridable<Vector3>				sortOrigin			= new Overridable<Vector3>(Vector3.zero);
		public Overridable<bool>				useSortSpline		= new Overridable<bool>(false);
		public Overridable<float>				sortPathBias		= new Overridable<float>(100.0f);
		public Overridable<SplineContainer>		sortSpline			= new Overridable<SplineContainer>(null);
		public Overridable<SplitMode>			splitMode			= new Overridable<SplitMode>(SplitMode.Elements);
		public Overridable<BuildStyle>			buildStyle			= new Overridable<BuildStyle>(BuildStyle.Appear);
		public Overridable<DismantleStyle>		dismantleStyle		= new Overridable<DismantleStyle>(DismantleStyle.Vanish);
		public Overridable<bool>				usePlaceCurve		= new Overridable<bool>(false);
		public Overridable<AnimationCurve>		placeCurve			= new Overridable<AnimationCurve>(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		public Overridable<bool>				useRotCurve			= new Overridable<bool>(false);
		public Overridable<AnimationCurve>		placeRotCurve		= new Overridable<AnimationCurve>(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		public Overridable<bool>				useScaleCurve		= new Overridable<bool>(false);
		public Overridable<AnimationCurve>		placeScaleCurve		= new Overridable<AnimationCurve>(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		public Overridable<bool>				perAxisScale		= new Overridable<bool>(false);
		public Overridable<AnimationCurve>		placeScaleCurveY	= new Overridable<AnimationCurve>(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		public Overridable<AnimationCurve>		placeScaleCurveZ	= new Overridable<AnimationCurve>(new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)));
		public Overridable<float>				maxScale			= new Overridable<float>(1.0f);
		public Overridable<MeshPivotMode>		meshPivot			= new Overridable<MeshPivotMode>(MeshPivotMode.Object);
		//public Overridable<Vector3>				rotate				= new Overridable<Vector3>(Vector3.zero);
		public OverridableRange<Vector3Range>	rotateRange			= new OverridableRange<Vector3Range>(new Vector3Range(Vector3.zero));
		public Overridable<SortMode>			sortMode			= new Overridable<SortMode>(SortMode.Position);
		public Overridable<SortDistanceMode>	sortDistanceMode	= new Overridable<SortDistanceMode>(SortDistanceMode.Closest);
		public Overridable<bool>				dontSplit			= new Overridable<bool>(false);
		public Overridable<PlaceMode>			placeMode			= new Overridable<PlaceMode>(PlaceMode.Time);
		public OverridableRange<FloatRange>		placeTimeRange		= new OverridableRange<FloatRange>(new FloatRange(1.0f));
		public Overridable<bool>				dontAdd				= new Overridable<bool>(false);
		public Overridable<bool>				addAll				= new Overridable<bool>(false);
		public Overridable<float>				sortModifier		= new Overridable<float>(0.0f);
		public bool								buildFromOverride	= false;
		public BuildFrom[]						buildFromObjects;	//	= new Overridable<BuildFrom[]>(null);
		public OverridableRange<FloatRange>		removeTimeRange		= new OverridableRange<FloatRange>(new FloatRange(1.0f));
		public Overridable<float>				minExplodeForce		= new Overridable<float>(1.0f);
		public Overridable<float>				maxExplodeForce		= new Overridable<float>(10.0f);
		public Overridable<Vector3>				dismantleRotate		= new Overridable<Vector3>(Vector3.zero);
		public Overridable<CollisionMode>		collisionMode		= new Overridable<CollisionMode>(CollisionMode.Raycast);
		public Overridable<LayerMask>			layers				= new Overridable<LayerMask>(0);
		public Overridable<float>				collisionY			= new Overridable<float>(0.0f);
		public Overridable<Vector3>				angVelRange			= new Overridable<Vector3>(new Vector3(180.0f, 180.0f, 180.0f));
		public Overridable<float>				gravityModifier		= new Overridable<float>(1.0f);
		public Overridable<Vector3>				dismantleProjection	= new Overridable<Vector3>(Vector3.one);
		[Range(0, 1)]
		public Overridable<float>				bounce				= new Overridable<float>(0.2f);
		public Overridable<float>				linearDrag			= new Overridable<float>(0.1f);
		public Overridable<float>				angularDrag			= new Overridable<float>(0.1f);
		public bool								showSplitOptions;
		public bool								showBuildOptions;
		public bool								showDismantleOptions;

		static public void Make(ref SplitOptions result, SplitOptions defaults, SplitOptions overrides)
		{
			result.buildDistRange.value			= overrides.buildDistRange.overrideState		? overrides.buildDistRange.value		: defaults.buildDistRange.value;
			result.origin.value					= overrides.origin.overrideState				? overrides.origin.value				: defaults.origin.value;
			result.projection.value				= overrides.projection.overrideState			? overrides.projection.value			: defaults.projection.value;
			result.sortOrigin.value				= overrides.sortOrigin.overrideState			? overrides.sortOrigin.value			: defaults.sortOrigin.value;
			result.useSortSpline.value			= overrides.useSortSpline.overrideState			? overrides.useSortSpline.value			: defaults.useSortSpline.value;
			result.sortSpline.value				= overrides.sortSpline.overrideState			? overrides.sortSpline.value			: defaults.sortSpline.value;
			result.sortPathBias.value			= overrides.sortPathBias.overrideState			? overrides.sortPathBias.value			: defaults.sortPathBias.value;
			result.splitMode.value				= overrides.splitMode.overrideState				? overrides.splitMode.value				: defaults.splitMode.value;
			result.buildStyle.value				= overrides.buildStyle.overrideState			? overrides.buildStyle.value			: defaults.buildStyle.value;
			result.dismantleStyle.value			= overrides.dismantleStyle.overrideState		? overrides.dismantleStyle.value		: defaults.dismantleStyle.value;
			result.usePlaceCurve.value			= overrides.usePlaceCurve.overrideState			? overrides.usePlaceCurve.value			: defaults.usePlaceCurve.value;
			result.placeCurve.value				= overrides.placeCurve.overrideState			? overrides.placeCurve.value			: defaults.placeCurve.value;
			result.useRotCurve.value			= overrides.useRotCurve.overrideState			? overrides.useRotCurve.value			: defaults.useRotCurve.value;
			result.placeRotCurve.value			= overrides.placeRotCurve.overrideState			? overrides.placeRotCurve.value			: defaults.placeRotCurve.value;
			result.useScaleCurve.value			= overrides.useScaleCurve.overrideState			? overrides.useScaleCurve.value			: defaults.useScaleCurve.value;
			result.placeScaleCurve.value		= overrides.placeScaleCurve.overrideState		? overrides.placeScaleCurve.value		: defaults.placeScaleCurve.value;
			result.perAxisScale.value			= overrides.perAxisScale.overrideState			? overrides.perAxisScale.value			: defaults.perAxisScale.value;
			result.placeScaleCurveY.value		= overrides.placeScaleCurveY.overrideState		? overrides.placeScaleCurveY.value		: defaults.placeScaleCurveY.value;
			result.placeScaleCurveZ.value		= overrides.placeScaleCurveZ.overrideState		? overrides.placeScaleCurveZ.value		: defaults.placeScaleCurveZ.value;
			result.maxScale.value				= overrides.maxScale.overrideState				? overrides.maxScale.value				: defaults.maxScale.value;
			result.meshPivot.value				= overrides.meshPivot.overrideState				? overrides.meshPivot.value				: defaults.meshPivot.value;
			//result.rotate.value					= overrides.rotate.overrideState				? overrides.rotate.value				: defaults.rotate.value;
			result.rotateRange.value			= overrides.rotateRange.overrideState			? overrides.rotateRange.value			: defaults.rotateRange.value;
			result.buildFromObjects				= overrides.buildFromOverride					? overrides.buildFromObjects			: defaults.buildFromObjects;
			result.sortMode.value				= overrides.sortMode.overrideState				? overrides.sortMode.value				: defaults.sortMode.value;
			result.sortDistanceMode.value		= overrides.sortDistanceMode.overrideState		? overrides.sortDistanceMode.value		: defaults.sortDistanceMode.value;
			result.dontSplit.value				= overrides.dontSplit.overrideState				? overrides.dontSplit.value				: defaults.dontSplit.value;
			result.placeMode.value				= overrides.placeMode.overrideState				? overrides.placeMode.value				: defaults.placeMode.value;
			result.placeTimeRange.value			= overrides.placeTimeRange.overrideState		? overrides.placeTimeRange.value		: defaults.placeTimeRange.value;
			result.removeTimeRange.value		= overrides.removeTimeRange.overrideState		? overrides.removeTimeRange.value		: defaults.removeTimeRange.value;
			result.dontAdd.value				= overrides.dontAdd.overrideState				? overrides.dontAdd.value				: defaults.dontAdd.value;
			result.addAll.value					= overrides.addAll.overrideState				? overrides.addAll.value				: defaults.addAll.value;
			result.sortModifier.value			= overrides.sortModifier.overrideState			? overrides.sortModifier.value			: defaults.sortModifier.value;
			result.minExplodeForce.value		= overrides.minExplodeForce.overrideState		? overrides.minExplodeForce.value		: defaults.minExplodeForce.value;
			result.maxExplodeForce.value		= overrides.maxExplodeForce.overrideState		? overrides.maxExplodeForce.value		: defaults.maxExplodeForce.value;
			result.dismantleRotate.value		= overrides.dismantleRotate.overrideState		? overrides.dismantleRotate.value		: defaults.dismantleRotate.value;
			result.collisionMode.value			= overrides.collisionMode.overrideState			? overrides.collisionMode.value			: defaults.collisionMode.value;
			result.layers.value					= overrides.layers.overrideState				? overrides.layers.value				: defaults.layers.value;
			result.collisionY.value				= overrides.collisionY.overrideState			? overrides.collisionY.value			: defaults.collisionY.value;
			result.angVelRange.value			= overrides.angVelRange.overrideState			? overrides.angVelRange.value			: defaults.angVelRange.value;
			result.gravityModifier.value		= overrides.gravityModifier.overrideState		? overrides.gravityModifier.value		: defaults.gravityModifier.value;
			result.dismantleProjection.value	= overrides.dismantleProjection.overrideState	? overrides.dismantleProjection.value	: defaults.dismantleProjection.value;
			result.bounce.value					= overrides.bounce.overrideState				? overrides.bounce.value				: defaults.bounce.value;
			result.linearDrag.value				= overrides.linearDrag.overrideState			? overrides.linearDrag.value			: defaults.linearDrag.value;
			result.angularDrag.value			= overrides.angularDrag.overrideState			? overrides.angularDrag.value			: defaults.angularDrag.value;

			result.buildDistRange.overrideState			= overrides.buildDistRange.overrideState;
			result.placeTimeRange.overrideState			= overrides.placeTimeRange.overrideState;
			result.origin.overrideState					= overrides.origin.overrideState;
			result.projection.overrideState				= overrides.projection.overrideState;
			result.sortOrigin.overrideState				= overrides.sortOrigin.overrideState;
			result.useSortSpline.overrideState			= overrides.useSortSpline.overrideState;
			result.sortSpline.overrideState				= overrides.sortSpline.overrideState;
			result.sortPathBias.overrideState			= overrides.sortPathBias.overrideState;
			result.splitMode.overrideState				= overrides.splitMode.overrideState;
			result.buildStyle.overrideState				= overrides.buildStyle.overrideState;
			result.dismantleStyle.overrideState			= overrides.dismantleStyle.overrideState;
			result.usePlaceCurve.overrideState			= overrides.usePlaceCurve.overrideState;
			result.placeCurve.overrideState				= overrides.placeCurve.overrideState;
			result.useRotCurve.overrideState			= overrides.useRotCurve.overrideState;
			result.placeRotCurve.overrideState			= overrides.placeRotCurve.overrideState;
			result.useScaleCurve.overrideState			= overrides.useScaleCurve.overrideState;
			result.placeScaleCurve.overrideState		= overrides.placeScaleCurve.overrideState;
			result.perAxisScale.overrideState			= overrides.perAxisScale.overrideState;
			result.placeScaleCurveY.overrideState		= overrides.placeScaleCurveY.overrideState;
			result.placeScaleCurveZ.overrideState		= overrides.placeScaleCurveZ.overrideState;
			result.maxScale.overrideState				= overrides.maxScale.overrideState;
			result.meshPivot.overrideState				= overrides.meshPivot.overrideState;
			//result.rotate.overrideState					= overrides.rotate.overrideState;
			result.rotateRange.overrideState			= overrides.rotateRange.overrideState;
			result.sortMode.overrideState				= overrides.sortMode.overrideState;
			result.sortDistanceMode.overrideState		= overrides.sortDistanceMode.overrideState;
			result.dontSplit.overrideState				= overrides.dontSplit.overrideState;
			result.placeMode.overrideState				= overrides.placeMode.overrideState;
			result.dontAdd.overrideState				= overrides.dontAdd.overrideState;
			result.addAll.overrideState					= overrides.addAll.overrideState;
			result.sortModifier.overrideState			= overrides.sortModifier.overrideState;
			result.buildFromObjects						= overrides.buildFromObjects;
			result.buildFromOverride					= overrides.buildFromOverride;
			result.removeTimeRange.overrideState		= overrides.removeTimeRange.overrideState;
			result.minExplodeForce.overrideState		= overrides.minExplodeForce.overrideState;
			result.maxExplodeForce.overrideState		= overrides.maxExplodeForce.overrideState;	
			result.dismantleRotate.overrideState		= overrides.dismantleRotate.overrideState;	
			result.collisionMode.overrideState			= overrides.collisionMode.overrideState;	
			result.layers.overrideState					= overrides.layers.overrideState;				
			result.collisionY.overrideState				= overrides.collisionY.overrideState;			
			result.angVelRange.overrideState			= overrides.angVelRange.overrideState;		
			result.gravityModifier.overrideState		= overrides.gravityModifier.overrideState;	
			result.dismantleProjection.overrideState	= overrides.dismantleProjection.overrideState;
			result.bounce.overrideState					= overrides.bounce.overrideState;				
			result.linearDrag.overrideState				= overrides.linearDrag.overrideState;			
			result.angularDrag.overrideState			= overrides.angularDrag.overrideState;		
		}

		public void Copy(MeshElement me, Artificer a)
		{
			// copy what we need to the mesh element
			me.origin				= origin.value;
			me.projection			= projection.value;
			me.sortOrigin			= sortOrigin.value;
			me.buildDist			= buildDistRange.value.GetValue(a);
			me.usePlaceCurve		= usePlaceCurve.value;
			me.placeCurve			= placeCurve.value;
			me.useRotCurve			= useRotCurve.value;
			me.placeRotCurve		= placeRotCurve.value;
			me.useScaleCurve		= useScaleCurve.value;
			me.placeScaleCurve		= placeScaleCurve.value;
			me.perAxisScale			= perAxisScale.value;
			me.placeScaleCurveY		= placeScaleCurveY.value;
			me.placeScaleCurveZ		= placeScaleCurveZ.value;
			me.maxScale				= maxScale.value;
			me.rotate				= rotateRange.value.GetValue(a);
			me.buildStyle			= buildStyle.value;
			me.dismantleStyle		= dismantleStyle.value;
			me.placeMode			= placeMode.value;
			me.placeTime			= placeTimeRange.value.GetValue(a);
			me.removeTime			= removeTimeRange.value.GetValue(a);
			me.minExplodeForce		= minExplodeForce.value;
			me.maxExplodeForce		= maxExplodeForce.value;
			me.dismantleRotate		= dismantleRotate.value;
			me.collisionMode		= collisionMode.value;
			me.layers				= layers.value;
			me.collisionY			= collisionY.value;
			me.angVelRange			= angVelRange.value;
			me.gravityModifier		= gravityModifier.value;
			me.dismantleProjection	= dismantleProjection.value;
			me.bounce				= bounce.value;
			me.linearDrag			= linearDrag.value;
			me.angularDrag			= angularDrag.value;
		}

		public void SetDefault(Artificer artificer)
		{
			buildDistRange.value		= artificer.buildDistRange;
			origin.value				= artificer.origin;
			projection.value			= artificer.projection;
			sortOrigin.value			= artificer.sortOrigin;
			useSortSpline.value			= artificer.useSortSpline;
			sortSpline.value			= artificer.sortSpline;
			sortPathBias.value			= artificer.sortPathBias;
			splitMode.value				= artificer.splitMode;
			buildStyle.value			= artificer.buildStyle;
			dismantleStyle.value		= artificer.dismantleStyle;
			usePlaceCurve.value			= artificer.usePlaceCurve;
			placeCurve.value			= artificer.placeCurve;
			useRotCurve.value			= artificer.useRotCurve;
			placeRotCurve.value			= artificer.placeRotCurve;
			useScaleCurve.value			= artificer.useScaleCurve;
			placeScaleCurve.value		= artificer.placeScaleCurve;
			perAxisScale.value			= artificer.perAxisScale;
			placeScaleCurveY.value		= artificer.placeScaleCurveY;
			placeScaleCurveZ.value		= artificer.placeScaleCurveZ;
			maxScale.value				= artificer.maxScale;
			meshPivot.value				= artificer.meshPivot;
			//rotate.value				= artificer.rotate;
			rotateRange.value			= artificer.rotateRange;
			buildFromObjects			= artificer.buildFromObjects;
			sortMode.value				= artificer.sortMode;
			sortDistanceMode.value		= artificer.sortDistanceMode;
			dontSplit.value				= artificer.dontSplit;
			placeMode.value				= artificer.placeMode;
			placeTimeRange.value		= artificer.placeTimeRange;
			dontAdd.value				= artificer.dontAdd;
			addAll.value				= artificer.addAll;
			sortModifier.value			= artificer.sortModifier;
			removeTimeRange.value		= artificer.removeTimeRange;
			minExplodeForce.value		= artificer.minExplodeForce;
			maxExplodeForce.value		= artificer.maxExplodeForce; 
			dismantleRotate.value		= artificer.dismantleRotate; 
			collisionMode.value			= artificer.collisionMode; 
			layers.value				= artificer.layers; 
			collisionY.value			= artificer.collisionY; 
			angVelRange.value			= artificer.angVelRange;
			gravityModifier.value		= artificer.gravityModifier; 
			dismantleProjection.value	= artificer.dismantleProjection;
			bounce.value				= artificer.bounce; 
			linearDrag.value			= artificer.linearDrag; 
			angularDrag.value			= artificer.angularDrag; 
		}
	}
}