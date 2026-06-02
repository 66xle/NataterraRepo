using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Artifice
{
	[CustomEditor(typeof(Adjust))]
	[CanEditMultipleObjects]
	public class AdjustEditor : Editor
	{
		Texture				logoImage;
		SerializedProperty	_splitOptions;
		SerializedProperty	_moveTo;
		SerializedProperty	_selection;
		SerializedProperty	_moveSelection;
		Material			_wireMat;

		private void OnEnable()
		{
		}

		public override void OnInspectorGUI()
		{
			bool changed = false;
			Adjust mod = (Adjust)target;

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
				if ( ArtificerUI.BigButton("Rebuild", "Rebuild the Objects data.") )
				{
					art.BuildData();
					MySetDirty(art.buildData);
				}
			}
			else
			{
				EditorGUILayout.HelpBox("No Artificer Component Found\n" + "You need an Artificer Component for Adjust to Work!", MessageType.Error);
			}

			if ( art )
			{
				if ( !art.useSplitParams )
				{
					EditorGUILayout.HelpBox("Adjust Params Not being Used\n" + "Artificer does not have Use Adjust Params Enabled!", MessageType.Warning);
					art.useSplitParams = EditorGUILayout.Toggle("Turn On Use Adjust Params", art.useSplitParams);
				}
				else
				{
					if ( art.buildData )
					{
						EditorGUILayout.HelpBox("Adjust Info\n" + "Mesh Elements: " + art.buildData.meshes.Count, MessageType.Info);
					}

					SerializedProperty _priority = serializedObject.FindProperty("priority");
					SerializedProperty _color = serializedObject.FindProperty("color");

					ArtificerUI.Property("Priority", _priority, ref changed, "Useful for overlapping Adjusts so you can alter which one is choosen, lower value takes precedence");
					SerializedProperty _showElements = serializedObject.FindProperty("showElements");
					ArtificerUI.Property("Show Elements", _showElements);
					ArtificerUI.Property("Color",	_color, ref changed, "Color for the Volume gizmos");

					ArtificerUI.Header("Adjust Options");

					EditorGUILayout.BeginVertical("box");
					_splitOptions	= serializedObject.FindProperty("options");
					_moveTo			= serializedObject.FindProperty("moveTo");

					SerializedProperty _adjustMode		= serializedObject.FindProperty("adjustMode");

					_selection		= serializedObject.FindProperty("selection");
					_moveSelection	= serializedObject.FindProperty("moveSelection");

					ArtificerUI.Property("Adjust Mode", _adjustMode);

					switch ( mod.adjustMode )
					{
						case AdjustMode.Selection:
							EditorGUI.BeginChangeCheck();
							ArtificerUI.Property("Selection", _selection, ref changed, "Which items these adjustments apply to");
							if ( EditorGUI.EndChangeCheck() )
								mod.UpdateSelection();
							break;
						case AdjustMode.Volume:
							SerializedProperty _volumes		= serializedObject.FindProperty("volumes");
							ArtificerUI.Property("Volumes",	_volumes);
							break;
					}

					ArtificerUI.PropertyOverride("Move To", _moveTo, ref changed, "Where to move the selection to in the build order");
					if ( mod.moveTo.overrideState )
					{
						ArtificerUI.Property("Move Selection", _moveSelection, ref changed, "Selection to move");
					}
					ArtificerUI.SplitOptionsParams(mod.options, _splitOptions, ref changed, true);

					EditorGUILayout.EndVertical();

					if ( GUI.changed )
					{
						for ( int i = 0; i < targets.Length; i++ )
						{
							Adjust sp = (Adjust)targets[i];
							MySetDirty(sp);
						}
					}
				}
			}
			serializedObject.ApplyModifiedProperties();
		}

		void MySetDirty(Object targ)
		{
			EditorUtility.SetDirty(targ);
			ArtificerUI.SetDirty(targ);
		}


		public static void DrawSolidCube(Vector3 center, Vector3 size)	//, Color color, Quaternion rotation)
		{
			if ( Event.current.type != EventType.Repaint )
				return;

			//Color prevColor = Handles.color;
			Matrix4x4 prevMatrix = Handles.matrix;

			//Handles.color = color;
			 var prevZTest = Handles.zTest;

        // THIS is the key line
			Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

			// Move + rotate + scale
			Handles.matrix = Handles.matrix * Matrix4x4.TRS(center, Quaternion.identity, size);

			// Draw unit cube scaled by matrix
			Handles.CubeHandleCap(0, Vector3.zero, Quaternion.identity, 1f, EventType.Repaint);

			Handles.matrix = prevMatrix;
			Handles.zTest = prevZTest;
			//Handles.color = prevColor;
		}

		void DrawVolume(AdjustVolume vol, Color col)
		{
			Color hc = col;
			hc.a *= 0.5f;
			Handles.color = hc;
			//DrawSolidCube(vol.box.center, vol.box.size);
			Handles.color = col;	//Color.white;
			Handles.DrawWireCube(vol.box.center, vol.box.size);

			Quaternion rot = Quaternion.identity;
			Vector3 off = vol.box.center;
			off = Handles.PositionHandle(off, rot);
			if ( off != vol.box.center )
			{
				Undo.RecordObject(target, "Volume Move");
				vol.box.center = off;
			}

			
			Vector3 s = vol.box.size;
			Vector3 c = vol.box.center;

			Vector3 pos = c;
			pos.y += s.y * 0.5f;
			float size = HandleUtility.GetHandleSize(pos) * 0.025f; 
			Vector3 pos1 = Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.DotHandleCap);

			s.y += (pos1.y - pos.y);
			c.y += (pos1.y - pos.y) * 0.5f;

			pos = c;
			pos.y -= s.y * 0.5f;
			size = HandleUtility.GetHandleSize(pos) * 0.025f; 
			pos1 = Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.DotHandleCap);

			s.y -= (pos1.y - pos.y);
			c.y += (pos1.y - pos.y) * 0.5f;

			pos = c;
			pos.x += s.x * 0.5f;
			size = HandleUtility.GetHandleSize(pos) * 0.025f; 
			pos1 = Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.DotHandleCap);

			s.x += (pos1.x - pos.x);
			c.x += (pos1.x - pos.x) * 0.5f;

			pos = c;
			pos.x -= s.x * 0.5f;
			size = HandleUtility.GetHandleSize(pos) * 0.025f; 
			pos1 = Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.DotHandleCap);

			s.x -= (pos1.x - pos.x);
			c.x += (pos1.x - pos.x) * 0.5f;

			pos = c;
			pos.z += s.z * 0.5f;
			size = HandleUtility.GetHandleSize(pos) * 0.025f; 
			pos1 = Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.DotHandleCap);

			s.z += (pos1.z - pos.z);
			c.z += (pos1.z - pos.z) * 0.5f;

			pos = c;
			pos.z -= s.z * 0.5f;
			size = HandleUtility.GetHandleSize(pos) * 0.025f; 
			pos1 = Handles.FreeMoveHandle(pos, size, Vector3.zero, Handles.DotHandleCap);

			s.z -= (pos1.z - pos.z);
			c.z += (pos1.z - pos.z) * 0.5f;

			if ( s != vol.box.size || c != vol.box.center )
			{
				Undo.RecordObject(target, "Volume Changed");
				vol.box.size = s;
				vol.box.center = c;
			}
		}

		void DrawVolumeSphere(AdjustVolume vol, Color col)
		{
			//Handles.SphereHandleCap(0, vol.box.center, Quaternion.identity, vol.radius, EventType.Repaint);

			Handles.DrawWireDisc(vol.box.center, Vector3.up, vol.radius);
			Handles.DrawWireDisc(vol.box.center, Vector3.right, vol.radius);
			Handles.DrawWireDisc(vol.box.center, Vector3.forward, vol.radius);

			Quaternion rot = Quaternion.identity;
			Vector3 off = vol.box.center;
			off = Handles.PositionHandle(off, rot);
			if ( off != vol.box.center )
			{
				Undo.RecordObject(target, "Volume Move");
				vol.box.center = off;
			}
		}

		void OnSceneGUI()
		{
			Adjust mod = (Adjust)target;
			if ( !mod.enabled )
				return;

			ArtificerSettings settings = ArtificerSettings.GetSettings();

			Handles.matrix = mod.transform.localToWorldMatrix;

			BuildFrom[] bfs = mod.options.buildFromObjects;

			if ( bfs != null )
			{
				for ( int i = 0; i < bfs.Length; i++ )
				{
					BuildFrom bf = bfs[i];
					if ( bf != null && bf.buildFrom )
					{
						Handles.matrix = bf.buildFrom.localToWorldMatrix;
						Handles.color = mod.color;
						Handles.DrawWireCube(bf.buildFromOffset, bf.buildFromBox);
					}
				}
			}

			if ( mod.adjustMode == AdjustMode.Volume )
			{
				Handles.matrix = mod.transform.localToWorldMatrix;
				Handles.color = mod.color;

				for ( int i = 0; i < mod.volumes.Count; i++ )
				{
					if ( mod.volumes[i].volumeShape == VolumeShape.Box )
					{
						DrawVolume(mod.volumes[i], mod.color);
					}
					else
						DrawVolumeSphere(mod.volumes[i], mod.color);
				}
			}

			if ( mod.showElements != AdjustShow.None )
			{
				List<MeshElement> elements = mod.GetElements();

				Artificer art = mod.GetComponentInParent<Artificer>();

				for ( int i = 0; i < elements.Count; i++ )
				{
					MeshElement me = elements[i];
					DrawMesh(art, art.transform, me, false, 1.0f);
				}
			}
		}

		void DrawMesh(Artificer mod, Transform tm, MeshElement me, bool highlight, float alpha)
		{
			Adjust adj = (Adjust)target;

			if ( adj.showElements == AdjustShow.Mesh )
			{
				if ( _wireMat == null )
				{
					Shader shader = Shader.Find("Hidden/Internal-Colored");
					_wireMat = new Material(shader)
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					_wireMat.SetInt("_ZWrite", 0);
					_wireMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Front);
					_wireMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
					_wireMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				}

				_wireMat.SetColor("_Color", adj.color);

				Matrix4x4 matrix = tm.localToWorldMatrix * me.tm;

				_wireMat.SetPass(0);
				GL.PushMatrix();
				GL.MultMatrix(matrix);
				GL.Begin(GL.LINES);

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
						GL.Vertex(v1);
						GL.Vertex(v2);
						GL.Vertex(v2);
						GL.Vertex(v0);
					}
				}

				GL.End();
				GL.PopMatrix();
			}

			if ( adj.showElements == AdjustShow.Bounds )
			{
				Matrix4x4 matrix = tm.localToWorldMatrix * me.tm;
				Matrix4x4 htm = Handles.matrix;
				Handles.color = adj.color;
				Handles.matrix = matrix;
				Handles.DrawWireCube(me.bounds.center, me.bounds.size);
				Handles.matrix = htm;
			}
		}

	}
}