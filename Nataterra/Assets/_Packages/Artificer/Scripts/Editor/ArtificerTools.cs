using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;

namespace Artifice
{
	[EditorTool("Artificer Sort Origin Tool", typeof(Artificer))]
	public class ArtificerToolPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/sort origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Sort Origin",
				tooltip	= "Change Sort Origin position"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				Artificer mod = gobj.GetComponent<Artificer>();
				if ( mod )
				{
					Handles.matrix = mod.transform.localToWorldMatrix;

					EditorGUI.BeginChangeCheck();

					Vector3 newpos = Handles.PositionHandle(mod.sortOrigin, Quaternion.identity);

					if ( EditorGUI.EndChangeCheck() )
					{
						Undo.RecordObject(mod, "Move Sort Origin");
						mod.sortOrigin = newpos;
						EditorUtility.SetDirty(mod);
						mod.rebuildNeeded = true;
					}
					Handles.color = Color.white;
					Handles.Label(mod.sortOrigin, "Sort Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.sortOrigin;
					points[1] = Vector3.zero;

					Handles.color = Color.blue;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}

	[EditorTool("Artificer Origin Tool", typeof(Artificer))]
	public class ArtificerOriginPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Origin",
				tooltip	= "Change the Origin Position"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		public override bool IsAvailable()
		{
			GameObject gobj = (GameObject)Selection.activeGameObject;

			if ( gobj )
			{
				Artificer mod = gobj.GetComponent<Artificer>();
				if ( mod )
				{
					return true;
				}
			}

			return false;
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				Artificer mod = gobj.GetComponent<Artificer>();
				if ( mod )
				{
					Handles.matrix = mod.transform.localToWorldMatrix;

					EditorGUI.BeginChangeCheck();
					Vector3 newpos = Handles.PositionHandle(mod.origin, Quaternion.identity);

					if ( EditorGUI.EndChangeCheck() )
					{
						Undo.RecordObject(mod, "Move Origin");
						mod.origin = newpos;
						EditorUtility.SetDirty(mod);
						mod.rebuildNeeded = true;
					}

					Handles.color = Color.white;
					Handles.Label(mod.origin, "Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.origin;
					points[1] = Vector3.zero;

					Handles.color = Color.red;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}

	[EditorTool("Artificer Explosion Origin Tool", typeof(Artificer))]
	public class ArtificerExplosionOriginPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/explosion origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Explosion Origin",
				tooltip	= "Change the Explosion Origin Position"
			};
		}

		public override bool IsAvailable()
		{
			GameObject gobj = (GameObject)Selection.activeGameObject;

			if ( gobj )
			{
				Artificer mod = gobj.GetComponent<Artificer>();
				if ( mod )
				{
					if ( mod.dismantleStyle == DismantleStyle.Explode )
						return true;
				}
			}

			return false;
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				Artificer mod = gobj.GetComponent<Artificer>();
				if ( mod )
				{
					if ( mod.dismantleStyle == DismantleStyle.Explode )
					{
						Handles.matrix = mod.transform.localToWorldMatrix;
#if UNITY_6000_1_OR_NEWER
						Handles.color = Color.orange;
#else
						Handles.color = new Color(1f, 0.6470588f, 0f, 1f);
#endif

						EditorGUI.BeginChangeCheck();

						Vector3 newpos = Handles.PositionHandle(mod.explodeOrigin, Quaternion.identity);

						if ( EditorGUI.EndChangeCheck() )
						{
							Undo.RecordObject(mod, "Move Explode Origin");
							mod.explodeOrigin = newpos;
							mod.rebuildNeeded = true;
							EditorUtility.SetDirty(mod);
						}

						Handles.color = Color.white;
						Handles.Label(mod.explodeOrigin, "Explode Origin");
					}
				}
			}
		}
	}

	[EditorTool("Adjust Origin Tool", typeof(Adjust))]
	public class AdjustOriginPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Adj Origin",
				tooltip	= "Change the Origin Position"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		public override bool IsAvailable()
		{
			GameObject gobj = (GameObject)Selection.activeGameObject;

			if ( gobj )
			{
				Adjust mod = gobj.GetComponent<Adjust>();
				if ( mod )
				{
					if ( mod.options.origin.overrideState )
						return true;
				}
			}

			return false;
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				Adjust[] mods = gobj.GetComponents<Adjust>();
				for ( int i = 0; i < mods.Length; i++ )
				{
					Adjust mod = mods[i];
					Handles.matrix = mod.transform.localToWorldMatrix;

					EditorGUI.BeginChangeCheck();
					Vector3 newpos = Handles.PositionHandle(mod.options.origin.value, Quaternion.identity);

					if ( EditorGUI.EndChangeCheck() )
					{
						Undo.RecordObject(mod, "Move Origin");
						mod.options.origin.value = newpos;
						EditorUtility.SetDirty(mod);
						//mod.rebuildNeeded = true;
					}

					Handles.color = Color.white;
					Handles.Label(mod.options.origin.value, "Adj Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.options.origin.value;
					points[1] = Vector3.zero;

					Handles.color = Color.red;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}

	[EditorTool("Adjust Origin Tool", typeof(Adjust))]
	public class AdjustSortOriginPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Adj Sort",
				tooltip	= "Change the Origin Position"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		public override bool IsAvailable()
		{
			GameObject gobj = (GameObject)Selection.activeGameObject;

			if ( gobj )
			{
				Adjust mod = gobj.GetComponent<Adjust>();
				if ( mod )
				{
					if ( mod.options.sortOrigin.overrideState )
						return true;
				}
			}

			return false;
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				Adjust[] mods = gobj.GetComponents<Adjust>();
				for ( int i = 0; i < mods.Length; i++ )
				{
					Adjust mod = mods[i];
					Handles.matrix = mod.transform.localToWorldMatrix;

					EditorGUI.BeginChangeCheck();
					Vector3 newpos = Handles.PositionHandle(mod.options.sortOrigin.value, Quaternion.identity);

					if ( EditorGUI.EndChangeCheck() )
					{
						Undo.RecordObject(mod, "Move Sort Origin");
						mod.options.sortOrigin.value = newpos;
						EditorUtility.SetDirty(mod);
						//mod.rebuildNeeded = true;
					}

					Handles.color = Color.white;
					Handles.Label(mod.options.sortOrigin.value, "Adj Sort Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.options.sortOrigin.value;
					points[1] = Vector3.zero;

					Handles.color = Color.blue;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}









#if false

	[EditorTool("Split Origin Tool", typeof(SplitParams))]
	public class SplitParamsOriginPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Split Origin",
				tooltip	= "Change the Origin Position"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		public override bool IsAvailable()
		{
			GameObject gobj = (GameObject)Selection.activeGameObject;

			if ( gobj )
			{
				SplitParams mod = gobj.GetComponent<SplitParams>();
				if ( mod )
				{
					if ( mod.splitOptions.origin.overrideState )
						return true;
				}
			}

			return false;
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				SplitParams[] mods = gobj.GetComponents<SplitParams>();
				for ( int i = 0; i < mods.Length; i++ )
				{
					SplitParams mod = mods[i];
					Handles.matrix = mod.transform.localToWorldMatrix;

					EditorGUI.BeginChangeCheck();
					Vector3 newpos = Handles.PositionHandle(mod.splitOptions.origin.value, Quaternion.identity);

					if ( EditorGUI.EndChangeCheck() )
					{
						Undo.RecordObject(mod, "Move Origin");
						mod.splitOptions.origin.value = newpos;
						EditorUtility.SetDirty(mod);
						//mod.rebuildNeeded = true;
					}

					Handles.color = Color.white;
					Handles.Label(mod.splitOptions.origin.value, "Adj Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.splitOptions.origin.value;
					points[1] = Vector3.zero;

					Handles.color = Color.red;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}

	[EditorTool("Split Sort Origin Tool", typeof(SplitParams))]
	public class SplitParamsSortOriginPosition : EditorTool
	{
		[SerializeField]
		Texture2D	m_ToolIcon;
		GUIContent	m_IconContent;

		void OnEnable()
		{
			if ( m_ToolIcon == null )
				m_ToolIcon = (Texture2D)Resources.Load<Texture>("Editor/origin");

			m_IconContent = new GUIContent()
			{
				image	= m_ToolIcon,
				text	= "Split Sort",
				tooltip	= "Change the Origin Position"
			};
		}

		public override GUIContent toolbarIcon
		{
			get { return m_IconContent; }
		}

		public override void OnToolGUI(EditorWindow window)
		{
			ToolsGUI();
		}

		public override bool IsAvailable()
		{
			GameObject gobj = (GameObject)Selection.activeGameObject;

			if ( gobj )
			{
				SplitParams mod = gobj.GetComponent<SplitParams>();
				if ( mod )
				{
					if ( mod.splitOptions.sortOrigin.overrideState )
						return true;
				}
			}

			return false;
		}

		static public void ToolsGUI()
		{
			ArtificerSettings settings = ArtificerSettings.GetSettings();

			GameObject gobj = (GameObject)Selection.activeGameObject;
			if ( gobj )
			{
				SplitParams[] mods = gobj.GetComponents<SplitParams>();
				for ( int i = 0; i < mods.Length; i++ )
				{
					SplitParams mod = mods[i];
					Handles.matrix = mod.transform.localToWorldMatrix;

					EditorGUI.BeginChangeCheck();
					Vector3 newpos = Handles.PositionHandle(mod.splitOptions.sortOrigin.value, Quaternion.identity);

					if ( EditorGUI.EndChangeCheck() )
					{
						Undo.RecordObject(mod, "Move Sort Origin");
						mod.splitOptions.sortOrigin.value = newpos;
						EditorUtility.SetDirty(mod);
						//mod.rebuildNeeded = true;
					}

					Handles.color = Color.white;
					Handles.Label(mod.splitOptions.sortOrigin.value, "Split Sort Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.splitOptions.sortOrigin.value;
					points[1] = Vector3.zero;

					Handles.color = Color.blue;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}
#endif


}