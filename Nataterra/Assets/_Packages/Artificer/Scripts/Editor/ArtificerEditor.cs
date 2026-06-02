using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace Artifice
{
	[CustomEditor(typeof(Artificer))]
	[CanEditMultipleObjects]
	public class ArtificerEditor : Editor
	{
		Texture				logoImage;
		SerializedProperty	_target;
		SerializedProperty	_exclude;
		SerializedProperty	_origin;
		SerializedProperty	_sortOrigin;
		SerializedProperty	_useSortSpline;
		SerializedProperty	_sortSpline;
		SerializedProperty	_sortPathBias;
		SerializedProperty	_useSplitParams;
		SerializedProperty	_buildMode;
		SerializedProperty	_buildTime;
		SerializedProperty	_dismantleTime;
		SerializedProperty	_buildStyle;
		SerializedProperty	_placeMode;
		SerializedProperty	_placeTimeRange;
		SerializedProperty	_placeCurve;
		SerializedProperty	_placeMaterial;
		SerializedProperty	_maxScale;
		SerializedProperty	_buildOnAwake;
		SerializedProperty	_onCompleteBuildThese;
		SerializedProperty	_onDismantled;
		SerializedProperty	_onDismantleLevel;
		SerializedProperty	_enableOnBuilt;
		SerializedProperty	_disableOnBuilt;
		SerializedProperty	_materialRemap;
		SerializedProperty	_buildDistRange;
		SerializedProperty	_meshPivot;
		SerializedProperty	_rotate;
		SerializedProperty	_rotateRange;
		SerializedProperty	_usePlaceCurve;
		SerializedProperty	_useRotCurve;
		SerializedProperty	_useScaleCurve;
		SerializedProperty	_rotCurve;
		SerializedProperty	_scaleCurve;
		SerializedProperty	_perAxisScale;
		SerializedProperty	_scaleCurveY;
		SerializedProperty	_scaleCurveZ;
		SerializedProperty	_buildData;
		SerializedProperty	_sortMode;
		SerializedProperty	_sortDistanceMode;
		SerializedProperty	_reverseSortOrder;
		SerializedProperty	_materialSortOrder;
		SerializedProperty	_splitMode;
		SerializedProperty	_usePosition;
		SerializedProperty	_useUV;
		SerializedProperty	_useUV2;
		SerializedProperty	_useNormal;
		SerializedProperty	_useColor;
		SerializedProperty	_useMaterial;
		SerializedProperty	_dismantleStyle;
		SerializedProperty	_removeTime;
		SerializedProperty	_useDisPlaceCurve;
		SerializedProperty	_useDisPlaceRotCurve;
		SerializedProperty	_useDisPlaceScaleCurve;
		SerializedProperty	_disPlaceCurve;
		SerializedProperty	_disPlaceRotCurve;
		SerializedProperty	_disPlaceScaleCurve;
		SerializedProperty	_minExplodeForce;
		SerializedProperty	_maxExplodeForce;
		SerializedProperty	_dismantleRotate;
		SerializedProperty	_simpleCollision;
		SerializedProperty	_collisionY;
		SerializedProperty	_bounce;
		SerializedProperty	_linearDrag;
		SerializedProperty	_angularDrag;
		SerializedProperty	_explodeOrigin;
		SerializedProperty	_builtEvent;
		SerializedProperty	_placedEvents;
		SerializedProperty	_dismantleEvent;
		SerializedProperty	_dismantledEvent;
		SerializedProperty	_placeEventIDs;
		SerializedProperty	_dismantleEventIDs;
		SerializedProperty	_customBuild;
		SerializedProperty	_customDismantle;
		SerializedProperty	_placing;
		SerializedProperty	_buildFromObjects;
		SerializedProperty	_buildFromMode;
		SerializedProperty	_completedObjects;
		SerializedProperty	_completedObjectEvent;
		SerializedProperty	_projection;
		SerializedProperty	_useUnscaledTime;
		SerializedProperty	_useBuildTimeCrv;
		SerializedProperty	_buildTimeCrv;
		SerializedProperty	_positionTolerance;
		SerializedProperty	_uvTolerance;
		SerializedProperty	_normalTolerance;
		SerializedProperty	_colorTolerance;
		SerializedProperty	_rotateMode;
		SerializedProperty	_seed;
		SerializedProperty	_angVelRange;
		SerializedProperty	_gravityModifier;
		SerializedProperty	_particleEnableSpeed;
		SerializedProperty	_lightEnableSpeed;
		SerializedProperty	_particleEnableCrv;
		SerializedProperty	_lightEnableCrv;
		SerializedProperty	_lineEnableSpeed;
		SerializedProperty	_audioEnableSpeed;
		SerializedProperty	_lineEnableCrv;
		SerializedProperty	_audioEnableCrv;
		SerializedProperty	_forceRange;
		SerializedProperty	_dismantleProjection;
		SerializedProperty	_collisionMode;
		SerializedProperty	_layers;
		SerializedProperty	_enableLights;
		SerializedProperty	_enableLines;
		SerializedProperty	_enableParticles;
		SerializedProperty	_enableAudio;
		SerializedProperty	_startMode;
		SerializedProperty	_boundsMode;
		SerializedProperty	_boundsValue;
		SerializedProperty	_buildClickPrefab;
		SerializedProperty	_canvas;
		SerializedProperty	_clickBuildPos;
		GUIStyle			largeLabel;
		int					overMesh	= -1;
		Material			_wireMat;
		//float				testSetBuild;

		private void OnEnable()
		{
			_target					= serializedObject.FindProperty("target");
			_exclude				= serializedObject.FindProperty("exclude");
			_origin					= serializedObject.FindProperty("origin");
			_sortOrigin				= serializedObject.FindProperty("sortOrigin");
			_useSortSpline			= serializedObject.FindProperty("useSortSpline");
			_sortSpline				= serializedObject.FindProperty("sortSpline");
			_sortPathBias			= serializedObject.FindProperty("sortPathBias");
			_useSplitParams			= serializedObject.FindProperty("useSplitParams");
			_buildDistRange			= serializedObject.FindProperty("buildDistRange");
			_meshPivot				= serializedObject.FindProperty("meshPivot");
			_buildMode				= serializedObject.FindProperty("buildMode");
			_buildTime				= serializedObject.FindProperty("buildTime");
			_dismantleTime			= serializedObject.FindProperty("dismantleTime");
			_buildStyle				= serializedObject.FindProperty("buildStyle");
			_placeMode				= serializedObject.FindProperty("placeMode");
			_placeTimeRange			= serializedObject.FindProperty("placeTimeRange");
			_placeCurve				= serializedObject.FindProperty("placeCurve");
			_rotCurve				= serializedObject.FindProperty("placeRotCurve");
			_scaleCurve				= serializedObject.FindProperty("placeScaleCurve");
			_scaleCurveY			= serializedObject.FindProperty("placeScaleCurveY");
			_scaleCurveZ			= serializedObject.FindProperty("placeScaleCurveZ");
			_perAxisScale			= serializedObject.FindProperty("perAxisScale");
			_usePlaceCurve			= serializedObject.FindProperty("usePlaceCurve");
			_useRotCurve			= serializedObject.FindProperty("useRotCurve");
			_useScaleCurve			= serializedObject.FindProperty("useScaleCurve");
			_placeMaterial			= serializedObject.FindProperty("placeMaterial");
			_maxScale				= serializedObject.FindProperty("maxScale");
			_buildOnAwake			= serializedObject.FindProperty("buildOnAwake");
			_onCompleteBuildThese	= serializedObject.FindProperty("onCompleteBuildThese");
			_onDismantled			= serializedObject.FindProperty("onDismantleComplete");
			//_onDismantleLevel		= serializedObject.FindProperty("onDismantleLevel");
			_enableOnBuilt			= serializedObject.FindProperty("enableOnBuilt");
			_disableOnBuilt			= serializedObject.FindProperty("disableOnBuilt");
			_materialRemap			= serializedObject.FindProperty("materialRemap");
			_rotate					= serializedObject.FindProperty("rotate");
			_rotateRange			= serializedObject.FindProperty("rotateRange");
			_buildData				= serializedObject.FindProperty("buildData");
			_sortMode				= serializedObject.FindProperty("sortMode");
			_sortDistanceMode		= serializedObject.FindProperty("sortDistanceMode");
			_reverseSortOrder		= serializedObject.FindProperty("reverseSortOrder");
			_materialSortOrder		= serializedObject.FindProperty("materialSortOrder");
			_splitMode				= serializedObject.FindProperty("splitMode");
			_usePosition			= serializedObject.FindProperty("usePosition");
			_useUV					= serializedObject.FindProperty("useUV");
			_useUV2					= serializedObject.FindProperty("useUV2");
			_useNormal				= serializedObject.FindProperty("useNormal");
			_useColor				= serializedObject.FindProperty("useColor");
			_useMaterial			= serializedObject.FindProperty("useMaterial");
			_placedEvents			= serializedObject.FindProperty("placedEvent");
			_builtEvent				= serializedObject.FindProperty("builtEvent");
			_dismantleEvent			= serializedObject.FindProperty("dismantleEvent");
			_dismantledEvent		= serializedObject.FindProperty("dismantledEvent");
			_dismantleStyle			= serializedObject.FindProperty("dismantleStyle");
			_removeTime				= serializedObject.FindProperty("removeTimeRange");
			_useDisPlaceCurve		= serializedObject.FindProperty("useDisPlaceCurve");
			_useDisPlaceRotCurve	= serializedObject.FindProperty("useDisPlaceRotCurve");
			_useDisPlaceScaleCurve	= serializedObject.FindProperty("useDisPlaceScaleCurve");
			_disPlaceCurve			= serializedObject.FindProperty("disPlaceCurve");
			_disPlaceRotCurve		= serializedObject.FindProperty("disPlaceRotCurve");
			_disPlaceScaleCurve		= serializedObject.FindProperty("disPlaceScaleCurve");
			_minExplodeForce		= serializedObject.FindProperty("minExplodeForce");
			_maxExplodeForce		= serializedObject.FindProperty("maxExplodeForce");
			_dismantleRotate		= serializedObject.FindProperty("dismantleRotate");
			_simpleCollision		= serializedObject.FindProperty("simpleCollision");
			_collisionY				= serializedObject.FindProperty("collisionY");
			_bounce					= serializedObject.FindProperty("bounce");
			_linearDrag				= serializedObject.FindProperty("linearDrag");
			_angularDrag			= serializedObject.FindProperty("angularDrag");
			_explodeOrigin			= serializedObject.FindProperty("explodeOrigin");
			_placeEventIDs			= serializedObject.FindProperty("placedEvents");
			_dismantleEventIDs		= serializedObject.FindProperty("dismantledEvents");
			_customBuild			= serializedObject.FindProperty("customBuild");
			_customDismantle		= serializedObject.FindProperty("customDismantle");
			_placing				= serializedObject.FindProperty("placing");
			_buildFromObjects		= serializedObject.FindProperty("buildFromObjects");
			_buildFromMode			= serializedObject.FindProperty("buildFromMode");
			_completedObjects		= serializedObject.FindProperty("completedObjects");
			_completedObjectEvent	= serializedObject.FindProperty("completedObjectEvent");
			_projection				= serializedObject.FindProperty("projection");
			_useUnscaledTime		= serializedObject.FindProperty("useUnscaledTime");
			_useBuildTimeCrv		= serializedObject.FindProperty("useBuildTimeCrv");
			_buildTimeCrv			= serializedObject.FindProperty("buildTimeCrv");
			_positionTolerance		= serializedObject.FindProperty("positionTolerance");
			_uvTolerance			= serializedObject.FindProperty("uvTolerance");
			_normalTolerance		= serializedObject.FindProperty("normalTolerance");
			_colorTolerance			= serializedObject.FindProperty("colorTolerance");
			_rotateMode				= serializedObject.FindProperty("rotateMode");
			_seed					= serializedObject.FindProperty("seed");
			_angVelRange			= serializedObject.FindProperty("angVelRange");
			_gravityModifier		= serializedObject.FindProperty("gravityModifier");
			_particleEnableSpeed	= serializedObject.FindProperty("particleEnableSpeed");
			_lightEnableSpeed		= serializedObject.FindProperty("lightEnableSpeed");
			_particleEnableCrv		= serializedObject.FindProperty("particleEnableCrv");
			_lightEnableCrv			= serializedObject.FindProperty("lightEnableCrv");
			_lineEnableSpeed		= serializedObject.FindProperty("lineEnableSpeed");
			_audioEnableSpeed		= serializedObject.FindProperty("audioEnableSpeed");
			_lineEnableCrv			= serializedObject.FindProperty("lineEnableCrv");
			_audioEnableCrv			= serializedObject.FindProperty("audioEnableCrv");
			_forceRange				= serializedObject.FindProperty("forceRange");
			_dismantleProjection	= serializedObject.FindProperty("dismantleProjection");
			_collisionMode			= serializedObject.FindProperty("collisionMode");
			_layers					= serializedObject.FindProperty("layers");
			_enableLights			= serializedObject.FindProperty("enableLights");
			_enableLines			= serializedObject.FindProperty("enableLines");
			_enableAudio			= serializedObject.FindProperty("enableAudio");
			_enableParticles		= serializedObject.FindProperty("enableParticles");
			_startMode				= serializedObject.FindProperty("startMode");
			_boundsMode				= serializedObject.FindProperty("boundsMode");
			_boundsValue			= serializedObject.FindProperty("boundsValue");
			_buildClickPrefab		= serializedObject.FindProperty("buildClickPrefab");
			_canvas					= serializedObject.FindProperty("canvas");
			_clickBuildPos			= serializedObject.FindProperty("clickBuildPos");

			//SceneView.duringSceneGui += HandleAnimation;
			ArtificerSettings.GetSettings();
		}


		void OnDisable()
		{
			SceneView.duringSceneGui -= HandleAnimation;
		}

		void HandleAnimation(SceneView sv)
		{
			//if ( Event.current.type != EventType.Repaint )
				//return;

			Artificer mod = (Artificer)target;
			//Debug.Log("plop");
			int advance = 0;

			if ( !Application.isPlaying )
			{
				//if (Event.current.type == EventType.Repaint)
				{
					if ( pause )
					{
						if ( advance > 0 )
						{
							mod.PlayEditMode(true, advance);
							//SceneView.lastActiveSceneView.Repaint();
						}
						else
							mod.PlayEditMode(false);
					}
					else
					{
						mod.PlayEditMode(true);
						//SceneView.lastActiveSceneView.Repaint();
					}
				}
			}

			sv.Repaint();
		}


		[MenuItem("CONTEXT/Artificer/Artificer Help")]
		static void OpenHelp(MenuCommand cmd)
		{
			Application.OpenURL("https://tubbycrumbles.gitbook/artificer");
		}

		[MenuItem("CONTEXT/Artificer/Artificer Videos")]
		static void OpenVideo(MenuCommand cmd)
		{
			Application.OpenURL("https://www.youtube.com/watch?v=GQSG8XoXa5o&list=PLedKiPwxa47mPzYvxyhekR7WB1Eb290Lp&pp=sAgC");
		}

		[MenuItem("Tools/Artificer/Artificer Help")]
		static void OpenHelp1(MenuCommand cmd)
		{
			Application.OpenURL("https://tubbycrumbles.gitbook/artificer");
		}

		[MenuItem("Tools/Artificer/Artificer Videos")]
		static void OpenVideo1(MenuCommand cmd)
		{
			Application.OpenURL("https://www.youtube.com/watch?v=GQSG8XoXa5o&list=PLedKiPwxa47mPzYvxyhekR7WB1Eb290Lp&pp=sAgC");
		}

		public override void OnInspectorGUI()
		{
			Artificer mod = (Artificer)target;

			Rect fullRect = new Rect(0, 0, EditorGUIUtility.currentViewWidth, Screen.height);

			serializedObject.Update();

			if ( logoImage == null )
				logoImage = (Texture)Resources.Load<Texture>("Artificer");

			if ( logoImage )
			{
				float h1 = (float)logoImage.height / ((float)logoImage.width / ((float)Screen.width - 0));

				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(h1));
				rect.xMin = 0;
				rect.width = EditorGUIUtility.currentViewWidth;

				GUI.DrawTexture(rect, logoImage, ScaleMode.ScaleAndCrop);
			}

			bool rebuildNeeded = false;
			bool changed = false;
			bool benabled = false;

			if ( Application.isPlaying && (mod.buildMode == BuildMode.None || mod.buildMode == BuildMode.Finished)  )
			{
				benabled = true;
			}

			using ( new EditorGUI.DisabledScope(!benabled) )
			{
				if ( mod.buildMode == BuildMode.None )
				{
					if ( GUILayout.Button(new GUIContent("Assemble", "Start the assembly animation in play mode")) )
					{
						mod.StartBuild();
					}
				}
				else
				{
					if ( mod.buildMode == BuildMode.Finished )
					{
						if ( GUILayout.Button(new GUIContent("Dismantle", "Start the dismantle animation in play mode")) )
						{
							mod.StartDismantle();
						}
					}
					else
					{
						if ( mod.buildMode == BuildMode.Build || mod.buildMode == BuildMode.Pause )
						{
							if ( GUILayout.Button("Building " + (mod.buildProgress * 100.0f).ToString("0") + "%") )
							{
							}
						}
						else
						{
							if ( GUILayout.Button("Dismantling " + ((1.0f - mod.buildProgress) * 100.0f).ToString("0") + "%") )
							{
							}
						}
					}
				}
			}


			using ( var v = new EditorGUILayout.VerticalScope("box") )
			{
				ArtificerUI.Property("Object To Build",				_target,	ref rebuildNeeded, "Object we want to split up and build");

				bool canBuild = false;
				if ( mod.target )
				{
					canBuild = true;
					MeshFilter mf = mod.target.GetComponentInChildren<MeshFilter>();
					if ( mf )
					{
						if ( mf.sharedMesh && !mf.sharedMesh.isReadable )    //mod.mesh && !mod.mesh.isReadable )
						{
							if ( ArtificerUI.BigButton("Make Meshes Readable", "Unreadable meshes found click to make all meshes readable") )
							{
								MakeMeshesReadable.ProcessMeshes(mod.target, true);
							}
							canBuild = false;
						}
					}

					if ( canBuild )
					{
						string btext;
						if ( !mod.buildData || mod.rebuildNeeded )
						{
							GUI.backgroundColor = new Color(1.0f, 0.75f, 0.0f, 1.0f);
							btext = "Rebuild Required";
						}
						else
						{
							GUI.backgroundColor = Color.green;
							btext = "Build Data Valid";
						}

						if ( ArtificerUI.BigButton(btext, "Split the target up into elements using the current settings.") )
						{
							mod.BuildData();
							MySetDirty(mod.buildData);
							GUI.changed = false;
						}

						GUI.backgroundColor = Color.white;
					}

					if ( !mod.buildData || mod.buildData.meshes.Count == 0 )
					{
						if ( mf.sharedMesh && !mf.sharedMesh.isReadable )
						{
							EditorGUILayout.HelpBox("Meshes Not Readable.\nPlease Click the Button to make meshes readable or change the import settings.", MessageType.Info);
						}
						else
							EditorGUILayout.HelpBox("No Build Data.\nPlease Click the Build Button to generate Build Data.", MessageType.Info);
					}
					else
					{
						EditorGUILayout.HelpBox("Build Data Info\n" + "Mesh Elements: " + mod.buildData.meshes.Count, MessageType.Info);
					}
				}

				if ( mod.buildData )
				{
					mod.showStart = Mathf.Clamp(mod.showStart, 0, mod.buildData.meshes.Count);
					mod.showEnd = Mathf.Clamp(mod.showEnd, 0, mod.buildData.meshes.Count);

					if ( mod.showEnd < mod.showStart )
						mod.showEnd = mod.showStart;

					if ( mod.buildData.meshes.Count > 0 )
					{
						mod.gizmoMode = (GizmoMode)EditorGUILayout.EnumPopup("Highlight Mode", mod.gizmoMode);

						if ( mod.gizmoMode != GizmoMode.None )
						{
							mod.gizmoValue = (GizmoValue)EditorGUILayout.EnumPopup("Highlight Value", mod.gizmoValue);
							mod.showElementBounds = EditorGUILayout.Toggle("Show Element Bounds", mod.showElementBounds);

							switch ( mod.gizmoMode )
							{
								case GizmoMode.None:
									break;
								case GizmoMode.Single:
									EditorGUILayout.BeginHorizontal();
									mod.showStart = EditorGUILayout.IntField("Highlight", mod.showStart);
									if ( GUILayout.Button("<", GUILayout.Width(16)) )
									{
										mod.showStart--;
									}
									if ( GUILayout.Button(">", GUILayout.Width(16)) )
									{
										mod.showStart++;
									}

									if ( mod.showStart < 0 )
										mod.showStart = 0;

									if ( mod.showStart >= mod.buildData.meshes.Count )
										mod.showStart = mod.buildData.meshes.Count - 1;

									EditorGUILayout.EndHorizontal();
									break;

								case GizmoMode.Range:
									float low = mod.showStart;
									float high = mod.showEnd;
									int gap = mod.showEnd - mod.showStart;
									EditorGUILayout.MinMaxSlider("Highlight [" + mod.showStart + "-" + mod.showEnd + "]", ref low, ref high, 0.0f, (float)mod.buildData.meshes.Count);

									mod.showStart = (int)low;
									mod.showEnd = (int)high;
									break;

								case GizmoMode.Selection:
									SerializedProperty prop = serializedObject.FindProperty("gizmoSelection");
									ArtificerUI.Property("Highlight Selection", prop);
									break;

								case GizmoMode.Playing:
									break;
							}

							mod.testPlace = EditorGUILayout.Slider("Place", mod.testPlace, 0.0f, 1.0f);
						}
						mod.showEditControls = EditorGUILayout.Toggle("Show Control Window", mod.showEditControls);
					}
				}
			}

			ArtificerUI.Property("Start Mode",	_startMode,	"Should the object start built or dismantled and should it start to build or dismantle");

			ArtificerUI.FoldOut(ref mod.showSplit, "Split Options");

			if ( mod.showSplit )
			{
				ArtificerUI.SetHelpUrl("https://tubbycrumbles.gitbook.io/artificer/using-artificer/inspector-sections/split-options#");

				using ( var v = new EditorGUILayout.VerticalScope("box") )
				{
					ArtificerUI.Property("Exclude Objects",						_exclude,			ref rebuildNeeded,	"List of Gameobjects to not include in the build",	"",	"exclude-objects");
					ArtificerUI.Property("Sort Origin",							_sortOrigin,		ref rebuildNeeded,	"Origin Point used to sort split elements from",	"",	"sort-origin");
					ArtificerUI.Property("Use Sort Spline",						_useSortSpline,		ref rebuildNeeded,	"Use a spline to control the sorting, useful for long thin objects like railways or roads", "", "use-sort-spline");
					if ( mod.useSortSpline )
					{
						ArtificerUI.Property("Sort Spline",						_sortSpline,		ref rebuildNeeded,	"The spline to use for sorting", "", "sort-spline");
						ArtificerUI.Property("Sort Spline Bias",				_sortPathBias,		ref rebuildNeeded,	"Controls whether distance along the spline or distance from splines controls the sort. Higher values will favour distance along the spline", "", "sort-spline-bias");
					}

					ArtificerUI.Property("Sort Mode",							_sortMode,			ref rebuildNeeded,	"How the elements should be sorted if at all", "", "sort-mode");
					ArtificerUI.Property("Sort Distance Mode",					_sortDistanceMode,	ref rebuildNeeded,	"Which point is used to calculate the sorting distance", "", "sort-distance-mode");
					ArtificerUI.Property("Reverse Sort Order",					_reverseSortOrder,	ref rebuildNeeded,	"Reverse the sort order", "", "reverse-sort-order");
					ArtificerUI.Property("Material Sort Order",					_materialSortOrder,	ref rebuildNeeded,	"If materials are to be used in the sort you can control the sort order with this list", "", "material-sort-order");
					ArtificerUI.Property("Split Mode",							_splitMode,			ref rebuildNeeded,	"How the target mesh will be split up, either by element or by material", "", "split-mode");
					ArtificerUI.Property("Use Adjust Params",					_useSplitParams,	ref rebuildNeeded,	"Use any SplitParam values found on the build object", "", "use-adjust-params");
					
					ArtificerUI.SetVideoUrl("https://www.youtube.com/watch?v=8Cf-mjA7fS0&t=");
					ArtificerUI.Property("Use Positions",						_usePosition,		ref rebuildNeeded,	"Use vertex positions in the mesh splitting", "", "use-positions");
					ArtificerUI.Property("Use UV",								_useUV,				ref rebuildNeeded,	"Use Uvs in the mesh splitting", "8s", "use-uv");
					ArtificerUI.Property("Use UV2",								_useUV2,			ref rebuildNeeded,	"Use UV 2 in the mesh splitting", "42s", "use-uv2");
					ArtificerUI.Property("Use Normals",							_useNormal,			ref rebuildNeeded,	"Use Normals in the mesh splitting", "58s", "use-normals");
					ArtificerUI.Property("Use Colors",							_useColor,			ref rebuildNeeded,	"Use Colors in the mesh splitting", "74s", "use-colors");
					ArtificerUI.Property("Use Material",						_useMaterial,		ref rebuildNeeded,	"Use Materials in the mesh splitting", "108s", "use-materials");
					ArtificerUI.Property("Material Remap",						_materialRemap,		ref rebuildNeeded,	"Remap materials on build object, easy to swap in a different material during the build process", "", "material-remap");
					ArtificerUI.Property("Place Material",						_placeMaterial,							"Material to use for the element being built", "", "place-material");
					
					EditorGUILayout.BeginHorizontal();
					ArtificerUI.Property("Build Profile",						_buildData,								"The Build Data to use to build the object", "", "build-profile");
					if ( GUILayout.Button("Clear", GUILayout.Width(48)) )
					{
						bool doclear = EditorUtility.DisplayDialog("Clear Build Data", "Do you want to clear the Build Data?", "Yes", "No");
						if ( doclear )
						{
							Undo.RecordObject(target, "Cleared Build Data");
							mod.buildData = null;
						}
					}
					EditorGUILayout.EndHorizontal();
					ArtificerUI.Property("Position Tolerance",					_positionTolerance, ref rebuildNeeded,	"How close vertices need to be to be the same", "", "position-tolerance");
					ArtificerUI.Property("UV Tolerance",						_uvTolerance,		ref rebuildNeeded,	"How close Uvs need to be to be the same", "", "uv-tolerance");
					ArtificerUI.Property("Normal Tolerance",					_normalTolerance,	ref rebuildNeeded,	"How close Normals need to be to be the same", "", "normal-tolerance");
					ArtificerUI.Property("Color Tolerance",						_colorTolerance,	ref rebuildNeeded,	"How close Colors need to be to be the same", "", "color-tolerance");
				}
			}

			ArtificerUI.FoldOut(ref mod.showBuild, "Build Options");

			if ( mod.showBuild )
			{
				ArtificerUI.SetHelpUrl("https://tubbycrumbles.gitbook.io/artificer/using-artificer/inspector-sections/build-options#");

				ArtificerUI.SetVideoUrl("https://www.youtube.com/watch?v=sivjOOT2tUk&t=");

				using ( var v = new EditorGUILayout.VerticalScope("box") )
				{
					ArtificerUI.Property("Build Time",							_buildTime,			ref changed,		"Time in seconds to build the whole object", "9s", "build-time");
					ArtificerUI.Property("Use Build Time Curve",				_useBuildTimeCrv,	ref changed,		"Should building be controlled by the build time curve or not.", "30s", "use-build-time-curve");
					ArtificerUI.Property("Build Time Curve",					_buildTimeCrv,		ref changed,		"The curve to control the speed of the building", "39s", "build-time-curve");
					ArtificerUI.PropertyFloatRange("Build Distance",			_buildDistRange,	ref rebuildNeeded,	"Controls the distance at which the elements appear from the origin when building", "55s", "build-distance");
					ArtificerUI.Property("Build Style",							_buildStyle,		ref rebuildNeeded,	"The style in which the parts are built", "", "build-style");
					ArtificerUI.Property("Origin",								_origin,			ref rebuildNeeded,	"Location of the origin, used for Radial build and dismantle styles", "103s", "origin");
					ArtificerUI.Property("Place Mode",							_placeMode,			ref rebuildNeeded,	"Whether placement is time or speed based", "147s", "place-mode");

					if ( mod.placeMode == PlaceMode.Time )
					{
						ArtificerUI.PropertyFloatRange("Place Time",			_placeTimeRange,	ref rebuildNeeded,	"Time in seconds to build each part of the object", "147s", "place-time");
					}
					else
						ArtificerUI.PropertyFloatRange("Place Speed",			_placeTimeRange,	ref rebuildNeeded,	"Speed at which element is placed", "159s", "place-speed");

					ArtificerUI.Property("Build On Awake",						_buildOnAwake,							"Have the object build on awake", "", "build-on-awake");
					ArtificerUI.Property("Mesh Pivot Mode",						_meshPivot,			ref rebuildNeeded,	"Controls where the pivot point of the mesh element is", "", "mesh-pivot-mode");
					ArtificerUI.Property("Rotate Mode",							_rotateMode,		ref changed,		"You can flip to rotation depending on the axis or use the rotate value for all objects.", "211s", "rotate-mode");
					ArtificerUI.Property("Rotate Range",						_rotateRange,		ref rebuildNeeded,	"Angles to rotate the element through as it is built", "193s", "rotate-range");
					ArtificerUI.Property("Projection",							_projection,		ref rebuildNeeded,	"Use to alter the build direction vector", "124s", "projection");
					ArtificerUI.Property("Random Seed",							_seed,				ref rebuildNeeded,	"Change the seed for the random numbers", "", "random-seed");
					ArtificerUI.Property("Build From Mode",						_buildFromMode,		ref rebuildNeeded,	"How to select the build from object", "", "build-from-mode");
					ArtificerUI.Property("Build From Objects",					_buildFromObjects,	ref rebuildNeeded,	"Array of locations the parts will build from, could be gameobject or splines", "", "build-from-objects");
					
					ArtificerUI.SetVideoUrl("https://www.youtube.com/watch?v=fUJu9Esnyg8&t=");

					ArtificerUI.PropertyTog(_usePlaceCurve,						_placeCurve,		ref rebuildNeeded,	"Use the place Curve to control the elements movement as its built", "9s", "use-place-curve");
					ArtificerUI.PropertyTog(_useRotCurve,						_rotCurve,			ref rebuildNeeded,	"Use the Rot Curve to control the elements rotation as its built", "32s", "use-rot-curve");
					ArtificerUI.Property("Use Scale Curve",						_useScaleCurve,		ref rebuildNeeded,	"Use this curve to scale the whole element", "55s", "use-scale-curve");

					ArtificerUI.Property("Per Axis Scale",						_perAxisScale,		ref rebuildNeeded,	"Have per axis scaling", "90s", "per-axis-scale");
					if ( !mod.perAxisScale )
					{
						ArtificerUI.Property("Scale Curve",						_scaleCurve,		ref rebuildNeeded,	"Curve to scale the element by on all axis", "71s", "scale-curves");
					}
					else
					{
						ArtificerUI.Property("Scale Curve X",					_scaleCurve,		ref rebuildNeeded,	"Curve to scale the element by on X axis", "97s", "scale-curves");
						ArtificerUI.Property("Scale Curve Y",					_scaleCurveY,		ref rebuildNeeded,	"Curve to scale the element by on Y axis", "97s", "scale-curves");
						ArtificerUI.Property("Scale Curve Z",					_scaleCurveZ,		ref rebuildNeeded,	"Curve to scale the element by on Z axis", "97s", "scale-curves");
					}

					ArtificerUI.Property("Use Unscaled Time",					_useUnscaledTime,	ref changed,		"Use Unscaled Time instead of deltatime for building", "", "use-unscaled-time");
					ArtificerUI.Property("Custom Builder",						_customBuild,		ref changed,		"Use a custom builder to assmeble the object", "", "custom-builder");
					ArtificerUI.Property("Placing",								_placing,			ref changed,		"Any element being moved will call this with its info, so you can trails or particles", "", "placing");

					ArtificerUI.Header("Completion Options");
					using ( var v1 = new EditorGUILayout.VerticalScope("box") )
					{
						ArtificerUI.Property("On Complete Build These",			_onCompleteBuildThese,	ref changed,	"Other objects to build when this one is building, useful to split big objects into stages and have it build as one", "", "completion-options#on-complete-build-these");
						ArtificerUI.Property("Enable On Built",					_enableOnBuilt,			ref changed, 	"List of GameObjects that will be enabled when object is fully built", "", "completion-options#enable-on-built");
						ArtificerUI.Property("Disable On Built",				_disableOnBuilt,		ref changed,	"List of GameObjects that will be disabled when the object is fully built", "", "completion-options#disable-on-built");
				
						using ( var v2 = new EditorGUILayout.VerticalScope("box") )
						{
							ArtificerUI.Property("Parts to Call Place Event",	_placeEventIDs,							"Ids for parts to call place event on, can be ranges", "", "completion-options#parts-to-call-place-events");
							ArtificerUI.Property("Place Events",				_placedEvents,							"A placed event will be invoked on these items being built", "");
						}

						EditorGUILayout.LabelField("Fully Built Event");
						ArtificerUI.Property("Built Event",						_builtEvent,							"Built Event", "", "completion-options#fully-built-event");


						ArtificerUI.Property("Completed Objects",				_completedObjects,						"When these objects are finished building the Completed Object Event will be invoked", "", "completion-options#completed-objects");
						ArtificerUI.Property("Completed Event",					_completedObjectEvent,					"Completed Object Event", "", "completion-options#completed-event");
					}
				}
			}

			ArtificerUI.FoldOut(ref mod.showDismantle, "Dismantle Options");

			if ( mod.showDismantle )
			{
				ArtificerUI.SetHelpUrl("https://tubbycrumbles.gitbook.io/artificer/using-artificer/inspector-sections/dismantle-options#");

				using ( var v = new EditorGUILayout.VerticalScope("box") )
				{
					ArtificerUI.Property("Dismantle Time",						_dismantleTime,			ref changed,		"How long to take to dismantle the object", "",	"dismantle-time");
					ArtificerUI.Property("Remove Time",							_removeTime,			ref rebuildNeeded,	"How quickly the object will dismantle",	"",	"remove-time");
					ArtificerUI.Property("Dismantle Style",						_dismantleStyle,		ref rebuildNeeded,	"In which way should the object dismantle",	"",	"dismantle-style");


					if ( mod.dismantleStyle == DismantleStyle.Explode )
					{
						EditorGUILayout.BeginVertical("box");
						ArtificerUI.Property("Min Explode Force",				_minExplodeForce,		ref changed,		"Min value for the explosion force",		"",	"min-explode-force");
						ArtificerUI.Property("Max Explode Force",				_maxExplodeForce,		ref changed,		"Max value for the explosion force",		"",	"max-explode-force");

						EditorGUILayout.BeginVertical("box");
						ArtificerUI.Property("Collision Mode",					_collisionMode,			ref changed,		"How the exploding parts will hit the scene if at all",	"",	"collision-mode");

						if ( mod.collisionMode == CollisionMode.Simple )
						{
							//ArtificerUI.Property("Simple Collision",			_simpleCollision,		ref changed,		"Use simple collision for the parts");
							ArtificerUI.Property("Collision Y",					_collisionY,			ref changed,		"If exploding this is the positon that objects will bounce",	"",	"collision-y");
							ArtificerUI.Property("Bounce",						_bounce,				ref changed,		"How much parts will bounce",									"",	"bounce");
						}

						if ( mod.collisionMode == CollisionMode.Raycast )
						{
							ArtificerUI.Property("Collision Layers",			_layers,				ref changed,		"Which Layers to raycast against",								"",	"collision-layers");
							ArtificerUI.Property("Bounce",						_bounce,				ref changed,		"How much parts will bounce",									"",	"bounce");
						}
						EditorGUILayout.EndVertical();

						ArtificerUI.Property("Angular Velocity",				_angVelRange,			ref changed,		"How fast the objects spin on each axis",							"",	"angular-velocity");
						ArtificerUI.PropertyVector3Range("Force",				_forceRange,			ref changed,		"Force to apply to exploded elements, like a wind for example",		"",	"force");
						ArtificerUI.Property("Gravity Modifier",				_gravityModifier,		ref changed,		"Alters the gravity value used in exploding dismantle",				"",	"gravity-modifier");
						ArtificerUI.Property("Linear Drag",						_linearDrag,			ref changed,		"How quickly parts will slow down",									"",	"linear_drag");
						ArtificerUI.Property("Angular Drag",					_angularDrag,			ref changed,		"How quickly parts will stop spinning",								"",	"angular-drag");
						ArtificerUI.Property("Explode Origin",					_explodeOrigin,			ref changed,		"Location of the center of the explosion",							"",	"explode-origin");
						ArtificerUI.Property("Projection",						_dismantleProjection,	ref changed,		"Use to alter the direction of the exploding parts",				"",	"projection");
						EditorGUILayout.EndVertical();
					}

					ArtificerUI.PropertyTog(_useDisPlaceCurve,					_disPlaceCurve,			ref rebuildNeeded,	"Use the place Curve to control the elements movement as its dismantled",	"",	"use-dis-place-curve");
					ArtificerUI.PropertyTog(_useDisPlaceRotCurve,				_disPlaceRotCurve,		ref rebuildNeeded,	"Use the Curve to control the elements rotation as its dismantled",			"",	"use-dis-place-rot-curve");
					ArtificerUI.PropertyTog(_useDisPlaceScaleCurve,				_disPlaceScaleCurve,	ref rebuildNeeded,	"Use the Curve to control the elements scale as its dismantled",			"",	"use-dis-place-scale-curve");
					ArtificerUI.Property("Dismantle Rotate",					_dismantleRotate,		ref changed,		"How much the part will rotate as removed",									"",	"dismantle-rotate");
					ArtificerUI.Property("Custom Dismantle",					_customDismantle,							"Use a custom dismantler to taker apart the object",						"",	"custom-dismantle");

					ArtificerUI.Header("Dismantled Options");

					EditorGUILayout.BeginVertical("box");
					//ArtificerUI.Property("On Dismantle Level",				_onDismantleLevel,		ref changed,		"Point at which the objects below will start dismantling");

					ArtificerUI.Property("On Dismantle Dismantle",				_onDismantled,			ref changed,		"Objects to dismantle when the dismantle level is reached",					"",	"on-dismantle-dismantle");


					using ( var v1 = new EditorGUILayout.VerticalScope("box"))
					{
						ArtificerUI.Property("Parts To Call Dismantle Event",	_dismantleEventIDs,		"",	"parts-to-call-dismantle-event");
						ArtificerUI.Property("Dismantle Event",					_dismantleEvent,		"");
					}

					EditorGUILayout.LabelField("Fully Dismantled Event");
					ArtificerUI.Property("Dismantled Event",					_dismantledEvent,		"",	"fully-dismantled-event");
					EditorGUILayout.EndVertical();
				}
			}

			ArtificerUI.FoldOut(ref mod.showExtra, "Extra Options");

			if ( mod.showExtra )
			{
				ArtificerUI.SetHelpUrl("https://tubbycrumbles.gitbook.io/artificer/using-artificer/inspector-sections/extra-options#");

				using ( var v = new EditorGUILayout.VerticalScope("box") )
				{
					ArtificerUI.Property("Build Click Prefab",					_buildClickPrefab,	"Prefab that will be used by the Click Build for the player to click to add an object");
					ArtificerUI.Property("Click Build Pos",						_clickBuildPos,		"Where is the click build prefab placed, either object position or part to be built pos");
					ArtificerUI.Property("Canvas",								_canvas,			"The Canvas to child the prefab to if its a UI item");
					ArtificerUI.Property("Bounds Mode",							_boundsMode,		"Which mode to use for Elements rendering bounds. Calc is accurate but slower, Value is faster", "", "bounds-mode");
					if ( mod.boundsMode == BoundsMode.Value )
					{
						ArtificerUI.Property("Bounds Value",					_boundsValue,		"Bounds value to use for all elements. Use the smallest value that doesn't cause any popping of meshes with camera changes", "", "bounds-value");
					}

					mod.handleParticles = EditorGUILayout.BeginToggleGroup(new GUIContent("Handle Particles", "Should Artificer control any particle systems"), mod.handleParticles);
					ArtificerUI.Property("Particle Enable At",					_enableParticles,							"When to turn the Particles on in the build process",				"",	"particle-enable-at");
					ArtificerUI.Property("Particle Enable Speed",				_particleEnableSpeed,						"How quickly any particle systems start up after build completes",	"",	"particle-enable-speed");
					ArtificerUI.Property("Particle Enable Curve",				_particleEnableCrv,							"Controls how the particle system will appear",						"",	"particle-enable-curve");
					EditorGUILayout.EndToggleGroup();

					mod.handleLights = EditorGUILayout.BeginToggleGroup(new GUIContent("Handle Lights", "Should Artificer control any lights"), mod.handleLights);

					ArtificerUI.Property("Lights Enable At",					_enableLights,								"When to turn the Lights on in the build process",					"",	"lights-enable-at");
					ArtificerUI.Property("Lights Enable Speed",					_lightEnableSpeed,							"How quickly any Lights fade up after build completes",				"",	"lights-enable-speed");
					ArtificerUI.Property("Lights Enable Curve",					_lightEnableCrv,							"Controls how the lights will appear",								"",	"lights-enable-curve");
					EditorGUILayout.EndToggleGroup();

					mod.handleLines = EditorGUILayout.BeginToggleGroup(new GUIContent("Handle Lines", "Should Artificer control any lines"), mod.handleLines);

					ArtificerUI.Property("Lines Enable At",						_enableLines,								"When to turn the Lines on in the build process",					"",	"lines-enable-at");
					ArtificerUI.Property("Line Enable Speed",					_lineEnableSpeed,							"How quickly any Lines appear after build completes",				"",	"line-enable-speed");
					ArtificerUI.Property("Line Enable Curve",					_lineEnableCrv,								"Controls how the lines will appear",								"",	"line-enable-curve");
					EditorGUILayout.EndToggleGroup();

					mod.handleAudio = EditorGUILayout.BeginToggleGroup(new GUIContent("Handle Audio", "Should Artificer control any audio"), mod.handleAudio);

					ArtificerUI.Property("Audio Enable At",						_enableAudio,								"When to turn the Audio on in the build process",					"",	"audio-enable-at");
					ArtificerUI.Property("Audio Enable Speed",					_audioEnableSpeed,							"How quickly any Audio fades up after build completes",				"",	"audio-enable-speed");
					ArtificerUI.Property("Audio Enable Curve",					_audioEnableCrv,							"Controls how the audio fades up",									"",	"audio-enable-curve");
					EditorGUILayout.EndToggleGroup();
				}
			}

			if ( GUI.changed )
			{
				for ( int i = 0; i < targets.Length; i++ )
				{
					Artificer ar = (Artificer)targets[i];
					MySetDirty(ar);
				}
			}

			serializedObject.ApplyModifiedProperties();

			if ( rebuildNeeded )
				mod.rebuildNeeded = true;
		}

		void MySetDirty(Object targ)
		{
			EditorUtility.SetDirty(targ);
			ArtificerUI.SetDirty(targ);
		}

		void DrawSpline(Artificer mod, Spline path, int index, int num)
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			if ( path == null ) return;

			int si;

			if ( mod.gizmoValue == GizmoValue.Sort )
				si = num;
			else
				si = index;

			float sl = settings.splineSegLength;
			if ( sl < 0.01f )
				sl = 0.01f;
			int count = (int)(path.GetLength() / sl); // 40;	//(int)(path.GetLength() / 0.1f);
			if ( count < 40 )
				count = 40;

			if ( count > 400 )
				count = 400;

			Vector3[] points = new Vector3[2];
			points[0] = path.EvaluatePosition(0f);
			Vector3 lp = points[0];

			if ( index == overMesh )
				Handles.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
			else
				Handles.color = settings.pathColor;

			for ( int i = 1; i <= count; i++ )
			{
				float t = i / (float)count;
				points[1] = path.EvaluatePosition(t);

				if ( (i & 1) != 0 )
					Handles.DrawAAPolyLine(settings.lineThickness, points);

				points[0] = points[1];
			}

			if ( index == overMesh )
			{
				Handles.Label(lp, si.ToString(), largeLabel);
				Handles.Label(points[1], si.ToString(), largeLabel);
			}
			else
			{
				Handles.Label(lp, si.ToString());
				Handles.Label(points[1], si.ToString());
			}

			for ( int i = 0; i < path.Count; i++ )
			{
				Handles.SphereHandleCap(0, path[i].Position, Quaternion.identity, 0.075f, EventType.Repaint);
			}
		}

		void DrawRadialLine(Artificer mod, MeshElement me, int index, int num)
		{
			int si;

			if ( mod.gizmoValue == GizmoValue.Sort )
				si = num;
			else
				si = index;

			ArtificerSettings settings = ArtificerSettings.GetSettings();

			Vector3[]	points = new Vector3[2];

			Vector3 dir = me.dir;	//(me.center - me.origin).normalized;
			Vector3 p = (dir * me.buildDist);

			points[0] = me.tm.GetPosition();
			points[1] = points[0] + p;

			if ( index == overMesh )
				Handles.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
			else
				Handles.color = settings.pathColor;
			Handles.DrawAAPolyLine(settings.lineThickness, points);

			if ( index == overMesh )
			{
				Handles.Label(points[0], si.ToString(), largeLabel);
				Handles.Label(points[1], si.ToString(), largeLabel);
			}
			else
			{
				Handles.Label(points[0], si.ToString());
				Handles.Label(points[1], si.ToString());
			}
		}

		void DrawCustomLine(Artificer mod, MeshElement me, int index, int num)
		{
			int si;

			if ( mod.gizmoValue == GizmoValue.Sort )
				si = num;
			else
				si = index;

			ArtificerSettings settings = ArtificerSettings.GetSettings();

			Vector3[] points = new Vector3[2];

			Matrix4x4 m4;
			Color col;

			mod.customBuild.Place(me, 0.0f, index, out m4, out col);

			points[0] = me.tm.GetPosition();
			points[1] = m4.GetPosition();

			if ( index == overMesh )
				Handles.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
			else
				Handles.color = settings.pathColor;
			Handles.DrawAAPolyLine(settings.lineThickness, points);

			if ( index == overMesh )
			{
				Handles.Label(points[0], si.ToString(), largeLabel);
				Handles.Label(points[1], si.ToString(), largeLabel);
			}
			else
			{
				Handles.Label(points[0], si.ToString());
				Handles.Label(points[1], si.ToString());
			}
		}

		void DrawVerticalLine(Artificer mod, MeshElement me, int index, int num)
		{
			int si;

			if ( mod.gizmoValue == GizmoValue.Sort )
				si = num;
			else
				si = index;

			ArtificerSettings settings = ArtificerSettings.GetSettings();

			Vector3[] points = new Vector3[2];

			Vector3 dir = me.dir;	//(me.center - me.origin).normalized;

			Vector3 p = (dir * me.buildDist);

			points[0] = me.tm.GetPosition();
			points[1] = points[0] + p;

			if ( index == overMesh )
				Handles.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
			else
				Handles.color = settings.pathColor;
			Handles.DrawAAPolyLine(settings.lineThickness, points);

			if ( index == overMesh )
			{
				Handles.Label(points[0], si.ToString(), largeLabel);
				Handles.Label(points[1], si.ToString(), largeLabel);
			}
			else
			{
				Handles.Label(points[0], si.ToString());
				Handles.Label(points[1], si.ToString());
			}
		}

		void ShowElement(int i)
		{
			Artificer mod = (Artificer)target;

			MeshElement me;
			if ( mod.gizmoValue == GizmoValue.Sort )
				me = mod.buildData.meshes[mod.buildData.sorted[i]];
			else
				me = mod.buildData.meshes[i];	//mod.buildData.sorted[i]];

			DrawMesh(mod, mod.transform, me, i == overMesh, mod.testPlace);

			if ( mod.customBuild )
			{
				DrawCustomLine(mod, me, i, mod.buildData.sorted[i]);
			}
			else
			{
				switch ( me.buildStyle )
				{
					case BuildStyle.None:		break;
					case BuildStyle.Appear:		break;
					case BuildStyle.Radial:		DrawRadialLine(mod, me, i, mod.buildData.sorted[i]); break;
					case BuildStyle.Vertical:	DrawVerticalLine(mod, me, i, mod.buildData.sorted[i]); break;
					case BuildStyle.Transform:	
						if ( me.havePath && me.path != null && me.path.Count > 1 )
							DrawSpline(mod, me.path, i, mod.buildData.sorted[i]);
						else
						{
						}
						break;
				}
			}
		}

		void DoIntersect(int s, int e, Vector3 mp)
		{
			Artificer mod = (Artificer)target;
			Ray mouseRay = HandleUtility.GUIPointToWorldRay(mp);
			overMesh = -1;
			float minDist = float.MaxValue;

			MeshElement me;

			for ( int i = s; i < e; i++ )
			{
				if ( mod.gizmoValue == GizmoValue.Sort )
					me = mod.buildData.meshes[mod.buildData.sorted[i]];
				else
					me = mod.buildData.meshes[i];

				if ( RayIntersectsMesh(mod, mouseRay, me, out float dist) )
				{
					if ( dist < minDist )
					{
						minDist = dist;
						overMesh = i;
					}
				}
			}
		}

		void DoIntersect(List<int> s, Vector3 mp)
		{
			Artificer mod = (Artificer)target;
			Ray mouseRay = HandleUtility.GUIPointToWorldRay(mp);
			overMesh = -1;
			float minDist = float.MaxValue;

			MeshElement me;
			for ( int i = 0; i < s.Count; i++ )
			{
				if ( s[i] >= 0 && s[i] < mod.buildData.meshes.Count )
				{
					if ( mod.gizmoValue == GizmoValue.Sort )
						me = mod.buildData.meshes[mod.buildData.sorted[s[i]]];
					else
						me = mod.buildData.meshes[s[i]];

					if ( RayIntersectsMesh(mod, mouseRay, me, out float dist) )
					{
						if ( dist < minDist )
						{
							minDist = dist;
							overMesh = s[i];
						}
					}
				}
			}
		}

		void DoIntersectBuild(List<BuildQueue> s, Vector3 mp)
		{
			Artificer mod = (Artificer)target;
			Ray mouseRay = HandleUtility.GUIPointToWorldRay(mp);
			overMesh = -1;
			float minDist = float.MaxValue;

			MeshElement me;
			for ( int i = 0; i < s.Count; i++ )
			{
				me = s[i].element;

				if ( RayIntersectsMesh(mod, mouseRay, me, out float dist) )
				{
					if ( dist < minDist )
					{
						minDist = dist;
						overMesh = mod.buildData.sorted[s[i].piece];	//s[i].piece;
					}
				}
			}

		}

		// Tools panel, move origin, move sort origin, move explode vector and distance
		// draw line for each elements build path
		// spline option
		// path from a point to location, so something like parts coming our of a box. So box transform and an initial direction, build a path from there to the position
		void OnSceneGUI()
		{
			Artificer mod = (Artificer)target;

			if ( !mod.buildData )
				return;

			if ( largeLabel == null )
			{
				largeLabel = new GUIStyle();
				largeLabel.fontSize = 24;       // bigger text
				largeLabel.normal.textColor = Color.white;
				largeLabel.alignment = TextAnchor.MiddleCenter;
			}

			Event e = Event.current;

			if ( e.type == EventType.MouseMove || e.type == EventType.Layout )
			{
				SceneView.RepaintAll();
			}

			ArtificerSettings settings = ArtificerSettings.GetSettings();

			Handles.matrix = mod.transform.localToWorldMatrix;

			if ( mod.buildData && mod.buildData.meshes.Count > 0 )
			{
				if ( overMesh > 0 )
				{
					//if ( e.type == EventType.MouseDown )
						//e.Use();
				}

				if ( mod.gizmoMode == GizmoMode.Range )
				{
					DoIntersect(mod.showStart, mod.showEnd, e.mousePosition);
					for ( int i = mod.showStart; i < mod.showEnd; i++ )
					{
						ShowElement(i);	//mod.buildData.sorted[i]);
					}
				}

				if ( mod.gizmoMode == GizmoMode.Single )
				{
					DoIntersect(mod.showStart, mod.showStart + 1, e.mousePosition);
					ShowElement(mod.showStart);
				}

				if ( mod.gizmoMode == GizmoMode.Selection )
				{
					List<int> sel = Adjust.GetSelection(mod.gizmoSelection);
					DoIntersect(sel, e.mousePosition);
					for ( int i = 0; i < sel.Count; i++ )
					{
						if ( sel[i] >= 0 && sel[i] < mod.buildData.meshes.Count )
							ShowElement(sel[i]);
					}
				}

				if ( mod.gizmoMode == GizmoMode.Playing && mod.buildMode != BuildMode.Dismantle )
				{
					List<BuildQueue> build = mod.GetBuildQueue();
					DoIntersectBuild(build, e.mousePosition);

					for ( int i = 0; i < build.Count; i++ )
					{
						ShowElement(mod.buildData.sorted[build[i].piece]);
					}
				}

				if ( mod.buildFromObjects != null )
				{
					for ( int i = 0; i < mod.buildFromObjects.Length; i++ )
					{
						BuildFrom bf = mod.buildFromObjects[i];
						if ( bf != null && bf.buildFrom )
						{
							SplineContainer spl = bf.buildFrom.GetComponent<SplineContainer>();
							if ( !spl )
							{
								Handles.matrix = bf.buildFrom.localToWorldMatrix;
#if UNITY_6000_1_OR_NEWER
								Handles.color = Color.lightBlue;
#else
								Handles.color = new Color(0.6784314f, 0.8470589f, 46f / 51f, 1f);
#endif
								Handles.DrawWireCube(bf.buildFromOffset, bf.buildFromBox);
							}
						}
					}
				}
			}

			if ( mod.showElementIDs && mod.buildMode != BuildMode.Dismantle )
				ShowElementIDs(mod);

			TestWindow();
		}

		public static int Width		= 220;
		public static int Height	= 132;
		public static Rect windowRect = new Rect(5, 5, 150, 95);
		static bool	pause = false;

		//static int myWindowID = 12341234;

		void TestWindow()
		{
			Artificer mod = (Artificer)target;
			if ( !mod.showEditControls )
				return;

			SceneView sceneView = SceneView.lastActiveSceneView;
	        if ( sceneView == null )
				return;

			Rect sceneRect = sceneView.position;

			float x = sceneRect.xMax - Width;
			float y = sceneRect.yMax - Height;

			Handles.BeginGUI();

			windowRect = GUILayout.Window(GetInstanceID(), windowRect, DrawWindow, "Artificer Controls");
			//windowRect = GUILayout.Window(myWindowID, windowRect, DrawWindow, "Artificer Controls");

			Handles.EndGUI();

			//SceneView.lastActiveSceneView.Repaint();

		}

		Texture2D MakeTex(int width, int height, Color col)
		{
			Texture2D tex = new Texture2D(width, height);
			Color[] pixels = new Color[width * height];
			for (int i = 0; i < pixels.Length; i++)
				pixels[i] = col;
			tex.SetPixels(pixels);
			tex.Apply();
			return tex;
		}

		void DrawWindow(int id)
		{
			Artificer mod = (Artificer)target;
			DrawWindow(mod);
		}

		public static void DrawWindow(Artificer mod)
		{
			int advance = 0;

			GUILayout.BeginVertical("box");

			bool guion = GUI.enabled;

			if ( Application.isPlaying )
				GUI.enabled = false;

			switch ( mod.buildMode )
			{
				case BuildMode.None:
				{
					mod.editModePlay = false;
					string btext = "Assemble";

					if ( mod.rebuildNeeded )
						btext = "Rebuild & Assemble";

					if ( GUILayout.Button(new GUIContent(btext, "Start the assembly animation playing")) )
					{
						if ( mod.rebuildNeeded )
							mod.BuildData();
						mod.editModePlay = true;
						mod.Init();
						mod.ClearMeshes();
						mod.StartBuildEdit();
					}
					break;
				}

				case BuildMode.Build:
					GUI.enabled = false;
					GUILayout.Button("Building " + (mod.buildProgress * 100.0f).ToString("0") + "%");
					break;
				case BuildMode.Dismantle:
					GUI.enabled = false;
					GUILayout.Button("Dismantling " + ((1.0f - mod.buildProgress) * 100.0f).ToString("0") + "%");
					break;
				case BuildMode.Click:
					break;
				case BuildMode.Finished:
				{
					mod.editModePlay = false;
					string btext = "Dismantle";

					if ( mod.rebuildNeeded )
						btext = "Rebuild & Dismantle";

					if ( GUILayout.Button(new GUIContent(btext, "Start the dismantling animation playing")) )
					{
						if ( mod.rebuildNeeded )
							mod.BuildData();
						mod.editModePlay = true;

						mod.UpdateMeshes();
						mod.ClearDismantle();
						mod.StartDismantle();
					}
					break;
				}
				case BuildMode.Pause:
					break;
				case BuildMode.Dismantled:
					break;
				default:
					break;
			}

			GUI.enabled = true;
			EditorGUILayout.BeginHorizontal();
			if ( pause )
			{
				if ( GUILayout.Button(new GUIContent("Resume", "Start the animation going again")) )
					pause = false;
			}
			else
			{
				if ( GUILayout.Button(new GUIContent("Pause", "Pause the animation")) )
					pause = true;
			}

			if ( GUILayout.Button(new GUIContent(">", "Step forward one frame in the animation"), GUILayout.Width(30)) )
			{
				advance = 2;
				pause = true;
			}

			if ( GUILayout.Button(new GUIContent(">>", "Step forward 4 frames in the animation"), GUILayout.Width(30)) )
			{
				advance = 4;
				pause = true;
			}

			if ( GUILayout.Button(new GUIContent(">>", "Step forward 32 frames in the animation"), GUILayout.Width(30)) )
			{
				advance = 32;
				pause = true;
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();

			GUI.enabled = true;
			if ( mod.buildMode == BuildMode.Finished )
				GUI.enabled = false;

			if ( GUILayout.Button(new GUIContent("Set Built", "Set the model to fully built state")) )
				mod.SetBuilt();

			GUI.enabled = true;

			if ( mod.buildMode == BuildMode.None )
				GUI.enabled = false;

			if ( GUILayout.Button(new GUIContent("Set Dismantled", "Set the model to fully dismantled state")) )
				mod.SetDismantled();

			EditorGUILayout.EndHorizontal();

			GUI.enabled = guion;

			mod.editPlaySpeed = EditorGUILayout.FloatField(new GUIContent("Play Speed", "Control the speed of the animation"), mod.editPlaySpeed);
			mod.editPlaySpeed = Mathf.Clamp(mod.editPlaySpeed, 0.0f, 10.0f);

			mod.showElementIDs = EditorGUILayout.Toggle(new GUIContent("Show Element IDs", "Show the id of each element as its built"), mod.showElementIDs);

			EditorGUILayout.HelpBox(new GUIContent("Elements: " + mod.GetBuildQueue().Count + "  Progress: " + mod.buildProgress.ToString("0.00")), true);

#if true
			//if ( !Application.isPlaying )
			if ( !EditorApplication.isPlaying )
			{
				//if (Event.current.type == EventType.Repaint)
				{
					if ( pause )
					{
						if ( advance > 0 )
						{
							mod.PlayEditMode(true, advance);
							//SceneView.lastActiveSceneView.Repaint();
						}
						else
							mod.PlayEditMode(false);
					}
					else
					{
						mod.PlayEditMode(true);
						//SceneView.lastActiveSceneView.Repaint();
					}
				}
			}
#endif
			GUILayout.EndVertical();

			GUILayout.Space(4);

			GUI.DragWindow();
		}

		void ShowElementIDs(Artificer mod)
		{
			List<BuildQueue> build = mod.GetBuildQueue();

			Handles.matrix = Matrix4x4.identity;

			for ( int i = 0; i < build.Count; i++ )
			{
				BuildQueue bq = build[i];

				int index = mod.buildData.sorted[bq.piece];	//element.id;	//piece;

				Vector3 pos = bq.pos;
				Handles.Label(pos, index.ToString());
			}
		}

		void DrawMesh(Artificer mod, Transform tm, MeshElement me, bool highlight, float alpha)
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			if ( _wireMat == null )
			{
				Shader shader = Shader.Find("Hidden/Internal-Colored");
				_wireMat = new Material(shader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				_wireMat.SetInt("_ZWrite", 0);
				_wireMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
				_wireMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				_wireMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			}

			if ( settings )
				_wireMat.SetColor("_Color", settings.mainColor);
			else
				_wireMat.SetColor("_Color", new Color(0.3f, 0.8f, 1f, 0.5f));

			if ( highlight )
			{
				_wireMat.SetColor("_Color", new Color(1.0f, 0.5f, 0f, 0.5f));
			}

			Matrix4x4 matrix = mod.PlaceElement(me, alpha, alpha);	//tm.localToWorldMatrix * me.tm;

			_wireMat.SetPass(0);
			GL.PushMatrix();
			GL.MultMatrix(matrix);
			GL.Begin(GL.TRIANGLES);

			Vector3[] verts = me.verts;

			for ( int i = 0; i < me.tris.Length; i++ )
			{
				int[] tris = me.tris[i].tris;

				for ( int j = 0; j < tris.Length; j += 3 )
				{
					Vector3 v0 = verts[tris[j]];
					Vector3 v1 = verts[tris[j + 1]];
					Vector3 v2 = verts[tris[j + 2]];

					GL.Vertex(v0);
					GL.Vertex(v1);
					GL.Vertex(v2);
				}
			}

			GL.End();
			GL.PopMatrix();

			if ( mod.showElementBounds )
			{
				Matrix4x4 htm = Handles.matrix;
				Handles.matrix = matrix;
				Handles.DrawWireCube(me.bounds.center, me.bounds.size);
				Handles.matrix = htm;
			}
		}

		bool RayIntersectsMesh(Artificer mod, Ray ray, MeshElement me, out float distance)
		{
			distance = float.MaxValue;

			Vector3[] vertices = me.verts;

			// Transform the ray into local space of the mesh
			Matrix4x4 worldToLocal = mod.PlaceElement(me, mod.testPlace, mod.testPlace).inverse;	// localToWorld.inverse;
			Ray localRay = new Ray(worldToLocal.MultiplyPoint(ray.origin), worldToLocal.MultiplyVector(ray.direction).normalized);

			bool hit = false;

			for ( int j = 0; j < me.tris.Length; j++ )
			{
				int[] triangles = me.tris[j].tris;
				for ( int i = 0; i < triangles.Length; i += 3 )
				{
					Vector3 v0 = vertices[triangles[i]];
					Vector3 v1 = vertices[triangles[i + 1]];
					Vector3 v2 = vertices[triangles[i + 2]];

					if ( RayTriangle(localRay, v0, v1, v2, out float t) )
					{
						if ( t < distance )
						{
							distance = t;
							hit = true;
						}
					}
				}
			}
			return hit;
		}

		bool RayTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out float t)
		{
			t = 0f;
			const float EPSILON = 1e-6f;
			Vector3 edge1 = v1 - v0;
			Vector3 edge2 = v2 - v0;
			Vector3 h = Vector3.Cross(ray.direction, edge2);
			float a = Vector3.Dot(edge1, h);
			if ( a > -EPSILON && a < EPSILON )
				return false;

			float f = 1.0f / a;
			Vector3 s = ray.origin - v0;
			float u = f * Vector3.Dot(s, h);
			if ( u < 0.0 || u > 1.0 )
				return false;

			Vector3 q = Vector3.Cross(s, edge1);
			float v = f * Vector3.Dot(ray.direction, q);
			if ( v < 0.0 || u + v > 1.0 )
				return false;

			t = f * Vector3.Dot(edge2, q);
			return t > EPSILON;
		}
	}

#if false
	[InitializeOnLoad]
	public static class SceneViewRenderHook
	{
		static SceneViewRenderHook()
		{
			RenderPipelineManager.beginContextRendering += BeginContext;
		}

		static void BeginContext(ScriptableRenderContext ctx, List<Camera> cams)
		{
			for ( int i = 0; i < cams.Count; i++ )
			{
				if (cams[i].cameraType == CameraType.SceneView)
					BeginCameraRendering(ctx, cams[i]);
			}
		}

		static void BeginCameraRendering(ScriptableRenderContext ctx, Camera cam)
		{
			//return;
			//if ( cam.cameraType != CameraType.SceneView )
				//return;

			GameObject obj = Selection.activeGameObject;

			if ( obj )
			{
				Artificer art = obj.GetComponent<Artificer>();

				if ( art )
				{
					int advance = 0;

					if ( !Application.isPlaying )
					{
						//if (Event.current.type == EventType.Repaint)
						{
							if ( false )	//pause )
							{
								if ( advance > 0 )
								{
									art.PlayEditMode(true, advance);
									//SceneView.lastActiveSceneView.Repaint();
								}
								else
								{

									art.PlayEditMode(false);
								}
							}
							else
							{
								art.PlayEditMode(true);
								//SceneView.lastActiveSceneView.Repaint();
							}
						}
					}

					SceneView.lastActiveSceneView?.Repaint();
				}
			}
		}
	}
#endif
}