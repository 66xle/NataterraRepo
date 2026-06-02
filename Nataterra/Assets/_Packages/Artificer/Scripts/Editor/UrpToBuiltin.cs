using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Artifice
{
	public class URPToBuiltinProjectConverter
	{
		[MenuItem("Tools/Artificer/Convert Selected Project Materials URP -> Builtin Standard")]
		static void ConvertSelectedProjectMaterials()
		{
			Object[] selectedAssets = Selection.objects;

			if ( selectedAssets.Length == 0 )
			{
				Debug.LogWarning("No assets selected.");
				return;
			}

			int convertedCount = 0;

			foreach ( Object obj in selectedAssets )
			{
				if ( obj is Material mat )
				{
					if ( ConvertMaterial(mat) )
						convertedCount++;
				}
			}

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log($"Material conversion complete. {convertedCount} materials converted.");
		}

		static bool ConvertMaterial(Material mat)
		{
			if ( mat.shader == null )
				return false;

			if ( mat.shader.name != "Universal Render Pipeline/Lit" )
				return false;

			Debug.Log($"Converting material: {mat.name}");

			Texture baseMap = mat.GetTexture("_BaseMap");
			Color baseColor = mat.GetColor("_BaseColor");

			Texture normalMap = mat.GetTexture("_BumpMap");
			float normalScale = mat.HasProperty("_BumpScale") ? mat.GetFloat("_BumpScale") : 1f;

			Texture metallicMap = mat.GetTexture("_MetallicGlossMap");
			float metallic = mat.HasProperty("_Metallic") ? mat.GetFloat("_Metallic") : 0f;
			float smoothness = mat.HasProperty("_Smoothness") ? mat.GetFloat("_Smoothness") : 0.5f;

			Texture emissionMap = mat.GetTexture("_EmissionMap");
			Color emissionColor = mat.HasProperty("_EmissionColor") ? mat.GetColor("_EmissionColor") : Color.black;

			Texture occlusionMap = mat.GetTexture("_OcclusionMap");

			Shader standard = Shader.Find("Standard");
			if ( standard == null )
			{
				Debug.LogError("Standard shader not found.");
				return false;
			}

			mat.shader = standard;

			if ( baseMap )
				mat.SetTexture("_MainTex", baseMap);

			mat.SetColor("_Color", baseColor);

			if ( normalMap )
			{
				mat.EnableKeyword("_NORMALMAP");
				mat.SetTexture("_BumpMap", normalMap);
				mat.SetFloat("_BumpScale", normalScale);
			}

			if ( metallicMap )
			{
				mat.EnableKeyword("_METALLICGLOSSMAP");
				mat.SetTexture("_MetallicGlossMap", metallicMap);
			}
			mat.SetFloat("_Metallic", metallic);
			mat.SetFloat("_Glossiness", smoothness);

			if ( occlusionMap )
			{
				mat.SetTexture("_OcclusionMap", occlusionMap);
				mat.SetFloat("_OcclusionStrength", 1f);
			}

			if ( emissionMap || emissionColor.maxColorComponent > 0.001f )
			{
				mat.EnableKeyword("_EMISSION");
				if ( emissionMap )
					mat.SetTexture("_EmissionMap", emissionMap);
				mat.SetColor("_EmissionColor", emissionColor);
			}

			EditorUtility.SetDirty(mat);
			return true;
		}
	}
}