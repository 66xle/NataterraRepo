using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Events;
using Unity.Mathematics;
using System;
using System.Collections;

// BHXDB7
// Have a set build state so can say instantly go to half built for example
// Option to clear build data and rebuild on start

namespace Artifice
{
	[System.Serializable]
	public class MaterialMap
	{
		public Material		from;
		public Material		to;
	}

	public class BuildQueue
	{
		public int			piece;
		public float		alpha;
		public float		calpha;
		public bool			build;
		public Vector3		velocity;
		public Vector3		angvel;
		public Vector3		force;
		public Vector3		rot;
		public Vector3		pos;
		public MeshElement	element;
		public Bounds		bounds;
		public bool			sleep;
	}

	[System.Serializable] public class ArtificerBuiltEvent : UnityEvent<Artificer>			{ }
	[System.Serializable] public class ArtificerPlacedEvent : UnityEvent<Artificer, int>	{ }
	[System.Serializable] public class ArtificerDismantledEvent : UnityEvent<Artificer>		{ }
	[System.Serializable] public class ArtificerDismantleEvent : UnityEvent<Artificer, int>	{ }
	[System.Serializable] public class ArtificerCompletedObjectEvent : UnityEvent<Artificer, GameObject> { }

	[System.Serializable]
	public class OnCommpleteBuild
	{
		[Range(0, 1)]
		public float		level	= 1.0f;
		public Artificer	build;
	}

	[System.Serializable]
	public class OnDismantleComplete
	{
		[Range(0, 1)]
		public float		level	= 1.0f;
		public Artificer	dismantle;
	}


	[AddComponentMenu("Artificer/Artificer")]
	public class Artificer : MonoBehaviour
	{
		public GameObject						target;
		public List<GameObject>					exclude					= new List<GameObject>();
		public FloatRange						buildDistRange			= new FloatRange(1.0f);
		public Vector3							origin;
		public Vector3							sortOrigin;
		public bool								useSortSpline			= false;
		public SplineContainer					sortSpline;
		public float							sortPathBias			= 100.0f;
		public Vector3							projection				= Vector3.one;
		public bool								useSplitParams			= false;
		public SplitMode						splitMode				= SplitMode.Elements;
		public BuildMode						buildMode				= BuildMode.Finished;
		public float							buildTime				= 4.0f;
		public float							dismantleTime			= 1.0f;
		public BuildStyle						buildStyle				= BuildStyle.Vertical;	//.Appear;
		public PlaceMode						placeMode				= PlaceMode.Time;
		public FloatRange						placeTimeRange			= new FloatRange(1.0f);
		public bool								usePlaceCurve			= false;
		public AnimationCurve					placeCurve				= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public bool								useRotCurve				= false;
		public AnimationCurve					placeRotCurve			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public bool								useScaleCurve			= false;
		public AnimationCurve					placeScaleCurve			= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve					placeScaleCurveY		= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve					placeScaleCurveZ		= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public bool								perAxisScale			= false;
		public bool								showPlacement;
		public Material							placeMaterial;			// shows where item should go, could use outline for this
		public float							buildLevel				= 0.0f;
		public float							maxScale				= 1.0f;
		public GameObject						buildClickPrefab;
		public bool								buildBit				= false;
		public int								buildIndex				= -1;
		public int								showingPart				= -1;
		public GameObject						clickHere;
		public Canvas							canvas;
		public bool								buildOnAwake			= false;
		public StartMode						startMode				= StartMode.AsIs;
		public ArtificerBuiltEvent				builtEvent;
		public ArtificerPlacedEvent				placedEvent;
		public ArtificerDismantleEvent			dismantleEvent;
		public ArtificerDismantledEvent			dismantledEvent;
		public ArtificerCompletedObjectEvent	completedObjectEvent;
		public OnCommpleteBuild[]				onCompleteBuildThese	= new OnCommpleteBuild[0];
		public OnDismantleComplete[]			onDismantleComplete		= new OnDismantleComplete[0];
		public GameObject[]						enableOnBuilt			= new GameObject[0];
		public GameObject[]						disableOnBuilt			= new GameObject[0];
		//public Artificer[]						onDismantled			= new Artificer[0];		   // have a build value to start, again could use manager for this
		[Range(0, 1)]
		public float							onDismantleLevel		= 0.0f;
		[Range(0, 1)]
		public float							buildProgress;
		public float							addAmount				= 0.0f;
		public float							buildTo					= 0.0f;
		public MaterialMap[]					materialRemap			= new MaterialMap[0];
		public MeshPivotMode					meshPivot				= MeshPivotMode.Center;
		public int								showStart;
		public int								showEnd;
		[Range(0, 1)]
		public float							testPlace;
		public Vector3							rotate					= Vector3.zero;
		public Vector3Range						rotateRange;
		public BuildFrom[]						buildFromObjects		= new BuildFrom[0];
		public BuildFromMode					buildFromMode			= BuildFromMode.Nearest;
		public SortMode							sortMode				= SortMode.Position;
		public SortDistanceMode					sortDistanceMode		= SortDistanceMode.Closest;
		public bool								reverseSortOrder		= false;
		public List<Material>					materialSortOrder		= new List<Material>();
		public BuildData						buildData;
		public bool								dontSplit				= false;
		public bool								dontAdd					= false;
		public bool								addAll					= false;
		public float							sortModifier			= 0.0f;
		public bool								usePosition				= true;
		public bool								useUV					= false;
		public bool								useUV2					= false;
		public bool								useNormal				= false;
		public bool								useColor				= false;
		public bool								useMaterial				= false;
		public float							positionTolerance		= 0.0001f;
		public float							uvTolerance				= 0.0001f;
		public float							normalTolerance			= 0.0001f;
		public float							colorTolerance			= 0.0001f;
		public string							placedEvents;
		public string							dismantledEvents;
		public DismantleStyle					dismantleStyle			= DismantleStyle.Radial;	//.Transform;
		public FloatRange						removeTimeRange			= new FloatRange(1.0f);
		public bool								useDisPlaceCurve		= false;
		public bool								useDisPlaceRotCurve		= false;
		public bool								useDisPlaceScaleCurve	= false;
		public AnimationCurve					disPlaceCurve			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve					disPlaceRotCurve		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve					disPlaceScaleCurve		= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public float							minExplodeForce			= 1.0f;
		public float							maxExplodeForce			= 10.0f;
		public Vector3							dismantleRotate;
		public bool								simpleCollision			= true;
		public CollisionMode					collisionMode			= CollisionMode.Raycast;
		public LayerMask						layers					= 1;
		public float							collisionY				= 0.0f;
		public Vector3							angVelRange				= new Vector3(180.0f, 180.0f, 180.0f);
		public float							gravityModifier			= 1.0f;
		public Vector3							dismantleProjection		= Vector3.one;
		[Range(0, 1)]
		public float							bounce					= 0.2f;
		public float							linearDrag				= 0.1f;
		public float							angularDrag				= 0.1f;
		public Vector3							explodeOrigin			= Vector3.zero;
		public bool								showDismantle			= false;
		public bool								showBuild				= false;
		public bool								showSplit				= false;
		public bool								showExtra				= false;
		public CustomBuild						customBuild;
		public CustomDismantle					customDismantle;
		public Placing							placing;
		public bool								rebuildNeeded			= false;
		public List<GameObject>					completedObjects		= new List<GameObject>();
		public bool								useUnscaledTime			= false;
		public bool								useBuildTimeCrv			= false;
		public AnimationCurve					buildTimeCrv			= new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public RotateMode						rotateMode				= RotateMode.Normal;
		public bool								handleParticles			= true;
		public bool								handleLights			= true;
		public bool								handleLines				= true;
		public bool								handleAudio				= true;
		public float							particleEnableSpeed		= 1.0f;
		public float							lightEnableSpeed		= 1.0f;
		public float							lineEnableSpeed			= 1.0f;
		public float							audioEnableSpeed		= 1.0f;
		[Range(0, 1)]
		public float							enableParticles			= 1.0f;
		[Range(0, 1)]
		public float							enableLights			= 1.0f;
		[Range(0, 1)]
		public float							enableLines				= 1.0f;
		[Range(0, 1)]
		public float							enableAudio				= 1.0f;
		public AnimationCurve					particleEnableCrv		= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve					lightEnableCrv			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve					lineEnableCrv			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public AnimationCurve					audioEnableCrv			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		public Vector3Range						forceRange;
		public GizmoMode						gizmoMode				= GizmoMode.None;
		[Multiline(3)]
		public string							gizmoSelection			= "";
		public GizmoValue						gizmoValue				= GizmoValue.Element;
		public bool								showElementBounds		= false;
		List<int>								placeEvents				= new List<int>();
		List<int>								dismantleEvents			= new List<int>();
		Transform								nodetm;
		AudioSource								asrc;
		List<BuildQueue>						buildQueue				= new List<BuildQueue>();
		List<BuildQueue>						built					= new List<BuildQueue>();
		List<BuildQueue>						dismantle				= new List<BuildQueue>();
		//Dictionary<string, Renderer>			renderers				= new Dictionary<string, Renderer>();
		Dictionary<int, Renderer>				renderers				= new Dictionary<int, Renderer>();
		Dictionary<int, List<MeshElement>>		renderElems				= new Dictionary<int, List<MeshElement>>();
		List<Renderer>							meshRenderers;
		static ArtificerPlacedEvent				_placedEvent;
		static ArtificerBuiltEvent				_builtEvent;
		static ArtificerDismantledEvent			_dismantledEvent;
		public System.Random					random;
		public int								seed;
		public bool								showEditControls		= true;
		public bool								editModePlay			= false;
		public bool								useLateUpdate			= false;
		public float							editPlaySpeed			= 1.0f;
		public bool								showElementIDs			= false;
		public BoundsMode						boundsMode				= BoundsMode.Calc;
		public Vector3							boundsValue				= new Vector3(100.0f, 100.0f, 100.0f);
		public ClickBuildPos					clickBuildPos			= ClickBuildPos.Object;

		static public ArtificerPlacedEvent PlacedEvent
		{
			get
			{
				if ( _placedEvent == null )	_placedEvent = new ArtificerPlacedEvent();
				return _placedEvent;
			}
		}

		static public ArtificerBuiltEvent BuiltEvent
		{
			get
			{
				if ( _builtEvent == null )	_builtEvent = new ArtificerBuiltEvent();
				return _builtEvent;
			}
		}

		static public ArtificerDismantledEvent DismantledEvent
		{
			get
			{
				if ( _dismantledEvent == null )	_dismantledEvent = new ArtificerDismantledEvent();
				return _dismantledEvent;
			}
		}

		private void OnValidate()
		{
			if ( buildFromObjects != null )
			{
				for ( int i = 0; i < buildFromObjects.Length; i++ )
				{
					if ( buildFromObjects[i] != null )
						buildFromObjects[i].Init();
				}
			}
		}

		void Reset()
		{
			if ( target == null )
			{
				target = gameObject;
			}
		}

		public float RandomValue()
		{
			return (float)random.NextDouble();
		}

		public float RandomRange(float min, float max)
		{
			return Mathf.Lerp(min, max, (float)random.NextDouble());
		}

		public int RandomRange(int min, int max)
		{
			return random.Next(min, max);
		}

		public bool IsBuilding()
		{
			if ( buildMode == BuildMode.Build || buildMode == BuildMode.Pause )
			{
				return true;
			}

			return false;
		}

		public bool IsDismantling()
		{
			if ( buildMode == BuildMode.Dismantle )
			{
				return true;
			}

			return false;
		}

		public bool ReadyToBuild()
		{
			if ( buildMode == BuildMode.None || buildMode == BuildMode.Dismantled )
			{
				return true;
			}

			return false;
		}

		public bool ReadyToDismantle()
		{
			if ( buildMode == BuildMode.Finished )
			{
				return true;
			}

			return false;
		}

		public void StartBuild()
		{
			buildMode = BuildMode.Build;
			buildProgress = 0.0f;
			//buildLevel = 0.0f;
			HideTarget();
		}

		public void ContinueBuild()
		{
			buildMode = BuildMode.Build;
		}

		public void PauseBuild(bool pause)
		{
			if ( pause )
				buildMode = BuildMode.Pause;
			else
				buildMode = BuildMode.Build;
		}

		public bool IsPaused()
		{
			if ( buildMode == BuildMode.Pause )
				return true;

			return false;
		}

		public void StartPartBuild(float steps, float amount)
		{
			buildTo = steps;
			addAmount = amount;
			StartBuild();
		}

		public void BuildParts(float steps, float amount)
		{
			buildTo += steps;
			addAmount = amount;
			buildMode = BuildMode.Build;
		}

		double editorTime;
		bool dohide = false;

		public void StartBuildEdit()
		{
			//editorTimeStart = Time.time;
#if UNITY_EDITOR
			editorTime = UnityEditor.EditorApplication.timeSinceStartup;
#endif
			buildMode = BuildMode.Build;
			buildProgress = 0.0f;
			buildLevel = 0.0f;
			built.Clear();
			buildQueue.Clear();
			HideTarget();
		}

		bool HaveMeshData()
		{
			if ( buildData && buildData.meshes.Count > 0 )
			{
				if ( buildData.meshes[0].rp != null )
					return true;
			}

			return false;
		}

		public void UpdateMeshes()
		{
			BuildRenderDict();

			for ( int i = 0; i < buildData.meshes.Count; i++ )
			{
				if ( buildData.meshes[i].mesh )
				{
					if ( buildData.meshes[i].rp == null )
					{
						MeshElement element = buildData.meshes[i];
						//Renderer mr = renderers[element.gameObject];
						Renderer mr = renderers[element.id];

						element.rp = new RenderParams[element.draw.Count];

						for ( int m = 0; m < element.draw.Count; m++ )
						{
							int index = element.draw[m];
							RenderParams rp = new RenderParams(element.mats[index]);

							rp.layer				= (int)mr.gameObject.layer;	//mr.la mr.renderingLayerMask;
							rp.renderingLayerMask	= mr.renderingLayerMask;
							rp.rendererPriority		= mr.rendererPriority;
							rp.worldBounds			= element.bounds;	//new Bounds(Vector3.zero, Vector3.one * 1000f);	//bq.element.bounds;
							rp.motionVectorMode		= mr.motionVectorGenerationMode;
							rp.reflectionProbeUsage	= mr.reflectionProbeUsage;
							rp.shadowCastingMode	= mr.shadowCastingMode;
							rp.receiveShadows		= mr.receiveShadows;
							rp.lightProbeUsage		= mr.lightProbeUsage;
#if UNITY_2023_2_OR_NEWER
#if UNITY_6000_3_OR_NEWER
							rp.entityId				= mr.GetEntityId();
#else
							rp.instanceID = mr.GetInstanceID();
#endif
#endif
							element.rp[m]		= rp;
						}
					}
				}
			}
		}

		public void SetBuilt()
		{
			ShowTarget();
		}

		public void SetDismantled()
		{
			HideTarget();
			buildQueue.Clear();
			built.Clear();
			dismantle.Clear();
			buildLevel = 0.0f;
			buildMode = BuildMode.None;
		}

		public void ClearDismantle()
		{
			dismantle.Clear();
		}

		public void StartDismantle()
		{
#if UNITY_EDITOR
			editorTime = UnityEditor.EditorApplication.timeSinceStartup;
#endif
			if ( dismantle == null || dismantle.Count == 0 )
			{
				BuildDismantle();
			}

			HideTarget();
			dohide = true;
			buildMode = BuildMode.Dismantle;
		}

		public void SetBuild(float alpha)
		{
			if ( !buildData )
				return;

			HideTarget();

			buildProgress = alpha;	// * (float)buildData.meshes.Count;
			buildLevel = alpha * (float)buildData.meshes.Count;

			int bl = (int)buildLevel;

			if ( bl > buildData.meshes.Count )
				bl = buildData.meshes.Count;

			int piece = bl;

			buildQueue.Clear();
			built.Clear();

			for ( int i = 0; i < piece; i++ )
			{
				BuildQueue bq = new BuildQueue();
				bq.piece = i;
				bq.calpha = 1.0f;

				bq.build = true;
				bq.element = buildData.meshes[buildData.sorted[i]];
				buildQueue.Add(bq);
			}
		}

		void BuildComplete()
		{
			if ( buildMode != BuildMode.None )
			{
				buildMode = BuildMode.Finished;

				builtEvent.Invoke(this);
				BuiltEvent.Invoke(this);

				if ( enableOnBuilt != null )
				{
					for ( int i = 0; i < enableOnBuilt.Length; i++ )
					{
						if ( enableOnBuilt[i] )
							enableOnBuilt[i].SetActive(true);
					}
				}

				for ( int i = 0; i < disableOnBuilt.Length; i++ )
				{
					if ( disableOnBuilt[i] )
						disableOnBuilt[i].SetActive(true);
				}
			}
		}

		public void ClearBuildData()
		{
			if ( buildData )
			{
				buildData.meshes.Clear();
				buildData = null;
			}
		}

		BuildFrom FindBuildFrom(BuildFrom[] targets, Vector3 pos)
		{
			BuildFrom bf = null;

			if ( targets != null )//&& buildFromObjects.Length == )
			{
				switch ( buildFromMode )
				{
					case BuildFromMode.Nearest:
						float dist = float.MaxValue;

						for ( int i = 0; i < targets.Length; i++ )
						{
							if ( targets[i] != null )
							{
								float d = targets[i].GetDist(pos);
								if ( d < dist )
								{
									bf = targets[i];
									dist = d;
								}
							}
						}
						break;

					case BuildFromMode.Random:
						bf = targets[RandomRange(0, targets.Length)];
						break;
				}

			}

			return bf;
		}

		// TODO: only needs to do renderers and we can add the new component here
		// if we use the component, then we dont need to change the names
		public void MakeChildNamesUnique(Transform root)
		{
			Transform[] objs = GetComponentsInChildren<Transform>();

			HashSet<string> usedNames = new HashSet<string>();

			for ( int i = 0; i < objs.Length; i++ )
			{
				Transform child = objs[i];
				string original = child.name;
				string newName = original;
				int counter = 1;

				// Ensure uniqueness
				while ( usedNames.Contains(newName) )
				{
					newName = $"{original}_{counter}";
					counter++;
				}

				child.name = newName;
				usedNames.Add(newName);
			}
		}

		public void MakeChildNamesUniqueNew(Transform root)
		{
			Renderer[] objs = GetComponentsInChildren<Renderer>(true);

			int	id = 0;

			for ( int i = 0; i < objs.Length; i++ )
			{
				ObjectID obid = objs[i].GetComponent<ObjectID>();
				if ( !obid )
				{
					obid = objs[i].gameObject.AddComponent<ObjectID>();
				}

				obid.hideFlags = HideFlags.HideInInspector;
				obid.id = id++;
				obid.rend = objs[i];
			}
		}

		public void RebuildData()
		{
			buildData = null;
			Init();
		}

		public void BuildData()
		{
			rebuildNeeded = false;

			if ( !target )	//|| !buildData )
				return;

			random = new System.Random(seed);

			//MakeChildNamesUnique(target.transform);
			MakeChildNamesUniqueNew(target.transform);

#if UNITY_EDITOR
			if ( buildData == null && !Application.isPlaying )
			{
				string path = "Assets/Artificer/Build Profiles";
				if ( !System.IO.Directory.Exists(path) )
					System.IO.Directory.CreateDirectory(path);

				string assetPath = System.IO.Path.Combine(path, $"{target.name}_BuildData.asset");

				assetPath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(assetPath);

				buildData = ScriptableObject.CreateInstance<BuildData>();
				UnityEditor.AssetDatabase.CreateAsset(buildData, assetPath);
				UnityEditor.AssetDatabase.SaveAssets();
			}
			else
			{
				if ( buildData == null && Application.isPlaying )
				{
					buildData = ScriptableObject.CreateInstance<BuildData>();
				}
			}
#else
			if ( !buildData )
				buildData = ScriptableObject.CreateInstance<BuildData>();
#endif

			SplineContainer spl = null;
			if ( useSortSpline )
				spl = sortSpline;

			if ( !buildData )
				return;

			GetAdjusters();

			buildData.meshes = GameObjectMeshSplitter.SplitGameObject(this, target, sortOrigin, spl, useSplitParams);
			if ( buildData.meshes == null || buildData.meshes.Count == 0 )
				return;

			buildData.sorted = BuildSortedIndexList(buildData.meshes);

			// do we do as components or a list in artificer
			if ( useSplitParams )
			{
				Adjust[] adjust = GetComponentsInChildren<Adjust>(false);

				for ( int i = 0; i < adjust.Length; i++ )
				{
					Adjust adj = adjust[i];

					if ( adj.enabled )	// && adj.adjustMode == AdjustMode.Selection
					{
						if ( adj.adjustMode == AdjustMode.Selection )
						{
							List<int> selection = adj.GetSelection();

							for ( int s = 0; s < selection.Count; s++ )
							{
								if ( selection[s] >= 0 && selection[s] < buildData.sorted.Count )
								{
									int mindex = selection[s];	//buildData.sorted[index];

									if ( adj.options.buildStyle.overrideState )			buildData.meshes[mindex].buildStyle		= adj.options.buildStyle.value;
									if ( adj.options.dismantleStyle.overrideState )		buildData.meshes[mindex].dismantleStyle	= adj.options.dismantleStyle.value;
									if ( adj.options.buildDistRange.overrideState )		buildData.meshes[mindex].buildDist		= adj.options.buildDistRange.value.GetValue(this);
									if ( adj.options.usePlaceCurve.overrideState )		buildData.meshes[mindex].usePlaceCurve	= adj.options.usePlaceCurve.value;
									if ( adj.options.placeCurve.overrideState )			buildData.meshes[mindex].placeCurve		= adj.options.placeCurve.value;
									if ( adj.options.useRotCurve.overrideState )		buildData.meshes[mindex].useRotCurve	= adj.options.useRotCurve.value;
									if ( adj.options.placeRotCurve.overrideState )		buildData.meshes[mindex].placeRotCurve	= adj.options.placeRotCurve.value;
									if ( adj.options.useScaleCurve.overrideState )		buildData.meshes[mindex].useScaleCurve	= adj.options.useScaleCurve.value;
									if ( adj.options.maxScale.overrideState )			buildData.meshes[mindex].maxScale		= adj.options.maxScale.value;
									if ( adj.options.rotateRange.overrideState )		buildData.meshes[mindex].rotate			= adj.options.rotateRange.value.GetValue(this);
									if ( adj.options.placeTimeRange.overrideState )		buildData.meshes[mindex].placeTime		= adj.options.placeTimeRange.value.GetValue(this);

									bool recalc = false;
									BuildFrom bf = null;
									if ( buildFromMode != BuildFromMode.None )
									{
										if ( adj.options.buildFromObjects.Length > 0 )
										{
											bf = FindBuildFrom(adj.options.buildFromObjects, buildData.meshes[mindex].tm.MultiplyPoint(buildData.meshes[mindex].center));
										}
										else
										{
											bf = FindBuildFrom(buildFromObjects, buildData.meshes[mindex].tm.MultiplyPoint(buildData.meshes[mindex].center));
										}

										if ( bf != null )
										{
											Transform	_buildFrom			= bf.buildFrom;
											Vector3		_buildFromBox		= bf.buildFromBox;
											Vector3		_buildFromOffset	= bf.buildFromOffset;
											float		_startTension		= bf.startTension;
											float		_endTension			= bf.endTension;
											SplineDir	_splineDirCalc		= bf.splineDirCalc;
											Vector3		_splineEndProject	= bf.splineEndProject;
											SplineMode	_splineMode			= bf.splineMode;
											Vector3		_startDir			= bf.startDir;
											if ( recalc )
												CalcSpline(buildData.meshes[mindex], buildData.meshes[mindex].tm1, _buildFrom, _buildFromBox, _buildFromOffset, _startTension, _endTension, _splineDirCalc, _splineEndProject, _splineMode, _startDir);
										}
									}
								}
							}

							if ( adj.moveTo.overrideState )
							{
								selection = adj.GetMoveSelection();

								for ( int s = 0; s < selection.Count; s++ )
								{
									if ( selection[s] >= 0 && selection[s] < buildData.sorted.Count )
									{
										int index = buildData.sorted.IndexOf(selection[s]);

										buildData.sorted.RemoveAt(index);
										buildData.sorted.Insert(adj.moveTo.value, selection[s]);
									}
								}
							}
						}
						else
						{
							// loop all meshes and check for in a volume
							for ( int s = 0; s < buildData.meshes.Count; s++ )
							{
								MeshElement me = buildData.meshes[s];
								Adjust adj1 = CheckInAdjust(me);
								if ( adj1 )
								{
									int mindex = s;
									if ( adj1.options.buildStyle.overrideState )		buildData.meshes[mindex].buildStyle		= adj1.options.buildStyle.value;
									if ( adj1.options.dismantleStyle.overrideState )	buildData.meshes[mindex].dismantleStyle	= adj1.options.dismantleStyle.value;
									if ( adj1.options.buildDistRange.overrideState )	buildData.meshes[mindex].buildDist		= adj1.options.buildDistRange.value.GetValue(this);
									if ( adj1.options.usePlaceCurve.overrideState )		buildData.meshes[mindex].usePlaceCurve	= adj1.options.usePlaceCurve.value;
									if ( adj1.options.placeCurve.overrideState )		buildData.meshes[mindex].placeCurve		= adj1.options.placeCurve.value;
									if ( adj1.options.useRotCurve.overrideState )		buildData.meshes[mindex].useRotCurve	= adj1.options.useRotCurve.value;
									if ( adj1.options.placeRotCurve.overrideState )		buildData.meshes[mindex].placeRotCurve	= adj1.options.placeRotCurve.value;
									if ( adj1.options.useScaleCurve.overrideState )		buildData.meshes[mindex].useScaleCurve	= adj1.options.useScaleCurve.value;
									if ( adj1.options.maxScale.overrideState )			buildData.meshes[mindex].maxScale		= adj1.options.maxScale.value;
									if ( adj1.options.rotateRange.overrideState )		buildData.meshes[mindex].rotate			= adj1.options.rotateRange.value.GetValue(this);
									if ( adj1.options.placeTimeRange.overrideState )	buildData.meshes[mindex].placeTime		= adj1.options.placeTimeRange.value.GetValue(this);

									bool recalc = false;
									BuildFrom bf = null;
									if ( adj1.options.buildFromObjects.Length > 0 )
									{
										bf = FindBuildFrom(adj1.options.buildFromObjects, buildData.meshes[mindex].tm.MultiplyPoint(buildData.meshes[mindex].center));
									}
									else
									{
										bf = FindBuildFrom(buildFromObjects, buildData.meshes[mindex].tm.MultiplyPoint(buildData.meshes[mindex].center));
									}

									if ( bf != null )
									{
										Transform	_buildFrom			= bf.buildFrom;
										Vector3		_buildFromBox		= bf.buildFromBox;
										Vector3		_buildFromOffset	= bf.buildFromOffset;
										float		_startTension		= bf.startTension;
										float		_endTension			= bf.endTension;
										SplineDir	_splineDirCalc		= bf.splineDirCalc;
										Vector3		_splineEndProject	= bf.splineEndProject;
										SplineMode	_splineMode			= bf.splineMode;
										Vector3		_startDir			= bf.startDir;
										if ( recalc )
											CalcSpline(buildData.meshes[mindex], buildData.meshes[mindex].tm1, _buildFrom, _buildFromBox, _buildFromOffset, _startTension, _endTension, _splineDirCalc, _splineEndProject, _splineMode, _startDir);
									}
								}
							}
						}
					}
				}
			}

			HashSet<Renderer> renderers = new HashSet<Renderer>();

			FindLastElements();
			renderers.Clear();
			// find first element for each object
			for ( int i = 0; i < buildData.sorted.Count; i++ )
			{
				if ( !renderers.Contains(buildData.meshes[buildData.sorted[i]].renderer) )
				{
					buildData.meshes[buildData.sorted[i]].firstElement = true;
					renderers.Add(buildData.meshes[buildData.sorted[i]].renderer);
				}
			}

#if UNITY_EDITOR
			if ( !Application.isPlaying )
			{
				if ( buildData )
				{
					UnityEditor.EditorUtility.SetDirty(buildData);
					UnityEditor.AssetDatabase.SaveAssetIfDirty(buildData);
				}
			}
#endif
		}

		struct LastElem
		{
			public int		index;
			public MeshElement	me;
		}

		// run through and find pos time
		void FindLastElements()
		{
			float dt = buildTime / (float)buildData.sorted.Count;

			for ( int i = 0; i < buildData.sorted.Count; i++ )
			{
				MeshElement me = buildData.meshes[buildData.sorted[i]];

				if ( me.placeMode == PlaceMode.Speed )
					me.bt = (dt * i) + (me.buildDist / me.placeTime);
				else
					me.bt = (dt * i) + me.placeTime;	//(me.buildDist / me.placeTime);
			}

			Dictionary<Renderer, MeshElement> lastValues = new Dictionary<Renderer, MeshElement>();

			for ( int i = buildData.sorted.Count - 1; i >= 0; i-- )
			{
				MeshElement me = buildData.meshes[buildData.sorted[i]];

				if ( !lastValues.ContainsKey(me.renderer) )
				{
					lastValues[me.renderer] = me;	//renderers.Add(buildData.meshes[buildData.sorted[i]].renderer);
				}
				else
				{
					MeshElement cme = lastValues[me.renderer];

					if ( me.bt > cme.bt )
					{
						lastValues[me.renderer] = me;
					}
				}
			}

			foreach (var value in lastValues.Values)
			{
				value.lastElement = LastElement.Last;
				if ( value.renderer && value.renderer.GetComponentInParent<LODGroup>() )
					value.lastElement |= LastElement.LOD;
			}
		}


		List<int> BuildSortedIndexList(List<MeshElement> list)
		{
			int count = list.Count;
			List<int> indices = new List<int>(count);

			for ( int i = 0; i < count; i++ )
				indices.Add(i);

			switch ( sortMode )
			{
				case SortMode.None:				break;
				case SortMode.Position:			indices.Sort((a, b) => SortElementsPosition(list[a], list[b])); break;
				case SortMode.Material:			indices.Sort((a, b) => SortElementsMaterial(list[a], list[b])); break;
				case SortMode.PositionMaterial:	indices.Sort((a, b) => SortElementsPositionMaterial(list[a], list[b])); break;
				case SortMode.MaterialPosition:	indices.Sort((a, b) => SortElementsMaterialPosition(list[a], list[b])); break;
				case SortMode.Random:			indices.Sort((a, b) => SortElementsPosition(list[a], list[b])); break;
			}

			if ( reverseSortOrder )
				indices.Reverse();

			return indices;
		}

		// TODO: add a small hidden ObjName component to any object that has a renderer and have the id as a string in there, and the renderer has a field
		// so then we get that Component instead of Render
		// so when the component is first added we add all the components and set the names. Those names are used from then on and not the ogameobject names, so they will be free to change
		void BuildRenderElems()
		{
			if ( renderElems == null )
			{
				renderElems = new Dictionary<int, List<MeshElement>>();
			}

			renderElems.Clear();

			for ( int i = 0; i < buildData.meshes.Count; i++ )
			{
				MeshElement elem = buildData.meshes[i];

				//if ( !renderElems.ContainsKey(elem.gameObject) )
				if ( !renderElems.ContainsKey(elem.id) )
				{
					List<MeshElement> mel = new List<MeshElement>();
					mel.Add(elem);
					//renderElems.Add(elem.gameObject, mel);
					renderElems.Add(elem.id, mel);
				}
				else
				{
					List<MeshElement> mel = renderElems[elem.id];	//gameObject];
					//List<MeshElement> mel = renderElems[elem.id];
					mel.Add(elem);
				}
			}
		}

#if false
		void BuildRenderDict()
		{
			renderers.Clear();
			Renderer[] mrs = target.GetComponentsInChildren<Renderer>(true);

			for ( int i = 0; i < mrs.Length; i++ )
			{
				if ( !renderers.ContainsKey(mrs[i].gameObject.name) )
					renderers.Add(mrs[i].gameObject.name, mrs[i]);
			}
		}
#else
		void BuildRenderDict()
		{
			renderers.Clear();
			Renderer[] mrs = target.GetComponentsInChildren<Renderer>(true);

			for ( int i = 0; i < mrs.Length; i++ )
			{
				ObjectID oid = mrs[i].GetComponent<ObjectID>();
				if ( oid )
				{
					if ( !renderers.ContainsKey(oid.id) )	//gameObject.name) )
						renderers.Add(oid.id, mrs[i]);
				}
				else
					Debug.LogWarning("Missing Object ID. Please rebuild the data for this object", gameObject);
			}
		}
#endif

		LODGroup[]	lods;

		// We should cache these bq values maybe 
		void BuildDismantle()
		{
			dismantle.Clear();

			for ( int i = 0; i < buildData.meshes.Count; i++ )
			{
				BuildQueue bq = new BuildQueue();
				bq.piece = i;
				if ( buildStyle == BuildStyle.Appear )
					bq.calpha = 1.0f;
				else
					bq.calpha = 0.0f;

				bq.build = true;
				bq.element = buildData.meshes[buildData.sorted[i]];

				// Recalc spline
				//CalcSpline();
				if ( bq.element.mesh == null )
				{
					Mesh mesh = new Mesh();

					if ( bq.element.verts.Length > 65535 )
						mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

					mesh.subMeshCount = bq.element.subMeshCount;
					mesh.SetVertices(bq.element.verts);
					if ( bq.element.uvs.Length > 0 ) mesh.SetUVs(0, bq.element.uvs);
					if ( bq.element.uv2 != null && bq.element.uv2.Length > 0 ) mesh.SetUVs(1, bq.element.uv2);
					//if ( newUV1.Count > 0 ) mesh.SetUVs(1, newUV1);
					//if ( newUV3.Count > 0 ) mesh.SetUVs(2, newUV3);
					//if ( newUV4.Count > 0 ) mesh.SetUVs(3, newUV4);
					if ( bq.element.normals.Length > 0 )	mesh.SetNormals(bq.element.normals);
					if ( bq.element.tangents.Length > 0 )	mesh.SetTangents(bq.element.tangents);
					if ( bq.element.colors.Length > 0 )		mesh.SetColors(bq.element.colors);

					for ( int s = 0; s < mesh.subMeshCount; s++ )
					{
						if ( bq.element.tris[s].tris.Length > 0 )
							mesh.SetTriangles(bq.element.tris[s].tris, s);
						else
							mesh.SetTriangles(System.Array.Empty<int>(), s);
					}

					mesh.RecalculateBounds();
					bq.element.mesh = mesh;
					//bq.element.bounds = TransformBounds(mesh.bounds, bq.element.tm1);
					if ( boundsMode == BoundsMode.Calc )
						TransformBoundsNew(mesh.bounds, bq.element.tm1, ref bq.bounds);
					else
					{
						bq.bounds.center = transform.position;
						bq.bounds.extents = boundsValue;
					}

					//Renderer mr = renderers[bq.element.gameObject];
					Renderer mr = renderers[bq.element.id];

					bq.element.rp = new RenderParams[bq.element.draw.Count];

					for ( int m = 0; m < bq.element.draw.Count; m++ )
					{
						int index = bq.element.draw[m];
						RenderParams rp = new RenderParams(bq.element.mats[index]);

						rp.layer				= (int)mr.gameObject.layer;	//mr.la mr.renderingLayerMask;
						rp.renderingLayerMask	= mr.renderingLayerMask;
						rp.rendererPriority		= mr.rendererPriority;
						rp.worldBounds			= bq.bounds;	//element.bounds;	//new Bounds(Vector3.zero, Vector3.one * 1000f);	//bq.element.bounds;
						rp.motionVectorMode		= mr.motionVectorGenerationMode;
						rp.reflectionProbeUsage	= mr.reflectionProbeUsage;
						rp.shadowCastingMode	= mr.shadowCastingMode;
						rp.receiveShadows		= mr.receiveShadows;
						rp.lightProbeUsage		= mr.lightProbeUsage;
#if UNITY_2023_2_OR_NEWER
#if UNITY_6000_3_OR_NEWER
						rp.entityId				= mr.GetEntityId();
#else
						rp.instanceID			= mr.GetInstanceID();
#endif
#endif
						bq.element.rp[m]		= rp;
					}
				}
				dismantle.Add(bq);
			}

			buildLevel = buildData.meshes.Count;
		}


		public List<BuildQueue> GetBuildQueue()
		{
			return buildQueue;
		}

		void Start()
		{
			Init();
		}

		public void Init()
		{
			nodetm = transform;

			random = new System.Random(seed);
			//if ( target )
				//meshRenderers = target.GetComponentsInChildren<Renderer>();

			lods = GetComponentsInChildren<LODGroup>();

			asrc = GetComponent<AudioSource>();

			if ( !buildData || buildData.meshes == null || buildData.meshes.Count == 0 )
				BuildData();

			BuildRenderDict();
			BuildRenderElems();
			switch ( buildMode )
			{
				case BuildMode.None:
					HideTarget();
					buildLevel = 0.0f;
					break;
				case BuildMode.Build:
					buildLevel = 0.0f;
					HideTarget();
					break;
				case BuildMode.Dismantle:
					break;
				case BuildMode.Click:
					break;
				case BuildMode.Finished:
					BuildDismantle();
					// Need to prepare dismantle info
					break;
				case BuildMode.Pause:
					break;
				case BuildMode.Dismantled:
					break;
			}

			if ( builtEvent == null )
				builtEvent = new ArtificerBuiltEvent();

			if ( placedEvent == null )
				placedEvent = new ArtificerPlacedEvent();

			if ( completedObjectEvent == null )
				completedObjectEvent = new ArtificerCompletedObjectEvent();

			placeEvents = Adjust.GetSelection(placedEvents);
			dismantleEvents = Adjust.GetSelection(dismantledEvents);

			built.Clear();
			buildQueue.Clear();

			switch ( startMode )
			{
				case StartMode.AsIs:
					UpdateMeshes();
					break;
				case StartMode.StartBuilt:
					SetBuilt();
					buildMode = BuildMode.Finished;
					break;
				case StartMode.StartDismantled:
					SetDismantled();
					break;
				case StartMode.Build:
					SetDismantled();
					ClearMeshes();
					StartBuildEdit();
					break;
				case StartMode.Dismantle:
					SetBuilt();
					UpdateMeshes();
					ClearDismantle();
					StartDismantle();
					break;

				case StartMode.ClickBuild:
					SetDismantled();
					buildMode = BuildMode.Click;
					buildIndex = -1;
					showingPart = 0;
					break;
			}
		}

		public void ClearMeshes()
		{
			for ( int i = 0; i < buildData.meshes.Count; i++ )
			{
				buildData.meshes[i].mesh = null;
			}
		}

		int SortElementsPosition(MeshElement m1, MeshElement m2)
		{
			if ( m1.distance > m2.distance ) return 1;
			if ( m1.distance < m2.distance ) return -1;

			return 0;
		}

		int SortElementsMaterial(MeshElement m1, MeshElement m2)
		{
			if ( m1.matSortID > m2.matSortID ) return 1;
			if ( m1.matSortID < m2.matSortID ) return -1;

			return 0;
		}

		int SortElementsPositionMaterial(MeshElement m1, MeshElement m2)
		{
			if ( m1.distance > m2.distance )	return 1;
			if ( m1.distance < m2.distance )	return -1;
			if ( m1.matSortID > m2.matSortID )	return 1;
			if ( m1.matSortID < m2.matSortID )	return -1;

			return 0;
		}

		int SortElementsMaterialPosition(MeshElement m1, MeshElement m2)
		{
			if ( m1.matSortID > m2.matSortID )	return 1;
			if ( m1.matSortID < m2.matSortID )	return -1;
			if ( m1.distance > m2.distance )	return 1;
			if ( m1.distance < m2.distance )	return -1;

			return 0;
		}

		void InitParticlesAndLights(GameObject go)
		{
			if ( !exclude.Contains(go) )
			{
				if ( handleParticles )
				{
					ParticleSystem ps = go.GetComponent<ParticleSystem>();
					if ( ps && ps.isPlaying )
					{
						PSystem psys = new PSystem();
						psys.particle = ps;
						psys.alpha = 0.0f;
						psys.speed = particleEnableSpeed;
						psys.scale = ps.transform.localScale;
						particles.Add(psys);
						ps.Stop();
						ps.Clear();
					}
				}

				if ( handleLights )
				{
					Light l = go.GetComponent<Light>();
					if ( l && l.enabled )
					{
						LSystem lsys = new LSystem();
						lsys.light = l;
						lsys.speed = lightEnableSpeed;
						lsys.alpha = 0.0f;
						lsys.intensity = l.intensity;
						lights.Add(lsys);
					}
				}

				if ( handleLines )
				{
					LineRenderer l = go.GetComponent<LineRenderer>();
					if ( l && l.enabled )
					{
						LineSystem lsys = new LineSystem();
						lsys.linerender = l;
						lsys.speed = lineEnableSpeed;
						lsys.alpha = 0.0f;
						lsys.scale = l.transform.localScale;
						lines.Add(lsys);
					}
				}

				if ( handleAudio )
				{
					AudioSource a = go.GetComponent<AudioSource>();
					if ( a && a.enabled && a.isPlaying )
					{
						AudioSystem asys = new AudioSystem();
						asys.audio = a;
						asys.speed = audioEnableSpeed;
						asys.alpha = 0.0f;
						asys.volume = a.volume;
						audios.Add(asys);
						a.Stop();
					}
				}

				for ( int i = 0; i < go.transform.childCount; i++ )
				{
					InitParticlesAndLights(go.transform.GetChild(i).gameObject);
				}

			}
		}

		void ShowTarget()
		{
			if ( target )
			{
				ShowTargetRecursive(target);
				buildMode = BuildMode.Finished;
			}
		}

		void HideTarget()
		{
			if ( meshRenderers == null )	//|| meshRenderers.Length == 0 )
				meshRenderers = new List<Renderer>();   //target.GetComponentsInChildren<MeshRenderer>();

			if ( particles == null || lights == null )
			{
				particles	= new List<PSystem>();
				lights		= new List<LSystem>();
				lines		= new List<LineSystem>();
				audios		= new List<AudioSystem>();

				InitParticlesAndLights(target);
			}

			particlesOn = false;
			lightsOn	= false;
			linesOn		= false;
			audioOn		= false;
			meshRenderers.Clear();

			HideTargetRecursive(target);

			if ( lods != null )
			{
				for ( int i = 0; i < lods.Length; i++ )
				{
					lods[i].ForceLOD(0);
				}
			}
		}

		public IEnumerator StartParticle(PSystem psys)
		{
			psys.particle.Play();
			while ( psys.alpha < 1.0f )
			{
				if ( useUnscaledTime )
					psys.alpha -= (Time.unscaledDeltaTime / psys.speed);
				else
					psys.alpha += Time.deltaTime / psys.speed;

				if ( psys.alpha >= 1.0f )
				{
					psys.alpha = 1.0f;

					psys.particle.transform.localScale = psys.scale;
				}
				else
				{
					psys.particle.transform.localScale = Vector3.Lerp(Vector3.zero, psys.scale, particleEnableCrv.Evaluate(psys.alpha));
				}

				yield return null;
			}
		}

		public IEnumerator StartLight(LSystem lsys)
		{
			lsys.light.enabled = true;
			while ( lsys.alpha < 1.0f )
			{
				if ( useUnscaledTime )
					lsys.alpha -= (Time.unscaledDeltaTime / lsys.speed);
				else
					lsys.alpha += Time.deltaTime / lsys.speed;

				if ( lsys.alpha >= 1.0f )
				{
					lsys.alpha = 1.0f;

					lsys.light.intensity = lsys.intensity;
				}
				else
				{
					lsys.light.intensity = Mathf.Lerp(0.0f, lsys.intensity, lightEnableCrv.Evaluate(lsys.alpha));
				}

				yield return null;
			}
		}

		public IEnumerator StartLine(LineSystem linesys)
		{
			while ( linesys.alpha < 1.0f )
			{
				if ( useUnscaledTime )
					linesys.alpha -= (Time.unscaledDeltaTime / linesys.speed);
				else
					linesys.alpha += Time.deltaTime / linesys.speed;

				if ( linesys.alpha >= 1.0f )
				{
					linesys.alpha = 1.0f;

					linesys.linerender.transform.localScale = linesys.scale;
				}
				else
				{
					linesys.linerender.transform.localScale = Vector3.Lerp(Vector3.zero, linesys.scale, lineEnableCrv.Evaluate(linesys.alpha));
				}

				yield return null;
			}
		}

		public IEnumerator StartAudio(AudioSystem asys)
		{
			asys.audio.Play();
			while ( asys.alpha < 1.0f )
			{
				if ( useUnscaledTime )
					asys.alpha -= (Time.unscaledDeltaTime / asys.speed);
				else
					asys.alpha += Time.deltaTime / asys.speed;

				if ( asys.alpha >= 1.0f )
				{
					asys.alpha = 1.0f;

					asys.audio.volume = asys.volume;
				}
				else
				{
					asys.audio.volume = Mathf.Lerp(0.0f, asys.volume, audioEnableCrv.Evaluate(asys.alpha));
				}

				yield return null;
			}
		}

		public struct AudioSystem
		{
			public AudioSource		audio;
			public float			speed;
			public float			volume;
			public float			alpha;
		}

		public struct LineSystem
		{
			public LineRenderer		linerender;
			public float			speed;
			public Vector3			scale;
			public float			alpha;
		}

		public struct PSystem
		{
			public ParticleSystem	particle;
			public float			speed;
			public Vector3			scale;
			public float			alpha;
		}

		public struct LSystem
		{
			public Light			light;
			public float			speed;
			public float			intensity;
			public float			alpha;
		}

		bool particlesOn;
		bool audioOn;
		bool linesOn;
		bool lightsOn;
		List<PSystem>		particles	= new List<PSystem>();
		List<LSystem>		lights		= new List<LSystem>();
		List<LineSystem>	lines		= new List<LineSystem>();
		List<AudioSystem>	audios		= new List<AudioSystem>();

		void ShowTargetRecursive(GameObject go)
		{
			if ( !exclude.Contains(go) )
			{
				Renderer r = go.GetComponent<Renderer>();
				if ( r )
				{
					Type ty = r.GetType();
					if ( r && ty != typeof(ParticleSystemRenderer) && ty != typeof(LineRenderer) )
					{
						r.enabled = true;
					}
				}
#if true
				if ( handleParticles )
				{
					ParticleSystem ps = go.GetComponent<ParticleSystem>();
					if ( ps && ps.isPlaying )
					{
					}
				}

				if ( handleLights )
				{
					Light l = go.GetComponent<Light>();
					if ( l )	//&& l.enabled )
					{
						l.intensity = 1.0f;	//0.0f;
						l.enabled = true;	//false;
					}
				}

				if ( handleLines )
				{
					LineRenderer l = go.GetComponent<LineRenderer>();
					if ( l )	//&& l.enabled )
						l.enabled = true;
				}

#endif
				for ( int i = 0; i < go.transform.childCount; i++ )
				{
					ShowTargetRecursive(go.transform.GetChild(i).gameObject);
				}
			}

		}

		void HideTargetRecursive(GameObject go)
		{
			if ( !exclude.Contains(go) )
			{
				Renderer r = go.GetComponent<Renderer>();
				if ( r )
				{
					Type ty = r.GetType();
					if ( r && ty != typeof(ParticleSystemRenderer) && ty != typeof(LineRenderer) )
					{
						meshRenderers.Add(r);
						r.enabled = false;
					}
				}
#if true
				if ( handleParticles )
				{
					ParticleSystem ps = go.GetComponent<ParticleSystem>();
					if ( ps && ps.isPlaying )
					{
						ps.Stop();
						ps.Clear();
					}
				}

				if ( handleLights )
				{
					Light l = go.GetComponent<Light>();
					if ( l && l.enabled )
					{
						l.intensity = 0.0f;
						l.enabled = false;
					}
				}

				if ( handleLines )
				{
					LineRenderer l = go.GetComponent<LineRenderer>();
					if ( l && l.enabled )
						l.enabled = false;
				}

				if ( handleAudio )
				{
					AudioSource a = go.GetComponent<AudioSource>();
					if ( a && a.enabled )
					{
						a.Stop();
					}
				}

#endif
				for ( int i = 0; i < go.transform.childCount; i++ )
				{
					HideTargetRecursive(go.transform.GetChild(i).gameObject);
				}
			}
		}

		private void OnEnable()
		{
			if ( buildOnAwake )
			{
				if ( buildMode == BuildMode.Dismantle )
				{
				}
				else
					StartBuild();
			}
		}

		private void Update()
		{
			Build();
			ShowPlacement();
			ManualBuild();
		}

		void LateUpdate1()
		{
		}

		void PlaySound(AudioClip clip)
		{
			if ( asrc )
			{
				if ( !asrc.isPlaying )
				{
					asrc.clip = clip;
					asrc.pitch = RandomRange(0.75f, 1.2f);
					asrc.Play();
				}
			}
		}

		float Max(Vector3 size)
		{
			if ( size.x > size.y )
			{
				if ( size.x > size.z )
					return size.x;
				else
					return size.z;
			}
			else
			{
				if ( size.y > size.z )
					return size.y;
				else
					return size.z;
			}
		}

		void Dismantle()
		{
			if ( !buildData )
				return;

			if ( buildMode == BuildMode.None )
				return;

			if ( buildMode == BuildMode.Dismantle )
			{
				if ( dohide )
				{
					HideTarget();
					dohide = false;
				}

				float prevLevel = buildLevel;
				int prevPiece = (int)buildLevel;

				float bspd = dismantleTime / buildData.meshes.Count;

#if UNITY_EDITOR
				float deltatime = (float)(UnityEditor.EditorApplication.timeSinceStartup - editorTime);	//0.0f;
				if ( !Application.isPlaying )
					deltatime = deltatime * editPlaySpeed;	//(Time.time - editorTime) * editPlaySpeed;	// Time.deltaTime;
				else
				{
					deltatime = Time.deltaTime;
					if ( useUnscaledTime )
						deltatime = Time.unscaledDeltaTime;
				}
				editorTime = UnityEditor.EditorApplication.timeSinceStartup;
#else
				float deltatime = 0.0f;
				if ( useUnscaledTime )
					deltatime = Time.unscaledDeltaTime;
				else
					deltatime = Time.deltaTime;	// / bspd);  // + addAmount; addAmount = 0.0f;
#endif
				buildLevel -= (deltatime / bspd);
				//buildLevel -= Time.deltaTime * dismantleSpeed;
				if ( buildLevel < 0.0f )
					buildLevel = 0.0f;

				int bl = (int)buildLevel;

				if ( bl < 0 )
					bl = 0;

				int piece = bl;

				buildProgress = buildLevel / (float)buildData.meshes.Count;

				if ( onDismantleComplete != null )	// buildProgress <= onDismantleLevel )
				{
					//if ( onDismantleCompleteBuildThese != null )
					{
						for ( int i = 0; i < onDismantleComplete.Length; i++ )
						{
							if ( buildProgress <= onDismantleComplete[i].level )
							{
								if ( onDismantleComplete[i].dismantle && onDismantleComplete[i].dismantle.buildMode != BuildMode.Build && onCompleteBuildThese[i].build.buildMode != BuildMode.Finished )
									onDismantleComplete[i].dismantle.StartDismantle();	//buildMode = BuildMode.Build;
							}
						}
					}

					//for ( int i = 0; i < onDismantled.Length; i++ )
					//{
						//if ( onDismantled[i] && onDismantled[i].buildMode == BuildMode.Finished )	//onDismantled[i].buildMode != BuildMode.Dismantle )
						//{
							//onDismantled[i].buildMode = BuildMode.Dismantle;
							//onDismantled[i].StartDismantle();	//.buildMode = BuildMode.Dismantle;
						//}
					//}
				}

				//Debug.Log("Dismantle Count " + dismantle.Count);
				//if ( dismantle.Count > 0 )
					//Debug.Log("dismantle 0 " + dismantle[0].element.gameObject);

				//Debug.Log("prev piece - 1:" + (prevPiece - 1) + " piece " + piece);
				for ( int i = prevPiece - 1; i >= piece; i-- )
				{
					BuildQueue bq = new BuildQueue();
					bq.element = dismantle[^1].element;	//buildData.meshes[buildData.sorted[i]];
					bq.piece = i;
					//if ( dismantleStyle == DismantleStyle.Vanish )
					if ( bq.element.dismantleStyle == DismantleStyle.Vanish )
						bq.calpha = 1.0f;
					else
						bq.calpha = 0.0f;

					//if ( bq.element.gameObject == "beer crate" )
					//{
						//Debug.Log("dismantle beer crate");
					//}
					//if ( dismantleStyle == DismantleStyle.Explode )
					if ( bq.element.dismantleStyle == DismantleStyle.Explode )
					{
						//float ms = Max(bq.element.bounds.size);

						bq.pos = (nodetm.localToWorldMatrix * bq.element.tm).GetPosition();
						bq.rot = (nodetm.localToWorldMatrix * bq.element.tm).rotation.eulerAngles;

						Vector3 edir = (bq.pos - nodetm.TransformPoint(explodeOrigin)).normalized;
						edir = Vector3.Scale(edir, bq.element.dismantleProjection);

						bq.velocity = edir * RandomRange(bq.element.minExplodeForce, bq.element.maxExplodeForce);
						bq.angvel.x = RandomRange(-bq.element.angVelRange.x, bq.element.angVelRange.x);
						bq.angvel.y = RandomRange(-bq.element.angVelRange.y, bq.element.angVelRange.y);
						bq.angvel.z = RandomRange(-bq.element.angVelRange.z, bq.element.angVelRange.z);

						bq.force = forceRange.GetValue(this);
						//bq.angvel /= ms;
					}

					bq.build = true;
					buildQueue.Add(bq);
					dismantle.RemoveAt(dismantle.Count - 1);

					if ( bq.element.lastElement.HasFlag(LastElement.Last) )
					{
						// disable the mesh renderer and add all elements to buildqueue
					}

					if ( customDismantle )
						customDismantle.AddedToDismantle(bq.element, i);
				}

				if ( piece == 0 && buildQueue.Count == 0 )
					DismantleComplete();

				// Swap in finished model when Built
				Matrix4x4 tm = Matrix4x4.Translate(nodetm.TransformPoint(Vector3.zero));
				Matrix4x4 localTM = nodetm.localToWorldMatrix;

				for ( int i = 0; i < dismantle.Count; i++ )
				{
					for ( int m = 0; m < dismantle[i].element.draw.Count; m++ )
					{
						int index = dismantle[i].element.draw[m];
						dismantle[i].element.rp[m].worldBounds = dismantle[i].bounds;
						Graphics.RenderMesh(dismantle[i].element.rp[m], dismantle[i].element.mesh, index, localTM * dismantle[i].element.tm);
					}
				}

				Vector3 dir = Vector3.up;
				float a = 0.0f;

				for ( int i = buildQueue.Count - 1; i >= 0; i-- )
				{
					BuildQueue bq = buildQueue[i];

					Vector3 p = Vector3.zero;

					DismantleStyle bs = bq.element.dismantleStyle;
					float placespeed = bq.element.removeTime;

					float ta = bq.calpha;

					//if ( useUnscaledTime )
						//bq.calpha += Time.unscaledDeltaTime / placespeed;
					//else
						//bq.calpha += Time.deltaTime / placespeed;

					bq.calpha += deltatime / placespeed;	// bug: is this right

					bq.calpha = Mathf.Clamp01(bq.calpha);
					if ( useDisPlaceCurve )
						a = 1.0f - disPlaceCurve.Evaluate(bq.calpha);
					else
						a = 1.0f - bq.calpha;

					if ( bq.element.mesh == null )
					{
						Mesh mesh = new Mesh();

						if ( bq.element.verts.Length > 65535 )
							mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

						mesh.subMeshCount = bq.element.subMeshCount;
						mesh.SetVertices(bq.element.verts);
						if ( bq.element.uvs.Length > 0 )		mesh.SetUVs(0, bq.element.uvs);
						if ( bq.element.uv2.Length > 0 )		mesh.SetUVs(1, bq.element.uv2);
						if ( bq.element.normals.Length > 0 )	mesh.SetNormals(bq.element.normals);
						if ( bq.element.tangents.Length > 0 )	mesh.SetTangents(bq.element.tangents);
						if ( bq.element.colors.Length > 0 )		mesh.SetColors(bq.element.colors);

						for ( int s = 0; s < mesh.subMeshCount; s++ )
						{
							if ( bq.element.tris[s].tris.Length > 0 )
								mesh.SetTriangles(bq.element.tris[s].tris, s);
							else
								mesh.SetTriangles(System.Array.Empty<int>(), s);
						}
						bq.element.mesh = mesh;

						mesh.RecalculateBounds();
						//bq.element.bounds = TransformBounds(mesh.bounds, bq.element.tm1);
						//TransformBoundsNew(mesh.bounds, bq.element.tm1, ref bq.element.bounds);
						if ( boundsMode == BoundsMode.Calc )
							TransformBoundsNew(mesh.bounds, bq.element.tm1, ref bq.bounds);
						else
						{
							bq.bounds.center = transform.position;
							bq.bounds.extents = boundsValue;
						}

						Renderer mr = renderers[bq.element.id];
						//Renderer mr = renderers[bq.element.gameObject];

						bq.element.rp = new RenderParams[bq.element.draw.Count];

						for ( int m = 0; m < bq.element.draw.Count; m++ )
						{
							int index = bq.element.draw[m];
							RenderParams rp = new RenderParams(bq.element.mats[index]);

							rp.layer					= (int)mr.gameObject.layer;	//(int)mr.renderingLayerMask;
							rp.renderingLayerMask		= mr.renderingLayerMask;
							rp.rendererPriority			= mr.rendererPriority;
							rp.worldBounds				= bq.bounds;	//element.bounds;
							rp.motionVectorMode			= mr.motionVectorGenerationMode;
							rp.reflectionProbeUsage		= mr.reflectionProbeUsage;
							rp.shadowCastingMode		= mr.shadowCastingMode;
							rp.receiveShadows			= mr.receiveShadows;
							rp.lightProbeUsage			= mr.lightProbeUsage;
#if UNITY_2023_2_OR_NEWER
#if UNITY_6000_3_OR_NEWER
							rp.entityId					= mr.GetEntityId();
#else
							rp.instanceID				= mr.GetInstanceID();
#endif
#endif
							bq.element.rp[m]			= rp;
						}
					}

					Matrix4x4 m4;
					Color col;

					if ( customDismantle )
					{
						customDismantle.Remove(bq.element, a, bq.piece, out m4, out col);
						m4 = transform.localToWorldMatrix * (bq.element.tm * m4);
					}
					else
						m4 = RemoveElement(bq, bq.element, a, deltatime);

					//if ( placing )
					//placing.PlacingElement(bq.element, bq.piece, m4, bq.calpha);
					//TransformBoundsNew(bq.element.mesh.bounds, m4, ref rpbounds);
					if ( boundsMode == BoundsMode.Calc )
						TransformBoundsNew(bq.element.mesh.bounds, m4, ref bq.bounds);

					for ( int m = 0; m < bq.element.draw.Count; m++ )
					{
						int index = bq.element.draw[m];
						bq.element.rp[m].worldBounds = bq.bounds;	//rpbounds;

						Graphics.RenderMesh(bq.element.rp[m], bq.element.mesh, index, m4);
					}

					if ( bq.calpha > 0.999f )
					{
						buildQueue.RemoveAt(i);

						if ( dismantleEvents.Contains(buildData.sorted[bq.piece]) )
						{
							dismantleEvent.Invoke(this, buildData.sorted[bq.piece]);
							DismantledEvent.Invoke(this);
						}

						if ( customDismantle )
							customDismantle.Dismantled(bq.element, bq.piece);
					}
				}
			}
		}

		void DismantleComplete()
		{
			buildMode = BuildMode.None;

			DismantledEvent.Invoke(this);
			//for ( int i = 0; i < onDismantled.Length; i++ )
			//{
			//	if ( onDismantled[i] )
			//		onDismantled[i].buildMode = BuildMode.Dismantle;
			//}

			//ShowTarget();
		}


		void EnableSpecials()
		{
			if ( !particlesOn && buildProgress >= enableParticles )
			{
				particlesOn = true;
				for ( int i = 0; i < particles.Count; i++ )
				{
					particles[i].particle.transform.localScale = Vector3.zero;
					StartCoroutine(StartParticle(particles[i]));
				}
			}

			if ( !lightsOn && buildProgress >= enableLights )
			{
				lightsOn = true;
				for ( int i = 0; i < lights.Count; i++ )
				{
					lights[i].light.intensity = 0.0f;
					StartCoroutine(StartLight(lights[i]));
				}
			}

			if ( !linesOn && buildProgress >= enableLines )
			{
				linesOn = true;
				for ( int i = 0; i < lines.Count; i++ )
				{
					lines[i].linerender.transform.localScale = Vector3.zero;
					lines[i].linerender.enabled = true;
					StartCoroutine(StartLine(lines[i]));
				}
			}

			if ( !audioOn && buildProgress >= enableAudio )
			{
				audioOn = true;
				for ( int i = 0; i < audios.Count; i++ )
				{
					audios[i].audio.volume = 0.0f;
					StartCoroutine(StartAudio(audios[i]));
				}
			}
		}

		void Build()
		{
			if ( !nodetm )
				nodetm = target.transform;

			if ( !buildData || !nodetm )
				return;

			if ( random == null )
			{
				random = new System.Random(seed);
			}

			if ( buildMode == BuildMode.Dismantle )
			{
				Dismantle();
				return;
			}

			if ( buildMode == BuildMode.Finished )
				return;

#if UNITY_EDITOR
			float deltatime = (float)(UnityEditor.EditorApplication.timeSinceStartup - editorTime);
			if ( !Application.isPlaying )
			{
				deltatime = deltatime * editPlaySpeed;	//(Time.time - editorTime) * editPlaySpeed;	// Time.deltaTime;
			}
			else
			{
				deltatime = Time.deltaTime;
				if ( useUnscaledTime )
					deltatime = Time.unscaledDeltaTime;
			}

			editorTime = UnityEditor.EditorApplication.timeSinceStartup;	//Time.time;
#else
			float deltatime = Time.deltaTime;
			if ( useUnscaledTime )
				deltatime = Time.unscaledDeltaTime;
#endif
			if ( buildMode != BuildMode.None )
			{
				float prevLevel = buildLevel;
				int prevPiece = (int)buildLevel;

				float bspd = buildTime / buildData.meshes.Count;

				switch ( buildMode )
				{
					case BuildMode.Build:
						if ( buildTo > 0.0f )
						{
							if ( buildLevel < (buildTo * buildData.meshes.Count) )
							{
								buildLevel += (deltatime / bspd) * addAmount;
								if ( buildLevel > (buildTo * buildData.meshes.Count) )
								{
									buildLevel = (buildTo * buildData.meshes.Count);
								}
								//Debug.Log("bt " + buildTo + " bl " + buildLevel);
								//addAmount = 0.0f;
							}
							else
								addAmount = 0.0f;
						}
						else
						{
							if ( useBuildTimeCrv )
							{
								float btv = buildTimeCrv.Evaluate(buildProgress);
								if ( btv <= 0.001f )
									btv = 0.001f;

								buildLevel += ((deltatime / bspd) * btv);	// + addAmount;
								//addAmount = 0.0f;
							}
							else
							{
								buildLevel += (deltatime / bspd);	// + addAmount;
								//addAmount = 0.0f;
							}
						}

						if ( buildLevel > (float)buildData.meshes.Count )
						{
							buildLevel = (float)buildData.meshes.Count;
						}
						break;

					//case BuildMode.Dismantle:	buildLevel -= Time.deltaTime * dismantleSpeed; break;
				}

				int bl = (int)buildLevel;
				if ( bl > buildData.meshes.Count )
					bl = buildData.meshes.Count;

				int piece = bl;

				buildProgress = buildLevel / (float)buildData.meshes.Count;

				if ( onCompleteBuildThese != null )
				{
					for ( int i = 0; i < onCompleteBuildThese.Length; i++ )
					{
						if ( buildProgress >= onCompleteBuildThese[i].level )
						{
							if ( onCompleteBuildThese[i].build && onCompleteBuildThese[i].build.buildMode != BuildMode.Build && onCompleteBuildThese[i].build.buildMode != BuildMode.Finished )
								onCompleteBuildThese[i].build.StartBuild();	//buildMode = BuildMode.Build;
						}
					}
				}

				for ( int i = prevPiece; i < piece; i++ )
				{
					BuildQueue bq = new BuildQueue();
					bq.piece = i;
					if ( buildStyle == BuildStyle.Appear )
						bq.calpha = 1.0f;
					else
						bq.calpha = 0.0f;

					bq.build = true;
					bq.element = buildData.meshes[buildData.sorted[i]];
					buildQueue.Add(bq);

					if ( customBuild )
						customBuild.AddedToBuild(bq.element, i);

					// Recalc spline
					//CalcSpline();
				}
			}

			// Swap in finished model when Built
			Matrix4x4 tm = Matrix4x4.Translate(nodetm.TransformPoint(Vector3.zero));	// Dont need
			Matrix4x4 localTM = nodetm.localToWorldMatrix;

			for ( int i = 0; i < built.Count; i++ )
			{
				for ( int m = 0; m < built[i].element.draw.Count; m++ )
				{
					int index = built[i].element.draw[m];
					Graphics.RenderMesh(built[i].element.rp[m], built[i].element.mesh, index, localTM * built[i].element.tm);
				}
			}

			Vector3 dir = Vector3.up;
			float a = 0.0f;

			for ( int i = 0; i < buildQueue.Count; )
			{
				BuildQueue bq = buildQueue[i];

				Vector3 p = Vector3.zero;

				BuildStyle bs = bq.element.buildStyle;
				AudioClip clip = null;
				float pa = 0.0f;
				float placespeed = bq.element.placeTime;

				bool first = false;
				if ( bq.calpha == 0.0f )
					first = true;

				float ta = bq.calpha;
				if ( buildMode == BuildMode.Build || buildMode == BuildMode.Click )
				{
					if ( bq.element.placeMode == PlaceMode.Speed )
						bq.calpha += (deltatime * (placespeed / bq.element.buildDist));	// / bq.element.buildDist;
					else
						bq.calpha += (deltatime / placespeed);
				}

				bq.calpha = Mathf.Clamp01(bq.calpha);
				if ( bq.element.usePlaceCurve )
					a = 1.0f - bq.element.placeCurve.Evaluate(bq.calpha);
				else
					a = 1.0f - bq.calpha;

				if ( clip && ta < pa && bq.calpha >= pa )
					PlaySound(clip);

				if ( bq.element.mesh == null )
				{
					Mesh mesh = new Mesh();

					if ( bq.element.verts.Length > 65535 )
						mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

					mesh.subMeshCount = bq.element.subMeshCount;
					mesh.SetVertices(bq.element.verts);
					if ( bq.element.uvs.Length > 0 ) mesh.SetUVs(0, bq.element.uvs);
					if ( bq.element.uv2 != null && bq.element.uv2.Length > 0 ) mesh.SetUVs(1, bq.element.uv2);
					//if ( newUV1.Count > 0 ) mesh.SetUVs(1, newUV1);
					//if ( newUV3.Count > 0 ) mesh.SetUVs(2, newUV3);
					//if ( newUV4.Count > 0 ) mesh.SetUVs(3, newUV4);
					if ( bq.element.normals.Length > 0 )	mesh.SetNormals(bq.element.normals);
					if ( bq.element.tangents.Length > 0 )	mesh.SetTangents(bq.element.tangents);
					if ( bq.element.colors.Length > 0 )		mesh.SetColors(bq.element.colors);

					for ( int s = 0; s < mesh.subMeshCount; s++ )
					{
						if ( bq.element.tris[s].tris.Length > 0 )
							mesh.SetTriangles(bq.element.tris[s].tris, s);
						else
							mesh.SetTriangles(System.Array.Empty<int>(), s);
					}

					mesh.RecalculateBounds();
					bq.element.mesh = mesh;
					//bq.element.bounds = TransformBounds(mesh.bounds, bq.element.tm1);
					if ( boundsMode == BoundsMode.Calc )
						TransformBoundsNew(mesh.bounds, bq.element.tm1, ref bq.bounds);
					else
					{
						bq.bounds.center = transform.position;
						bq.bounds.extents = boundsValue;
					}

					//Renderer mr = renderers[bq.element.gameObject];
					Renderer mr = renderers[bq.element.id];

					bq.element.rp = new RenderParams[bq.element.draw.Count];

					for ( int m = 0; m < bq.element.draw.Count; m++ )
					{
						int index = bq.element.draw[m];
						RenderParams rp = new RenderParams(bq.element.mats[index]);

						rp.layer				= (int)mr.gameObject.layer;	//mr.la mr.renderingLayerMask;
						rp.renderingLayerMask	= mr.renderingLayerMask;
						rp.rendererPriority		= mr.rendererPriority;
						rp.worldBounds			= bq.bounds;	//bq.element.bounds;	//new Bounds(Vector3.zero, Vector3.one * 1000f);	//bq.element.bounds;
						rp.motionVectorMode		= mr.motionVectorGenerationMode;
						rp.reflectionProbeUsage	= mr.reflectionProbeUsage;
						rp.shadowCastingMode	= mr.shadowCastingMode;
						rp.receiveShadows		= mr.receiveShadows;
						rp.lightProbeUsage		= mr.lightProbeUsage;
#if UNITY_2023_2_OR_NEWER
#if UNITY_6000_3_OR_NEWER
						rp.entityId				= mr.GetEntityId();
#else
						rp.instanceID = mr.GetInstanceID();
#endif
#endif
						bq.element.rp[m]		= rp;
					}
				}

				Matrix4x4 m4;
				Color col;

				if ( customBuild )
				{
					customBuild.Place(bq.element, 1.0f - a, bq.piece, out m4, out col);
					m4 = transform.localToWorldMatrix * (bq.element.tm * m4);
				}
				else
					m4 = PlaceElement(bq.element, 1.0f - a, bq.calpha);

				if ( placing )
				{
					if ( first )
						placing.StartingElement(bq.element, bq.piece, m4);

					placing.PlacingElement(bq.element, bq.piece, m4, bq.alpha);
				}

#if UNITY_EDITOR
				bq.pos = m4.GetPosition();
#endif
				if ( boundsMode == BoundsMode.Calc )
					TransformBoundsNew(bq.element.mesh.bounds, m4, ref bq.bounds);

				for ( int m = 0; m < bq.element.draw.Count; m++ )
				{
					int index = bq.element.draw[m];

					//bq.element.rp[m].worldBounds = TransformBounds(bq.element.mesh.bounds, m4);
					bq.element.rp[m].worldBounds = bq.bounds;	//rpbounds;

					Graphics.RenderMesh(bq.element.rp[m], bq.element.mesh, index, m4);
				}

				if ( bq.calpha > 0.999f )
				{
					buildQueue.RemoveAt(i);
					built.Add(bq);
					dismantle.Add(bq);

					if ( bq.element.lastElement.HasFlag(LastElement.Last) )
					{
						//GameObject comobj = CompletedObject(bq.element.gameObject);
						GameObject comobj = CompletedObject(bq.element.id);	// TODO: need to do something with this, just get the render and use the gameobject

						if ( comobj )
						{
							completedObjectEvent.Invoke(this, comobj);
						}
#if true
						// remove all from built list that share the mr and enable the object/renderer
						for ( int j = built.Count - 1; j >= 0; j-- )
						{
							if ( built[j].element.id == bq.element.id )
							{
								//dismantle.Add(built[j]);
								built.RemoveAt(j);
							}
						}
						//renderers[bq.element.gameObject].enabled = true;
						renderers[bq.element.id].enabled = true;

						if ( bq.element.lastElement.HasFlag(LastElement.LOD) )
						{
							LODGroup lg = GetComponentInParent<LODGroup>();
							if ( lg )
							{
								lg.ForceLOD(-1);
							}
						}
#endif
					}

					if ( placeEvents.Contains(buildData.sorted[bq.piece]) )
					{
						placedEvent.Invoke(this, buildData.sorted[bq.piece]);
						PlacedEvent.Invoke(this, buildData.sorted[bq.piece]);
					}

					if ( customBuild )
						customBuild.Built(bq.element, bq.piece);

					if ( placing )
					{
						placing.PlacedElement(bq.element, bq.piece, m4);
					}
				}
				else
					i++;
			}

			EnableSpecials();

			if ( built.Count == 0 && buildQueue.Count == 0 && (int)buildLevel >= buildData.meshes.Count )
			{
				BuildComplete();
			}
		}

		GameObject CompletedObject(string objname)
		{
			for ( int i = 0; i < completedObjects.Count; i++ )
			{
				if ( completedObjects[i].name.Equals(objname) )
				{
					return completedObjects[i];
				}
			}

			return null;
		}

		GameObject CompletedObject(int id)
		{
			for ( int i = 0; i < completedObjects.Count; i++ )
			{
				if ( completedObjects[i].GetComponent<ObjectID>().id == id )
				{
					return completedObjects[i];
				}
			}

			return null;
		}


		public void ShowPlacing(Material placingmat)
		{
			if ( placingmat )
			{
				RenderParams rp = new RenderParams(placingmat);

				for ( int i = 0; i < buildQueue.Count; i++ )
				{
					BuildQueue bq = buildQueue[i];

					for ( int j = 0; j < bq.element.draw.Count; j++ )
					{
						int index = bq.element.draw[j];
						Graphics.RenderMesh(rp, bq.element.mesh, index, transform.localToWorldMatrix * bq.element.tm);
					}
				}
			}
		}


		public void ShowUnBuiltElements(Material unbuiltmat, bool includePlacing = true, Material placingmat = null)
		{
			RenderParams rp = new RenderParams(unbuiltmat);

			int bl = (int)buildLevel;
			if ( bl > buildData.meshes.Count )
				bl = buildData.meshes.Count;

			for ( int i = bl; i < buildData.meshes.Count; i++ )
			{
				MeshElement me = buildData.meshes[buildData.sorted[i]];

				if ( me.mesh == null )
				{
					Mesh mesh = new Mesh();

					if ( me.verts.Length > 65535 )
						mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

					mesh.subMeshCount = me.subMeshCount;
					mesh.SetVertices(me.verts);
					if ( me.uvs.Length > 0 ) mesh.SetUVs(0, me.uvs);
					if ( me.uv2 != null && me.uv2.Length > 0 ) mesh.SetUVs(1, me.uv2);
					if ( me.normals.Length > 0 )	mesh.SetNormals(me.normals);
					if ( me.tangents.Length > 0 )	mesh.SetTangents(me.tangents);
					if ( me.colors.Length > 0 )		mesh.SetColors(me.colors);

					for ( int s = 0; s < mesh.subMeshCount; s++ )
					{
						if ( me.tris[s].tris.Length > 0 )
							mesh.SetTriangles(me.tris[s].tris, s);
						else
							mesh.SetTriangles(System.Array.Empty<int>(), s);
					}

					mesh.RecalculateBounds();
					me.mesh = mesh;
				}

				for ( int j = 0; j < me.draw.Count; j++ )
				{
					int index = me.draw[j];
					Graphics.RenderMesh(rp, me.mesh, index, transform.localToWorldMatrix * me.tm);
				}
			}

			if ( includePlacing )
			{
				if ( placingmat )
					rp = new RenderParams(placingmat);

				for ( int i = 0; i < buildQueue.Count; i++ )
				{
					BuildQueue bq = buildQueue[i];

					for ( int j = 0; j < bq.element.draw.Count; j++ )
					{
						int index = bq.element.draw[j];
						Graphics.RenderMesh(rp, bq.element.mesh, index, transform.localToWorldMatrix * bq.element.tm);
					}
				}
			}
		}

		public void SetBuiltLevel(float alpha)
		{
#if UNITY_EDITOR
			if ( alpha < 0.0f || alpha > 1.0f )
			{
				Debug.LogWarning("SetBuiltLevel Alpha should be 0-1", gameObject);
			}
#endif
			alpha = Mathf.Clamp01(alpha);
			buildLevel = alpha * buildData.meshes.Count;
			buildProgress = buildLevel / (float)buildData.meshes.Count;

			built.Clear();
			dismantle.Clear();
			buildQueue.Clear();
			HideTarget();

			buildMode = BuildMode.Pause;

			int bl = (int)buildLevel;
			for ( int i = 0; i < bl; i++ )
			{
				MeshElement me = buildData.meshes[buildData.sorted[i]];

				BuildQueue bq = new BuildQueue();
				bq.piece	= i;
				bq.calpha	= 1.0f;
				bq.build	= true;
				bq.element	= me;





				if ( bq.element.mesh == null )
				{
					Mesh mesh = new Mesh();

					if ( bq.element.verts.Length > 65535 )
						mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

					mesh.subMeshCount = bq.element.subMeshCount;
					mesh.SetVertices(bq.element.verts);
					if ( bq.element.uvs.Length > 0 ) mesh.SetUVs(0, bq.element.uvs);
					if ( bq.element.uv2 != null && bq.element.uv2.Length > 0 ) mesh.SetUVs(1, bq.element.uv2);
					//if ( newUV1.Count > 0 ) mesh.SetUVs(1, newUV1);
					//if ( newUV3.Count > 0 ) mesh.SetUVs(2, newUV3);
					//if ( newUV4.Count > 0 ) mesh.SetUVs(3, newUV4);
					if ( bq.element.normals.Length > 0 )	mesh.SetNormals(bq.element.normals);
					if ( bq.element.tangents.Length > 0 )	mesh.SetTangents(bq.element.tangents);
					if ( bq.element.colors.Length > 0 )		mesh.SetColors(bq.element.colors);

					for ( int s = 0; s < mesh.subMeshCount; s++ )
					{
						if ( bq.element.tris[s].tris.Length > 0 )
							mesh.SetTriangles(bq.element.tris[s].tris, s);
						else
							mesh.SetTriangles(System.Array.Empty<int>(), s);
					}

					mesh.RecalculateBounds();
					bq.element.mesh = mesh;
					//bq.element.bounds = TransformBounds(mesh.bounds, bq.element.tm1);
					if ( boundsMode == BoundsMode.Calc )
						TransformBoundsNew(mesh.bounds, bq.element.tm1, ref bq.bounds);
					else
					{
						bq.bounds.center = transform.position;
						bq.bounds.extents = boundsValue;
					}

					//Renderer mr = renderers[bq.element.gameObject];
					Renderer mr = renderers[bq.element.id];

					bq.element.rp = new RenderParams[bq.element.draw.Count];

					for ( int m = 0; m < bq.element.draw.Count; m++ )
					{
						int index = bq.element.draw[m];
						RenderParams rp = new RenderParams(bq.element.mats[index]);

						rp.layer				= (int)mr.gameObject.layer;	//mr.la mr.renderingLayerMask;
						rp.renderingLayerMask	= mr.renderingLayerMask;
						rp.rendererPriority		= mr.rendererPriority;
						rp.worldBounds			= bq.bounds;//bq.element.bounds;	//new Bounds(Vector3.zero, Vector3.one * 1000f);	//bq.element.bounds;
						rp.motionVectorMode		= mr.motionVectorGenerationMode;
						rp.reflectionProbeUsage	= mr.reflectionProbeUsage;
						rp.shadowCastingMode	= mr.shadowCastingMode;
						rp.receiveShadows		= mr.receiveShadows;
						rp.lightProbeUsage		= mr.lightProbeUsage;
#if UNITY_2023_2_OR_NEWER
#if UNITY_6000_3_OR_NEWER
						rp.entityId				= mr.GetEntityId();
#else
						rp.instanceID = mr.GetInstanceID();
#endif
#endif
						bq.element.rp[m]		= rp;
					}
				}








				if ( customBuild )
					customBuild.AddedToBuild(bq.element, i);

				built.Add(bq);
				dismantle.Add(bq);

				if ( me.lastElement == LastElement.Last )
				{
					//Debug.Log("Last " + me.gameObject + " ds " + me.dismantleStyle);
					//GameObject comobj = CompletedObject(me.gameObject);
					GameObject comobj = CompletedObject(me.id);

					if ( comobj )
						completedObjectEvent.Invoke(this, comobj);

					//renderers[me.gameObject].enabled = true;
					renderers[me.id].enabled = true;

					if ( me.lastElement.HasFlag(LastElement.LOD) )
					{
						LODGroup lg = GetComponentInParent<LODGroup>();
						if ( lg )
							lg.ForceLOD(-1);
					}

					for ( int j = built.Count - 1; j >= 0; j-- )
					{
						if ( built[j].element.id == bq.element.id )
							built.RemoveAt(j);
					}
				}
			}
		}

		void ShowPlacement()
		{
			if ( showPlacement )
			{
				RenderParams rp = new RenderParams(placeMaterial);
				rp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

				Matrix4x4 tm = Matrix4x4.Translate(transform.TransformPoint(Vector3.zero));

				for ( int i = 0; i < buildQueue.Count; i++ )
				{
					MeshElement me = buildQueue[i].element;
					Vector3 p = transform.TransformPoint(Vector3.zero);

					Graphics.RenderMesh(rp, me.mesh, 0, transform.localToWorldMatrix * me.tm);
				}
			}
		}

		void ShowElement(int index)
		{
			Matrix4x4 tm = Matrix4x4.Translate(transform.TransformPoint(Vector3.zero));

			MeshElement me = buildData.meshes[index];
			Vector3 p = transform.TransformPoint(Vector3.zero);
			RenderParams rp = new RenderParams(placeMaterial);
			rp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

			for ( int i = 0; i < me.mats.Length; i++ )
				Graphics.RenderMesh(rp, me.mesh, i, transform.localToWorldMatrix * me.tm);
		}

		void ManualBuild()
		{
			if ( buildMode == BuildMode.Click )
			{
				if ( !clickHere && buildClickPrefab )
				{
					clickHere = Instantiate(buildClickPrefab);
					BuildClick bc = clickHere.GetComponent<BuildClick>();
					if ( bc )
						bc.artificer = this;

					if ( canvas )
					{
						Transform ctm = clickHere.transform;
						ctm.parent = canvas.GetComponent<RectTransform>();
					}
				}

				if ( showingPart == buildIndex )
				{
					showingPart++;

				}

				if ( showingPart >= buildData.sorted.Count )
					return;

				if ( buildBit )
				{
					buildBit = false;
					buildIndex = showingPart;

					BuildQueue bq = new BuildQueue();
					bq.piece = buildIndex;
					if ( buildStyle == BuildStyle.Appear )
						bq.calpha = 1.0f;
					else
						bq.calpha = 0.0f;

					bq.build = true;
					bq.element = buildData.meshes[buildData.sorted[buildIndex]];
					buildQueue.Add(bq);
				}

				if ( clickHere )
				{
					Vector3 pos = Vector3.zero;

					switch ( clickBuildPos )
					{
						case ClickBuildPos.Object:
							pos = transform.position;
							break;
						case ClickBuildPos.Part:
							pos = transform.TransformPoint(buildData.meshes[buildData.sorted[showingPart]].center);
							break;
					}

					if ( canvas )
						clickHere.transform.position = Camera.main.WorldToScreenPoint(pos);	//transform.TransformPoint(buildData.meshes[buildData.sorted[showingPart]].center));
					else
						clickHere.transform.position = transform.TransformPoint(pos);	//buildData.meshes[buildData.sorted[showingPart]].center);
				}

				ShowElement(buildData.sorted[showingPart]);
			}
		}

		Material RemapMat(Material m)
		{
			for ( int i = 0; i < materialRemap.Length; i++ )
			{
				if ( materialRemap[i].from == m )
					return materialRemap[i].to;
			}

			return m;
		}

		Dictionary<Material, Material>	remappedMats;

		public void RemapMaterials(ref Material[] mats)
		{
			remappedMats = new Dictionary<Material, Material>();

			if ( materialRemap != null && materialRemap.Length > 0 && mats.Length > 0 )
			{
				for ( int i = 0; i < mats.Length; i++ )
				{
					Material m = RemapMat(mats[i]);

					remappedMats.Add(m, mats[i]);
					mats[i] = m;
				}
			}
		}

		public bool IsBuilt()
		{
			if ( buildMode == BuildMode.Finished )
				return true;

			return false;
		}

		public bool IsDismantled()
		{
			if ( buildMode == BuildMode.None )
				return true;

			return false;
		}

		public static Bounds TransformBounds(Bounds localBounds, Matrix4x4 localToWorld)
		{
			Vector3 center = localBounds.center;
			Vector3 extents = localBounds.extents;

			Vector3[] corners = new Vector3[8]
			{
				new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z),
				new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z),
				new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z),
				new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z),

				new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z),
				new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z),
				new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z),
				new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z),
			};

			for ( int i = 0; i < corners.Length; i++ )
				corners[i] = localToWorld.MultiplyPoint3x4(corners[i]);

			Bounds worldBounds = new Bounds(corners[0], Vector3.zero);
			for ( int i = 1; i < corners.Length; i++ )
				worldBounds.Encapsulate(corners[i]);

			return worldBounds;
		}


		static Vector3[]	corners = new Vector3[8];

		public static void TransformBoundsNew(Bounds localBounds, Matrix4x4 localToWorld, ref Bounds worldBounds)
		{
			Vector3 center = localBounds.center;
			Vector3 extents = localBounds.extents;

			corners[0] = new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z);
			corners[1] = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
			corners[2] = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
			corners[3] = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);

			corners[4] = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
			corners[5] = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
			corners[6] = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
			corners[7] = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);

			for ( int i = 0; i < corners.Length; i++ )
				corners[i] = localToWorld.MultiplyPoint3x4(corners[i]);

			for ( int i = 0; i < corners.Length; i++ )
				worldBounds.Encapsulate(corners[i]);
		}

		public Matrix4x4 PlaceElement(MeshElement me, float calpha, float alpha)
		{
			float a = 0.0f;
			AnimationCurve rotcrv = me.placeRotCurve;
			AnimationCurve crv = me.placeCurve;
			AnimationCurve sclcrv = me.placeScaleCurve;

			Vector3 rot;

			if ( me.useRotCurve )
				rot = me.rotate * (1.0f - rotcrv.Evaluate(alpha));
			else
				rot = me.rotate * (1.0f - alpha);

			switch ( rotateMode )
			{
				case RotateMode.Normal:
					break;
				case RotateMode.FlipXAxis:
					if ( me.center.x < 0.0f )
						rot = -rot;
					break;
				case RotateMode.FlipYAxis:
					if ( me.center.y < 0.0f )
						rot = -rot;
					break;
				case RotateMode.FlipZAxis:
					if ( me.center.z < 0.0f )
						rot = -rot;
					break;
			}

			Vector3 scale = Vector3.one;
			float sa = alpha;

			if ( me.perAxisScale )
			{
				if ( me.useScaleCurve )
				{
					scale.x = sclcrv.Evaluate(alpha);
					scale.y = me.placeScaleCurveY.Evaluate(alpha);
					scale.z = me.placeScaleCurveZ.Evaluate(alpha);
				}
				else
				{
					sa = alpha;
					scale.x = sa;
					scale.y = sa;
					scale.z = sa;
				}
			}
			else
			{
				if ( me.useScaleCurve )
					sa = sclcrv.Evaluate(alpha);
				else
					sa = alpha;

				scale.x = sa;
				scale.y = sa;
				scale.z = sa;
			}

			Vector3 p;
			if ( me.usePlaceCurve )
				a = crv.Evaluate(alpha);
			else
				a = alpha;

			switch ( me.buildStyle )
			{
				case BuildStyle.None:
					break;

				case BuildStyle.Appear:
					return transform.localToWorldMatrix * me.tm;

				case BuildStyle.Radial:
					{
#if false
						dir = (me.center - me.origin);// - me.center);  //.normalized;
						dir.x *= me.projection.x;
						dir.y *= me.projection.y;
						dir.z *= me.projection.z;
						p = (dir.normalized * me.buildDist * (1.0f - a));
#else
						p = (me.dir * me.buildDist * (1.0f - a));
#endif
						Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
						m4 = transform.localToWorldMatrix * (me.tm * m4);

						return m4;
					}

				case BuildStyle.Vertical:
					{
						// We can do this in the build
#if false
						dir = (me.center - me.origin);	// - me.center);  //.normalized;
						dir.x *= me.projection.x;
						dir.y *= me.projection.y;
						dir.z *= me.projection.z;
						p = (dir.normalized * me.buildDist * (1.0f - a));
#else
						p = (me.dir * me.buildDist * (1.0f - a));
#endif
						Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
						return transform.localToWorldMatrix * (me.tm * m4);
					}

				case BuildStyle.Transform:
					{
						if ( me.havePath && me.path != null && me.path.Count > 1 )
						{
							p = me.path.EvaluatePosition(a);
							Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
							return transform.localToWorldMatrix * m4;
						}
						else
						{
#if false
							dir = (me.center - me.origin);	// - me.center);  //.normalized;
							dir.x *= me.projection.x;
							dir.y *= me.projection.y;
							dir.z *= me.projection.z;
							p = (dir.normalized * me.buildDist * (1.0f - a));
#else
							p = (me.dir * me.buildDist * (1.0f - a));
#endif
							Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
							return transform.localToWorldMatrix * (me.tm * m4);
						}
					}
			}

			return transform.localToWorldMatrix;
		}

		public Matrix4x4 RemoveElement(BuildQueue bq, MeshElement me, float calpha, float dt)
		{
			float a = 0.0f;
			AnimationCurve rotcrv	= disPlaceRotCurve;
			AnimationCurve crv		= disPlaceCurve;
			AnimationCurve sclcrv	= disPlaceScaleCurve;

			Vector3 rot;

			if ( useDisPlaceRotCurve )
				rot = dismantleRotate * (1.0f - rotcrv.Evaluate(calpha));
			else
				rot = dismantleRotate * (1.0f - calpha);

			float sa = calpha;

			Vector3 scale = Vector3.one;
			if ( me.dismantleStyle != DismantleStyle.Explode )
			{
				if ( useDisPlaceScaleCurve )
					sa = sclcrv.Evaluate(calpha);
				else
					sa = calpha;

				scale.x = sa;
				scale.y = sa;
				scale.z = sa;
			}

			Vector3 p;
			if ( useDisPlaceCurve )
				a = crv.Evaluate(calpha);
			else
				a = calpha;

			switch ( me.dismantleStyle )
			{
				case DismantleStyle.None:
					break;

				case DismantleStyle.Vanish:
					break;

				case DismantleStyle.Radial:
				{
#if false
					dir = (me.center - me.origin);	// - me.center);	//.normalized;
					p = (dir.normalized * me.buildDist * (1.0f - calpha));
#else
					p = (me.dir * me.buildDist * (1.0f - a));
#endif
					Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
					m4 = transform.localToWorldMatrix * (me.tm * m4);

					return m4;
				}

				case DismantleStyle.Vertical:
				{
#if false
					dir = (me.origin - me.center);	//Vector3.up;
					p = (dir.normalized * me.buildDist * (1.0f - calpha));
#else
					p = (me.dir * me.buildDist * (1.0f - a));
#endif
					Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
					return transform.localToWorldMatrix * (me.tm * m4);
				}

				case DismantleStyle.Transform:
				{
					if ( me.havePath && me.path != null && me.path.Count > 1 )
					{
						p = me.path.EvaluatePosition(a);
						Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
						return transform.localToWorldMatrix * m4;
					}
					else
					{
#if false
						dir = (me.origin - me.center);	//.normalized;
						p = (dir.normalized * me.buildDist * (1.0f - a));
#else
						p = (me.dir * me.buildDist * (1.0f - a));
#endif
						Matrix4x4 m4 = Matrix4x4.TRS(p, Quaternion.Euler(rot), scale);
						return transform.localToWorldMatrix * (me.tm * m4);
					}
				}

				case DismantleStyle.Explode:
				{
					if ( !bq.sleep )	//bq.velocity.sqrMagnitude > 0.0f )
					{
						bq.velocity -= bq.velocity * Mathf.Min(1.0f, me.linearDrag * dt);
						bq.velocity += (bq.force + (Physics.gravity * me.gravityModifier)) * dt;
					}
					else
						bq.velocity = Vector3.zero;

					if ( bq.angvel.sqrMagnitude > 0.0f )
						bq.angvel -= bq.angvel * Mathf.Min(1.0f, me.angularDrag * dt);

					bq.rot += bq.angvel * dt;

					switch ( me.collisionMode )
					{
						case CollisionMode.None:
							break;
						case CollisionMode.Simple:
							bq.pos += bq.velocity * dt;
							if ( bq.pos.y < me.collisionY )
							{
								bq.pos.y = me.collisionY;
								bq.velocity.y = Mathf.Abs(bq.velocity.y);
								bq.velocity	*= me.bounce;
								bq.angvel	*= me.bounce;
							}
							break;

						case CollisionMode.Raycast:
							if ( !bq.velocity.Equals(Vector3.zero) && Physics.RaycastNonAlloc(bq.pos, bq.velocity, hits, bq.velocity.magnitude * dt, me.layers, QueryTriggerInteraction.Ignore) > 0 )
							{
								bq.pos = hits[0].point + hits[0].normal * 0.01f;
								bq.velocity = Vector3.Reflect(bq.velocity, hits[0].normal) * me.bounce;
								bq.angvel *= me.bounce;
								if ( bq.velocity.sqrMagnitude < 0.01f )
								{
									bq.velocity = Vector3.zero;
									bq.sleep = true;
								}
							}
							else
								bq.pos += bq.velocity * dt;
							break;
					}

					if ( useDisPlaceScaleCurve )
					{
						sa = sclcrv.Evaluate(calpha);

						scale.x = sa;	// * me.maxScale;
						scale.y = sa;	// * me.maxScale;
						scale.z = sa;	// * me.maxScale;
					}

					Matrix4x4 m4 = Matrix4x4.TRS(bq.pos, Quaternion.Euler(bq.rot), scale);
					return m4;
				}
			}

			return transform.localToWorldMatrix;
		}

		static RaycastHit[] hits = new RaycastHit[1];

		Material GetRemapped(Material m)
		{

			return m;
		}

		public int GetMatSortID(List<Material> mats)
		{
			if ( materialSortOrder.Count > 0 )
			{
				int low = 100;

				for ( int i = 0; i < mats.Count; i++ )
				{
					int id = materialSortOrder.IndexOf(mats[i]);
					if ( id >= 0 && id < low )
						low = id;
				}

				return low;
			}

			return 0;
		}

		public int GetMatSortID(Material mat)
		{
			if ( materialSortOrder.Count > 0 )
			{
				int low = 100;

				Material m = mat;
				if ( remappedMats.ContainsKey(mat) )
					m = remappedMats[mat];

				int id = materialSortOrder.IndexOf(m);	//at);
				if ( id >= 0 && id < low )
					low = id;

				return low;
			}

			return 0;
		}

		public static float GetNearestPoint(SplineContainer spline, Vector3 pos, out Vector3 nearest, out float alpha, int res = 4, int steps = 2)
		{
			Vector3 lpos = spline.transform.InverseTransformPoint(pos);

			float3 nearf3;

			float dist = SplineUtility.GetNearestPoint(spline.Spline, lpos, out nearf3, out alpha, res, 2);

			nearest = spline.transform.TransformPoint(nearf3);

			return dist;
		}

		public void CalcSpline(MeshElement me, SplitOptions so, Transform tm)
		{
			BuildFrom bf = null;

			Vector3 wpos = tm.TransformPoint(me.center);

			if ( so.buildFromOverride )
				bf = FindBuildFrom(so.buildFromObjects, wpos);
			else
				bf = FindBuildFrom(buildFromObjects, wpos);

			if ( bf != null )
				CalcSpline(me, me.tm1, bf.buildFrom, bf.buildFromBox, bf.buildFromOffset, bf.startTension, bf.endTension, bf.splineDirCalc, bf.splineEndProject, bf.splineMode, bf.startDir);	//   //Transform buildfrom, float tension1, float tension2)
		}

		// Have a recalcSpline for runtime so if the buildfrom has moved

		public void CalcSpline(MeshElement me, Matrix4x4 tm, Transform buildfrom, Vector3 buildfrombox, Vector3 buildfromoffset, float starttension, float endtension, SplineDir splinedir, Vector3 splineproject, SplineMode splinemode, Vector3 startDir)   //Transform buildfrom, float tension1, float tension2)
		{
			me.havePath = false;
			if ( buildfrom == null )
				return;
			me.path = new Spline();
			me.havePath = true;

			Vector3 startvec, endvec, startpoint, endpoint;

			SplineContainer spl = buildfrom.GetComponent<SplineContainer>();

			Quaternion rot1 = buildfrom.rotation;
			Quaternion rot2 = transform.rotation;
			Vector3 fp = Vector3.zero;
			if ( spl )
			{
				float a;
				GetNearestPoint(spl, tm.MultiplyPoint(me.center), out startpoint, out a);
				startpoint = transform.worldToLocalMatrix.MultiplyPoint3x4(startpoint);
			}
			else
			{
				fp.x = RandomRange(-buildfrombox.x * 0.5f, buildfrombox.x * 0.5f);
				fp.y = RandomRange(-buildfrombox.y * 0.5f, buildfrombox.y * 0.5f);
				fp.z = RandomRange(-buildfrombox.z * 0.5f, buildfrombox.z * 0.5f);

				fp += buildfromoffset;

				fp = buildfrom.TransformPoint(fp);

				startpoint = transform.worldToLocalMatrix.MultiplyPoint3x4(fp);
			}

			endpoint = me.tm.GetPosition();

			startvec = starttension * (rot1 * startDir);	//Vector3.up);

			Vector3 dir = Vector3.up;
			switch ( splinedir )
			{
				case SplineDir.Origin:		dir = (me.origin - endpoint); break;
				case SplineDir.SortOrigin:	dir = (me.sortOrigin - endpoint); break;
			}

			//dir = transform.InverseTransformDirection(dir);

			dir = Vector3.Scale(dir, splineproject).normalized;

			//endvec = endtension * (rot2 * dir);
			endvec = endtension * dir;

			me.path.Add(new BezierKnot(startpoint, -startvec, startvec));
			me.path.Add(new BezierKnot(endpoint, -endvec, endvec));

			if ( spl )
			{
				switch ( splinemode )
				{
					case SplineMode.BuildFrom:
						// Just use the spline as is
						break;

					case SplineMode.MoveAlong:
						me.path = CombineSplinesWithC2Join(spl, tm.MultiplyPoint(me.center), me.path, starttension);
						break;
				}
			}
		}

		public Spline CombineSplinesWithC2Join(SplineContainer firstSpline, Vector3 worldPoint, Spline newSpline, float starttension)	//float smoothFactor = 0.5f, float backDistance = 0.1f, float moveDistance = 0.1f)
		{
			Vector3 nearest;
			float t;

			float dist = GetNearestPoint(firstSpline, worldPoint, out nearest, out t);

			float totalLength = firstSpline.CalculateLength();
			float dtBack = 0.1f / totalLength;
			t = Mathf.Max(0f, t - dtBack);
			if ( t > 1.0f )
				t = 1.0f;

			Vector3 tangentFirst1 = target.transform.InverseTransformVector(firstSpline.EvaluateTangent(t)).normalized;
			Spline partialFirst = new Spline();
			float accumulated = 0f;

			Vector3 scale = firstSpline.transform.localScale;

			for ( int i = 0; i < firstSpline.Spline.Count; i++ )
			{
				BezierKnot k = firstSpline.Spline[i];
				if ( i > 0 )
					accumulated += Vector3.Distance(firstSpline.Spline[i].Position, firstSpline.Spline[i - 1].Position);

				float knotT = accumulated / totalLength;
				if ( knotT > t )
					break;

				k.Position		= target.transform.InverseTransformPoint(firstSpline.transform.TransformPoint(k.Position));
				k.TangentIn		= target.transform.InverseTransformVector(firstSpline.transform.TransformDirection(Vector3.Scale(k.TangentIn, scale)));
				k.TangentOut	= target.transform.InverseTransformVector(firstSpline.transform.TransformDirection(Vector3.Scale(k.TangentOut, scale)));

				partialFirst.Add(k);
			}

			for ( int i = 1; i < newSpline.Count; i++ )
				partialFirst.Add(newSpline[i]);

			BezierKnot k1 = partialFirst[partialFirst.Count - 2];
			k1.TangentIn = -tangentFirst1 * starttension;
			k1.TangentOut = tangentFirst1 * starttension;
			k1.Rotation = quaternion.identity;
			partialFirst[partialFirst.Count - 2] = k1;

			return partialFirst;
		}

		Dictionary<int, Adjust> adjustInfo = new Dictionary<int, Adjust>();
		Adjust[] adjs;

		void GetAdjusters()
		{
			adjs = target.GetComponentsInChildren<Adjust>();

			adjustInfo.Clear();

			for ( int i = 0; i < adjs.Length; i++ )
			{
				Adjust adj = adjs[i];

				if ( adj.enabled && adj.adjustMode == AdjustMode.Selection )
				{
					List<int> sel = Adjust.GetSelection(adj.selection);

					for ( int j = 0; j < sel.Count; j++ )
					{
						if ( adjustInfo.ContainsKey(sel[j]) )
						{
							adjustInfo[sel[j]] = adj;
						}
						else
						{
							adjustInfo.Add(sel[j], adj);
						}
					}
				}
			}
		}

		public Adjust CheckInAdjust(MeshElement me)
		{
			Adjust inAdj = null;
			if ( enabled )
			{
				Vector3 p = transform.TransformPoint(me.center);

				float priority = float.MaxValue;

				for ( int i = 0; i < adjs.Length; i++ )
				{
					float fp;	// = float.MaxValue;
					if ( adjs[i].InVolume(me, transform, out fp) )
					{
						if ( fp < priority )
						{
							inAdj = adjs[i];
							priority = fp;
						}
					}
				}
			}

			if ( inAdj )
			{
				inAdj.AddElement(me);
			}
			return inAdj;
		}

		public SplitOptions GetOptions(int i)
		{
			if ( adjustInfo.ContainsKey(i) )
			{
				Adjust adj = adjustInfo[i];
				if ( adj != null && adj.options != null )
					return adj.options;
			}

			return null;
		}

		public void PlayEditMode(bool play, int frames = 0)
		{
#if UNITY_EDITOR
			if ( editModePlay )
			{
				if ( !HaveMeshData() )
					UpdateMeshes();

				if ( !play )
				{
					editorTime = UnityEditor.EditorApplication.timeSinceStartup;
				}
				else
				{
					editorTime -= (UnityEditor.EditorApplication.timeSinceStartup - editorTime) * frames;
				}
				Build();
			}
#endif
		}
	}
}