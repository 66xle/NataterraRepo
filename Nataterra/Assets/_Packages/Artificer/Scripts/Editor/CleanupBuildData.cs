using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Artifice
{
	public class CleanupBuildData : EditorWindow
	{
		string			folderPath	= "";
		Vector2			scroll;
		List<SOEntry>	entries		= new();

		private class SOEntry
		{
			public string		path;
			public bool			delete;
			public bool			expanded;
			public List<string>	referencedBy = new();
		}

		[MenuItem("Tools/Artificer/BuildData Object Reference Cleaner")]
		public static void Open()
		{
			GetWindow<CleanupBuildData>("SO Reference Cleaner");
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Scriptable Object Reference Cleaner", EditorStyles.boldLabel);
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Folder", GUILayout.Width(50));
			EditorGUILayout.TextField(folderPath);

			if ( GUILayout.Button("Select...", GUILayout.Width(80)) )
			{
				SelectFolder();
			}
			EditorGUILayout.EndHorizontal();

			using ( new EditorGUI.DisabledScope(string.IsNullOrEmpty(folderPath)) )
			{
				if ( GUILayout.Button("Scan") )
				{
					Scan();
				}
			}

			if (entries.Count == 0)
				return;

			EditorGUILayout.Space();
			scroll = EditorGUILayout.BeginScrollView(scroll);

			foreach ( var entry in entries )
			{
				EditorGUILayout.BeginVertical("box");

				EditorGUILayout.BeginHorizontal();
				entry.delete = EditorGUILayout.Toggle(entry.delete, GUILayout.Width(20));
				entry.expanded = EditorGUILayout.Foldout(entry.expanded, entry.path, true);
				EditorGUILayout.EndHorizontal();

				if ( entry.expanded )
				{
					if ( entry.referencedBy.Count == 0 )
					{
						EditorGUILayout.LabelField("No references found", EditorStyles.miniLabel);
					}
					else
					{
						EditorGUILayout.LabelField("Referenced By:", EditorStyles.miniBoldLabel);

						foreach ( var refPath in entry.referencedBy )
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("• " + refPath, EditorStyles.miniLabel);

							if ( GUILayout.Button("Ping", GUILayout.Width(40)) )
							{
								var obj = AssetDatabase.LoadAssetAtPath<Object>(refPath);
								EditorGUIUtility.PingObject(obj);
							}

							EditorGUILayout.EndHorizontal();
						}
					}
				}

				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndScrollView();

			using ( new EditorGUI.DisabledScope(!entries.Any(e => e.delete)) )
			{
				if ( GUILayout.Button("Delete Selected") )
				{
					DeleteSelected();
				}
			}
		}

		private void SelectFolder()
		{
			string selected = EditorUtility.OpenFolderPanel("Select BuildData Folder", Application.dataPath, "");

			if ( string.IsNullOrEmpty(selected) )
				return;

			if ( !selected.StartsWith(Application.dataPath) )
			{
				EditorUtility.DisplayDialog("Invalid Folder", "Selected folder must be inside this project's Assets folder.", "OK");
				return;
			}

			folderPath = "Assets" + selected.Substring(Application.dataPath.Length);
		}

		private void Scan()
		{
			entries.Clear();

			try
			{
				string[] soGuids = AssetDatabase.FindAssets("t:BuildData", new[] { folderPath });

				var soPaths = new HashSet<string>(soGuids.Select(AssetDatabase.GUIDToAssetPath));

				var referencedByMap = new Dictionary<string, List<string>>();
				foreach ( var so in soPaths )
					referencedByMap[so] = new List<string>();

				string[] candidateGuids = AssetDatabase.FindAssets("t:Prefab t:Scene t:BuildData");

				for ( int i = 0; i < candidateGuids.Length; i++ )
				{
					string assetPath = AssetDatabase.GUIDToAssetPath(candidateGuids[i]);

					if ( soPaths.Contains(assetPath) )
						continue;

					if ( EditorUtility.DisplayCancelableProgressBar("Scanning References", assetPath, (float)i / candidateGuids.Length))
					{
						return;
					}

					var deps = AssetDatabase.GetDependencies(assetPath, true);

					foreach ( var dep in deps )
					{
						if ( soPaths.Contains(dep) )
						{
							referencedByMap[dep].Add(assetPath);
						}
					}
				}

				foreach ( var soPath in soPaths )
				{
					if ( referencedByMap[soPath].Count == 0 )
					{
						entries.Add(new SOEntry
						{
							path			= soPath,
							delete			= false,
							expanded		= false,
							referencedBy	= referencedByMap[soPath]
						});
					}
				}
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		private void DeleteSelected()
		{
			if ( !EditorUtility.DisplayDialog("Delete BuildData Objects", "Are you sure you want to delete the selected BuildData Objects?\nThis cannot be undone.", "Delete", "Cancel") )
			{
				return;
			}

			foreach ( var entry in entries.Where(e => e.delete) )
			{
				AssetDatabase.DeleteAsset(entry.path);
			}

			AssetDatabase.Refresh();
			entries.Clear();
		}
	}
}