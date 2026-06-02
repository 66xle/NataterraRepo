using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Artifice
{
	public class ArtificerUI
	{
		static GUIStyle		headerStyle;
		static GUIStyle		foldoutStyle;
		static GUIStyle		buttonStyle;
		static GUIStyle		medbuttonStyle;
		static Texture		buttonBack;
		public static bool	dragging = false;

		static string helpUrl	= "https://tubbycrumbles.gitbook.io/artificer/using-artificer/artificing-your-first-object/inspector-sections/build-options/build-from#";
		static string videoUrl	= "https://youtube.com";

		// https://www.youtube.com/watch?v=sivjOOT2tUk&t=9s

		static public void SetHelpUrl(string url)
		{
			helpUrl = url;
		}

		static public void SetVideoUrl(string url)
		{
			videoUrl = url;
		}

		static public void SetDirty(Object target)
		{
			if ( target )
			{
				EditorUtility.SetDirty(target);
				if ( !Application.isPlaying )
					EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}

		static public void Header(string name)
		{
			if ( headerStyle == null )
			{
				headerStyle = new GUIStyle();
				headerStyle.normal.textColor	= Color.grey;
				headerStyle.fontSize			= 16;
				headerStyle.fontStyle			= FontStyle.Bold;
			}
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.LabelField(name, headerStyle);
			EditorGUILayout.EndVertical();
		}

		static public bool BigButton(string name, string tip = "")
		{
			if ( buttonStyle == null )
			{

				buttonStyle = new GUIStyle(EditorStyles.miniButton);
				buttonStyle.fontSize	= 16;
				buttonStyle.fontStyle	= FontStyle.Bold;
				buttonStyle.fixedHeight	= 40;
			}

			//if ( !buttonBack )
			//	buttonBack = (Texture)Resources.Load<Texture>("blueprint");

			if ( GUILayout.Button(new GUIContent(name, buttonBack, tip), buttonStyle) )
				return true;

			return false;
		}

		static public bool MediumButton(string name, string tip = "")
		{
			if ( medbuttonStyle == null )
			{
				medbuttonStyle = new GUIStyle(EditorStyles.miniButton);
				medbuttonStyle.fontSize		= 12;
				medbuttonStyle.fontStyle	= FontStyle.Bold;
				medbuttonStyle.fixedHeight	= 30;
			}

			if ( GUILayout.Button(new GUIContent(name, tip), medbuttonStyle) )
				return true;

			return false;
		}

		static public void FoldOut(ref bool open, string name, string tip = "")
		{
			if ( headerStyle == null )
			{
				headerStyle = new GUIStyle();
				headerStyle.normal.textColor	= Color.white;
				headerStyle.fontSize			= 16;
				headerStyle.fontStyle			= FontStyle.Bold;
			}

			if ( foldoutStyle == null )
			{
				foldoutStyle = new GUIStyle(EditorStyles.foldout);
				Color col = new Color(0.8f, 0.8f, 0.8f, 1.0f);
				foldoutStyle.normal.textColor		= col;
				foldoutStyle.fontSize				= 16;
				foldoutStyle.fontStyle				= FontStyle.Bold;
				foldoutStyle.fontStyle				= FontStyle.Bold;
				foldoutStyle.normal.textColor		= col;
				foldoutStyle.onNormal.textColor		= col;
				foldoutStyle.hover.textColor		= col;
				foldoutStyle.onHover.textColor		= col;
				foldoutStyle.focused.textColor		= col;
				foldoutStyle.onFocused.textColor	= col;
				foldoutStyle.active.textColor		= col;
				foldoutStyle.onActive.textColor		= col;
			}

			EditorGUILayout.BeginVertical("box");
			open = EditorGUILayout.Foldout(open, new GUIContent(name, tip), true, foldoutStyle);
			EditorGUILayout.EndVertical();
		}

		static public void ShaderProperty(MaterialEditor matedit, MaterialProperty prop, string name, string tip = "")
		{
			matedit.ShaderProperty(prop, new GUIContent(name, tip));
		}

		static void BeginChangeCheck()
		{
			EditorGUI.BeginChangeCheck();
		}

		static bool EndChangeCheck()
		{
			return EditorGUI.EndChangeCheck();
		}

		static public void RecordObject(Object target, string name)
		{
			Undo.RecordObject(target, "Changed " + name);
		}

		static public void RecordObjects(Object[] targets, string name)
		{
			Undo.RecordObjects(targets, "Changed " + name);
		}

		static public void Slider(Object target, string name, ref float val, float min, float max, string tip)
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.Slider(new GUIContent(name, tip), val, min, max);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				SetDirty(target);
			}
		}

		static public void ColorField(Object target, string name, ref Color val, string tip = "")
		{
			BeginChangeCheck();
			Color newval = EditorGUILayout.ColorField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				SetDirty(target);
			}
		}

		static public void Texture2D(Object target, string name, ref Texture2D val, bool flag = true, string tip = "")
		{
			BeginChangeCheck();
			Texture2D newobj = (Texture2D)EditorGUILayout.ObjectField(new GUIContent(name, tip), val, typeof(Texture2D), flag);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newobj;
				SetDirty(target);
			}
		}

		static public void Vector4(Object target, string name, ref Vector4 val, string tip = "")
		{
			BeginChangeCheck();
			Vector4 newval = EditorGUILayout.Vector4Field(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				SetDirty(target);
			}
		}

		static public void Vector3(Object target, string name, ref Vector3 val, string tip = "")
		{
			BeginChangeCheck();
			Vector3 newval = EditorGUILayout.Vector3Field(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				SetDirty(target);
			}
		}

		static public bool Float(Object target, string name, ref float val, string tip = "")
		{
			BeginChangeCheck();
			float newval = EditorGUILayout.FloatField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				SetDirty(target);
				return true;
			}

			return false;
		}

		static public void Property(string name, SerializedProperty prop, string tip = "", string video = "", string help = "")
		{
			if ( video.Length == 0 )
			{
				EditorGUILayout.PropertyField(prop, new GUIContent(name, tip));
			}
			else
			{
				//EditorGUILayout.BeginHorizontal();

				EditorGUILayout.PropertyField(prop, new GUIContent(name, tip));
				//HelpWindowButton(video);

				//EditorGUILayout.EndHorizontal();
			}

			if ( help.Length != 0 )
			{
				Rect rect = GUILayoutUtility.GetLastRect();
			    Event e = Event.current;
				rect.width = EditorGUIUtility.labelWidth;
				if ( e.type == EventType.MouseDown && rect.Contains(e.mousePosition) )
				{
					GenericMenu menu = new GenericMenu();
					string furl = helpUrl + help;

					menu.AddItem(new GUIContent("Help"), false,
					() => Application.OpenURL(furl));

					menu.ShowAsContext();
					e.Use();
				}
			}
		}

		static public void Property(string name, SerializedProperty prop, ref bool changed, string tip = "", string video = "", string help = "")
		{
			if ( video.Length == 0 )
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(prop, new GUIContent(name, tip));
				if ( EditorGUI.EndChangeCheck() )
					changed = true;
			}
			else
			{
				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(prop, new GUIContent(name, tip));
				if ( EditorGUI.EndChangeCheck() )
					changed = true;
				HelpWindowButton(video);

				EditorGUILayout.EndHorizontal();
			}

			if ( help.Length != 0 )
			{
				Rect rect = GUILayoutUtility.GetLastRect();
			    Event e = Event.current;
				rect.width = EditorGUIUtility.labelWidth;
				if ( e.type == EventType.MouseDown && rect.Contains(e.mousePosition) )
				{
					string furl = helpUrl + help;
					GenericMenu menu = new GenericMenu();
					menu.AddItem(new GUIContent("Help"), false,
					() => Application.OpenURL(furl));

					if ( video.Length > 0 )
					{
						string vurl = videoUrl + video;
						menu.AddItem(new GUIContent("Video dark"), false,
						() => Application.OpenURL(vurl));
					}

					menu.ShowAsContext();
					e.Use();
				}
			}
		}

		static public void Property(SerializedProperty property, string propname, string name, ref bool changed, string tip = "", string video = "", string help = "")
		{
			SerializedProperty prop = property.FindPropertyRelative(propname);

			if ( video.Length == 0 )
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(prop, new GUIContent(name, tip));
				if ( EditorGUI.EndChangeCheck() )
					changed = true;
			}
			else
			{
				//EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(prop, new GUIContent(name, tip));
				if ( EditorGUI.EndChangeCheck() )
					changed = true;
				//HelpWindowButton(video);

				//EditorGUILayout.EndHorizontal();
			}
		}


		static readonly string[] modeLabels = { "Constant", "Random Between Two Values" };

		public static void PropertyFloatRange(string label, SerializedProperty property, ref bool changed, string tooltip = null, string video = null, string help = null)
		{
			EditorGUILayout.BeginHorizontal();

			// Get serialized properties
			var modeProp = property.FindPropertyRelative("mode");
			//var valueProp = property.FindPropertyRelative("value");
			var minProp = property.FindPropertyRelative("min");
			var maxProp = property.FindPropertyRelative("max");

			EditorGUI.BeginChangeCheck();

			if ( (FloatRange.Mode)modeProp.enumValueIndex == FloatRange.Mode.Constant )
			{
				EditorGUILayout.PropertyField(maxProp, new GUIContent(label));
			}
			else
			{
				float lw = EditorGUIUtility.labelWidth;
				EditorGUILayout.PropertyField(minProp, new GUIContent(label));  //new GUIContent("  "));
				EditorGUIUtility.labelWidth = 12.0f;
				EditorGUILayout.PropertyField(maxProp, new GUIContent("  "));

				EditorGUIUtility.labelWidth = lw;
			}

			modeProp.enumValueIndex = EditorGUILayout.Popup(modeProp.enumValueIndex, modeLabels, GUILayout.Width(18));
			if ( EditorGUI.EndChangeCheck() )
				changed = true;

			EditorGUILayout.EndHorizontal();

			if ( help != null )
			{
				Rect rect = GUILayoutUtility.GetLastRect();
			    Event e = Event.current;
				rect.width = EditorGUIUtility.labelWidth;
				if ( e.type == EventType.MouseDown && rect.Contains(e.mousePosition) )
				{
					GenericMenu menu = new GenericMenu();
					string furl = helpUrl + help;

					menu.AddItem(new GUIContent("Help"), false,
					() => Application.OpenURL(furl));

					menu.ShowAsContext();
					e.Use();
				}
			}
		}

		public static void PropertyVector3Range(string label, SerializedProperty property, ref bool changed, string tooltip = null, string video = null, string help = null)
		{
			EditorGUILayout.BeginHorizontal();

			// Get serialized properties
			var modeProp = property.FindPropertyRelative("mode");
			//var valueProp = property.FindPropertyRelative("value");
			var minProp = property.FindPropertyRelative("min");
			var maxProp = property.FindPropertyRelative("max");

			EditorGUI.BeginChangeCheck();

			if ( (FloatRange.Mode)modeProp.enumValueIndex == FloatRange.Mode.Constant )
			{
				EditorGUILayout.PropertyField(maxProp, new GUIContent(label));
			}
			else
			{
				//float lw = EditorGUIUtility.labelWidth;
				EditorGUILayout.PropertyField(minProp, new GUIContent(label));  //new GUIContent("  "));
				//EditorGUIUtility.labelWidth = 12.0f;
				//EditorGUILayout.PropertyField(maxProp, new GUIContent("  "));

				//EditorGUIUtility.labelWidth = lw;
			}

			modeProp.enumValueIndex = EditorGUILayout.Popup(modeProp.enumValueIndex, modeLabels, GUILayout.Width(18));

			EditorGUILayout.EndHorizontal();

			if ( (FloatRange.Mode)modeProp.enumValueIndex == FloatRange.Mode.RandomBetweenTwo )
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(maxProp, new GUIContent("  "));
				EditorGUILayout.LabelField(" ", GUILayout.Width(18.0f));
				EditorGUILayout.EndHorizontal();
			}

			if ( EditorGUI.EndChangeCheck() )
			{
				changed = true;
			}
		}


		public static void PropertyTog(SerializedProperty toggleProp, SerializedProperty targetProp, ref bool changed, string tooltip = null, string video = null, string help = null)
		{
			if ( toggleProp == null || targetProp == null )
				return;

			Rect rect = EditorGUILayout.GetControlRect();

			GUIContent label = new GUIContent(toggleProp.displayName, tooltip);

			Rect contentRect = EditorGUI.PrefixLabel(rect, label);

			float toggleWidth = 18f;
			float buttonWidth = !string.IsNullOrEmpty(video) ? 16f : 0f; // Adjust button width as needed
			float spacing = 4f;

			Rect toggleRect = new Rect(contentRect.x, contentRect.y, toggleWidth, contentRect.height);

			Rect fieldRect = new Rect(toggleRect.x + toggleWidth + spacing, contentRect.y, contentRect.width - toggleWidth - spacing - buttonWidth, contentRect.height);

			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(toggleRect, toggleProp, GUIContent.none);

			using ( new EditorGUI.DisabledScope(!toggleProp.boolValue) )
			{
				EditorGUI.PropertyField(fieldRect, targetProp, GUIContent.none, true);
			}

			if ( EditorGUI.EndChangeCheck() )
				changed = true;

			if ( !string.IsNullOrEmpty(video) )
			{
				if ( ArtificerSettings.GetSettings().showVideoHelp )
				{
					if ( !icon )
						icon = Resources.Load<Texture>("video dark");

					Rect buttonRect = new Rect(fieldRect.x + fieldRect.width + 2.0f, contentRect.y, buttonWidth, contentRect.height);
					if ( GUI.Button(buttonRect, new GUIContent(icon, "Play Video"), IconStyle) )
					{
						string url = videoUrl + video;
						Application.OpenURL(url);
					}
				}
			}

			if ( !string.IsNullOrEmpty(help) )
			{
				Rect rect1 = GUILayoutUtility.GetLastRect();
				Event e = Event.current;
				rect1.width = EditorGUIUtility.labelWidth;
				if ( e.type == EventType.MouseDown && rect1.Contains(e.mousePosition) )
				{
					GenericMenu menu = new GenericMenu();
					string furl = helpUrl + help;

					menu.AddItem(new GUIContent("Help"), false, () => Application.OpenURL(furl));

					if ( !string.IsNullOrEmpty(video) )
					{
						string vurl = videoUrl + video;
						menu.AddItem(new GUIContent("Video dark"), false, () => Application.OpenURL(vurl));
					}

					menu.ShowAsContext();
					e.Use();
				}
			}
		}

		public static void PropertyOverride(string name, SerializedProperty prop, ref bool changed, string tooltip = null, string video = null, string help = null)
		{
			if ( prop == null )
				return;

			SerializedProperty toggleProp = prop.FindPropertyRelative("overrideState");
			SerializedProperty valueProp = prop.FindPropertyRelative("value");

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PropertyField(toggleProp, GUIContent.none, GUILayout.Width(18));
			EditorGUILayout.PropertyField(valueProp, new GUIContent(name, tooltip));
			EditorGUILayout.EndHorizontal();

			if ( !string.IsNullOrEmpty(help) )
			{
				Rect rect1 = GUILayoutUtility.GetLastRect();
				Event e = Event.current;
				rect1.width = EditorGUIUtility.labelWidth;
				if ( e.type == EventType.MouseDown && rect1.Contains(e.mousePosition) )
				{
					GenericMenu menu = new GenericMenu();
					string furl = helpUrl + help;

					menu.AddItem(new GUIContent("Help"), false,
					() => Application.OpenURL(furl));

					menu.ShowAsContext();
					e.Use();
				}
			}
		}


		public static void PropertyOverride(SerializedProperty so, string pname, string name, ref bool changed, string tooltip = null, string video = null, string help = null)
		{
			if ( so == null )
				return;

			SerializedProperty prop = so.FindPropertyRelative(pname);

			if ( prop != null )
			{
				SerializedProperty toggleProp = prop.FindPropertyRelative("overrideState");
				SerializedProperty valueProp = prop.FindPropertyRelative("value");

				EditorGUILayout.BeginHorizontal();

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(toggleProp, GUIContent.none, GUILayout.Width(18));
				EditorGUILayout.PropertyField(valueProp, new GUIContent(name, tooltip));

				if ( EditorGUI.EndChangeCheck() )
				{
					changed = true;
				}
#if false
				if ( !string.IsNullOrEmpty(video) )
				{
					HelpWindowButton(video);
				}
#endif
				EditorGUILayout.EndHorizontal();
			}

			if ( !string.IsNullOrEmpty(help) )
			{
				Rect rect1 = GUILayoutUtility.GetLastRect();
				Event e = Event.current;
				rect1.width = EditorGUIUtility.labelWidth;
				if ( e.type == EventType.MouseDown && rect1.Contains(e.mousePosition) )
				{
					GenericMenu menu = new GenericMenu();
					string furl = helpUrl + help;

					menu.AddItem(new GUIContent("Help"), false,
					() => Application.OpenURL(furl));

					menu.ShowAsContext();
					e.Use();
				}
			}
		}


		static public void Int(Object target, string name, ref int val, string tip = "")
		{
			BeginChangeCheck();
			int newval = EditorGUILayout.IntField(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				SetDirty(target);
			}
		}

		static public bool Toggle(Object target, string name, ref bool val, string tip = "")
		{
			bool changed = false;
			BeginChangeCheck();
			bool newval = EditorGUILayout.Toggle(new GUIContent(name, tip), val);
			if ( EndChangeCheck() )
			{
				RecordObject(target, "Changed " + name);
				val = newval;
				changed = true;
				SetDirty(target);
			}

			return changed;
		}

		static public void Curve(Object target, string name, ref AnimationCurve crv, string tip = "")
		{
			BeginChangeCheck();
			AnimationCurve newcrv = EditorGUILayout.CurveField(new GUIContent(name, tip), crv);
			if ( EndChangeCheck() )
			{
				Undo.RegisterCompleteObjectUndo(target, "Changed " + name);
				crv = newcrv;
				SetDirty(target);
			}
		}

		static public void Transform(Object target, string name, ref Transform obj, string tip = "")
		{
			BeginChangeCheck();
			Transform newobj = (Transform)EditorGUILayout.ObjectField(new GUIContent(name, tip), obj, typeof(Transform), true);
			if ( EndChangeCheck() )
			{
				Undo.RegisterCompleteObjectUndo(target, "Changed " + name);
				obj = newobj;
				SetDirty(target);
			}
		}

		public static string GetPathToHelpVideos()
		{
			//var dirs = Directory.GetDirectories(Application.dataPath, "HelpVideos", SearchOption.AllDirectories);
			//return dirs.Length != 0 ? dirs[0] : string.Empty;
			return @"http://www.macspeedee.com/Artificer/VideoHelpers/";
		}

		static GUIStyle _helpBoxStyle;

		public static GUIStyle HelpBoxStyle
		{
			get
			{
				if ( _helpBoxStyle == null )
				{
					_helpBoxStyle = new GUIStyle("button");
					_helpBoxStyle.alignment		= TextAnchor.MiddleCenter;
					_helpBoxStyle.stretchHeight	= false;
					_helpBoxStyle.stretchWidth	= false;
				}

				return _helpBoxStyle;
			}
		}

		static GUIStyle _iconStyle;

		public static GUIStyle IconStyle
		{
			get
			{
				if ( _iconStyle == null )
				{
					_iconStyle = new GUIStyle("button");
					_iconStyle.alignment			= TextAnchor.MiddleCenter;
					_iconStyle.stretchHeight		= false;
					_iconStyle.stretchWidth			= false;
					_iconStyle.margin				= new RectOffset(0, 0, 0, 0);
					_iconStyle.padding				= new RectOffset(0, 0, 0, 0);
					_iconStyle.border				= new RectOffset(0, 0, 0, 0);
					_iconStyle.normal.background	= null; // remove default button background
					_iconStyle.hover.background		= null;
					_iconStyle.active.background	= null;
				}

				return _iconStyle;
			}
		}

		//static VideoTooltip window;
		//static string pathToHelpVideos;
#if false
		public static void OpenHelpVideoWindow(string filename)
		{
			if ( window != null ) window.Close();
			if ( window == null ) window = (VideoTooltip)EditorWindow.GetWindow(typeof(VideoTooltip));
			if ( string.IsNullOrEmpty(pathToHelpVideos) ) pathToHelpVideos = GetPathToHelpVideos();
			window.VideoClipFileURI = System.IO.Path.Combine(pathToHelpVideos, filename + ".mp4");
			window.maxSize = new Vector2(854, 480);
			window.minSize = new Vector2(854, 480);
			window.Show();
		}
#endif
		static Texture icon;

		static void HelpWindowButton(string fileName)
		{
			if ( !string.IsNullOrEmpty(fileName) )
			{
				if ( ArtificerSettings.GetSettings().showVideoHelp )
				{
					if ( !icon )
						icon = Resources.Load<Texture>("video dark");

					if ( GUILayout.Button(new GUIContent(icon), IconStyle, GUILayout.Width(16), GUILayout.Height(16)) )
					{
						string url = videoUrl + fileName;
						//OpenHelpVideoWindow(fileName);
						Application.OpenURL(url);
					}
				}
			}
		}

		public static void SplitOptionsParams(SplitOptions so, SerializedProperty _splitOptions, ref bool changed, bool adjust)
		{
			ArtificerUI.FoldOut(ref so.showSplitOptions, "Split Options");

			string burl = "https://tubbycrumbles.gitbook.io/artificer/using-artificer/";

			if ( adjust )
			{
				burl = burl + "adjust/";
			}
			else
				burl = burl + "split-params/";

			if ( so.showSplitOptions )
			{
				SetHelpUrl(burl + "split-options#");

				EditorGUILayout.BeginVertical("box");
				ArtificerUI.PropertyOverride(_splitOptions, "sortOrigin",			"Sort Origin",			ref changed, "Origin Point used to sort split elements from", "", "sort-origin");
				ArtificerUI.PropertyOverride(_splitOptions, "useSortSpline",		"Use Sort Spline",		ref changed, "Use a spline to control the sorting, useful for long thin objects like railways or roads", "", "use-sort-spline");
				ArtificerUI.PropertyOverride(_splitOptions, "sortSpline",			"Sort Spline",			ref changed, "The spline to use for sorting", "", "sort-spline");
				ArtificerUI.PropertyOverride(_splitOptions, "sortPathBias",			"Sort Spline Bias",		ref changed, "Controls whether distance along or distance from the spline controls the sort order", "", "sort-path-bias");
				ArtificerUI.PropertyOverride(_splitOptions, "sortMode",				"Sort Mode",			ref changed, "How the elements should be sorted if at all", "", "sort-mode");
				ArtificerUI.PropertyOverride(_splitOptions, "sortDistanceMode",		"Sort Distance Mode",	ref changed, "Which point is used to calculate the sorting distance", null, "sort-distance-mode");
				if ( !adjust )
				{
					ArtificerUI.PropertyOverride(_splitOptions, "splitMode",		"Split Mode",			ref changed, "How the target mesh will be split up, either by element or by material", null, "split-mode");
					ArtificerUI.PropertyOverride(_splitOptions, "dontSplit",		"Dont Split",			ref changed, "Whether this object should be split up at all", null, "dont-split");
					ArtificerUI.PropertyOverride(_splitOptions, "dontAdd",			"Dont Add",				ref changed, "Dont add this object to the build list", null, "dont-add");
					ArtificerUI.PropertyOverride(_splitOptions, "addAll",			"Add All",				ref changed, "When sorting the elements add all this objects meshes as a group", null, "add-all");
				}
				ArtificerUI.PropertyOverride(_splitOptions, "sortModifier",			"Sort Modifier",		ref changed, "A value added to the sort distance so you can alter the position in the sort for the object", null, "sort-modifier");
				EditorGUILayout.EndVertical();
			}

			ArtificerUI.FoldOut(ref so.showBuildOptions, "Build Options");

			if ( so.showBuildOptions )
			{
				SetHelpUrl(burl + "build-options#");

				EditorGUILayout.BeginVertical("box");
				ArtificerUI.PropertyOverride(_splitOptions, "buildDistRange",		"Build Dist R",			ref changed, "How far the part appears away when being placed", null, "build-dist-r");
				ArtificerUI.PropertyOverride(_splitOptions, "origin",				"Origin",				ref changed, "The origin is used to calculate the direction of placement when radial style is used", null, "origin");
				ArtificerUI.PropertyOverride(_splitOptions, "projection",			"Projection",			ref changed, "Use to alter the build direction vector", null, "projection");
				//ArtificerUI.PropertyOverride(_splitOptions, "buildTime",			"Build Time",			ref changed, "Time in seconds to build the whole object");
				ArtificerUI.PropertyOverride(_splitOptions, "buildStyle",			"Build Style",			ref changed, "The style in which the parts are built", null, "build-style");
				ArtificerUI.PropertyOverride(_splitOptions, "usePlaceCurve",		"Use Place Curve",		ref changed, "Use the place Curve to control the elements movement as its built", null, "use-place-curve");
				ArtificerUI.PropertyOverride(_splitOptions, "placeCurve",			"Place Curve",			ref changed, "Curve to control the elements movement as its built", null, "place-curve");
				ArtificerUI.PropertyOverride(_splitOptions, "useRotCurve",			"Use Rot Curve",		ref changed, "Use the Rot Curve to control the elements rotation as its built", null, "use-rot-curve");
				ArtificerUI.PropertyOverride(_splitOptions, "placeRotCurve",		"Place Rot Curve",		ref changed, "Curve to control the elements rotation as its built", null, "place-rot-curve");
				ArtificerUI.PropertyOverride(_splitOptions, "useScaleCurve",		"Use Scale Curve",		ref changed, "Use the scale curve to scale the whole element as it is built", null, "use-scale-curve");
				ArtificerUI.PropertyOverride(_splitOptions, "placeScaleCurve",		"Place Scale Curve",	ref changed, "Curve to control the elements scaling as its built", null, "place-scale-curve");
				ArtificerUI.PropertyOverride(_splitOptions, "perAxisScale",			"Per Axis Scale",		ref changed, "Allow per axis scaling for the element as it is built", null, "per-axis-scale");
				ArtificerUI.PropertyOverride(_splitOptions, "placeScaleCurveY",		"Place Scale Curve Y",	ref changed, "The Curve for the Y Scaling", null, "place-scale");
				ArtificerUI.PropertyOverride(_splitOptions, "placeScaleCurveZ",		"Place Scale Curve Z",	ref changed, "The curve for the Z scaling", null, "place_scale");
				//ArtificerUI.PropertyOverride(_splitOptions, "maxScale",				"Max Scale",			ref changed, "Overall scale of the mesh, best left as 1");
				ArtificerUI.PropertyOverride(_splitOptions, "meshPivot",			"Mesh Pivot",			ref changed, "Controls where the pivot point of the mesh element is", null, "mesh-pivot");
				//ArtificerUI.PropertyOverride(_splitOptions, "rotate",				"Rotate",				ref changed, "Angles to rotate the element through as it is built");
				ArtificerUI.PropertyOverride(_splitOptions, "rotateRange",			"Rotate Range",			ref changed, "Angles to rotate the element through as it is built", null, "rotate range");
				ArtificerUI.PropertyOverride(_splitOptions, "placeMode",			"Place Mode",			ref changed, "Whether to use time of speed for placement", null, "place-mode");
				if ( so.placeMode.value == PlaceMode.Time )
				{
					ArtificerUI.PropertyOverride(_splitOptions, "placeTimeRange",			"Place Time",	ref changed, "Time in seconds to build each part of the object", null, "place-time");
				}
				else
					ArtificerUI.PropertyOverride(_splitOptions, "placeTimeRange",			"Place Speed",	ref changed, "Time in seconds to build each part of the object", null, "place-speed");

				//ArtificerUI.PropertyOverride(_splitOptions, "placeTimeRange",		"Place Time Range",		ref changed, "Time in seconds to build each part of the object");
				ArtificerUI.Property(_splitOptions,			"buildFromOverride",	"Build From Override",	ref changed, "Whether to use the build from objects", "", "build-from-override");
				ArtificerUI.Property(_splitOptions,			"buildFromObjects",		"Build From Objects",	ref changed, "Array of build from locations", "", "build-from-objects");
				EditorGUILayout.EndVertical();
			}

			ArtificerUI.FoldOut(ref so.showDismantleOptions, "Dismantle Options");

			if ( so.showDismantleOptions )
			{
				SetHelpUrl(burl + "dismantle-options#");

				EditorGUILayout.BeginVertical("box");
				ArtificerUI.PropertyOverride(_splitOptions, "removeTimeRange",		"Remove Time",			ref changed, "How long it takes to remove the parts", null, "remove-time");
				//ArtificerUI.PropertyOverride(_splitOptions, "dismantleTime",		"Dismantle Time",		ref changed, "How long to take to dismantle the object");
				ArtificerUI.PropertyOverride(_splitOptions, "dismantleStyle",		"Dismantle Style",		ref changed, "The style in which the parts are dismantled", null, "dismantle-style");

				if ( so.dismantleStyle.value == DismantleStyle.Explode && so.dismantleStyle.overrideState )
				{
					ArtificerUI.PropertyOverride(_splitOptions, "minExplodeForce",		"Min Explode Force",	ref changed, "", null, "min-explode-force");
					ArtificerUI.PropertyOverride(_splitOptions, "maxExplodeForce",		"Max Explode Force",	ref changed, "", null, "max-explode-force");
					ArtificerUI.PropertyOverride(_splitOptions, "dismantleRotate",		"Dismantle Rotate",		ref changed, "", null, "dismantle-rotate");
					ArtificerUI.PropertyOverride(_splitOptions, "angVelRange",			"Ang Vel Range",		ref changed, "", null, "ang-vel-range");
					ArtificerUI.PropertyOverride(_splitOptions, "gravityModifier",		"Gravity Modifier",		ref changed, "", null, "gravity-modifier");
					ArtificerUI.PropertyOverride(_splitOptions, "dismantleProjection",	"Dismantle Projection",	ref changed, "", null, "dismantle-projection");
					ArtificerUI.PropertyOverride(_splitOptions, "bounce",				"Bounce",				ref changed, "", null, "bounce");
					ArtificerUI.PropertyOverride(_splitOptions, "linearDrag",			"Linear Drag",			ref changed, "", null, "linear-drag");
					ArtificerUI.PropertyOverride(_splitOptions, "angularDrag",			"Angular Drag",			ref changed, "", null, "angular-drag");
					ArtificerUI.PropertyOverride(_splitOptions, "collisionMode",		"Collision Mode",		ref changed, "", null, "collision-mode");

					if ( so.collisionMode.overrideState )
					{
						if ( so.collisionMode.value == CollisionMode.Raycast && so.collisionMode.overrideState )
							ArtificerUI.PropertyOverride(_splitOptions, "layers",			"Layers",			ref changed, "", null, "layers");
						else
							ArtificerUI.PropertyOverride(_splitOptions, "collisionY",		"Collision Y",		ref changed, "World Y pos to collide against", null, "collision-y");
					}
				}

				EditorGUILayout.EndVertical();
			}
		}

		public static void DrawSection(string title)
		{
		    GUILayout.Space(8);

			Rect rect = GUILayoutUtility.GetRect(0, 18, GUILayout.ExpandWidth(true));

			// Line
			Rect line = new Rect(rect.x, rect.y + rect.height * 0.5f, rect.width, 1);
			EditorGUI.DrawRect(line, EditorGUIUtility.isProSkin ? new Color(0.25f, 0.25f, 0.25f) : new Color(0.7f, 0.7f, 0.7f));

			// Text background
			Rect labelRect = new Rect(rect.x + 6, rect.y, EditorStyles.boldLabel.CalcSize(new GUIContent(title)).x + 6, rect.height);
			//EditorGUI.DrawRect(labelRect, EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.82f, 0.82f, 0.82f));

			// Text
			EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);
		}
	}
}