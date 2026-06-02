using UnityEditor;
using UnityEngine;

namespace Artifice
{
	[CreateAssetMenu(menuName = "Artificer/Settings")]
	public class ArtificerSettings : ScriptableObject
	{
		public bool					showMove			= true;
		public Color				mainColor			= new Color(0.3f, 0.8f, 1f, 0.5f);
		public Color				pathColor			= new Color(1.0f, 1.0f, 1.0f, 1.0f);
		public float				lineThickness		= 4.0f;
		public float				splineSegLength		= 0.1f;
		public bool					showVideoHelp		= true;
		static ArtificerSettings	settings;

		static public ArtificerSettings GetSettings()
		{
			if ( settings == null )
			{
				string[] sets = AssetDatabase.FindAssets("t:ArtificerSettings");

				if ( sets.Length > 0 )
					settings = (ArtificerSettings)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(sets[0]), typeof(ArtificerSettings));
				else
				{
					settings = CreateInstance<ArtificerSettings>();
					AssetDatabase.CreateAsset(settings, "Assets/Artificer/Scripts/Editor/ArtificerSettings.asset");
					AssetDatabase.SaveAssets();
				}
			}

			return settings;
		}

		[SettingsProvider]
		public static SettingsProvider CreateCustomSettingsProvider()
		{
			string path = "Preferences/Artificer";

			SettingsProvider provider = new SettingsProvider(path, SettingsScope.User)
			{
				label = "Artificer",

				guiHandler = (searchContext) =>
				{
					GetSettings();

					if ( settings )
					{
						EditorGUILayout.LabelField("Gizmo Settings");
						EditorGUI.indentLevel++;

						settings.mainColor			= EditorGUILayout.ColorField(new GUIContent("Main Gizmo Color",		"Main Color of the Gizmo outlines"), settings.mainColor);
						settings.lineThickness		= EditorGUILayout.Slider(new GUIContent("Line Thickness",			"Thickness of the various gizmo lines"), settings.lineThickness, 0.01f, 12.0f);
						settings.pathColor			= EditorGUILayout.ColorField(new GUIContent("Path Line Color",		"Color of the Path Spline"), settings.pathColor);
						settings.splineSegLength	= EditorGUILayout.FloatField(new GUIContent("Path Seg Length",		"Length of the Path Spline segments"), settings.splineSegLength);
						settings.showVideoHelp		= EditorGUILayout.Toggle(new GUIContent("Show Video Help",			"Show the Video Help Icons in the Inspector"), settings.showVideoHelp);

						EditorGUI.indentLevel--;

						if ( GUI.changed )
							EditorUtility.SetDirty(settings);
					}
				},

				keywords = new string[] { "Artificer", "Setting", "Build", "Split" }
			};

			return provider;
		}
	}
}