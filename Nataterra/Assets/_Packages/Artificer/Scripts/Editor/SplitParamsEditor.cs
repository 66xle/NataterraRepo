using UnityEditor;
using UnityEngine;

namespace Artifice
{
	[CustomEditor(typeof(SplitParams))]
	[CanEditMultipleObjects]
	public class SplitParamsEditor : Editor
	{
		Texture				logoImage;
		SerializedProperty	_splitOptions;
		private void OnEnable()
		{
		}

		public override void OnInspectorGUI()
		{
			bool changed = false;
			SplitParams mod = (SplitParams)target;

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

			Artificer art = mod.GetComponentInParent<Artificer>();

			if ( art )
			{
				GUI.backgroundColor = Color.green;

				if ( ArtificerUI.BigButton("Rebuild", "Rebuild The Data for the object") )
				{
					art.BuildData();
				}

				GUI.backgroundColor = Color.white;
			}

			mod.applyToChildren = EditorGUILayout.Toggle("Apply to Children", mod.applyToChildren);

			//ArtificerUI.Header("Split Options");

			EditorGUILayout.BeginVertical("box");
			_splitOptions = serializedObject.FindProperty("splitOptions");
			ArtificerUI.SplitOptionsParams(mod.splitOptions, _splitOptions, ref changed, false);

			EditorGUILayout.EndVertical();

			if ( GUI.changed )
			{
				for ( int i = 0; i < targets.Length; i++ )
				{
					SplitParams sp = (SplitParams)targets[i];
					MySetDirty(sp);
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		void MySetDirty(Object targ)
		{
			EditorUtility.SetDirty(targ);
			ArtificerUI.SetDirty(targ);
		}

		// Tools panel, move origin, move sort origin, move explode vector and distance
		// draw line for each elements build path
		// spline option
		// path from a point to location, so something like parts coming our of a box. So box transform and an initial direction, build a path from there to the position
		void OnSceneGUI()
		{
			SplitParams mod = (SplitParams)target;

			ArtificerSettings settings = ArtificerSettings.GetSettings();

			Handles.matrix = mod.transform.localToWorldMatrix;

			BuildFrom[] bfs = mod.splitOptions.buildFromObjects;	//.value;

			if ( bfs != null )
			{
				for ( int i = 0; i < bfs.Length; i++ )
				{
					BuildFrom bf = bfs[i];
					if ( bf != null && bf.buildFrom )
					{
						Handles.matrix = bf.buildFrom.localToWorldMatrix;
						Handles.color = Color.green;
						Handles.DrawWireCube(bf.buildFromOffset, bf.buildFromBox);
					}
				}
			}

			//TestWindow();
		}

		void TestWindow()
		{
			SplitParams mod = (SplitParams)target;

			Artificer art = mod.GetComponentInParent<Artificer>();

			if ( art )
			{
				if ( !art.showEditControls )
					return;

				SceneView sceneView = SceneView.lastActiveSceneView;
				if ( sceneView == null )
					return;

				Rect sceneRect = sceneView.position;

				float x = sceneRect.xMax - ArtificerEditor.Width;
				float y = sceneRect.yMax - ArtificerEditor.Height;

				Handles.BeginGUI();

				ArtificerEditor.windowRect = GUILayout.Window(GetInstanceID(), ArtificerEditor.windowRect, DrawWindow, "Artificer Controls");

				Handles.EndGUI();
			}
		}

		void DrawWindow(int id)
		{
			SplitParams mod = (SplitParams)target;
			Artificer art = mod.GetComponentInParent<Artificer>();
			if ( art )
				ArtificerEditor.DrawWindow(art);
		}
	}
}