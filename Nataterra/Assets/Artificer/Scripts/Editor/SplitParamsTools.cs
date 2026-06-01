using UnityEditor;
using UnityEngine;
using UnityEditor.EditorTools;

namespace Artifice
{
	[EditorTool("SplitParams Origin Tool", typeof(SplitParams))]
	public class SplitParamsOrigin : EditorTool
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
				tooltip	= "Change Origin position"
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

			GameObject gobj = (GameObject)Selection.activeObject;
			if ( gobj )
			{
				SplitParams mod = gobj.GetComponent<SplitParams>();
				if ( mod && mod.splitOptions.origin.overrideState )
				{
					Handles.matrix = mod.transform.localToWorldMatrix;

					mod.splitOptions.origin.value = Handles.PositionHandle(mod.splitOptions.origin.value, Quaternion.identity);
					Handles.color = Color.white;
					Handles.Label(mod.splitOptions.origin.value, "Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.splitOptions.origin.value;
					points[1] = Vector3.zero;

					Handles.color = Color.red;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}

	[EditorTool("Split Params Sort Origin Tool", typeof(SplitParams))]
	public class SplitParamsSortOrigin : EditorTool
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
				image = m_ToolIcon,
				text = "Split Sort",
				tooltip = "Change Sort Origin Position"
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

			GameObject gobj = (GameObject)Selection.activeObject;
			if ( gobj )
			{
				SplitParams mod = gobj.GetComponent<SplitParams>();
				if ( mod && mod.splitOptions.sortOrigin.overrideState )
				{
					Handles.matrix = mod.transform.localToWorldMatrix;

					mod.splitOptions.sortOrigin.value = Handles.PositionHandle(mod.splitOptions.sortOrigin.value, Quaternion.identity);
					Handles.color = Color.white;
					Handles.Label(mod.splitOptions.sortOrigin.value, "Sort Origin");

					Vector3[] points = new Vector3[2];

					points[0] = mod.splitOptions.sortOrigin.value;
					points[1] = Vector3.zero;	//mod.origin + Vector3.up * mod.buildDist;

					Handles.color = Color.blue;
					Handles.DrawAAPolyLine(settings.lineThickness, points);
				}
			}
		}
	}
}