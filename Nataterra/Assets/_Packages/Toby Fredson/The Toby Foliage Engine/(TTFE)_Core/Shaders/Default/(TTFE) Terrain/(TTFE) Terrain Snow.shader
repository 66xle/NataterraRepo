// Made with Amplify Shader Editor v1.9.5.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toby Fredson/The Toby Foliage Engine/(TTFE) Terrain Snow"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Toggle(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)] _EnableInstancedPerPixelNormal("Enable Instanced Per-Pixel Normal", Float) = 0
		[HideInInspector]_TerrainHolesTexture("_TerrainHolesTexture", 2D) = "white" {}
		[HideInInspector]_Control("Control", 2D) = "white" {}
		[HideInInspector]_Control1("Control1", 2D) = "black" {}
		[HideInInspector]_Splat0("Splat0", 2D) = "white" {}
		[HideInInspector]_Normal0("Normal0", 2D) = "bump" {}
		[HideInInspector]_NormalScale0("NormalScale0", Float) = 1
		[HideInInspector]_Mask0("Mask0", 2D) = "white" {}
		[HideInInspector]_Smoothness0("Smoothness0", Range( 0 , 1)) = 0
		[HideInInspector]_Splat1("Splat1", 2D) = "white" {}
		[HideInInspector]_Normal1("Normal1", 2D) = "bump" {}
		[HideInInspector]_NormalScale1("NormalScale1", Float) = 1
		[HideInInspector]_Mask1("Mask1", 2D) = "white" {}
		[HideInInspector]_Smoothness1("Smoothness1", Range( 0 , 1)) = 0
		[HideInInspector]_Splat2("Splat2", 2D) = "white" {}
		[HideInInspector]_Normal2("Normal2", 2D) = "bump" {}
		[HideInInspector]_NormalScale2("NormalScale2", Float) = 1
		[HideInInspector]_Mask2("Mask2", 2D) = "white" {}
		[HideInInspector]_Smoothness2("Smoothness2", Range( 0 , 1)) = 1
		[HideInInspector]_Splat3("Splat3", 2D) = "white" {}
		[HideInInspector]_Mask3("Mask3", 2D) = "white" {}
		[HideInInspector]_NormalScale3("_NormalScale3", Float) = 1
		[HideInInspector]_Smoothness3("Smoothness3", Range( 0 , 1)) = 1
		[HideInInspector]_Normal3("Normal3", 2D) = "bump" {}
		[HideInInspector]_Splat4("Splat4", 2D) = "white" {}
		[HideInInspector]_Mask4("Mask4", 2D) = "white" {}
		[HideInInspector]_NormalScale4("NormalScale4", Float) = 1
		[HideInInspector]_Smoothness4("Smoothness4", Range( 0 , 1)) = 1
		[HideInInspector]_Normal4("Normal4", 2D) = "bump" {}
		[HideInInspector]_Splat5("Splat5", 2D) = "white" {}
		[HideInInspector]_Normal5("Normal5", 2D) = "bump" {}
		[HideInInspector]_Mask5("Mask5", 2D) = "white" {}
		[HideInInspector]_NormalScale5("NormalScale5", Float) = 1
		[HideInInspector]_Smoothness5("Smoothness5", Range( 0 , 1)) = 1
		[HideInInspector]_Splat6("Splat6", 2D) = "white" {}
		[HideInInspector]_Smoothness6("Smoothness6", Range( 0 , 1)) = 1
		[HideInInspector]_Normal6("Normal6", 2D) = "bump" {}
		[HideInInspector]_NormalScale6("NormalScale6", Float) = 1
		[HideInInspector]_Mask6("Mask6", 2D) = "white" {}
		[HideInInspector]_Splat7("Splat7", 2D) = "white" {}
		[HideInInspector]_Normal7("Normal7", 2D) = "bump" {}
		[HideInInspector]_NormalScale7("_NormalScale7", Float) = 1
		[HideInInspector]_Smoothness7("Smoothness7", Range( 0 , 1)) = 0
		[HideInInspector]_Mask7("Mask7", 2D) = "white" {}
		
		[DiffusionProfile]_DiffusionProfile("Diffusion Profile", Float) = 0[HideInInspector]_DiffusionProfile_Asset("Diffusion Profile", Vector) = ( 0, 0, 0, 0 )
		[Header(______________(TTFE) TERRAIN SNOW________________)][Header(_____________________________________________________)][Header(Texture Maps)][NoScaleOffset]_HeatMap("Heat Map", 2D) = "white" {}
		[NoScaleOffset][Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		[NoScaleOffset]_ColorMapProjection("Color Map Projection", 2D) = "white" {}
		[Header(_____________________________________________________)][Header(Texture Settings)][Header((Color Map))]_ColorMapIntensity("Color Map Intensity", Range( 0 , 1)) = 1
		[Toggle]_ColorMap("Color Map", Float) = 1
		[Header((Normal))]_NormalIntensity("Normal Intensity", Range( 0 , 1)) = 1
		[Toggle]_TerrainNormalMap("Terrain Normal Map", Float) = 0
		_SmoothnessIntensity("Smoothness Intensity", Range( 0 , 1)) = 1
		[Header((Slope))]_SlopeMax("Slope Max", Float) = 0
		_SlopeMin("Slope Min", Float) = 0
		[Toggle]_SnowSlope("Snow Slope", Float) = 1
		[Header(_____________________________________________________)][Header(Season Settings)][Header((Snow Control))]_SnowAmount("Snow Amount", Range( 0 , 10)) = 1
		_SnowScale("Snow Scale", Float) = 0
		_SnowOffset("Snow Offset", Float) = 0
		[Toggle]_NormalMapBasedSnow("Normal Map Based Snow", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector] _RenderQueueType("Render Queue Type", Float) = 1
		[HideInInspector][ToggleUI] _AddPrecomputedVelocity("Add Precomputed Velocity", Float) = 1
		[HideInInspector][ToggleUI] _SupportDecals("Support Decals", Float) = 1.0
		[HideInInspector] _StencilRef("Stencil Ref", Int) = 0 // StencilUsage.Clear
		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefDepth("Stencil Ref Depth", Int) = 0 // Nothing
		[HideInInspector] _StencilWriteMaskDepth("Stencil Write Mask Depth", Int) = 8 // StencilUsage.TraceReflectionRay
		[HideInInspector] _StencilRefMV("Stencil Ref MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilWriteMaskMV("Stencil Write Mask MV", Int) = 32 // StencilUsage.ObjectMotionVector
		[HideInInspector] _StencilRefDistortionVec("Stencil Ref Distortion Vec", Int) = 4 				// DEPRECATED
		[HideInInspector] _StencilWriteMaskDistortionVec("Stencil Write Mask Distortion Vec", Int) = 4	// DEPRECATED
		[HideInInspector] _StencilWriteMaskGBuffer("Stencil Write Mask GBuffer", Int) = 3 // StencilUsage.RequiresDeferredLighting | StencilUsage.SubsurfaceScattering
		[HideInInspector] _StencilRefGBuffer("Stencil Ref GBuffer", Int) = 2 // StencilUsage.RequiresDeferredLighting
		[HideInInspector] _ZTestGBuffer("ZTest GBuffer", Int) = 4
		[HideInInspector][ToggleUI] _RequireSplitLighting("Require Split Lighting", Float) = 1
		[HideInInspector][ToggleUI] _ReceivesSSR("Receives SSR", Float) = 1
		[HideInInspector][ToggleUI] _ReceivesSSRTransparent("Receives SSR Transparent", Float) = 0
		[HideInInspector] _SurfaceType("Surface Type", Float) = 0
		[HideInInspector] _BlendMode("Blend Mode", Float) = 0
		[HideInInspector] _SrcBlend("Src Blend", Float) = 1
		[HideInInspector] _DstBlend("Dst Blend", Float) = 0
		[HideInInspector] _DstBlend2("__dst2", Float) = 0
		[HideInInspector] _AlphaSrcBlend("Alpha Src Blend", Float) = 1
		[HideInInspector] _AlphaDstBlend("Alpha Dst Blend", Float) = 0
		[HideInInspector][ToggleUI] _ZWrite("ZWrite", Float) = 1
		[HideInInspector][ToggleUI] _TransparentZWrite("Transparent ZWrite", Float) = 0
		[HideInInspector] _CullMode("Cull Mode", Float) = 2
		[HideInInspector] _TransparentSortPriority("Transparent Sort Priority", Float) = 0
		[HideInInspector][ToggleUI] _EnableFogOnTransparent("Enable Fog", Float) = 1
		[HideInInspector] _CullModeForward("Cull Mode Forward", Float) = 2 // This mode is dedicated to Forward to correctly handle backface then front face rendering thin transparent
		[HideInInspector][Enum(UnityEngine.Rendering.HighDefinition.TransparentCullMode)] _TransparentCullMode("Transparent Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector] _ZTestDepthEqualForOpaque("ZTest Depth Equal For Opaque", Int) = 4 // Less equal
		[HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)] _ZTestTransparent("ZTest Transparent", Int) = 4 // Less equal
		[HideInInspector][ToggleUI] _TransparentBackfaceEnable("Transparent Backface Enable", Float) = 0
		[HideInInspector][ToggleUI] _AlphaCutoffEnable("Alpha Cutoff Enable", Float) = 0
		[HideInInspector][ToggleUI] _UseShadowThreshold("Use Shadow Threshold", Float) = 0
		[HideInInspector][ToggleUI] _DoubleSidedEnable("Double Sided Enable", Float) = 0
		[HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)] _DoubleSidedNormalMode("Double Sided Normal Mode", Float) = 2
		[HideInInspector] _DoubleSidedConstants("DoubleSidedConstants", Vector) = (1,1,-1,0)

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector][ToggleUI] _TransparentWritingMotionVec("Transparent Writing MotionVec", Float) = 0
		[HideInInspector][ToggleUI] _PerPixelSorting("_PerPixelSorting", Float) = 0.0
		[HideInInspector][Enum(UnityEngine.Rendering.HighDefinition.OpaqueCullMode)] _OpaqueCullMode("Opaque Cull Mode", Int) = 2 // Back culling by default
		[HideInInspector][ToggleUI] _EnableBlendModePreserveSpecularLighting("Enable Blend Mode Preserve Specular Lighting", Float) = 1
		[HideInInspector] _EmissionColor("Color", Color) = (1, 1, 1)

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

		[HideInInspector][Enum(Auto, 0, On, 1, Off, 2)] _DoubleSidedGIMode("Double sided GI mode", Float) = 0

		[HideInInspector][ToggleUI] _AlphaToMaskInspectorValue("_AlphaToMaskInspectorValue", Float) = 0 // Property used to save the alpha to mask state in the inspector
        [HideInInspector][ToggleUI] _AlphaToMask("__alphaToMask", Float) = 0

		//_Refrac ( "Refraction Model", Float) = 0
        [HideInInspector][ToggleUI]_DepthOffsetEnable("Boolean", Float) = 1
        [HideInInspector][ToggleUI]_ConservativeDepthOffsetEnable("Boolean", Float) = 1
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" "Queue"="Geometry" "TerrainCompatible"="True" "IgnoreProjector"="True" "SplatCount"="8" "MaskMapR"="Metallic" "MaskMapG"="AO" "MaskMapB"="Height" "MaskMapA"="Smoothness" }

		HLSLINCLUDE
		#pragma target 4.5
		#pragma exclude_renderers glcore gles gles3 ps4 ps5 

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		struct GlobalSurfaceDescription // GBuffer Forward META TransparentBackface
		{
			float3 BaseColor;
			float3 Normal;
			float3 BentNormal;
			float3 Specular;
			float CoatMask;
			float Metallic;
			float3 Emission;
			float Smoothness;
			float Occlusion;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPrepass;
			float AlphaClipThresholdDepthPostpass;
			float SpecularOcclusion;
			float SpecularAAScreenSpaceVariance;
			float SpecularAAThreshold;
			float RefractionIndex;
			float3 RefractionColor;
			float RefractionDistance;
			float DiffusionProfile;
			float TransmissionMask;
			float Thickness;
			float SubsurfaceMask;
			float Anisotropy;
			float3 Tangent;
			float IridescenceMask;
			float IridescenceThickness;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;
		};

		struct AlphaSurfaceDescription // ShadowCaster
		{
			float3 Emission;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;
		};

		struct SceneSurfaceDescription // SceneSelection
		{
		    float3 Emission;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float RefractionIndex;
			float3 RefractionColor;
			float RefractionDistance;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;
		};

		struct PrePassSurfaceDescription // DepthPrePass
		{
			float3 Normal;
			float3 Emission;
			float Smoothness;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPrepass;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;
		};

		struct PostPassSurfaceDescription //DepthPostPass
		{
			float3 Emission;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float AlphaClipThresholdDepthPostpass;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;
		};

		struct SmoothSurfaceDescription // MotionVectors DepthOnly
		{
			float3 Normal;
			float3 Emission;
			float Smoothness;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;
		};

        struct PickingSurfaceDescription //Picking
		{
            float3 BentNormal;
			float3 Emission;
			float Alpha;
			float AlphaClipThreshold;
			float AlphaClipThresholdShadow;
			float3 BakedGI;
			float3 BakedBackGI;
			float DepthOffset;
			float4 VTPackedFeedback;

			float3 ObjectSpaceNormal;
			float3 WorldSpaceNormal;
			float3 TangentSpaceNormal;
			float3 ObjectSpaceViewDirection;
			float3 WorldSpaceViewDirection;
			float3 ObjectSpacePosition;
		};

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlaneASE (float3 pos, float4 plane)
		{
			return dot (float4(pos,1.0f), plane);
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlaneASE(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlaneASE(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlaneASE(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlaneASE(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlaneASE(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="GBuffer" "TerrainCompatible"="True" "IgnoreProjector"="True" "SplatCount"="8" "MaskMapR"="Metallic" "MaskMapG"="AO" "MaskMapB"="Height" "MaskMapA"="Smoothness" }

			Cull [_CullMode]
			ZTest [_ZTestGBuffer]

			Stencil
			{
				Ref [_StencilRefGBuffer]
				WriteMask [_StencilWriteMaskGBuffer]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM
            #pragma shader_feature_local_fragment _ _DISABLE_DECALS
            #pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
            #pragma shader_feature_local _ _ALPHATEST_ON
            #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
            #pragma shader_feature_local _ _DOUBLESIDED_ON
            #define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
            #define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
            #pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
            #define _AMBIENT_OCCLUSION 1
            #define ASE_SRP_VERSION 170003
            #ifdef UNITY_COLORSPACE_GAMMA//ASE Color Space Def
            #define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)//ASE Color Space Def
            #else // Linear values//ASE Color Space Def
            #define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)//ASE Color Space Def
            #endif//ASE Color Space Def
            #define ASE_USING_SAMPLING_MACROS 1

            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

			#pragma multi_compile_fragment _ RENDERING_LAYERS
            #pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile_fragment _ PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
            #pragma multi_compile_fragment _ DECAL_SURFACE_GRADIENT
            #pragma multi_compile _ USE_LEGACY_LIGHTMAPS

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_GBUFFER

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif

            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float4 _DiffuseRemapScale0;
			TEXTURE2D(_Splat0);
			SAMPLER(sampler_Splat0);
			float4 _DiffuseRemapScale1;
			TEXTURE2D(_Splat1);
			float4 _DiffuseRemapScale2;
			TEXTURE2D(_Splat2);
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_Splat3);
			TEXTURE2D(_Control1);
			SAMPLER(sampler_Control1);
			float4 _DiffuseRemapScale4;
			TEXTURE2D(_Splat4);
			SAMPLER(sampler_Splat4);
			float4 _DiffuseRemapScale5;
			TEXTURE2D(_Splat5);
			float4 _DiffuseRemapScale6;
			TEXTURE2D(_Splat6);
			float4 _DiffuseRemapScale7;
			TEXTURE2D(_Splat7);
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_ColorMapProjection);
			SAMPLER(sampler_ColorMapProjection);
			TEXTURE2D(_HeatMap);
			SAMPLER(sampler_HeatMap);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			float4x4 unity_CameraProjection;
			float4x4 unity_CameraInvProjection;
			float4x4 unity_WorldToCamera;
			float4x4 unity_CameraToWorld;
			TEXTURE2D(_Normal0);
			SAMPLER(sampler_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_Normal4);
			SAMPLER(sampler_Normal4);
			TEXTURE2D(_Normal5);
			TEXTURE2D(_Normal6);
			TEXTURE2D(_Normal7);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Mask1);
			TEXTURE2D(_Mask2);
			TEXTURE2D(_Mask3);
			TEXTURE2D(_Mask4);
			SAMPLER(sampler_Mask4);
			TEXTURE2D(_Mask5);
			TEXTURE2D(_Mask6);
			TEXTURE2D(_Mask7);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_RELATIVE_WORLD_POS
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 uv1 : TEXCOORD3;
				float4 uv2 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};


			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 PerturbNormal107_g58( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			float3 PerturbNormal107_g57( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;

				surfaceData.baseColor =					surfaceDescription.BaseColor;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness =					surfaceDescription.Thickness;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.transmissionMask =			surfaceDescription.TransmissionMask;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
                        surfaceData.ior =                       surfaceDescription.RefractionIndex;
                        surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                        surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
                    float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;
        
                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
                    GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
				#endif

				#if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
				    ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData); 
				#endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif  
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

				float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				float2 break291_g55 = _Control_ST.zw;
				float2 appendResult293_g55 = (float2(( break291_g55.x + 0.001 ) , ( break291_g55.y + 0.0001 )));
				float2 vertexToFrag286_g55 = ( ( inputMesh.ase_texcoord.xy * _Control_ST.xy ) + appendResult293_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord5.xy = vertexToFrag286_g55;
				float2 break951_g55 = _Control1_ST.zw;
				float2 appendResult956_g55 = (float2(( break951_g55.x + 0.001 ) , ( break951_g55.y + 0.0001 )));
				float2 vertexToFrag958_g55 = ( ( inputMesh.ase_texcoord.xy * _Control1_ST.xy ) + appendResult956_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord6.xy = vertexToFrag958_g55;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				float ase_vertexTangentSign = inputMesh.tangentOS.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord7.xyz = ase_worldBitangent;
				
				outputPackedVaryingsMeshToPS.ase_texcoord5.zw = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord6.zw = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord7.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.uv1.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.uv2.xyzw = inputMesh.uv2;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput,
						OUTPUT_GBUFFER(outGBuffer)
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
						)
			{

				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				float3 positionRWS = packedInput.positionRWS.xyz;
				float3 normalWS = packedInput.normalWS.xyz;
				float4 tangentWS = packedInput.tangentWS.xyzw;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.uv1.xyzw;
				input.texCoord2 = packedInput.uv2.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				SurfaceData surfaceData;
				BuiltinData builtinData;

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 vertexToFrag286_g55 = packedInput.ase_texcoord5.xy;
				float4 Control26_g55 = SAMPLE_TEXTURE2D( _Control, sampler_Control, vertexToFrag286_g55 );
				float2 uv_Splat0 = packedInput.ase_texcoord5.zw * _Splat0_ST.xy + _Splat0_ST.zw;
				float4 tex2DNode414_g55 = SAMPLE_TEXTURE2D( _Splat0, sampler_Splat0, uv_Splat0 );
				float3 Splat0342_g55 = (tex2DNode414_g55).rgb;
				float2 uv_Splat1 = packedInput.ase_texcoord5.zw * _Splat1_ST.xy + _Splat1_ST.zw;
				float4 tex2DNode420_g55 = SAMPLE_TEXTURE2D( _Splat1, sampler_Splat0, uv_Splat1 );
				float3 Splat1379_g55 = (tex2DNode420_g55).rgb;
				float2 uv_Splat2 = packedInput.ase_texcoord5.zw * _Splat2_ST.xy + _Splat2_ST.zw;
				float4 tex2DNode417_g55 = SAMPLE_TEXTURE2D( _Splat2, sampler_Splat0, uv_Splat2 );
				float3 Splat2357_g55 = (tex2DNode417_g55).rgb;
				float2 uv_Splat3 = packedInput.ase_texcoord5.zw * _Splat3_ST.xy + _Splat3_ST.zw;
				float4 tex2DNode423_g55 = SAMPLE_TEXTURE2D( _Splat3, sampler_Splat0, uv_Splat3 );
				float3 Splat3390_g55 = (tex2DNode423_g55).rgb;
				float4 weightedBlendVar9_g55 = Control26_g55;
				float3 weightedBlend9_g55 = ( weightedBlendVar9_g55.x*( (_DiffuseRemapScale0).rgb * Splat0342_g55 ) + weightedBlendVar9_g55.y*( (_DiffuseRemapScale1).rgb * Splat1379_g55 ) + weightedBlendVar9_g55.z*( (_DiffuseRemapScale2).rgb * Splat2357_g55 ) + weightedBlendVar9_g55.w*( (_DiffuseRemapScale3).rgb * Splat3390_g55 ) );
				float2 vertexToFrag958_g55 = packedInput.ase_texcoord6.xy;
				float4 Control1922_g55 = SAMPLE_TEXTURE2D( _Control1, sampler_Control1, vertexToFrag958_g55 );
				float2 uv_Splat4 = packedInput.ase_texcoord5.zw * _Splat4_ST.xy + _Splat4_ST.zw;
				float4 tex2DNode696_g55 = SAMPLE_TEXTURE2D( _Splat4, sampler_Splat4, uv_Splat4 );
				float3 Splat4752_g55 = (tex2DNode696_g55).rgb;
				float2 uv_Splat5 = packedInput.ase_texcoord5.zw * _Splat5_ST.xy + _Splat5_ST.zw;
				float4 tex2DNode702_g55 = SAMPLE_TEXTURE2D( _Splat5, sampler_Splat0, uv_Splat5 );
				float3 Splat5743_g55 = (tex2DNode702_g55).rgb;
				float2 uv_Splat6 = packedInput.ase_texcoord5.zw * _Splat6_ST.xy + _Splat6_ST.zw;
				float4 tex2DNode709_g55 = SAMPLE_TEXTURE2D( _Splat6, sampler_Splat0, uv_Splat6 );
				float3 Splat6759_g55 = (tex2DNode709_g55).rgb;
				float2 uv_Splat7 = packedInput.ase_texcoord5.zw * _Splat7_ST.xy + _Splat7_ST.zw;
				float4 tex2DNode719_g55 = SAMPLE_TEXTURE2D( _Splat7, sampler_Splat0, uv_Splat7 );
				float3 Splat7762_g55 = (tex2DNode719_g55).rgb;
				float4 weightedBlendVar912_g55 = Control1922_g55;
				float3 weightedBlend912_g55 = ( weightedBlendVar912_g55.x*( (_DiffuseRemapScale4).rgb * Splat4752_g55 ) + weightedBlendVar912_g55.y*( (_DiffuseRemapScale5).rgb * Splat5743_g55 ) + weightedBlendVar912_g55.z*( (_DiffuseRemapScale6).rgb * Splat6759_g55 ) + weightedBlendVar912_g55.w*( (_DiffuseRemapScale7).rgb * Splat7762_g55 ) );
				float3 temp_output_968_0_g55 = ( weightedBlend9_g55 + weightedBlend912_g55 );
				float3 localClipHoles453_g55 = ( temp_output_968_0_g55 );
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord5.zw * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float Hole453_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole453_g55 == 0.0f ? -1 : 1);
				#endif
				}
				float3 BaseColorOut1373 = localClipHoles453_g55;
				float2 uv_ColorMapProjection1434 = packedInput.ase_texcoord5.zw;
				float4 ColorMap_Top1443 = ( unity_ColorSpaceDouble * SAMPLE_TEXTURE2D( _ColorMapProjection, sampler_ColorMapProjection, uv_ColorMapProjection1434 ) );
				float4 lerpResult1381 = lerp( float4( BaseColorOut1373 , 0.0 ) , ( ColorMap_Top1443 * float4( BaseColorOut1373 , 0.0 ) ) , _ColorMapIntensity);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				float2 appendResult1413 = (float2(ase_parentObjectScale.x , ase_parentObjectScale.z));
				float3 ase_worldPos = GetAbsolutePositionWS( positionRWS );
				float2 appendResult1414 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 temp_output_1418_0 = ( appendResult1413 * appendResult1414 );
				float simpleNoise1424 = SimpleNoise( temp_output_1418_0*400.0 );
				float temp_output_1430_0 = (simpleNoise1424*0.6 + 0.6);
				float4 temp_cast_3 = (temp_output_1430_0).xxxx;
				float2 uv_HeatMap1415 = packedInput.ase_texcoord5.zw;
				float temp_output_1420_0 = ( 1.0 - SAMPLE_TEXTURE2D( _HeatMap, sampler_HeatMap, uv_HeatMap1415 ).b );
				float2 uv_NormalMap1416 = packedInput.ase_texcoord5.zw;
				float3 unpack1416 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, uv_NormalMap1416 ), _NormalIntensity );
				unpack1416.z = lerp( 1, unpack1416.z, saturate(_NormalIntensity) );
				float3 tex2DNode1416 = unpack1416;
				float temp_output_1427_0 = saturate( normalWS.y );
				float3 _Tangent = float3(0,1,0);
				float3 ase_worldBitangent = packedInput.ase_texcoord7.xyz;
				float3x3 ase_tangentToWorldFast = float3x3(tangentWS.xyz.x,ase_worldBitangent.x,normalWS.x,tangentWS.xyz.y,ase_worldBitangent.y,normalWS.y,tangentWS.xyz.z,ase_worldBitangent.z,normalWS.z);
				float3 tangentToWorldDir1403 = normalize( mul( ase_tangentToWorldFast, _Tangent ) );
				float dotResult1404 = dot( tangentToWorldDir1403 , _Tangent );
				float lerpResult1408 = lerp( _SlopeMax , _SlopeMin , abs( dotResult1404 ));
				float Slope1417 = saturate( lerpResult1408 );
				float temp_output_1446_0 = saturate( ( (( _NormalMapBasedSnow )?( ( (temp_output_1420_0*-4.39 + 1.1) * (tex2DNode1416.g*_SnowScale + _SnowOffset) ) ):( (temp_output_1420_0*_SnowScale + _SnowOffset) )) * (( _SnowSlope )?( ( temp_output_1427_0 * Slope1417 ) ):( temp_output_1427_0 )) * _SnowAmount ) );
				float4 lerpResult1389 = lerp( (( _ColorMap )?( lerpResult1381 ):( float4( BaseColorOut1373 , 0.0 ) )) , temp_cast_3 , temp_output_1446_0);
				float4 AlbedoSnow_Output1392 = lerpResult1389;
				
				float4 Normal0341_g55 = SAMPLE_TEXTURE2D( _Normal0, sampler_Normal0, uv_Splat0 );
				float3 unpack490_g55 = UnpackNormalScale( Normal0341_g55, _NormalScale0 );
				unpack490_g55.z = lerp( 1, unpack490_g55.z, saturate(_NormalScale0) );
				float4 Normal1378_g55 = SAMPLE_TEXTURE2D( _Normal1, sampler_Normal0, uv_Splat1 );
				float3 unpack496_g55 = UnpackNormalScale( Normal1378_g55, _NormalScale1 );
				unpack496_g55.z = lerp( 1, unpack496_g55.z, saturate(_NormalScale1) );
				float4 Normal2356_g55 = SAMPLE_TEXTURE2D( _Normal2, sampler_Normal0, uv_Splat2 );
				float3 unpack494_g55 = UnpackNormalScale( Normal2356_g55, _NormalScale2 );
				unpack494_g55.z = lerp( 1, unpack494_g55.z, saturate(_NormalScale2) );
				float4 Normal3398_g55 = SAMPLE_TEXTURE2D( _Normal3, sampler_Normal0, uv_Splat3 );
				float3 unpack491_g55 = UnpackNormalScale( Normal3398_g55, _NormalScale3 );
				unpack491_g55.z = lerp( 1, unpack491_g55.z, saturate(_NormalScale3) );
				float4 weightedBlendVar473_g55 = Control26_g55;
				float3 weightedBlend473_g55 = ( weightedBlendVar473_g55.x*unpack490_g55 + weightedBlendVar473_g55.y*unpack496_g55 + weightedBlendVar473_g55.z*unpack494_g55 + weightedBlendVar473_g55.w*unpack491_g55 );
				float4 Normal4746_g55 = SAMPLE_TEXTURE2D( _Normal4, sampler_Normal4, uv_Splat4 );
				float3 unpack856_g55 = UnpackNormalScale( Normal4746_g55, _NormalScale4 );
				unpack856_g55.z = lerp( 1, unpack856_g55.z, saturate(_NormalScale4) );
				float4 Normal5740_g55 = SAMPLE_TEXTURE2D( _Normal5, sampler_Normal0, uv_Splat5 );
				float3 unpack857_g55 = UnpackNormalScale( Normal5740_g55, _NormalScale5 );
				unpack857_g55.z = lerp( 1, unpack857_g55.z, saturate(_NormalScale5) );
				float4 Normal6754_g55 = SAMPLE_TEXTURE2D( _Normal6, sampler_Normal0, uv_Splat6 );
				float3 unpack858_g55 = UnpackNormalScale( Normal6754_g55, _NormalScale6 );
				unpack858_g55.z = lerp( 1, unpack858_g55.z, saturate(_NormalScale6) );
				float4 Normal7764_g55 = SAMPLE_TEXTURE2D( _Normal7, sampler_Normal0, uv_Splat7 );
				float3 unpack854_g55 = UnpackNormalScale( Normal7764_g55, _NormalScale7 );
				unpack854_g55.z = lerp( 1, unpack854_g55.z, saturate(_NormalScale7) );
				float4 weightedBlendVar860_g55 = Control1922_g55;
				float3 weightedBlend860_g55 = ( weightedBlendVar860_g55.x*unpack856_g55 + weightedBlendVar860_g55.y*unpack857_g55 + weightedBlendVar860_g55.z*unpack858_g55 + weightedBlendVar860_g55.w*unpack854_g55 );
				float3 break513_g55 = ( weightedBlend473_g55 + weightedBlend860_g55 );
				float3 appendResult514_g55 = (float3(break513_g55.x , break513_g55.y , ( break513_g55.z + 0.001 )));
				#ifdef _TERRAIN_INSTANCED_PERPIXEL_NORMAL
				float3 staticSwitch503_g55 = appendResult514_g55;
				#else
				float3 staticSwitch503_g55 = appendResult514_g55;
				#endif
				float3 NormalOut1374 = staticSwitch503_g55;
				float3 surf_pos107_g58 = ase_worldPos;
				float3 surf_norm107_g58 = normalWS;
				float simpleNoise1429 = SimpleNoise( temp_output_1418_0*2.0 );
				float height107_g58 = simpleNoise1429;
				float scale107_g58 = 8.0;
				float3 localPerturbNormal107_g58 = PerturbNormal107_g58( surf_pos107_g58 , surf_norm107_g58 , height107_g58 , scale107_g58 );
				float3x3 ase_worldToTangent = float3x3(tangentWS.xyz,ase_worldBitangent,normalWS);
				float3 worldToTangentDir42_g58 = mul( ase_worldToTangent, localPerturbNormal107_g58);
				float3 surf_pos107_g57 = ase_worldPos;
				float3 surf_norm107_g57 = normalWS;
				float height107_g57 = temp_output_1430_0;
				float scale107_g57 = 0.06;
				float3 localPerturbNormal107_g57 = PerturbNormal107_g57( surf_pos107_g57 , surf_norm107_g57 , height107_g57 , scale107_g57 );
				float3 worldToTangentDir42_g57 = mul( ase_worldToTangent, localPerturbNormal107_g57);
				float2 temp_output_1_0_g59 = BlendNormal( worldToTangentDir42_g58 , worldToTangentDir42_g57 ).xy;
				float dotResult4_g59 = dot( temp_output_1_0_g59 , temp_output_1_0_g59 );
				float3 appendResult10_g59 = (float3((temp_output_1_0_g59).x , (temp_output_1_0_g59).y , sqrt( ( 1.0 - saturate( dotResult4_g59 ) ) )));
				float3 normalizeResult12_g59 = normalize( appendResult10_g59 );
				float3 lerpResult1378 = lerp( NormalOut1374 , normalizeResult12_g59 , temp_output_1446_0);
				float3 NormalMap_Top1450 = tex2DNode1416;
				float3 NormalSnow_Output1393 = (( _TerrainNormalMap )?( BlendNormal( NormalMap_Top1450 , lerpResult1378 ) ):( lerpResult1378 ));
				
				float4 tex2DNode416_g55 = SAMPLE_TEXTURE2D( _Mask0, sampler_Mask0, uv_Splat0 );
				float Mask0A335_g55 = tex2DNode416_g55.a;
				float4 tex2DNode422_g55 = SAMPLE_TEXTURE2D( _Mask1, sampler_Mask0, uv_Splat1 );
				float Mask1A369_g55 = tex2DNode422_g55.a;
				float4 tex2DNode419_g55 = SAMPLE_TEXTURE2D( _Mask2, sampler_Mask0, uv_Splat2 );
				float Mask2A360_g55 = tex2DNode419_g55.a;
				float4 tex2DNode425_g55 = SAMPLE_TEXTURE2D( _Mask3, sampler_Mask0, uv_Splat3 );
				float Mask3A391_g55 = tex2DNode425_g55.a;
				float4 weightedBlendVar547_g55 = Control26_g55;
				float weightedBlend547_g55 = ( weightedBlendVar547_g55.x*( _Smoothness0 * Mask0A335_g55 ) + weightedBlendVar547_g55.y*( _Smoothness1 * Mask1A369_g55 ) + weightedBlendVar547_g55.z*( _Smoothness2 * Mask2A360_g55 ) + weightedBlendVar547_g55.w*( _Smoothness3 * Mask3A391_g55 ) );
				float4 tex2DNode697_g55 = SAMPLE_TEXTURE2D( _Mask4, sampler_Mask4, uv_Splat4 );
				float Mask4A750_g55 = tex2DNode697_g55.a;
				float4 tex2DNode704_g55 = SAMPLE_TEXTURE2D( _Mask5, sampler_Mask0, uv_Splat5 );
				float Mask5A745_g55 = tex2DNode704_g55.a;
				float4 tex2DNode711_g55 = SAMPLE_TEXTURE2D( _Mask6, sampler_Mask0, uv_Splat6 );
				float Mask6A758_g55 = tex2DNode711_g55.a;
				float4 tex2DNode717_g55 = SAMPLE_TEXTURE2D( _Mask7, sampler_Mask0, uv_Splat7 );
				float Mask7A768_g55 = tex2DNode717_g55.a;
				float4 weightedBlendVar826_g55 = Control1922_g55;
				float weightedBlend826_g55 = ( weightedBlendVar826_g55.x*( _Smoothness4 * Mask4A750_g55 ) + weightedBlendVar826_g55.y*( _Smoothness5 * Mask5A745_g55 ) + weightedBlendVar826_g55.z*( _Smoothness6 * Mask6A758_g55 ) + weightedBlendVar826_g55.w*( _Smoothness7 * Mask7A768_g55 ) );
				float SmoothOut1380 = ( _SmoothnessIntensity * ( weightedBlend547_g55 + weightedBlend826_g55 ) );
				float lerpResult1391 = lerp( SmoothOut1380 , (simpleNoise1424*1.4 + 0.0) , temp_output_1446_0);
				float Smoothness_Output1394 = lerpResult1391;
				
				float Mask0G409_g55 = tex2DNode416_g55.g;
				float Mask1G371_g55 = tex2DNode422_g55.g;
				float Mask2G358_g55 = tex2DNode419_g55.g;
				float Mask3G389_g55 = tex2DNode425_g55.g;
				float4 weightedBlendVar602_g55 = Control26_g55;
				float weightedBlend602_g55 = ( weightedBlendVar602_g55.x*saturate( Mask0G409_g55 ) + weightedBlendVar602_g55.y*saturate( Mask1G371_g55 ) + weightedBlendVar602_g55.z*saturate( Mask2G358_g55 ) + weightedBlendVar602_g55.w*saturate( Mask3G389_g55 ) );
				float Mask4G748_g55 = tex2DNode697_g55.g;
				float Mask5G742_g55 = tex2DNode704_g55.g;
				float Mask6G756_g55 = tex2DNode711_g55.g;
				float Mask7G766_g55 = tex2DNode717_g55.g;
				float4 weightedBlendVar799_g55 = Control1922_g55;
				float weightedBlend799_g55 = ( weightedBlendVar799_g55.x*saturate( Mask4G748_g55 ) + weightedBlendVar799_g55.y*saturate( Mask5G742_g55 ) + weightedBlendVar799_g55.z*saturate( Mask6G756_g55 ) + weightedBlendVar799_g55.w*saturate( Mask7G766_g55 ) );
				float AoOut1457 = saturate( ( weightedBlend602_g55 + weightedBlend799_g55 ) );
				float lerpResult1460 = lerp( AoOut1457 , 1.0 , temp_output_1446_0);
				float Ao_Output1466 = lerpResult1460;
				
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.BaseColor = AlbedoSnow_Output1392.rgb;
				surfaceDescription.Normal = NormalSnow_Output1393;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = 0;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = Smoothness_Output1394;
				surfaceDescription.Occlusion = Ao_Output1466;
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				surfaceDescription.AlphaClipThresholdDepthPrepass = 0.5;
				surfaceDescription.AlphaClipThresholdDepthPostpass = 0.5;

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 0;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.TransmissionMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = _DiffusionProfile;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif
				#ifdef ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				#endif

				GetSurfaceAndBuiltinData( surfaceDescription, input, V, posInput, surfaceData, builtinData );
				ENCODE_INTO_GBUFFER( surfaceData, builtinData, posInput.positionSS, outGBuffer );
				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "META"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#ifdef UNITY_COLORSPACE_GAMMA//ASE Color Space Def
			#define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)//ASE Color Space Def
			#else // Linear values//ASE Color Space Def
			#define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)//ASE Color Space Def
			#endif//ASE Color Space Def
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma shader_feature _ EDITOR_VISUALIZATION
			#pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

            #define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
            #define SCENEPICKINGPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

            #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
            #undef  _REFRACTION_PLANE
            #undef  _REFRACTION_SPHERE
            #define _REFRACTION_THIN
            #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float4 _DiffuseRemapScale0;
			TEXTURE2D(_Splat0);
			SAMPLER(sampler_Splat0);
			float4 _DiffuseRemapScale1;
			TEXTURE2D(_Splat1);
			float4 _DiffuseRemapScale2;
			TEXTURE2D(_Splat2);
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_Splat3);
			TEXTURE2D(_Control1);
			SAMPLER(sampler_Control1);
			float4 _DiffuseRemapScale4;
			TEXTURE2D(_Splat4);
			SAMPLER(sampler_Splat4);
			float4 _DiffuseRemapScale5;
			TEXTURE2D(_Splat5);
			float4 _DiffuseRemapScale6;
			TEXTURE2D(_Splat6);
			float4 _DiffuseRemapScale7;
			TEXTURE2D(_Splat7);
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_ColorMapProjection);
			SAMPLER(sampler_ColorMapProjection);
			TEXTURE2D(_HeatMap);
			SAMPLER(sampler_HeatMap);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			float4x4 unity_CameraProjection;
			float4x4 unity_CameraInvProjection;
			float4x4 unity_WorldToCamera;
			float4x4 unity_CameraToWorld;
			TEXTURE2D(_Normal0);
			SAMPLER(sampler_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_Normal4);
			SAMPLER(sampler_Normal4);
			TEXTURE2D(_Normal5);
			TEXTURE2D(_Normal6);
			TEXTURE2D(_Normal7);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Mask1);
			TEXTURE2D(_Mask2);
			TEXTURE2D(_Mask3);
			TEXTURE2D(_Mask4);
			SAMPLER(sampler_Mask4);
			TEXTURE2D(_Mask5);
			TEXTURE2D(_Mask6);
			TEXTURE2D(_Mask7);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#if SHADERPASS == SHADERPASS_LIGHT_TRANSPORT
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/MetaPass.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

        	#ifdef HAVE_VFX_MODIFICATION
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
        	#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				#ifdef EDITOR_VISUALIZATION
				float2 VizUV : TEXCOORD0;
				float4 LightCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 PerturbNormal107_g58( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			float3 PerturbNormal107_g57( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.uv0.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;

				surfaceData.baseColor =					surfaceDescription.BaseColor;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness = 				surfaceDescription.Thickness;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.transmissionMask =			surfaceDescription.TransmissionMask;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
                        surfaceData.ior =                       surfaceDescription.RefractionIndex;
                        surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                        surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

                #ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

                #ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
                #endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;
        
                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
                    GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
				#endif

				#if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
				    ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData); 
				#endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				float2 break291_g55 = _Control_ST.zw;
				float2 appendResult293_g55 = (float2(( break291_g55.x + 0.001 ) , ( break291_g55.y + 0.0001 )));
				float2 vertexToFrag286_g55 = ( ( inputMesh.uv0.xy * _Control_ST.xy ) + appendResult293_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord2.xy = vertexToFrag286_g55;
				float2 break951_g55 = _Control1_ST.zw;
				float2 appendResult956_g55 = (float2(( break951_g55.x + 0.001 ) , ( break951_g55.y + 0.0001 )));
				float2 vertexToFrag958_g55 = ( ( inputMesh.uv0.xy * _Control1_ST.xy ) + appendResult956_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = vertexToFrag958_g55;
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				outputPackedVaryingsMeshToPS.ase_texcoord4.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				outputPackedVaryingsMeshToPS.ase_texcoord5.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				outputPackedVaryingsMeshToPS.ase_texcoord6.xyz = ase_worldTangent;
				float ase_vertexTangentSign = inputMesh.tangentOS.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord7.xyz = ase_worldBitangent;
				
				outputPackedVaryingsMeshToPS.ase_texcoord2.zw = inputMesh.uv0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord4.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord5.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord6.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord7.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;

				outputPackedVaryingsMeshToPS.positionCS = UnityMetaVertexPosition(inputMesh.positionOS, inputMesh.uv1.xy, inputMesh.uv2.xy, unity_LightmapST, unity_DynamicLightmapST);

				#ifdef EDITOR_VISUALIZATION
					float2 vizUV = 0;
					float4 lightCoord = 0;
					UnityEditorVizData(inputMesh.positionOS.xyz, inputMesh.uv0.xy, inputMesh.uv1.xy, inputMesh.uv2.xy, vizUV, lightCoord);

					outputPackedVaryingsMeshToPS.VizUV.xy = vizUV;
					outputPackedVaryingsMeshToPS.LightCoord = lightCoord;
				#endif

				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 uv3 : TEXCOORD3;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.uv3 = v.uv3;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv0 = patch[0].uv0 * bary.x + patch[1].uv0 * bary.y + patch[2].uv0 * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.uv3 = patch[0].uv3 * bary.x + patch[1].uv3 * bary.y + patch[2].uv3 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			float4 Frag(PackedVaryingsMeshToPS packedInput  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 vertexToFrag286_g55 = packedInput.ase_texcoord2.xy;
				float4 Control26_g55 = SAMPLE_TEXTURE2D( _Control, sampler_Control, vertexToFrag286_g55 );
				float2 uv_Splat0 = packedInput.ase_texcoord2.zw * _Splat0_ST.xy + _Splat0_ST.zw;
				float4 tex2DNode414_g55 = SAMPLE_TEXTURE2D( _Splat0, sampler_Splat0, uv_Splat0 );
				float3 Splat0342_g55 = (tex2DNode414_g55).rgb;
				float2 uv_Splat1 = packedInput.ase_texcoord2.zw * _Splat1_ST.xy + _Splat1_ST.zw;
				float4 tex2DNode420_g55 = SAMPLE_TEXTURE2D( _Splat1, sampler_Splat0, uv_Splat1 );
				float3 Splat1379_g55 = (tex2DNode420_g55).rgb;
				float2 uv_Splat2 = packedInput.ase_texcoord2.zw * _Splat2_ST.xy + _Splat2_ST.zw;
				float4 tex2DNode417_g55 = SAMPLE_TEXTURE2D( _Splat2, sampler_Splat0, uv_Splat2 );
				float3 Splat2357_g55 = (tex2DNode417_g55).rgb;
				float2 uv_Splat3 = packedInput.ase_texcoord2.zw * _Splat3_ST.xy + _Splat3_ST.zw;
				float4 tex2DNode423_g55 = SAMPLE_TEXTURE2D( _Splat3, sampler_Splat0, uv_Splat3 );
				float3 Splat3390_g55 = (tex2DNode423_g55).rgb;
				float4 weightedBlendVar9_g55 = Control26_g55;
				float3 weightedBlend9_g55 = ( weightedBlendVar9_g55.x*( (_DiffuseRemapScale0).rgb * Splat0342_g55 ) + weightedBlendVar9_g55.y*( (_DiffuseRemapScale1).rgb * Splat1379_g55 ) + weightedBlendVar9_g55.z*( (_DiffuseRemapScale2).rgb * Splat2357_g55 ) + weightedBlendVar9_g55.w*( (_DiffuseRemapScale3).rgb * Splat3390_g55 ) );
				float2 vertexToFrag958_g55 = packedInput.ase_texcoord3.xy;
				float4 Control1922_g55 = SAMPLE_TEXTURE2D( _Control1, sampler_Control1, vertexToFrag958_g55 );
				float2 uv_Splat4 = packedInput.ase_texcoord2.zw * _Splat4_ST.xy + _Splat4_ST.zw;
				float4 tex2DNode696_g55 = SAMPLE_TEXTURE2D( _Splat4, sampler_Splat4, uv_Splat4 );
				float3 Splat4752_g55 = (tex2DNode696_g55).rgb;
				float2 uv_Splat5 = packedInput.ase_texcoord2.zw * _Splat5_ST.xy + _Splat5_ST.zw;
				float4 tex2DNode702_g55 = SAMPLE_TEXTURE2D( _Splat5, sampler_Splat0, uv_Splat5 );
				float3 Splat5743_g55 = (tex2DNode702_g55).rgb;
				float2 uv_Splat6 = packedInput.ase_texcoord2.zw * _Splat6_ST.xy + _Splat6_ST.zw;
				float4 tex2DNode709_g55 = SAMPLE_TEXTURE2D( _Splat6, sampler_Splat0, uv_Splat6 );
				float3 Splat6759_g55 = (tex2DNode709_g55).rgb;
				float2 uv_Splat7 = packedInput.ase_texcoord2.zw * _Splat7_ST.xy + _Splat7_ST.zw;
				float4 tex2DNode719_g55 = SAMPLE_TEXTURE2D( _Splat7, sampler_Splat0, uv_Splat7 );
				float3 Splat7762_g55 = (tex2DNode719_g55).rgb;
				float4 weightedBlendVar912_g55 = Control1922_g55;
				float3 weightedBlend912_g55 = ( weightedBlendVar912_g55.x*( (_DiffuseRemapScale4).rgb * Splat4752_g55 ) + weightedBlendVar912_g55.y*( (_DiffuseRemapScale5).rgb * Splat5743_g55 ) + weightedBlendVar912_g55.z*( (_DiffuseRemapScale6).rgb * Splat6759_g55 ) + weightedBlendVar912_g55.w*( (_DiffuseRemapScale7).rgb * Splat7762_g55 ) );
				float3 temp_output_968_0_g55 = ( weightedBlend9_g55 + weightedBlend912_g55 );
				float3 localClipHoles453_g55 = ( temp_output_968_0_g55 );
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord2.zw * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float Hole453_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole453_g55 == 0.0f ? -1 : 1);
				#endif
				}
				float3 BaseColorOut1373 = localClipHoles453_g55;
				float2 uv_ColorMapProjection1434 = packedInput.ase_texcoord2.zw;
				float4 ColorMap_Top1443 = ( unity_ColorSpaceDouble * SAMPLE_TEXTURE2D( _ColorMapProjection, sampler_ColorMapProjection, uv_ColorMapProjection1434 ) );
				float4 lerpResult1381 = lerp( float4( BaseColorOut1373 , 0.0 ) , ( ColorMap_Top1443 * float4( BaseColorOut1373 , 0.0 ) ) , _ColorMapIntensity);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				float2 appendResult1413 = (float2(ase_parentObjectScale.x , ase_parentObjectScale.z));
				float3 ase_worldPos = packedInput.ase_texcoord4.xyz;
				float2 appendResult1414 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 temp_output_1418_0 = ( appendResult1413 * appendResult1414 );
				float simpleNoise1424 = SimpleNoise( temp_output_1418_0*400.0 );
				float temp_output_1430_0 = (simpleNoise1424*0.6 + 0.6);
				float4 temp_cast_3 = (temp_output_1430_0).xxxx;
				float2 uv_HeatMap1415 = packedInput.ase_texcoord2.zw;
				float temp_output_1420_0 = ( 1.0 - SAMPLE_TEXTURE2D( _HeatMap, sampler_HeatMap, uv_HeatMap1415 ).b );
				float2 uv_NormalMap1416 = packedInput.ase_texcoord2.zw;
				float3 unpack1416 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, uv_NormalMap1416 ), _NormalIntensity );
				unpack1416.z = lerp( 1, unpack1416.z, saturate(_NormalIntensity) );
				float3 tex2DNode1416 = unpack1416;
				float3 ase_worldNormal = packedInput.ase_texcoord5.xyz;
				float temp_output_1427_0 = saturate( ase_worldNormal.y );
				float3 _Tangent = float3(0,1,0);
				float3 ase_worldTangent = packedInput.ase_texcoord6.xyz;
				float3 ase_worldBitangent = packedInput.ase_texcoord7.xyz;
				float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
				float3 tangentToWorldDir1403 = normalize( mul( ase_tangentToWorldFast, _Tangent ) );
				float dotResult1404 = dot( tangentToWorldDir1403 , _Tangent );
				float lerpResult1408 = lerp( _SlopeMax , _SlopeMin , abs( dotResult1404 ));
				float Slope1417 = saturate( lerpResult1408 );
				float temp_output_1446_0 = saturate( ( (( _NormalMapBasedSnow )?( ( (temp_output_1420_0*-4.39 + 1.1) * (tex2DNode1416.g*_SnowScale + _SnowOffset) ) ):( (temp_output_1420_0*_SnowScale + _SnowOffset) )) * (( _SnowSlope )?( ( temp_output_1427_0 * Slope1417 ) ):( temp_output_1427_0 )) * _SnowAmount ) );
				float4 lerpResult1389 = lerp( (( _ColorMap )?( lerpResult1381 ):( float4( BaseColorOut1373 , 0.0 ) )) , temp_cast_3 , temp_output_1446_0);
				float4 AlbedoSnow_Output1392 = lerpResult1389;
				
				float4 Normal0341_g55 = SAMPLE_TEXTURE2D( _Normal0, sampler_Normal0, uv_Splat0 );
				float3 unpack490_g55 = UnpackNormalScale( Normal0341_g55, _NormalScale0 );
				unpack490_g55.z = lerp( 1, unpack490_g55.z, saturate(_NormalScale0) );
				float4 Normal1378_g55 = SAMPLE_TEXTURE2D( _Normal1, sampler_Normal0, uv_Splat1 );
				float3 unpack496_g55 = UnpackNormalScale( Normal1378_g55, _NormalScale1 );
				unpack496_g55.z = lerp( 1, unpack496_g55.z, saturate(_NormalScale1) );
				float4 Normal2356_g55 = SAMPLE_TEXTURE2D( _Normal2, sampler_Normal0, uv_Splat2 );
				float3 unpack494_g55 = UnpackNormalScale( Normal2356_g55, _NormalScale2 );
				unpack494_g55.z = lerp( 1, unpack494_g55.z, saturate(_NormalScale2) );
				float4 Normal3398_g55 = SAMPLE_TEXTURE2D( _Normal3, sampler_Normal0, uv_Splat3 );
				float3 unpack491_g55 = UnpackNormalScale( Normal3398_g55, _NormalScale3 );
				unpack491_g55.z = lerp( 1, unpack491_g55.z, saturate(_NormalScale3) );
				float4 weightedBlendVar473_g55 = Control26_g55;
				float3 weightedBlend473_g55 = ( weightedBlendVar473_g55.x*unpack490_g55 + weightedBlendVar473_g55.y*unpack496_g55 + weightedBlendVar473_g55.z*unpack494_g55 + weightedBlendVar473_g55.w*unpack491_g55 );
				float4 Normal4746_g55 = SAMPLE_TEXTURE2D( _Normal4, sampler_Normal4, uv_Splat4 );
				float3 unpack856_g55 = UnpackNormalScale( Normal4746_g55, _NormalScale4 );
				unpack856_g55.z = lerp( 1, unpack856_g55.z, saturate(_NormalScale4) );
				float4 Normal5740_g55 = SAMPLE_TEXTURE2D( _Normal5, sampler_Normal0, uv_Splat5 );
				float3 unpack857_g55 = UnpackNormalScale( Normal5740_g55, _NormalScale5 );
				unpack857_g55.z = lerp( 1, unpack857_g55.z, saturate(_NormalScale5) );
				float4 Normal6754_g55 = SAMPLE_TEXTURE2D( _Normal6, sampler_Normal0, uv_Splat6 );
				float3 unpack858_g55 = UnpackNormalScale( Normal6754_g55, _NormalScale6 );
				unpack858_g55.z = lerp( 1, unpack858_g55.z, saturate(_NormalScale6) );
				float4 Normal7764_g55 = SAMPLE_TEXTURE2D( _Normal7, sampler_Normal0, uv_Splat7 );
				float3 unpack854_g55 = UnpackNormalScale( Normal7764_g55, _NormalScale7 );
				unpack854_g55.z = lerp( 1, unpack854_g55.z, saturate(_NormalScale7) );
				float4 weightedBlendVar860_g55 = Control1922_g55;
				float3 weightedBlend860_g55 = ( weightedBlendVar860_g55.x*unpack856_g55 + weightedBlendVar860_g55.y*unpack857_g55 + weightedBlendVar860_g55.z*unpack858_g55 + weightedBlendVar860_g55.w*unpack854_g55 );
				float3 break513_g55 = ( weightedBlend473_g55 + weightedBlend860_g55 );
				float3 appendResult514_g55 = (float3(break513_g55.x , break513_g55.y , ( break513_g55.z + 0.001 )));
				#ifdef _TERRAIN_INSTANCED_PERPIXEL_NORMAL
				float3 staticSwitch503_g55 = appendResult514_g55;
				#else
				float3 staticSwitch503_g55 = appendResult514_g55;
				#endif
				float3 NormalOut1374 = staticSwitch503_g55;
				float3 surf_pos107_g58 = ase_worldPos;
				float3 surf_norm107_g58 = ase_worldNormal;
				float simpleNoise1429 = SimpleNoise( temp_output_1418_0*2.0 );
				float height107_g58 = simpleNoise1429;
				float scale107_g58 = 8.0;
				float3 localPerturbNormal107_g58 = PerturbNormal107_g58( surf_pos107_g58 , surf_norm107_g58 , height107_g58 , scale107_g58 );
				float3x3 ase_worldToTangent = float3x3(ase_worldTangent,ase_worldBitangent,ase_worldNormal);
				float3 worldToTangentDir42_g58 = mul( ase_worldToTangent, localPerturbNormal107_g58);
				float3 surf_pos107_g57 = ase_worldPos;
				float3 surf_norm107_g57 = ase_worldNormal;
				float height107_g57 = temp_output_1430_0;
				float scale107_g57 = 0.06;
				float3 localPerturbNormal107_g57 = PerturbNormal107_g57( surf_pos107_g57 , surf_norm107_g57 , height107_g57 , scale107_g57 );
				float3 worldToTangentDir42_g57 = mul( ase_worldToTangent, localPerturbNormal107_g57);
				float2 temp_output_1_0_g59 = BlendNormal( worldToTangentDir42_g58 , worldToTangentDir42_g57 ).xy;
				float dotResult4_g59 = dot( temp_output_1_0_g59 , temp_output_1_0_g59 );
				float3 appendResult10_g59 = (float3((temp_output_1_0_g59).x , (temp_output_1_0_g59).y , sqrt( ( 1.0 - saturate( dotResult4_g59 ) ) )));
				float3 normalizeResult12_g59 = normalize( appendResult10_g59 );
				float3 lerpResult1378 = lerp( NormalOut1374 , normalizeResult12_g59 , temp_output_1446_0);
				float3 NormalMap_Top1450 = tex2DNode1416;
				float3 NormalSnow_Output1393 = (( _TerrainNormalMap )?( BlendNormal( NormalMap_Top1450 , lerpResult1378 ) ):( lerpResult1378 ));
				
				float4 tex2DNode416_g55 = SAMPLE_TEXTURE2D( _Mask0, sampler_Mask0, uv_Splat0 );
				float Mask0A335_g55 = tex2DNode416_g55.a;
				float4 tex2DNode422_g55 = SAMPLE_TEXTURE2D( _Mask1, sampler_Mask0, uv_Splat1 );
				float Mask1A369_g55 = tex2DNode422_g55.a;
				float4 tex2DNode419_g55 = SAMPLE_TEXTURE2D( _Mask2, sampler_Mask0, uv_Splat2 );
				float Mask2A360_g55 = tex2DNode419_g55.a;
				float4 tex2DNode425_g55 = SAMPLE_TEXTURE2D( _Mask3, sampler_Mask0, uv_Splat3 );
				float Mask3A391_g55 = tex2DNode425_g55.a;
				float4 weightedBlendVar547_g55 = Control26_g55;
				float weightedBlend547_g55 = ( weightedBlendVar547_g55.x*( _Smoothness0 * Mask0A335_g55 ) + weightedBlendVar547_g55.y*( _Smoothness1 * Mask1A369_g55 ) + weightedBlendVar547_g55.z*( _Smoothness2 * Mask2A360_g55 ) + weightedBlendVar547_g55.w*( _Smoothness3 * Mask3A391_g55 ) );
				float4 tex2DNode697_g55 = SAMPLE_TEXTURE2D( _Mask4, sampler_Mask4, uv_Splat4 );
				float Mask4A750_g55 = tex2DNode697_g55.a;
				float4 tex2DNode704_g55 = SAMPLE_TEXTURE2D( _Mask5, sampler_Mask0, uv_Splat5 );
				float Mask5A745_g55 = tex2DNode704_g55.a;
				float4 tex2DNode711_g55 = SAMPLE_TEXTURE2D( _Mask6, sampler_Mask0, uv_Splat6 );
				float Mask6A758_g55 = tex2DNode711_g55.a;
				float4 tex2DNode717_g55 = SAMPLE_TEXTURE2D( _Mask7, sampler_Mask0, uv_Splat7 );
				float Mask7A768_g55 = tex2DNode717_g55.a;
				float4 weightedBlendVar826_g55 = Control1922_g55;
				float weightedBlend826_g55 = ( weightedBlendVar826_g55.x*( _Smoothness4 * Mask4A750_g55 ) + weightedBlendVar826_g55.y*( _Smoothness5 * Mask5A745_g55 ) + weightedBlendVar826_g55.z*( _Smoothness6 * Mask6A758_g55 ) + weightedBlendVar826_g55.w*( _Smoothness7 * Mask7A768_g55 ) );
				float SmoothOut1380 = ( _SmoothnessIntensity * ( weightedBlend547_g55 + weightedBlend826_g55 ) );
				float lerpResult1391 = lerp( SmoothOut1380 , (simpleNoise1424*1.4 + 0.0) , temp_output_1446_0);
				float Smoothness_Output1394 = lerpResult1391;
				
				float Mask0G409_g55 = tex2DNode416_g55.g;
				float Mask1G371_g55 = tex2DNode422_g55.g;
				float Mask2G358_g55 = tex2DNode419_g55.g;
				float Mask3G389_g55 = tex2DNode425_g55.g;
				float4 weightedBlendVar602_g55 = Control26_g55;
				float weightedBlend602_g55 = ( weightedBlendVar602_g55.x*saturate( Mask0G409_g55 ) + weightedBlendVar602_g55.y*saturate( Mask1G371_g55 ) + weightedBlendVar602_g55.z*saturate( Mask2G358_g55 ) + weightedBlendVar602_g55.w*saturate( Mask3G389_g55 ) );
				float Mask4G748_g55 = tex2DNode697_g55.g;
				float Mask5G742_g55 = tex2DNode704_g55.g;
				float Mask6G756_g55 = tex2DNode711_g55.g;
				float Mask7G766_g55 = tex2DNode717_g55.g;
				float4 weightedBlendVar799_g55 = Control1922_g55;
				float weightedBlend799_g55 = ( weightedBlendVar799_g55.x*saturate( Mask4G748_g55 ) + weightedBlendVar799_g55.y*saturate( Mask5G742_g55 ) + weightedBlendVar799_g55.z*saturate( Mask6G756_g55 ) + weightedBlendVar799_g55.w*saturate( Mask7G766_g55 ) );
				float AoOut1457 = saturate( ( weightedBlend602_g55 + weightedBlend799_g55 ) );
				float lerpResult1460 = lerp( AoOut1457 , 1.0 , temp_output_1446_0);
				float Ao_Output1466 = lerpResult1460;
				
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.BaseColor = AlbedoSnow_Output1392.rgb;
				surfaceDescription.Normal = NormalSnow_Output1393;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = 0;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = Smoothness_Output1394;
				surfaceDescription.Occlusion = Ao_Output1466;
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.TransmissionMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = _DiffusionProfile;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);
				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
				LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

				float4 res = float4( 0.0, 0.0, 0.0, 1.0 );
				UnityMetaInput metaInput;
				metaInput.Albedo = lightTransportData.diffuseColor.rgb;
				metaInput.Emission = lightTransportData.emissiveColor;

			#ifdef EDITOR_VISUALIZATION
				metaInput.VizUV = packedInput.VizUV;
				metaInput.LightCoord = packedInput.LightCoord;
			#endif
				res = UnityMetaFragment(metaInput);

				return res;
			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			Cull [_CullMode]
			ZWrite On
			ZClip [_ZClip]
			ZTest LEqual
			ColorMask 0

			HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

			#pragma multi_compile_fragment _ SHADOWS_SHADOWMASK

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_SHADOWS

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

        	#ifdef HAVE_VFX_MODIFICATION
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
        	#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;

				// refraction ShadowCaster
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                    #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
				float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;
        
                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

                #if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
                    ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
                #endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				outputPackedVaryingsMeshToPS.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
							#ifdef WRITE_MSAA_DEPTH
							, out float4 depthColor : SV_Target0
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target1
								#endif
							#else
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target0
								#endif
							#endif

							#if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
			UNITY_SETUP_INSTANCE_ID(packedInput);

				float3 positionRWS = packedInput.positionRWS.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord1.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
				surfaceDescription.AlphaClipThresholdShadow = 0.5;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				float bias = max(abs(ddx(posInput.deviceDepth)), abs(ddy(posInput.deviceDepth))) * _SlopeScaleDepthBias;
				outputDepth += bias;
				#endif

				#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.vmesh.positionCS.z;
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif

                #if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
				    DecalPrepassData decalPrepassData;
                    #ifdef _DISABLE_DECALS
				    ZERO_INITIALIZE(DecalPrepassData, decalPrepassData);
                    #else
				    decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
                    #endif
				    decalPrepassData.renderingLayerMask = GetMeshRenderingLayerMask();
				    EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
				#endif
			}
			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off

			HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma editor_sync_compilation
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
		    #define SCENESELECTIONPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif

            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif

            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

        	#ifdef HAVE_VFX_MODIFICATION
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
        	#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SceneSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;

				//refraction SceneSelectionPass
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
                        surfaceData.ior =                       surfaceDescription.RefractionIndex;
                        surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                        surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
                    float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;
        
                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

                #if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
                     ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
                #endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(SceneSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				outputPackedVaryingsMeshToPS.ase_texcoord1.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord1.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						, out float4 outColor : SV_Target0
						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS = packedInput.positionRWS.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SceneSurfaceDescription surfaceDescription = (SceneSurfaceDescription)0;
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord1.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

				outColor = float4( _ObjectId, _PassValue, 1.0, 1.0 );
			}
			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			Cull [_CullMode]
			ZWrite On

			Stencil
			{
				Ref [_StencilRefDepth]
				WriteMask [_StencilWriteMaskDepth]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

            #pragma multi_compile _ WRITE_NORMAL_BUFFER
            #pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
            #pragma multi_compile_fragment _ WRITE_DECAL_BUFFER WRITE_RENDERING_LAYER

			#pragma vertex Vert
			#pragma fragment Frag

            #define SHADERPASS SHADERPASS_DEPTH_ONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif

            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			TEXTURE2D(_Normal0);
			TEXTURE2D(_Splat0);
			SAMPLER(sampler_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_Splat3);
			TEXTURE2D(_Control1);
			SAMPLER(sampler_Control1);
			TEXTURE2D(_Normal4);
			TEXTURE2D(_Splat4);
			SAMPLER(sampler_Normal4);
			TEXTURE2D(_Normal5);
			TEXTURE2D(_Splat5);
			TEXTURE2D(_Normal6);
			TEXTURE2D(_Splat6);
			TEXTURE2D(_Normal7);
			TEXTURE2D(_Splat7);
			float4x4 unity_CameraProjection;
			float4x4 unity_CameraInvProjection;
			float4x4 unity_WorldToCamera;
			float4x4 unity_CameraToWorld;
			TEXTURE2D(_HeatMap);
			SAMPLER(sampler_HeatMap);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Mask1);
			TEXTURE2D(_Mask2);
			TEXTURE2D(_Mask3);
			TEXTURE2D(_Mask4);
			SAMPLER(sampler_Mask4);
			TEXTURE2D(_Mask5);
			TEXTURE2D(_Mask6);
			TEXTURE2D(_Mask7);
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

        	#ifdef HAVE_VFX_MODIFICATION
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
        	#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_RELATIVE_WORLD_POS
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 PerturbNormal107_g58( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			float3 PerturbNormal107_g57( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;

				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
                        surfaceData.ior =                       surfaceDescription.RefractionIndex;
                        surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                        surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
				    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
				    float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

                #if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
                     ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData);
                #endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			#if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalPrepassBuffer.hlsl"
			#endif

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				float2 break291_g55 = _Control_ST.zw;
				float2 appendResult293_g55 = (float2(( break291_g55.x + 0.001 ) , ( break291_g55.y + 0.0001 )));
				float2 vertexToFrag286_g55 = ( ( inputMesh.ase_texcoord.xy * _Control_ST.xy ) + appendResult293_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = vertexToFrag286_g55;
				float2 break951_g55 = _Control1_ST.zw;
				float2 appendResult956_g55 = (float2(( break951_g55.x + 0.001 ) , ( break951_g55.y + 0.0001 )));
				float2 vertexToFrag958_g55 = ( ( inputMesh.ase_texcoord.xy * _Control1_ST.xy ) + appendResult956_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord4.xy = vertexToFrag958_g55;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				float ase_vertexTangentSign = inputMesh.tangentOS.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord5.xyz = ase_worldBitangent;
				
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord4.zw = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord5.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
							#ifdef WRITE_MSAA_DEPTH
							, out float4 depthColor : SV_Target0
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target1
								#endif
							#else
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target0
								#endif
							#endif

							#if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
			UNITY_SETUP_INSTANCE_ID(packedInput);

				float3 positionRWS = packedInput.positionRWS.xyz;
				float3 normalWS = packedInput.normalWS.xyz;
				float4 tangentWS = packedInput.tangentWS.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 vertexToFrag286_g55 = packedInput.ase_texcoord3.xy;
				float4 Control26_g55 = SAMPLE_TEXTURE2D( _Control, sampler_Control, vertexToFrag286_g55 );
				float2 uv_Splat0 = packedInput.ase_texcoord3.zw * _Splat0_ST.xy + _Splat0_ST.zw;
				float4 Normal0341_g55 = SAMPLE_TEXTURE2D( _Normal0, sampler_Normal0, uv_Splat0 );
				float3 unpack490_g55 = UnpackNormalScale( Normal0341_g55, _NormalScale0 );
				unpack490_g55.z = lerp( 1, unpack490_g55.z, saturate(_NormalScale0) );
				float2 uv_Splat1 = packedInput.ase_texcoord3.zw * _Splat1_ST.xy + _Splat1_ST.zw;
				float4 Normal1378_g55 = SAMPLE_TEXTURE2D( _Normal1, sampler_Normal0, uv_Splat1 );
				float3 unpack496_g55 = UnpackNormalScale( Normal1378_g55, _NormalScale1 );
				unpack496_g55.z = lerp( 1, unpack496_g55.z, saturate(_NormalScale1) );
				float2 uv_Splat2 = packedInput.ase_texcoord3.zw * _Splat2_ST.xy + _Splat2_ST.zw;
				float4 Normal2356_g55 = SAMPLE_TEXTURE2D( _Normal2, sampler_Normal0, uv_Splat2 );
				float3 unpack494_g55 = UnpackNormalScale( Normal2356_g55, _NormalScale2 );
				unpack494_g55.z = lerp( 1, unpack494_g55.z, saturate(_NormalScale2) );
				float2 uv_Splat3 = packedInput.ase_texcoord3.zw * _Splat3_ST.xy + _Splat3_ST.zw;
				float4 Normal3398_g55 = SAMPLE_TEXTURE2D( _Normal3, sampler_Normal0, uv_Splat3 );
				float3 unpack491_g55 = UnpackNormalScale( Normal3398_g55, _NormalScale3 );
				unpack491_g55.z = lerp( 1, unpack491_g55.z, saturate(_NormalScale3) );
				float4 weightedBlendVar473_g55 = Control26_g55;
				float3 weightedBlend473_g55 = ( weightedBlendVar473_g55.x*unpack490_g55 + weightedBlendVar473_g55.y*unpack496_g55 + weightedBlendVar473_g55.z*unpack494_g55 + weightedBlendVar473_g55.w*unpack491_g55 );
				float2 vertexToFrag958_g55 = packedInput.ase_texcoord4.xy;
				float4 Control1922_g55 = SAMPLE_TEXTURE2D( _Control1, sampler_Control1, vertexToFrag958_g55 );
				float2 uv_Splat4 = packedInput.ase_texcoord3.zw * _Splat4_ST.xy + _Splat4_ST.zw;
				float4 Normal4746_g55 = SAMPLE_TEXTURE2D( _Normal4, sampler_Normal4, uv_Splat4 );
				float3 unpack856_g55 = UnpackNormalScale( Normal4746_g55, _NormalScale4 );
				unpack856_g55.z = lerp( 1, unpack856_g55.z, saturate(_NormalScale4) );
				float2 uv_Splat5 = packedInput.ase_texcoord3.zw * _Splat5_ST.xy + _Splat5_ST.zw;
				float4 Normal5740_g55 = SAMPLE_TEXTURE2D( _Normal5, sampler_Normal0, uv_Splat5 );
				float3 unpack857_g55 = UnpackNormalScale( Normal5740_g55, _NormalScale5 );
				unpack857_g55.z = lerp( 1, unpack857_g55.z, saturate(_NormalScale5) );
				float2 uv_Splat6 = packedInput.ase_texcoord3.zw * _Splat6_ST.xy + _Splat6_ST.zw;
				float4 Normal6754_g55 = SAMPLE_TEXTURE2D( _Normal6, sampler_Normal0, uv_Splat6 );
				float3 unpack858_g55 = UnpackNormalScale( Normal6754_g55, _NormalScale6 );
				unpack858_g55.z = lerp( 1, unpack858_g55.z, saturate(_NormalScale6) );
				float2 uv_Splat7 = packedInput.ase_texcoord3.zw * _Splat7_ST.xy + _Splat7_ST.zw;
				float4 Normal7764_g55 = SAMPLE_TEXTURE2D( _Normal7, sampler_Normal0, uv_Splat7 );
				float3 unpack854_g55 = UnpackNormalScale( Normal7764_g55, _NormalScale7 );
				unpack854_g55.z = lerp( 1, unpack854_g55.z, saturate(_NormalScale7) );
				float4 weightedBlendVar860_g55 = Control1922_g55;
				float3 weightedBlend860_g55 = ( weightedBlendVar860_g55.x*unpack856_g55 + weightedBlendVar860_g55.y*unpack857_g55 + weightedBlendVar860_g55.z*unpack858_g55 + weightedBlendVar860_g55.w*unpack854_g55 );
				float3 break513_g55 = ( weightedBlend473_g55 + weightedBlend860_g55 );
				float3 appendResult514_g55 = (float3(break513_g55.x , break513_g55.y , ( break513_g55.z + 0.001 )));
				#ifdef _TERRAIN_INSTANCED_PERPIXEL_NORMAL
				float3 staticSwitch503_g55 = appendResult514_g55;
				#else
				float3 staticSwitch503_g55 = appendResult514_g55;
				#endif
				float3 NormalOut1374 = staticSwitch503_g55;
				float3 ase_worldPos = GetAbsolutePositionWS( positionRWS );
				float3 surf_pos107_g58 = ase_worldPos;
				float3 surf_norm107_g58 = normalWS;
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				float2 appendResult1413 = (float2(ase_parentObjectScale.x , ase_parentObjectScale.z));
				float2 appendResult1414 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 temp_output_1418_0 = ( appendResult1413 * appendResult1414 );
				float simpleNoise1429 = SimpleNoise( temp_output_1418_0*2.0 );
				float height107_g58 = simpleNoise1429;
				float scale107_g58 = 8.0;
				float3 localPerturbNormal107_g58 = PerturbNormal107_g58( surf_pos107_g58 , surf_norm107_g58 , height107_g58 , scale107_g58 );
				float3 ase_worldBitangent = packedInput.ase_texcoord5.xyz;
				float3x3 ase_worldToTangent = float3x3(tangentWS.xyz,ase_worldBitangent,normalWS);
				float3 worldToTangentDir42_g58 = mul( ase_worldToTangent, localPerturbNormal107_g58);
				float3 surf_pos107_g57 = ase_worldPos;
				float3 surf_norm107_g57 = normalWS;
				float simpleNoise1424 = SimpleNoise( temp_output_1418_0*400.0 );
				float temp_output_1430_0 = (simpleNoise1424*0.6 + 0.6);
				float height107_g57 = temp_output_1430_0;
				float scale107_g57 = 0.06;
				float3 localPerturbNormal107_g57 = PerturbNormal107_g57( surf_pos107_g57 , surf_norm107_g57 , height107_g57 , scale107_g57 );
				float3 worldToTangentDir42_g57 = mul( ase_worldToTangent, localPerturbNormal107_g57);
				float2 temp_output_1_0_g59 = BlendNormal( worldToTangentDir42_g58 , worldToTangentDir42_g57 ).xy;
				float dotResult4_g59 = dot( temp_output_1_0_g59 , temp_output_1_0_g59 );
				float3 appendResult10_g59 = (float3((temp_output_1_0_g59).x , (temp_output_1_0_g59).y , sqrt( ( 1.0 - saturate( dotResult4_g59 ) ) )));
				float3 normalizeResult12_g59 = normalize( appendResult10_g59 );
				float2 uv_HeatMap1415 = packedInput.ase_texcoord3.zw;
				float temp_output_1420_0 = ( 1.0 - SAMPLE_TEXTURE2D( _HeatMap, sampler_HeatMap, uv_HeatMap1415 ).b );
				float2 uv_NormalMap1416 = packedInput.ase_texcoord3.zw;
				float3 unpack1416 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, uv_NormalMap1416 ), _NormalIntensity );
				unpack1416.z = lerp( 1, unpack1416.z, saturate(_NormalIntensity) );
				float3 tex2DNode1416 = unpack1416;
				float temp_output_1427_0 = saturate( normalWS.y );
				float3 _Tangent = float3(0,1,0);
				float3x3 ase_tangentToWorldFast = float3x3(tangentWS.xyz.x,ase_worldBitangent.x,normalWS.x,tangentWS.xyz.y,ase_worldBitangent.y,normalWS.y,tangentWS.xyz.z,ase_worldBitangent.z,normalWS.z);
				float3 tangentToWorldDir1403 = normalize( mul( ase_tangentToWorldFast, _Tangent ) );
				float dotResult1404 = dot( tangentToWorldDir1403 , _Tangent );
				float lerpResult1408 = lerp( _SlopeMax , _SlopeMin , abs( dotResult1404 ));
				float Slope1417 = saturate( lerpResult1408 );
				float temp_output_1446_0 = saturate( ( (( _NormalMapBasedSnow )?( ( (temp_output_1420_0*-4.39 + 1.1) * (tex2DNode1416.g*_SnowScale + _SnowOffset) ) ):( (temp_output_1420_0*_SnowScale + _SnowOffset) )) * (( _SnowSlope )?( ( temp_output_1427_0 * Slope1417 ) ):( temp_output_1427_0 )) * _SnowAmount ) );
				float3 lerpResult1378 = lerp( NormalOut1374 , normalizeResult12_g59 , temp_output_1446_0);
				float3 NormalMap_Top1450 = tex2DNode1416;
				float3 NormalSnow_Output1393 = (( _TerrainNormalMap )?( BlendNormal( NormalMap_Top1450 , lerpResult1378 ) ):( lerpResult1378 ));
				
				float4 tex2DNode416_g55 = SAMPLE_TEXTURE2D( _Mask0, sampler_Mask0, uv_Splat0 );
				float Mask0A335_g55 = tex2DNode416_g55.a;
				float4 tex2DNode422_g55 = SAMPLE_TEXTURE2D( _Mask1, sampler_Mask0, uv_Splat1 );
				float Mask1A369_g55 = tex2DNode422_g55.a;
				float4 tex2DNode419_g55 = SAMPLE_TEXTURE2D( _Mask2, sampler_Mask0, uv_Splat2 );
				float Mask2A360_g55 = tex2DNode419_g55.a;
				float4 tex2DNode425_g55 = SAMPLE_TEXTURE2D( _Mask3, sampler_Mask0, uv_Splat3 );
				float Mask3A391_g55 = tex2DNode425_g55.a;
				float4 weightedBlendVar547_g55 = Control26_g55;
				float weightedBlend547_g55 = ( weightedBlendVar547_g55.x*( _Smoothness0 * Mask0A335_g55 ) + weightedBlendVar547_g55.y*( _Smoothness1 * Mask1A369_g55 ) + weightedBlendVar547_g55.z*( _Smoothness2 * Mask2A360_g55 ) + weightedBlendVar547_g55.w*( _Smoothness3 * Mask3A391_g55 ) );
				float4 tex2DNode697_g55 = SAMPLE_TEXTURE2D( _Mask4, sampler_Mask4, uv_Splat4 );
				float Mask4A750_g55 = tex2DNode697_g55.a;
				float4 tex2DNode704_g55 = SAMPLE_TEXTURE2D( _Mask5, sampler_Mask0, uv_Splat5 );
				float Mask5A745_g55 = tex2DNode704_g55.a;
				float4 tex2DNode711_g55 = SAMPLE_TEXTURE2D( _Mask6, sampler_Mask0, uv_Splat6 );
				float Mask6A758_g55 = tex2DNode711_g55.a;
				float4 tex2DNode717_g55 = SAMPLE_TEXTURE2D( _Mask7, sampler_Mask0, uv_Splat7 );
				float Mask7A768_g55 = tex2DNode717_g55.a;
				float4 weightedBlendVar826_g55 = Control1922_g55;
				float weightedBlend826_g55 = ( weightedBlendVar826_g55.x*( _Smoothness4 * Mask4A750_g55 ) + weightedBlendVar826_g55.y*( _Smoothness5 * Mask5A745_g55 ) + weightedBlendVar826_g55.z*( _Smoothness6 * Mask6A758_g55 ) + weightedBlendVar826_g55.w*( _Smoothness7 * Mask7A768_g55 ) );
				float SmoothOut1380 = ( _SmoothnessIntensity * ( weightedBlend547_g55 + weightedBlend826_g55 ) );
				float lerpResult1391 = lerp( SmoothOut1380 , (simpleNoise1424*1.4 + 0.0) , temp_output_1446_0);
				float Smoothness_Output1394 = lerpResult1391;
				
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord3.zw * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.Normal = NormalSnow_Output1393;
				surfaceDescription.Smoothness = Smoothness_Output1394;
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

                #if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
				outputDepth = posInput.deviceDepth;
				#endif

                #if SHADERPASS == SHADERPASS_SHADOWS
                float bias = max(abs(ddx(posInput.deviceDepth)), abs(ddy(posInput.deviceDepth))) * _SlopeScaleDepthBias;
                outputDepth += bias;
                #endif

				#ifdef SCENESELECTIONPASS
    				outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
				#elif defined(SCENEPICKINGPASS)
    				outColor = unity_SelectionID;
				#else
    				#ifdef WRITE_MSAA_DEPTH
    				depthColor = packedInput.positionCS.z;
    				depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
    				#endif

    				#if defined(WRITE_NORMAL_BUFFER)
    				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
    				#endif

    				#if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
    				DecalPrepassData decalPrepassData;
                    #ifdef _DISABLE_DECALS
				    ZERO_INITIALIZE(DecalPrepassData, decalPrepassData);
                    #else
    				decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
                    #endif
    				decalPrepassData.renderingLayerMask = GetMeshRenderingLayerMask();
    				EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
    				#endif

				#endif // SCENESELECTIONPASS

			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "MotionVectors"
			Tags { "LightMode"="MotionVectors" }

			Cull [_CullMode]

			ZWrite On

			Stencil
			{
				Ref [_StencilRefMV]
				WriteMask [_StencilWriteMaskMV]
				Comp Always
				Pass Replace
			}


			HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

            #pragma multi_compile _ WRITE_NORMAL_BUFFER
            #pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
            #pragma multi_compile_fragment _ WRITE_DECAL_BUFFER_AND_RENDERING_LAYER

			#ifdef WRITE_DECAL_BUFFER_AND_RENDERING_LAYER
			#define WRITE_DECAL_BUFFER
			#endif

			#pragma vertex Vert
			#pragma fragment Frag

            #define SHADERPASS SHADERPASS_MOTION_VECTORS

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
               #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			     #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			TEXTURE2D(_Normal0);
			TEXTURE2D(_Splat0);
			SAMPLER(sampler_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Splat1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Splat2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_Splat3);
			TEXTURE2D(_Control1);
			SAMPLER(sampler_Control1);
			TEXTURE2D(_Normal4);
			TEXTURE2D(_Splat4);
			SAMPLER(sampler_Normal4);
			TEXTURE2D(_Normal5);
			TEXTURE2D(_Splat5);
			TEXTURE2D(_Normal6);
			TEXTURE2D(_Splat6);
			TEXTURE2D(_Normal7);
			TEXTURE2D(_Splat7);
			float4x4 unity_CameraProjection;
			float4x4 unity_CameraInvProjection;
			float4x4 unity_WorldToCamera;
			float4x4 unity_CameraToWorld;
			TEXTURE2D(_HeatMap);
			SAMPLER(sampler_HeatMap);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Mask1);
			TEXTURE2D(_Mask2);
			TEXTURE2D(_Mask3);
			TEXTURE2D(_Mask4);
			SAMPLER(sampler_Mask4);
			TEXTURE2D(_Mask5);
			TEXTURE2D(_Mask6);
			TEXTURE2D(_Mask7);
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

        	#ifdef HAVE_VFX_MODIFICATION
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
        	#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 vmeshPositionCS : SV_Position;
				float3 vmeshInterp00 : TEXCOORD0;
				float3 vpassInterpolators0 : TEXCOORD1; //interpolators0
				float3 vpassInterpolators1 : TEXCOORD2; //interpolators1
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 PerturbNormal107_g58( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			float3 PerturbNormal107_g57( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);

				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;

				// refraction
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
                        surfaceData.ior =                       surfaceDescription.RefractionIndex;
                        surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                        surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
                    float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

				#if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
				    ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData); 
				#endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				float2 break291_g55 = _Control_ST.zw;
				float2 appendResult293_g55 = (float2(( break291_g55.x + 0.001 ) , ( break291_g55.y + 0.0001 )));
				float2 vertexToFrag286_g55 = ( ( inputMesh.ase_texcoord.xy * _Control_ST.xy ) + appendResult293_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord3.xy = vertexToFrag286_g55;
				float2 break951_g55 = _Control1_ST.zw;
				float2 appendResult956_g55 = (float2(( break951_g55.x + 0.001 ) , ( break951_g55.y + 0.0001 )));
				float2 vertexToFrag958_g55 = ( ( inputMesh.ase_texcoord.xy * _Control1_ST.xy ) + appendResult956_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord4.xy = vertexToFrag958_g55;
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				outputPackedVaryingsMeshToPS.ase_texcoord5.xyz = ase_worldPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				outputPackedVaryingsMeshToPS.ase_texcoord6.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				outputPackedVaryingsMeshToPS.ase_texcoord7.xyz = ase_worldTangent;
				float ase_vertexTangentSign = inputMesh.tangentOS.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord8.xyz = ase_worldBitangent;
				
				outputPackedVaryingsMeshToPS.ase_texcoord3.zw = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord4.zw = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord5.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord6.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord7.w = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord8.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =  defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);

				float3 VMESHpositionRWS = positionRWS;
				float4 VMESHpositionCS = TransformWorldToHClip(positionRWS);

				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(VMESHpositionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						if (_TransparentCameraOnlyMotionVectors > 0)
						{
							previousPositionRWS = VMESHpositionRWS.xyz;
						}
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}

				outputPackedVaryingsMeshToPS.vmeshPositionCS = VMESHpositionCS;
				outputPackedVaryingsMeshToPS.vmeshInterp00.xyz = VMESHpositionRWS;

				outputPackedVaryingsMeshToPS.vpassInterpolators0 = float3(VPASSpositionCS.xyw);
				outputPackedVaryingsMeshToPS.vpassInterpolators1 = float3(VPASSpreviousPositionCS.xyw);
				return outputPackedVaryingsMeshToPS;
			}

			#if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalPrepassBuffer.hlsl"
			#endif

			#if ( 0 ) // TEMPORARY: defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.previousPositionOS = v.previousPositionOS;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
				o.precomputedVelocity = v.precomputedVelocity;
				#endif
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.previousPositionOS = patch[0].previousPositionOS * bary.x + patch[1].previousPositionOS * bary.y + patch[2].previousPositionOS * bary.z;
				#if defined (_ADD_PRECOMPUTED_VELOCITY)
					o.precomputedVelocity = patch[0].precomputedVelocity * bary.x + patch[1].precomputedVelocity * bary.y + patch[2].precomputedVelocity * bary.z;
				#endif
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_DECAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_NORMAL SV_Target3
			#elif defined(WRITE_DECAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_NORMAL SV_Target2
			#else
			#define SV_TARGET_NORMAL SV_Target1
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
				#ifdef WRITE_MSAA_DEPTH
					, out float4 depthColor : SV_Target0
					, out float4 outMotionVector : SV_Target1
						#ifdef WRITE_DECAL_BUFFER
						, out float4 outDecalBuffer : SV_Target2
						#endif
					#else
					, out float4 outMotionVector : SV_Target0
						#ifdef WRITE_DECAL_BUFFER
						, out float4 outDecalBuffer : SV_Target1
						#endif
					#endif

					#ifdef WRITE_NORMAL_BUFFER
					, out float4 outNormalBuffer : SV_TARGET_NORMAL
					#endif

					#ifdef _DEPTHOFFSET_ON
					, out float outputDepth : DEPTH_OFFSET_SEMANTIC
					#endif
				
				)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.vmeshPositionCS;
				input.positionRWS = packedInput.vmeshInterp00.xyz;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceData surfaceData;
				BuiltinData builtinData;

				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				float2 vertexToFrag286_g55 = packedInput.ase_texcoord3.xy;
				float4 Control26_g55 = SAMPLE_TEXTURE2D( _Control, sampler_Control, vertexToFrag286_g55 );
				float2 uv_Splat0 = packedInput.ase_texcoord3.zw * _Splat0_ST.xy + _Splat0_ST.zw;
				float4 Normal0341_g55 = SAMPLE_TEXTURE2D( _Normal0, sampler_Normal0, uv_Splat0 );
				float3 unpack490_g55 = UnpackNormalScale( Normal0341_g55, _NormalScale0 );
				unpack490_g55.z = lerp( 1, unpack490_g55.z, saturate(_NormalScale0) );
				float2 uv_Splat1 = packedInput.ase_texcoord3.zw * _Splat1_ST.xy + _Splat1_ST.zw;
				float4 Normal1378_g55 = SAMPLE_TEXTURE2D( _Normal1, sampler_Normal0, uv_Splat1 );
				float3 unpack496_g55 = UnpackNormalScale( Normal1378_g55, _NormalScale1 );
				unpack496_g55.z = lerp( 1, unpack496_g55.z, saturate(_NormalScale1) );
				float2 uv_Splat2 = packedInput.ase_texcoord3.zw * _Splat2_ST.xy + _Splat2_ST.zw;
				float4 Normal2356_g55 = SAMPLE_TEXTURE2D( _Normal2, sampler_Normal0, uv_Splat2 );
				float3 unpack494_g55 = UnpackNormalScale( Normal2356_g55, _NormalScale2 );
				unpack494_g55.z = lerp( 1, unpack494_g55.z, saturate(_NormalScale2) );
				float2 uv_Splat3 = packedInput.ase_texcoord3.zw * _Splat3_ST.xy + _Splat3_ST.zw;
				float4 Normal3398_g55 = SAMPLE_TEXTURE2D( _Normal3, sampler_Normal0, uv_Splat3 );
				float3 unpack491_g55 = UnpackNormalScale( Normal3398_g55, _NormalScale3 );
				unpack491_g55.z = lerp( 1, unpack491_g55.z, saturate(_NormalScale3) );
				float4 weightedBlendVar473_g55 = Control26_g55;
				float3 weightedBlend473_g55 = ( weightedBlendVar473_g55.x*unpack490_g55 + weightedBlendVar473_g55.y*unpack496_g55 + weightedBlendVar473_g55.z*unpack494_g55 + weightedBlendVar473_g55.w*unpack491_g55 );
				float2 vertexToFrag958_g55 = packedInput.ase_texcoord4.xy;
				float4 Control1922_g55 = SAMPLE_TEXTURE2D( _Control1, sampler_Control1, vertexToFrag958_g55 );
				float2 uv_Splat4 = packedInput.ase_texcoord3.zw * _Splat4_ST.xy + _Splat4_ST.zw;
				float4 Normal4746_g55 = SAMPLE_TEXTURE2D( _Normal4, sampler_Normal4, uv_Splat4 );
				float3 unpack856_g55 = UnpackNormalScale( Normal4746_g55, _NormalScale4 );
				unpack856_g55.z = lerp( 1, unpack856_g55.z, saturate(_NormalScale4) );
				float2 uv_Splat5 = packedInput.ase_texcoord3.zw * _Splat5_ST.xy + _Splat5_ST.zw;
				float4 Normal5740_g55 = SAMPLE_TEXTURE2D( _Normal5, sampler_Normal0, uv_Splat5 );
				float3 unpack857_g55 = UnpackNormalScale( Normal5740_g55, _NormalScale5 );
				unpack857_g55.z = lerp( 1, unpack857_g55.z, saturate(_NormalScale5) );
				float2 uv_Splat6 = packedInput.ase_texcoord3.zw * _Splat6_ST.xy + _Splat6_ST.zw;
				float4 Normal6754_g55 = SAMPLE_TEXTURE2D( _Normal6, sampler_Normal0, uv_Splat6 );
				float3 unpack858_g55 = UnpackNormalScale( Normal6754_g55, _NormalScale6 );
				unpack858_g55.z = lerp( 1, unpack858_g55.z, saturate(_NormalScale6) );
				float2 uv_Splat7 = packedInput.ase_texcoord3.zw * _Splat7_ST.xy + _Splat7_ST.zw;
				float4 Normal7764_g55 = SAMPLE_TEXTURE2D( _Normal7, sampler_Normal0, uv_Splat7 );
				float3 unpack854_g55 = UnpackNormalScale( Normal7764_g55, _NormalScale7 );
				unpack854_g55.z = lerp( 1, unpack854_g55.z, saturate(_NormalScale7) );
				float4 weightedBlendVar860_g55 = Control1922_g55;
				float3 weightedBlend860_g55 = ( weightedBlendVar860_g55.x*unpack856_g55 + weightedBlendVar860_g55.y*unpack857_g55 + weightedBlendVar860_g55.z*unpack858_g55 + weightedBlendVar860_g55.w*unpack854_g55 );
				float3 break513_g55 = ( weightedBlend473_g55 + weightedBlend860_g55 );
				float3 appendResult514_g55 = (float3(break513_g55.x , break513_g55.y , ( break513_g55.z + 0.001 )));
				#ifdef _TERRAIN_INSTANCED_PERPIXEL_NORMAL
				float3 staticSwitch503_g55 = appendResult514_g55;
				#else
				float3 staticSwitch503_g55 = appendResult514_g55;
				#endif
				float3 NormalOut1374 = staticSwitch503_g55;
				float3 ase_worldPos = packedInput.ase_texcoord5.xyz;
				float3 surf_pos107_g58 = ase_worldPos;
				float3 ase_worldNormal = packedInput.ase_texcoord6.xyz;
				float3 surf_norm107_g58 = ase_worldNormal;
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				float2 appendResult1413 = (float2(ase_parentObjectScale.x , ase_parentObjectScale.z));
				float2 appendResult1414 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 temp_output_1418_0 = ( appendResult1413 * appendResult1414 );
				float simpleNoise1429 = SimpleNoise( temp_output_1418_0*2.0 );
				float height107_g58 = simpleNoise1429;
				float scale107_g58 = 8.0;
				float3 localPerturbNormal107_g58 = PerturbNormal107_g58( surf_pos107_g58 , surf_norm107_g58 , height107_g58 , scale107_g58 );
				float3 ase_worldTangent = packedInput.ase_texcoord7.xyz;
				float3 ase_worldBitangent = packedInput.ase_texcoord8.xyz;
				float3x3 ase_worldToTangent = float3x3(ase_worldTangent,ase_worldBitangent,ase_worldNormal);
				float3 worldToTangentDir42_g58 = mul( ase_worldToTangent, localPerturbNormal107_g58);
				float3 surf_pos107_g57 = ase_worldPos;
				float3 surf_norm107_g57 = ase_worldNormal;
				float simpleNoise1424 = SimpleNoise( temp_output_1418_0*400.0 );
				float temp_output_1430_0 = (simpleNoise1424*0.6 + 0.6);
				float height107_g57 = temp_output_1430_0;
				float scale107_g57 = 0.06;
				float3 localPerturbNormal107_g57 = PerturbNormal107_g57( surf_pos107_g57 , surf_norm107_g57 , height107_g57 , scale107_g57 );
				float3 worldToTangentDir42_g57 = mul( ase_worldToTangent, localPerturbNormal107_g57);
				float2 temp_output_1_0_g59 = BlendNormal( worldToTangentDir42_g58 , worldToTangentDir42_g57 ).xy;
				float dotResult4_g59 = dot( temp_output_1_0_g59 , temp_output_1_0_g59 );
				float3 appendResult10_g59 = (float3((temp_output_1_0_g59).x , (temp_output_1_0_g59).y , sqrt( ( 1.0 - saturate( dotResult4_g59 ) ) )));
				float3 normalizeResult12_g59 = normalize( appendResult10_g59 );
				float2 uv_HeatMap1415 = packedInput.ase_texcoord3.zw;
				float temp_output_1420_0 = ( 1.0 - SAMPLE_TEXTURE2D( _HeatMap, sampler_HeatMap, uv_HeatMap1415 ).b );
				float2 uv_NormalMap1416 = packedInput.ase_texcoord3.zw;
				float3 unpack1416 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, uv_NormalMap1416 ), _NormalIntensity );
				unpack1416.z = lerp( 1, unpack1416.z, saturate(_NormalIntensity) );
				float3 tex2DNode1416 = unpack1416;
				float temp_output_1427_0 = saturate( ase_worldNormal.y );
				float3 _Tangent = float3(0,1,0);
				float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
				float3 tangentToWorldDir1403 = normalize( mul( ase_tangentToWorldFast, _Tangent ) );
				float dotResult1404 = dot( tangentToWorldDir1403 , _Tangent );
				float lerpResult1408 = lerp( _SlopeMax , _SlopeMin , abs( dotResult1404 ));
				float Slope1417 = saturate( lerpResult1408 );
				float temp_output_1446_0 = saturate( ( (( _NormalMapBasedSnow )?( ( (temp_output_1420_0*-4.39 + 1.1) * (tex2DNode1416.g*_SnowScale + _SnowOffset) ) ):( (temp_output_1420_0*_SnowScale + _SnowOffset) )) * (( _SnowSlope )?( ( temp_output_1427_0 * Slope1417 ) ):( temp_output_1427_0 )) * _SnowAmount ) );
				float3 lerpResult1378 = lerp( NormalOut1374 , normalizeResult12_g59 , temp_output_1446_0);
				float3 NormalMap_Top1450 = tex2DNode1416;
				float3 NormalSnow_Output1393 = (( _TerrainNormalMap )?( BlendNormal( NormalMap_Top1450 , lerpResult1378 ) ):( lerpResult1378 ));
				
				float4 tex2DNode416_g55 = SAMPLE_TEXTURE2D( _Mask0, sampler_Mask0, uv_Splat0 );
				float Mask0A335_g55 = tex2DNode416_g55.a;
				float4 tex2DNode422_g55 = SAMPLE_TEXTURE2D( _Mask1, sampler_Mask0, uv_Splat1 );
				float Mask1A369_g55 = tex2DNode422_g55.a;
				float4 tex2DNode419_g55 = SAMPLE_TEXTURE2D( _Mask2, sampler_Mask0, uv_Splat2 );
				float Mask2A360_g55 = tex2DNode419_g55.a;
				float4 tex2DNode425_g55 = SAMPLE_TEXTURE2D( _Mask3, sampler_Mask0, uv_Splat3 );
				float Mask3A391_g55 = tex2DNode425_g55.a;
				float4 weightedBlendVar547_g55 = Control26_g55;
				float weightedBlend547_g55 = ( weightedBlendVar547_g55.x*( _Smoothness0 * Mask0A335_g55 ) + weightedBlendVar547_g55.y*( _Smoothness1 * Mask1A369_g55 ) + weightedBlendVar547_g55.z*( _Smoothness2 * Mask2A360_g55 ) + weightedBlendVar547_g55.w*( _Smoothness3 * Mask3A391_g55 ) );
				float4 tex2DNode697_g55 = SAMPLE_TEXTURE2D( _Mask4, sampler_Mask4, uv_Splat4 );
				float Mask4A750_g55 = tex2DNode697_g55.a;
				float4 tex2DNode704_g55 = SAMPLE_TEXTURE2D( _Mask5, sampler_Mask0, uv_Splat5 );
				float Mask5A745_g55 = tex2DNode704_g55.a;
				float4 tex2DNode711_g55 = SAMPLE_TEXTURE2D( _Mask6, sampler_Mask0, uv_Splat6 );
				float Mask6A758_g55 = tex2DNode711_g55.a;
				float4 tex2DNode717_g55 = SAMPLE_TEXTURE2D( _Mask7, sampler_Mask0, uv_Splat7 );
				float Mask7A768_g55 = tex2DNode717_g55.a;
				float4 weightedBlendVar826_g55 = Control1922_g55;
				float weightedBlend826_g55 = ( weightedBlendVar826_g55.x*( _Smoothness4 * Mask4A750_g55 ) + weightedBlendVar826_g55.y*( _Smoothness5 * Mask5A745_g55 ) + weightedBlendVar826_g55.z*( _Smoothness6 * Mask6A758_g55 ) + weightedBlendVar826_g55.w*( _Smoothness7 * Mask7A768_g55 ) );
				float SmoothOut1380 = ( _SmoothnessIntensity * ( weightedBlend547_g55 + weightedBlend826_g55 ) );
				float lerpResult1391 = lerp( SmoothOut1380 , (simpleNoise1424*1.4 + 0.0) , temp_output_1446_0);
				float Smoothness_Output1394 = lerpResult1391;
				
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord3.zw * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.Normal = NormalSnow_Output1393;
				surfaceDescription.Smoothness = Smoothness_Output1394;
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

				float4 VPASSpositionCS = float4(packedInput.vpassInterpolators0.xy, 0.0, packedInput.vpassInterpolators0.z);
				float4 VPASSpreviousPositionCS = float4(packedInput.vpassInterpolators1.xy, 0.0, packedInput.vpassInterpolators1.z);

				#ifdef _DEPTHOFFSET_ON
				VPASSpositionCS.w += builtinData.depthOffset;
				VPASSpreviousPositionCS.w += builtinData.depthOffset;
				#endif

				float2 motionVector = CalculateMotionVector( VPASSpositionCS, VPASSpreviousPositionCS );
				EncodeMotionVector( motionVector * 0.5, outMotionVector );

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if( forceNoMotion )
					outMotionVector = float4( 2.0, 0.0, 0.0, 0.0 );

				#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.vmeshPositionCS.z;
					depthColor.a = SharpenAlpha(builtinData.opacity, builtinData.alphaClipTreshold);
				#endif

				#if defined(WRITE_NORMAL_BUFFER)
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), outNormalBuffer);
				#endif

				#if defined(WRITE_DECAL_BUFFER)
					DecalPrepassData decalPrepassData;
					#ifdef _DISABLE_DECALS
					ZERO_INITIALIZE(DecalPrepassData, decalPrepassData);
					#else
					decalPrepassData.geomNormalWS = surfaceData.geomNormalWS;
					#endif
					decalPrepassData.renderingLayerMask = GetMeshRenderingLayerMask();
					EncodeIntoDecalPrepassBuffer(decalPrepassData, outDecalBuffer);
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif
			}

			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="Forward" }

			Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
			Blend 1 SrcAlpha OneMinusSrcAlpha
			Blend 2 One [_DstBlend2]
			Blend 3 One [_DstBlend2]
			Blend 4 One OneMinusSrcAlpha

			Cull [_CullModeForward]
			ZTest [_ZTestDepthEqualForOpaque]
			ZWrite [_ZWrite]

			Stencil
			{
				Ref [_StencilRef]
				WriteMask [_StencilWriteMask]
				Comp Always
				Pass Replace
			}


            ColorMask [_ColorMaskTransparentVelOne] 1
            ColorMask [_ColorMaskTransparentVelTwo] 2

			HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#ifdef UNITY_COLORSPACE_GAMMA//ASE Color Space Def
			#define unity_ColorSpaceDouble half4(2.0, 2.0, 2.0, 2.0)//ASE Color Space Def
			#else // Linear values//ASE Color Space Def
			#define unity_ColorSpaceDouble half4(4.59479380, 4.59479380, 4.59479380, 2.0)//ASE Color Space Def
			#endif//ASE Color Space Def
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

            #pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
            #pragma multi_compile_fragment PUNCTUAL_SHADOW_LOW PUNCTUAL_SHADOW_MEDIUM PUNCTUAL_SHADOW_HIGH
            #pragma multi_compile_fragment DIRECTIONAL_SHADOW_LOW DIRECTIONAL_SHADOW_MEDIUM DIRECTIONAL_SHADOW_HIGH
            #pragma multi_compile_fragment AREA_SHADOW_MEDIUM AREA_SHADOW_HIGH
            #pragma multi_compile_fragment _ PROBE_VOLUMES_L1 PROBE_VOLUMES_L2
            #pragma multi_compile_fragment SCREEN_SPACE_SHADOWS_OFF SCREEN_SPACE_SHADOWS_ON
            #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST

            #pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fragment DECALS_OFF DECALS_3RT DECALS_4RT
            #pragma multi_compile_fragment _ DECAL_SURFACE_GRADIENT
            #pragma multi_compile _ USE_LEGACY_LIGHTMAPS

			#ifndef SHADER_STAGE_FRAGMENT
			#define SHADOW_LOW
			#define USE_FPTL_LIGHTLIST
			#endif

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_FORWARD
		    #define HAS_LIGHTLOOP 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif

            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

		    #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
			#undef  _REFRACTION_PLANE
			#undef  _REFRACTION_SPHERE
			#define _REFRACTION_THIN
		    #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

			CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
			float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
			int _ObjectId;
			int _PassValue;
            #endif

			TEXTURE2D(_Control);
			SAMPLER(sampler_Control);
			float4 _DiffuseRemapScale0;
			TEXTURE2D(_Splat0);
			SAMPLER(sampler_Splat0);
			float4 _DiffuseRemapScale1;
			TEXTURE2D(_Splat1);
			float4 _DiffuseRemapScale2;
			TEXTURE2D(_Splat2);
			float4 _DiffuseRemapScale3;
			TEXTURE2D(_Splat3);
			TEXTURE2D(_Control1);
			SAMPLER(sampler_Control1);
			float4 _DiffuseRemapScale4;
			TEXTURE2D(_Splat4);
			SAMPLER(sampler_Splat4);
			float4 _DiffuseRemapScale5;
			TEXTURE2D(_Splat5);
			float4 _DiffuseRemapScale6;
			TEXTURE2D(_Splat6);
			float4 _DiffuseRemapScale7;
			TEXTURE2D(_Splat7);
			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			TEXTURE2D(_ColorMapProjection);
			SAMPLER(sampler_ColorMapProjection);
			TEXTURE2D(_HeatMap);
			SAMPLER(sampler_HeatMap);
			TEXTURE2D(_NormalMap);
			SAMPLER(sampler_NormalMap);
			float4x4 unity_CameraProjection;
			float4x4 unity_CameraInvProjection;
			float4x4 unity_WorldToCamera;
			float4x4 unity_CameraToWorld;
			TEXTURE2D(_Normal0);
			SAMPLER(sampler_Normal0);
			TEXTURE2D(_Normal1);
			TEXTURE2D(_Normal2);
			TEXTURE2D(_Normal3);
			TEXTURE2D(_Normal4);
			SAMPLER(sampler_Normal4);
			TEXTURE2D(_Normal5);
			TEXTURE2D(_Normal6);
			TEXTURE2D(_Normal7);
			TEXTURE2D(_Mask0);
			SAMPLER(sampler_Mask0);
			TEXTURE2D(_Mask1);
			TEXTURE2D(_Mask2);
			TEXTURE2D(_Mask3);
			TEXTURE2D(_Mask4);
			SAMPLER(sampler_Mask4);
			TEXTURE2D(_Mask5);
			TEXTURE2D(_Mask6);
			TEXTURE2D(_Mask7);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"

        	#ifdef HAVE_VFX_MODIFICATION
        	#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
        	#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_RELATIVE_WORLD_POS
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 uv1 : TEXCOORD3;
				float4 uv2 : TEXCOORD4;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 vpassPositionCS : TEXCOORD5;
					float3 vpassPreviousPositionCS : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			inline float noise_randomValue (float2 uv) { return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453); }
			inline float noise_interpolate (float a, float b, float t) { return (1.0-t)*a + (t*b); }
			inline float valueNoise (float2 uv)
			{
				float2 i = floor(uv);
				float2 f = frac( uv );
				f = f* f * (3.0 - 2.0 * f);
				uv = abs( frac(uv) - 0.5);
				float2 c0 = i + float2( 0.0, 0.0 );
				float2 c1 = i + float2( 1.0, 0.0 );
				float2 c2 = i + float2( 0.0, 1.0 );
				float2 c3 = i + float2( 1.0, 1.0 );
				float r0 = noise_randomValue( c0 );
				float r1 = noise_randomValue( c1 );
				float r2 = noise_randomValue( c2 );
				float r3 = noise_randomValue( c3 );
				float bottomOfGrid = noise_interpolate( r0, r1, f.x );
				float topOfGrid = noise_interpolate( r2, r3, f.x );
				float t = noise_interpolate( bottomOfGrid, topOfGrid, f.y );
				return t;
			}
			
			float SimpleNoise(float2 UV)
			{
				float t = 0.0;
				float freq = pow( 2.0, float( 0 ) );
				float amp = pow( 0.5, float( 3 - 0 ) );
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(1));
				amp = pow(0.5, float(3-1));
				t += valueNoise( UV/freq )*amp;
				freq = pow(2.0, float(2));
				amp = pow(0.5, float(3-2));
				t += valueNoise( UV/freq )*amp;
				return t;
			}
			
			float3 PerturbNormal107_g58( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			float3 PerturbNormal107_g57( float3 surf_pos, float3 surf_norm, float height, float scale )
			{
				// "Bump Mapping Unparametrized Surfaces on the GPU" by Morten S. Mikkelsen
				float3 vSigmaS = ddx( surf_pos );
				float3 vSigmaT = ddy( surf_pos );
				float3 vN = surf_norm;
				float3 vR1 = cross( vSigmaT , vN );
				float3 vR2 = cross( vN , vSigmaS );
				float fDet = dot( vSigmaS , vR1 );
				float dBs = ddx( height );
				float dBt = ddy( height );
				float3 vSurfGrad = scale * 0.05 * sign( fDet ) * ( dBs * vR1 + dBt * vR2 );
				return normalize ( abs( fDet ) * vN - vSurfGrad );
			}
			
			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData, out float3 bentNormalWS)
			{
				ZERO_INITIALIZE(SurfaceData, surfaceData);
				surfaceData.specularOcclusion = 1.0;
				surfaceData.thickness = 0.0;

				surfaceData.baseColor =                 surfaceDescription.BaseColor;
				surfaceData.perceptualSmoothness =		surfaceDescription.Smoothness;
				surfaceData.ambientOcclusion =			surfaceDescription.Occlusion;
				surfaceData.metallic =					surfaceDescription.Metallic;
				surfaceData.coatMask =					surfaceDescription.CoatMask;

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceData.specularOcclusion =			surfaceDescription.SpecularOcclusion;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceData.subsurfaceMask =			surfaceDescription.SubsurfaceMask;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceData.thickness = 				surfaceDescription.Thickness;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceData.transmissionMask =			surfaceDescription.TransmissionMask;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceData.diffusionProfileHash =		asuint(surfaceDescription.DiffusionProfile);
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceData.specularColor =				surfaceDescription.Specular;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceData.anisotropy =				surfaceDescription.Anisotropy;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceData.iridescenceMask =			surfaceDescription.IridescenceMask;
				surfaceData.iridescenceThickness =		surfaceDescription.IridescenceThickness;
				#endif

				// refraction
                #if defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE) || defined(_REFRACTION_THIN)
                    if (_EnableSSRefraction)
                    {
                        surfaceData.ior =                       surfaceDescription.RefractionIndex;
                        surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
                        surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
                        surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
                        surfaceDescription.Alpha = 1.0;
                    }
                    else
                    {
                        surfaceData.ior = 1.0;
                        surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                        surfaceData.atDistance = 1.0;
                        surfaceData.transmittanceMask = 0.0;
                        surfaceDescription.Alpha = 1.0;
                    }
                #else
                    surfaceData.ior = 1.0;
                    surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
                    surfaceData.atDistance = 1.0;
                    surfaceData.transmittanceMask = 0.0;
                #endif

				surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
				#endif

				#ifdef _MATERIAL_FEATURE_COLORED_TRANSMISSION
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_COLORED_TRANSMISSION;
				#endif

                #ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
                    surfaceData.normalWS = float3(0, 1, 0);
                #endif

				#ifdef _MATERIAL_FEATURE_CLEAR_COAT
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
				#endif

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
                    surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
				#endif

				#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
                    surfaceData.baseColor *= ( 1.0 - Max3( surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b ) );
				#endif

				#ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
				#else
                    float3 doubleSidedConstants = float3( 1.0, 1.0, 1.0 );
				#endif

				float3 normalTS = float3(0.0f, 0.0f, 1.0f);
				normalTS = surfaceDescription.Normal;

                #ifdef DECAL_NORMAL_BLENDING
					normalTS = SurfaceGradientFromTangentSpaceNormalAndFromTBN(normalTS, fragInputs.tangentToWorld[0], fragInputs.tangentToWorld[1]);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;

                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, fragInputs.tangentToWorld[2], normalTS);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                     }
                    #endif

                    GetNormalWS_SG(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);
                #else
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS, doubleSidedConstants);

                    #if HAVE_DECALS
                    if (_EnableDecals)
                    {
                        float alpha = 1.0;
                        alpha = surfaceDescription.Alpha;
        
                        DecalSurfaceData decalSurfaceData = GetDecalSurfaceData(posInput, fragInputs, alpha);
                        ApplyDecalToSurfaceNormal(decalSurfaceData, surfaceData.normalWS.xyz);
                        ApplyDecalToSurfaceDataNoNormal(decalSurfaceData, surfaceData);
                    }
                    #endif
                #endif

				surfaceData.geomNormalWS = fragInputs.tangentToWorld[2];
                surfaceData.tangentWS = normalize(fragInputs.tangentToWorld[0].xyz );
                surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);

				bentNormalWS = surfaceData.normalWS;

				#ifdef ASE_BENT_NORMAL
                    GetNormalWS( fragInputs, surfaceDescription.BentNormal, bentNormalWS, doubleSidedConstants );
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
                    surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.tangentToWorld);
				#endif

				#if defined(DEBUG_DISPLAY)
				    #if !defined(SHADER_STAGE_RAY_TRACING)
				    if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
				    {
					   #ifdef FRAG_INPUTS_USE_TEXCOORD0
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG(posInput.positionSS, fragInputs.texCoord0);
                       #else
                           surfaceData.baseColor = GET_TEXTURE_STREAMING_DEBUG_NO_UV(posInput.positionSS);
                       #endif
					   surfaceData.metallic = 0;
				    }
					#endif
				    ApplyDebugToSurfaceData(fragInputs.tangentToWorld, surfaceData); 
				#endif

                #if defined(_SPECULAR_OCCLUSION_CUSTOM)
                #elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
                #elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
                    surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
                #endif

                #ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
                    surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.tangentToWorld[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
                #endif
			}

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif  
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

				float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);
			}

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				float2 break291_g55 = _Control_ST.zw;
				float2 appendResult293_g55 = (float2(( break291_g55.x + 0.001 ) , ( break291_g55.y + 0.0001 )));
				float2 vertexToFrag286_g55 = ( ( inputMesh.ase_texcoord.xy * _Control_ST.xy ) + appendResult293_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord7.xy = vertexToFrag286_g55;
				float2 break951_g55 = _Control1_ST.zw;
				float2 appendResult956_g55 = (float2(( break951_g55.x + 0.001 ) , ( break951_g55.y + 0.0001 )));
				float2 vertexToFrag958_g55 = ( ( inputMesh.ase_texcoord.xy * _Control1_ST.xy ) + appendResult956_g55 );
				outputPackedVaryingsMeshToPS.ase_texcoord8.xy = vertexToFrag958_g55;
				float3 ase_worldNormal = TransformObjectToWorldNormal(inputMesh.normalOS);
				float3 ase_worldTangent = TransformObjectToWorldDir(inputMesh.tangentOS.xyz);
				float ase_vertexTangentSign = inputMesh.tangentOS.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				outputPackedVaryingsMeshToPS.ase_texcoord9.xyz = ase_worldBitangent;
				
				outputPackedVaryingsMeshToPS.ase_texcoord7.zw = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord8.zw = 0;
				outputPackedVaryingsMeshToPS.ase_texcoord9.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(positionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}
				#endif

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.uv1.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.uv2.xyzw = inputMesh.uv2;

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outputPackedVaryingsMeshToPS.vpassPositionCS = float3(VPASSpositionCS.xyw);
					outputPackedVaryingsMeshToPS.vpassPreviousPositionCS = float3(VPASSpreviousPositionCS.xyw);
				#endif
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.uv1 = v.uv1;
				o.uv2 = v.uv2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.uv1 = patch[0].uv1 * bary.x + patch[1].uv1 * bary.y + patch[2].uv1 * bary.z;
				o.uv2 = patch[0].uv2 * bary.x + patch[1].uv2 * bary.y + patch[2].uv2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplayMaterial.hlsl"

            #if defined(_TRANSPARENT_REFRACTIVE_SORT) || defined(_ENABLE_FOG_ON_TRANSPARENT)
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Water/Shaders/UnderWaterUtilities.hlsl"
            #endif

            #ifdef UNITY_VIRTUAL_TEXTURING
                #ifdef OUTPUT_SPLIT_LIGHTING
                   #define DIFFUSE_LIGHTING_TARGET SV_Target2
                   #define SSS_BUFFER_TARGET SV_Target3
                #elif defined(_WRITE_TRANSPARENT_MOTION_VECTOR)
                   #define MOTION_VECTOR_TARGET SV_Target2
                    #ifdef _TRANSPARENT_REFRACTIVE_SORT
                        #define BEFORE_REFRACTION_TARGET SV_Target3
                        #define BEFORE_REFRACTION_ALPHA_TARGET SV_Target4
                #endif
            	#endif
            #if defined(SHADER_API_PSSL)
            	#pragma PSSL_target_output_format(target 1 FMT_32_ABGR)
            #endif
            #else
                #ifdef OUTPUT_SPLIT_LIGHTING
                #define DIFFUSE_LIGHTING_TARGET SV_Target1
                #define SSS_BUFFER_TARGET SV_Target2
                #elif defined(_WRITE_TRANSPARENT_MOTION_VECTOR)
                #define MOTION_VECTOR_TARGET SV_Target1
                #ifdef _TRANSPARENT_REFRACTIVE_SORT
                     #define BEFORE_REFRACTION_TARGET SV_Target2
                     #define BEFORE_REFRACTION_ALPHA_TARGET SV_Target3
                #endif
            #endif
            #endif

			void Frag(PackedVaryingsMeshToPS packedInput
				, out float4 outColor:SV_Target0
            #ifdef UNITY_VIRTUAL_TEXTURING
				, out float4 outVTFeedback : SV_Target1
            #endif
            #ifdef OUTPUT_SPLIT_LIGHTING
				, out float4 outDiffuseLighting : DIFFUSE_LIGHTING_TARGET
				, OUTPUT_SSSBUFFER(outSSSBuffer) : SSS_BUFFER_TARGET
            #elif defined(_WRITE_TRANSPARENT_MOTION_VECTOR)
				, out float4 outMotionVec : MOTION_VECTOR_TARGET
                #ifdef _TRANSPARENT_REFRACTIVE_SORT
                , out float4 outBeforeRefractionColor : BEFORE_REFRACTION_TARGET
                , out float4 outBeforeRefractionAlpha : BEFORE_REFRACTION_ALPHA_TARGET
                #endif
            #endif
            #ifdef _DEPTHOFFSET_ON
				, out float outputDepth : DEPTH_OFFSET_SEMANTIC
            #endif
		    
						)
			{
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outMotionVec = float4(2.0, 0.0, 0.0, 1.0);
				#endif

				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( packedInput );
				UNITY_SETUP_INSTANCE_ID( packedInput );
				float3 positionRWS = packedInput.positionRWS.xyz;
				float3 normalWS = packedInput.normalWS.xyz;
				float4 tangentWS = packedInput.tangentWS.xyzw;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.tangentToWorld = BuildTangentToWorld(tangentWS, normalWS);
				input.texCoord1 = packedInput.uv1.xyzw;
				input.texCoord2 = packedInput.uv2.xyzw;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE(packedInput.cullFace, true, false);
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				AdjustFragInputsToOffScreenRendering(input, _OffScreenRendering > 0, _OffScreenDownsampleFactor);
				uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();

				PositionInputs posInput = GetPositionInput( input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex );

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float2 vertexToFrag286_g55 = packedInput.ase_texcoord7.xy;
				float4 Control26_g55 = SAMPLE_TEXTURE2D( _Control, sampler_Control, vertexToFrag286_g55 );
				float2 uv_Splat0 = packedInput.ase_texcoord7.zw * _Splat0_ST.xy + _Splat0_ST.zw;
				float4 tex2DNode414_g55 = SAMPLE_TEXTURE2D( _Splat0, sampler_Splat0, uv_Splat0 );
				float3 Splat0342_g55 = (tex2DNode414_g55).rgb;
				float2 uv_Splat1 = packedInput.ase_texcoord7.zw * _Splat1_ST.xy + _Splat1_ST.zw;
				float4 tex2DNode420_g55 = SAMPLE_TEXTURE2D( _Splat1, sampler_Splat0, uv_Splat1 );
				float3 Splat1379_g55 = (tex2DNode420_g55).rgb;
				float2 uv_Splat2 = packedInput.ase_texcoord7.zw * _Splat2_ST.xy + _Splat2_ST.zw;
				float4 tex2DNode417_g55 = SAMPLE_TEXTURE2D( _Splat2, sampler_Splat0, uv_Splat2 );
				float3 Splat2357_g55 = (tex2DNode417_g55).rgb;
				float2 uv_Splat3 = packedInput.ase_texcoord7.zw * _Splat3_ST.xy + _Splat3_ST.zw;
				float4 tex2DNode423_g55 = SAMPLE_TEXTURE2D( _Splat3, sampler_Splat0, uv_Splat3 );
				float3 Splat3390_g55 = (tex2DNode423_g55).rgb;
				float4 weightedBlendVar9_g55 = Control26_g55;
				float3 weightedBlend9_g55 = ( weightedBlendVar9_g55.x*( (_DiffuseRemapScale0).rgb * Splat0342_g55 ) + weightedBlendVar9_g55.y*( (_DiffuseRemapScale1).rgb * Splat1379_g55 ) + weightedBlendVar9_g55.z*( (_DiffuseRemapScale2).rgb * Splat2357_g55 ) + weightedBlendVar9_g55.w*( (_DiffuseRemapScale3).rgb * Splat3390_g55 ) );
				float2 vertexToFrag958_g55 = packedInput.ase_texcoord8.xy;
				float4 Control1922_g55 = SAMPLE_TEXTURE2D( _Control1, sampler_Control1, vertexToFrag958_g55 );
				float2 uv_Splat4 = packedInput.ase_texcoord7.zw * _Splat4_ST.xy + _Splat4_ST.zw;
				float4 tex2DNode696_g55 = SAMPLE_TEXTURE2D( _Splat4, sampler_Splat4, uv_Splat4 );
				float3 Splat4752_g55 = (tex2DNode696_g55).rgb;
				float2 uv_Splat5 = packedInput.ase_texcoord7.zw * _Splat5_ST.xy + _Splat5_ST.zw;
				float4 tex2DNode702_g55 = SAMPLE_TEXTURE2D( _Splat5, sampler_Splat0, uv_Splat5 );
				float3 Splat5743_g55 = (tex2DNode702_g55).rgb;
				float2 uv_Splat6 = packedInput.ase_texcoord7.zw * _Splat6_ST.xy + _Splat6_ST.zw;
				float4 tex2DNode709_g55 = SAMPLE_TEXTURE2D( _Splat6, sampler_Splat0, uv_Splat6 );
				float3 Splat6759_g55 = (tex2DNode709_g55).rgb;
				float2 uv_Splat7 = packedInput.ase_texcoord7.zw * _Splat7_ST.xy + _Splat7_ST.zw;
				float4 tex2DNode719_g55 = SAMPLE_TEXTURE2D( _Splat7, sampler_Splat0, uv_Splat7 );
				float3 Splat7762_g55 = (tex2DNode719_g55).rgb;
				float4 weightedBlendVar912_g55 = Control1922_g55;
				float3 weightedBlend912_g55 = ( weightedBlendVar912_g55.x*( (_DiffuseRemapScale4).rgb * Splat4752_g55 ) + weightedBlendVar912_g55.y*( (_DiffuseRemapScale5).rgb * Splat5743_g55 ) + weightedBlendVar912_g55.z*( (_DiffuseRemapScale6).rgb * Splat6759_g55 ) + weightedBlendVar912_g55.w*( (_DiffuseRemapScale7).rgb * Splat7762_g55 ) );
				float3 temp_output_968_0_g55 = ( weightedBlend9_g55 + weightedBlend912_g55 );
				float3 localClipHoles453_g55 = ( temp_output_968_0_g55 );
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord7.zw * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float Hole453_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture ).r;
				{
				#ifdef _ALPHATEST_ON
				clip(Hole453_g55 == 0.0f ? -1 : 1);
				#endif
				}
				float3 BaseColorOut1373 = localClipHoles453_g55;
				float2 uv_ColorMapProjection1434 = packedInput.ase_texcoord7.zw;
				float4 ColorMap_Top1443 = ( unity_ColorSpaceDouble * SAMPLE_TEXTURE2D( _ColorMapProjection, sampler_ColorMapProjection, uv_ColorMapProjection1434 ) );
				float4 lerpResult1381 = lerp( float4( BaseColorOut1373 , 0.0 ) , ( ColorMap_Top1443 * float4( BaseColorOut1373 , 0.0 ) ) , _ColorMapIntensity);
				float3 ase_parentObjectScale = ( 1.0 / float3( length( GetWorldToObjectMatrix()[ 0 ].xyz ), length( GetWorldToObjectMatrix()[ 1 ].xyz ), length( GetWorldToObjectMatrix()[ 2 ].xyz ) ) );
				float2 appendResult1413 = (float2(ase_parentObjectScale.x , ase_parentObjectScale.z));
				float3 ase_worldPos = GetAbsolutePositionWS( positionRWS );
				float2 appendResult1414 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 temp_output_1418_0 = ( appendResult1413 * appendResult1414 );
				float simpleNoise1424 = SimpleNoise( temp_output_1418_0*400.0 );
				float temp_output_1430_0 = (simpleNoise1424*0.6 + 0.6);
				float4 temp_cast_3 = (temp_output_1430_0).xxxx;
				float2 uv_HeatMap1415 = packedInput.ase_texcoord7.zw;
				float temp_output_1420_0 = ( 1.0 - SAMPLE_TEXTURE2D( _HeatMap, sampler_HeatMap, uv_HeatMap1415 ).b );
				float2 uv_NormalMap1416 = packedInput.ase_texcoord7.zw;
				float3 unpack1416 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_NormalMap, uv_NormalMap1416 ), _NormalIntensity );
				unpack1416.z = lerp( 1, unpack1416.z, saturate(_NormalIntensity) );
				float3 tex2DNode1416 = unpack1416;
				float temp_output_1427_0 = saturate( normalWS.y );
				float3 _Tangent = float3(0,1,0);
				float3 ase_worldBitangent = packedInput.ase_texcoord9.xyz;
				float3x3 ase_tangentToWorldFast = float3x3(tangentWS.xyz.x,ase_worldBitangent.x,normalWS.x,tangentWS.xyz.y,ase_worldBitangent.y,normalWS.y,tangentWS.xyz.z,ase_worldBitangent.z,normalWS.z);
				float3 tangentToWorldDir1403 = normalize( mul( ase_tangentToWorldFast, _Tangent ) );
				float dotResult1404 = dot( tangentToWorldDir1403 , _Tangent );
				float lerpResult1408 = lerp( _SlopeMax , _SlopeMin , abs( dotResult1404 ));
				float Slope1417 = saturate( lerpResult1408 );
				float temp_output_1446_0 = saturate( ( (( _NormalMapBasedSnow )?( ( (temp_output_1420_0*-4.39 + 1.1) * (tex2DNode1416.g*_SnowScale + _SnowOffset) ) ):( (temp_output_1420_0*_SnowScale + _SnowOffset) )) * (( _SnowSlope )?( ( temp_output_1427_0 * Slope1417 ) ):( temp_output_1427_0 )) * _SnowAmount ) );
				float4 lerpResult1389 = lerp( (( _ColorMap )?( lerpResult1381 ):( float4( BaseColorOut1373 , 0.0 ) )) , temp_cast_3 , temp_output_1446_0);
				float4 AlbedoSnow_Output1392 = lerpResult1389;
				
				float4 Normal0341_g55 = SAMPLE_TEXTURE2D( _Normal0, sampler_Normal0, uv_Splat0 );
				float3 unpack490_g55 = UnpackNormalScale( Normal0341_g55, _NormalScale0 );
				unpack490_g55.z = lerp( 1, unpack490_g55.z, saturate(_NormalScale0) );
				float4 Normal1378_g55 = SAMPLE_TEXTURE2D( _Normal1, sampler_Normal0, uv_Splat1 );
				float3 unpack496_g55 = UnpackNormalScale( Normal1378_g55, _NormalScale1 );
				unpack496_g55.z = lerp( 1, unpack496_g55.z, saturate(_NormalScale1) );
				float4 Normal2356_g55 = SAMPLE_TEXTURE2D( _Normal2, sampler_Normal0, uv_Splat2 );
				float3 unpack494_g55 = UnpackNormalScale( Normal2356_g55, _NormalScale2 );
				unpack494_g55.z = lerp( 1, unpack494_g55.z, saturate(_NormalScale2) );
				float4 Normal3398_g55 = SAMPLE_TEXTURE2D( _Normal3, sampler_Normal0, uv_Splat3 );
				float3 unpack491_g55 = UnpackNormalScale( Normal3398_g55, _NormalScale3 );
				unpack491_g55.z = lerp( 1, unpack491_g55.z, saturate(_NormalScale3) );
				float4 weightedBlendVar473_g55 = Control26_g55;
				float3 weightedBlend473_g55 = ( weightedBlendVar473_g55.x*unpack490_g55 + weightedBlendVar473_g55.y*unpack496_g55 + weightedBlendVar473_g55.z*unpack494_g55 + weightedBlendVar473_g55.w*unpack491_g55 );
				float4 Normal4746_g55 = SAMPLE_TEXTURE2D( _Normal4, sampler_Normal4, uv_Splat4 );
				float3 unpack856_g55 = UnpackNormalScale( Normal4746_g55, _NormalScale4 );
				unpack856_g55.z = lerp( 1, unpack856_g55.z, saturate(_NormalScale4) );
				float4 Normal5740_g55 = SAMPLE_TEXTURE2D( _Normal5, sampler_Normal0, uv_Splat5 );
				float3 unpack857_g55 = UnpackNormalScale( Normal5740_g55, _NormalScale5 );
				unpack857_g55.z = lerp( 1, unpack857_g55.z, saturate(_NormalScale5) );
				float4 Normal6754_g55 = SAMPLE_TEXTURE2D( _Normal6, sampler_Normal0, uv_Splat6 );
				float3 unpack858_g55 = UnpackNormalScale( Normal6754_g55, _NormalScale6 );
				unpack858_g55.z = lerp( 1, unpack858_g55.z, saturate(_NormalScale6) );
				float4 Normal7764_g55 = SAMPLE_TEXTURE2D( _Normal7, sampler_Normal0, uv_Splat7 );
				float3 unpack854_g55 = UnpackNormalScale( Normal7764_g55, _NormalScale7 );
				unpack854_g55.z = lerp( 1, unpack854_g55.z, saturate(_NormalScale7) );
				float4 weightedBlendVar860_g55 = Control1922_g55;
				float3 weightedBlend860_g55 = ( weightedBlendVar860_g55.x*unpack856_g55 + weightedBlendVar860_g55.y*unpack857_g55 + weightedBlendVar860_g55.z*unpack858_g55 + weightedBlendVar860_g55.w*unpack854_g55 );
				float3 break513_g55 = ( weightedBlend473_g55 + weightedBlend860_g55 );
				float3 appendResult514_g55 = (float3(break513_g55.x , break513_g55.y , ( break513_g55.z + 0.001 )));
				#ifdef _TERRAIN_INSTANCED_PERPIXEL_NORMAL
				float3 staticSwitch503_g55 = appendResult514_g55;
				#else
				float3 staticSwitch503_g55 = appendResult514_g55;
				#endif
				float3 NormalOut1374 = staticSwitch503_g55;
				float3 surf_pos107_g58 = ase_worldPos;
				float3 surf_norm107_g58 = normalWS;
				float simpleNoise1429 = SimpleNoise( temp_output_1418_0*2.0 );
				float height107_g58 = simpleNoise1429;
				float scale107_g58 = 8.0;
				float3 localPerturbNormal107_g58 = PerturbNormal107_g58( surf_pos107_g58 , surf_norm107_g58 , height107_g58 , scale107_g58 );
				float3x3 ase_worldToTangent = float3x3(tangentWS.xyz,ase_worldBitangent,normalWS);
				float3 worldToTangentDir42_g58 = mul( ase_worldToTangent, localPerturbNormal107_g58);
				float3 surf_pos107_g57 = ase_worldPos;
				float3 surf_norm107_g57 = normalWS;
				float height107_g57 = temp_output_1430_0;
				float scale107_g57 = 0.06;
				float3 localPerturbNormal107_g57 = PerturbNormal107_g57( surf_pos107_g57 , surf_norm107_g57 , height107_g57 , scale107_g57 );
				float3 worldToTangentDir42_g57 = mul( ase_worldToTangent, localPerturbNormal107_g57);
				float2 temp_output_1_0_g59 = BlendNormal( worldToTangentDir42_g58 , worldToTangentDir42_g57 ).xy;
				float dotResult4_g59 = dot( temp_output_1_0_g59 , temp_output_1_0_g59 );
				float3 appendResult10_g59 = (float3((temp_output_1_0_g59).x , (temp_output_1_0_g59).y , sqrt( ( 1.0 - saturate( dotResult4_g59 ) ) )));
				float3 normalizeResult12_g59 = normalize( appendResult10_g59 );
				float3 lerpResult1378 = lerp( NormalOut1374 , normalizeResult12_g59 , temp_output_1446_0);
				float3 NormalMap_Top1450 = tex2DNode1416;
				float3 NormalSnow_Output1393 = (( _TerrainNormalMap )?( BlendNormal( NormalMap_Top1450 , lerpResult1378 ) ):( lerpResult1378 ));
				
				float4 tex2DNode416_g55 = SAMPLE_TEXTURE2D( _Mask0, sampler_Mask0, uv_Splat0 );
				float Mask0A335_g55 = tex2DNode416_g55.a;
				float4 tex2DNode422_g55 = SAMPLE_TEXTURE2D( _Mask1, sampler_Mask0, uv_Splat1 );
				float Mask1A369_g55 = tex2DNode422_g55.a;
				float4 tex2DNode419_g55 = SAMPLE_TEXTURE2D( _Mask2, sampler_Mask0, uv_Splat2 );
				float Mask2A360_g55 = tex2DNode419_g55.a;
				float4 tex2DNode425_g55 = SAMPLE_TEXTURE2D( _Mask3, sampler_Mask0, uv_Splat3 );
				float Mask3A391_g55 = tex2DNode425_g55.a;
				float4 weightedBlendVar547_g55 = Control26_g55;
				float weightedBlend547_g55 = ( weightedBlendVar547_g55.x*( _Smoothness0 * Mask0A335_g55 ) + weightedBlendVar547_g55.y*( _Smoothness1 * Mask1A369_g55 ) + weightedBlendVar547_g55.z*( _Smoothness2 * Mask2A360_g55 ) + weightedBlendVar547_g55.w*( _Smoothness3 * Mask3A391_g55 ) );
				float4 tex2DNode697_g55 = SAMPLE_TEXTURE2D( _Mask4, sampler_Mask4, uv_Splat4 );
				float Mask4A750_g55 = tex2DNode697_g55.a;
				float4 tex2DNode704_g55 = SAMPLE_TEXTURE2D( _Mask5, sampler_Mask0, uv_Splat5 );
				float Mask5A745_g55 = tex2DNode704_g55.a;
				float4 tex2DNode711_g55 = SAMPLE_TEXTURE2D( _Mask6, sampler_Mask0, uv_Splat6 );
				float Mask6A758_g55 = tex2DNode711_g55.a;
				float4 tex2DNode717_g55 = SAMPLE_TEXTURE2D( _Mask7, sampler_Mask0, uv_Splat7 );
				float Mask7A768_g55 = tex2DNode717_g55.a;
				float4 weightedBlendVar826_g55 = Control1922_g55;
				float weightedBlend826_g55 = ( weightedBlendVar826_g55.x*( _Smoothness4 * Mask4A750_g55 ) + weightedBlendVar826_g55.y*( _Smoothness5 * Mask5A745_g55 ) + weightedBlendVar826_g55.z*( _Smoothness6 * Mask6A758_g55 ) + weightedBlendVar826_g55.w*( _Smoothness7 * Mask7A768_g55 ) );
				float SmoothOut1380 = ( _SmoothnessIntensity * ( weightedBlend547_g55 + weightedBlend826_g55 ) );
				float lerpResult1391 = lerp( SmoothOut1380 , (simpleNoise1424*1.4 + 0.0) , temp_output_1446_0);
				float Smoothness_Output1394 = lerpResult1391;
				
				float Mask0G409_g55 = tex2DNode416_g55.g;
				float Mask1G371_g55 = tex2DNode422_g55.g;
				float Mask2G358_g55 = tex2DNode419_g55.g;
				float Mask3G389_g55 = tex2DNode425_g55.g;
				float4 weightedBlendVar602_g55 = Control26_g55;
				float weightedBlend602_g55 = ( weightedBlendVar602_g55.x*saturate( Mask0G409_g55 ) + weightedBlendVar602_g55.y*saturate( Mask1G371_g55 ) + weightedBlendVar602_g55.z*saturate( Mask2G358_g55 ) + weightedBlendVar602_g55.w*saturate( Mask3G389_g55 ) );
				float Mask4G748_g55 = tex2DNode697_g55.g;
				float Mask5G742_g55 = tex2DNode704_g55.g;
				float Mask6G756_g55 = tex2DNode711_g55.g;
				float Mask7G766_g55 = tex2DNode717_g55.g;
				float4 weightedBlendVar799_g55 = Control1922_g55;
				float weightedBlend799_g55 = ( weightedBlendVar799_g55.x*saturate( Mask4G748_g55 ) + weightedBlendVar799_g55.y*saturate( Mask5G742_g55 ) + weightedBlendVar799_g55.z*saturate( Mask6G756_g55 ) + weightedBlendVar799_g55.w*saturate( Mask7G766_g55 ) );
				float AoOut1457 = saturate( ( weightedBlend602_g55 + weightedBlend799_g55 ) );
				float lerpResult1460 = lerp( AoOut1457 , 1.0 , temp_output_1446_0);
				float Ao_Output1466 = lerpResult1460;
				
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.BaseColor = AlbedoSnow_Output1392.rgb;
				surfaceDescription.Normal = NormalSnow_Output1393;
				surfaceDescription.BentNormal = float3( 0, 0, 1 );
				surfaceDescription.CoatMask = 0;
				surfaceDescription.Metallic = 0;

				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif

				surfaceDescription.Emission = 0;
				surfaceDescription.Smoothness = Smoothness_Output1394;
				surfaceDescription.Occlusion = Ao_Output1466;
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
				surfaceDescription.RefractionColor = float3( 1, 1, 1 );
				surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif

				#ifdef _MATERIAL_FEATURE_TRANSMISSION
				surfaceDescription.TransmissionMask = 1;
				#endif

				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = _DiffusionProfile;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3( 1, 0, 0 );
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				#ifdef ASE_BAKEDGI
				surfaceDescription.BakedGI = 0;
				#endif

				#ifdef ASE_BAKEDBACKGI
				surfaceDescription.BakedBackGI = 0;
				#endif

				#ifdef _DEPTHOFFSET_ON
				surfaceDescription.DepthOffset = 0;
				#endif

				#ifdef UNITY_VIRTUAL_TEXTURING
				surfaceDescription.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
				#endif

				SurfaceData surfaceData;
				BuiltinData builtinData;
				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

				BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

				PreLightData preLightData = GetPreLightData(V, posInput, bsdfData);

				outColor = float4(0.0, 0.0, 0.0, 0.0);

				#ifdef DEBUG_DISPLAY
				#ifdef OUTPUT_SPLIT_LIGHTING
					outDiffuseLighting = float4(0, 0, 0, 1);
					ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#endif

			    bool viewMaterial = GetMaterialDebugColor(outColor, input, builtinData, posInput, surfaceData, bsdfData);

				if (!viewMaterial)
				{
					if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_DIFFUSE_COLOR || _DebugFullScreenMode == FULLSCREENDEBUGMODE_VALIDATE_SPECULAR_COLOR)
					{
						float3 result = float3(0.0, 0.0, 0.0);
						GetPBRValidatorDebug(surfaceData, result);
						outColor = float4(result, 1.0f);
					}
					else if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_TRANSPARENCY_OVERDRAW)
					{
						float4 result = _DebugTransparencyOverdrawWeight * float4(TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_COST, TRANSPARENCY_OVERDRAW_A);
						outColor = result;
					}
					else
                #endif
					{
                #ifdef _SURFACE_TYPE_TRANSPARENT
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
                #else
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
                #endif
						LightLoopOutput lightLoopOutput;
						LightLoop(V, posInput, preLightData, bsdfData, builtinData, featureFlags, lightLoopOutput);

						// Alias
						float3 diffuseLighting = lightLoopOutput.diffuseLighting;
						float3 specularLighting = lightLoopOutput.specularLighting;

						diffuseLighting *= GetCurrentExposureMultiplier();
						specularLighting *= GetCurrentExposureMultiplier();

                #ifdef OUTPUT_SPLIT_LIGHTING
						if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
						{
							outColor = float4(specularLighting, 1.0);
							outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
						}
						else
						{
							outColor = float4(diffuseLighting + specularLighting, 1.0);
							outDiffuseLighting = float4(0, 0, 0, 1);
						}
						ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
                #else
						outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);

						#ifdef _ENABLE_FOG_ON_TRANSPARENT
						outColor = EvaluateAtmosphericScattering(posInput, V, outColor);
                        #endif

                        #ifdef _TRANSPARENT_REFRACTIVE_SORT
                        ComputeRefractionSplitColor(posInput, outColor, outBeforeRefractionColor, outBeforeRefractionAlpha);
                        #endif
                #endif

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
						float4 VPASSpositionCS = float4(packedInput.vpassPositionCS.xy, 0.0, packedInput.vpassPositionCS.z);
						float4 VPASSpreviousPositionCS = float4(packedInput.vpassPreviousPositionCS.xy, 0.0, packedInput.vpassPreviousPositionCS.z);
						bool forceNoMotion = any(unity_MotionVectorsParams.yw == 0.0);
                #if defined(HAVE_VFX_MODIFICATION) && !VFX_FEATURE_MOTION_VECTORS
                        forceNoMotion = true;
                #endif
				        if (!forceNoMotion)
						{
							float2 motionVec = CalculateMotionVector(VPASSpositionCS, VPASSpreviousPositionCS);
							EncodeMotionVector(motionVec * 0.5, outMotionVec);
							outMotionVec.zw = 1.0;
						}
				#endif
				}

				#ifdef DEBUG_DISPLAY
				}
				#endif

				#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
				#endif

                #ifdef UNITY_VIRTUAL_TEXTURING
				    float vtAlphaValue = builtinData.opacity;
                    #if defined(HAS_REFRACTION) && HAS_REFRACTION
					vtAlphaValue = 1.0f - bsdfData.transmittanceMask;
                #endif
				outVTFeedback = PackVTFeedbackWithAlpha(builtinData.vtPackedFeedback, input.positionSS.xy, vtAlphaValue);
				outVTFeedback.rgb *= outVTFeedback.a; // premuliplied alpha
                #endif

			}
			ENDHLSL
		}

		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
	UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"

		Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

            Cull [_CullMode]

            HLSLPROGRAM
			#pragma shader_feature_local_fragment _ _DISABLE_DECALS
			#pragma shader_feature_local_fragment _ _DISABLE_SSR_TRANSPARENT
			#pragma shader_feature_local _ _ALPHATEST_ON
			#pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
			#pragma shader_feature_local _ _DOUBLESIDED_ON
			#define _SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL 1
			#define _MATERIAL_FEATURE_SUBSURFACE_SCATTERING 1
			#pragma shader_feature_local_fragment _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
			#define _AMBIENT_OCCLUSION 1
			#define ASE_SRP_VERSION 170003
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma editor_sync_compilation
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

			#pragma vertex Vert
			#pragma fragment Frag

			#define SHADERPASS SHADERPASS_DEPTH_ONLY
			#define SCENEPICKINGPASS 1

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_TANGENT_TO_WORLD

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

            //#if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            //#define FRAG_INPUTS_ENABLE_STRIPPING
            //#endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
                #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif

            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
                #define RAYTRACING_SHADER_GRAPH_LOW
            #endif

            #ifndef SHADER_UNLIT
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif

			#if defined(_DOUBLESIDED_ON) && !defined(ASE_NEED_CULLFACE)
			    #define ASE_NEED_CULLFACE 1
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
			#define OUTPUT_SPLIT_LIGHTING
		    #endif

            #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
            #undef  _REFRACTION_PLANE
            #undef  _REFRACTION_SPHERE
            #define _REFRACTION_THIN
            #endif

            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

            #ifndef DEBUG_DISPLAY
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
	
            CBUFFER_START( UnityPerMaterial )
			float4 _Splat5_ST;
			float4 _Control_ST;
			float4 _Splat0_ST;
			float4 _Splat1_ST;
			float4 _Splat2_ST;
			float4 _Splat3_ST;
			float4 _Control1_ST;
			float4 _Splat4_ST;
			float4 _Splat6_ST;
			float4 _Splat7_ST;
			float4 _TerrainHolesTexture_ST;
			half _NormalScale7;
			half _NormalScale6;
			float _ColorMap;
			float _SmoothnessIntensity;
			half _NormalScale5;
			float _Smoothness0;
			float _Smoothness6;
			float _Smoothness2;
			float _Smoothness3;
			float _Smoothness4;
			float _Smoothness5;
			half _NormalScale4;
			float _Smoothness1;
			half _NormalScale3;
			float _SlopeMin;
			half _NormalScale1;
			half _NormalScale0;
			float _TerrainNormalMap;
			float _SnowAmount;
			float _Smoothness7;
			float _SlopeMax;
			float _SnowSlope;
			float _NormalIntensity;
			float _SnowOffset;
			float _SnowScale;
			float _NormalMapBasedSnow;
			float _ColorMapIntensity;
			half _NormalScale2;
			float _DiffusionProfile;
			float4 _EmissionColor;
			float _AlphaCutoff;
			float _RenderQueueType;
			#ifdef _ADD_PRECOMPUTED_VELOCITY
			    float _AddPrecomputedVelocity;
			#endif
			float _StencilRef;
			float _StencilWriteMask;
			float _StencilRefDepth;
			float _StencilWriteMaskDepth;
			float _StencilRefMV;
			float _StencilWriteMaskMV;
			float _StencilRefDistortionVec;
			float _StencilWriteMaskDistortionVec;
			float _StencilWriteMaskGBuffer;
			float _StencilRefGBuffer;
			float _ZTestGBuffer;
			float _RequireSplitLighting;
			float _ReceivesSSR;
			float _SurfaceType;
			float _BlendMode;
            #ifdef SUPPORT_BLENDMODE_PRESERVE_SPECULAR_LIGHTING
			    float _EnableBlendModePreserveSpecularLighting;
            #endif
			float _SrcBlend;
			float _DstBlend;
			float _DstBlend2;
			float _AlphaSrcBlend;
			float _AlphaDstBlend;
			float _ZWrite;
			float _TransparentZWrite;
			float _CullMode;
			float _TransparentSortPriority;
			float _EnableFogOnTransparent;
			float _CullModeForward;
			float _TransparentCullMode;
			float _ZTestDepthEqualForOpaque;
			float _ZTestTransparent;
			float _TransparentBackfaceEnable;
			float _AlphaCutoffEnable;
			float _UseShadowThreshold;
			float _DoubleSidedEnable;
			float _DoubleSidedNormalMode;
			float4 _DoubleSidedConstants;
			#ifdef ASE_TESSELLATION
			    float _TessPhongStrength;
			    float _TessValue;
			    float _TessMin;
			    float _TessMax;
			    float _TessEdgeLength;
			    float _TessMaxDisp;
			#endif
			UNITY_TEXTURE_STREAMING_DEBUG_VARS;
			CBUFFER_END

            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif

            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif

			TEXTURE2D(_TerrainHolesTexture);
			SAMPLER(sampler_TerrainHolesTexture);
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				TEXTURE2D(_TerrainHeightmapTexture);//ASE Terrain Instancing
				TEXTURE2D( _TerrainNormalmapTexture);//ASE Terrain Instancing
				SAMPLER(sampler_TerrainNormalmapTexture);//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
				UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
			UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
			CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
				#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
					float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
					float4 _TerrainHeightmapScale;//ASE Terrain Instancing
				#endif//ASE Terrain Instancing
			CBUFFER_END//ASE Terrain Instancing


            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap
			#pragma shader_feature_local _TERRAIN_8_LAYERS
			#pragma shader_feature_local _NORMALMAP
			#pragma shader_feature_local _MASKMAP


			struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 uv1 : TEXCOORD1;
				float4 uv2 : TEXCOORD2;
				float3 previousPositionOS : TEXCOORD4;
				float3 precomputedVelocity : TEXCOORD5;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_Position;
				float3 positionRWS : TEXCOORD0;
				float3 normalWS : TEXCOORD1;
				float4 tangentWS : TEXCOORD2;
				float4 uv1 : TEXCOORD3;
				float4 uv2 : TEXCOORD4;
				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					float3 vpassPositionCS : TEXCOORD5;
					float3 vpassPreviousPositionCS : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				#if defined(SHADER_STAGE_FRAGMENT) && defined(ASE_NEED_CULLFACE)
				FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
				#endif
			};

			AttributesMesh ApplyMeshModification( AttributesMesh inputMesh )
			{
			#ifdef UNITY_INSTANCING_ENABLED
				float2 patchVertex = inputMesh.positionOS.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP( Terrain, _TerrainPatchInstanceData );
				float2 sampleCoords = ( patchVertex.xy + instanceData.xy ) * instanceData.z;
				float height = UnpackHeightmap( _TerrainHeightmapTexture.Load( int3( sampleCoords, 0 ) ) );
				inputMesh.positionOS.xz = sampleCoords* _TerrainHeightmapScale.xz;
				inputMesh.positionOS.y = height* _TerrainHeightmapScale.y;
				#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
					inputMesh.normalOS = float3(0, 1, 0);
				#else
					inputMesh.normalOS = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb* 2 - 1;
				#endif
				inputMesh.ase_texcoord.xy = sampleCoords* _TerrainHeightmapRecipSize.zw;
			#endif
				return inputMesh;
			}
			

			// Get Surface And BuiltinData
			void GetSurfaceAndBuiltinData(PickingSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
			{
				#ifdef LOD_FADE_CROSSFADE
                    LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
				#endif

                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif  
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants);

                #ifdef DEBUG_DISPLAY
                if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
                {
                    surfaceDescription.Alpha = 1.0f;
                }
                #endif

				#ifdef _ALPHATEST_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
				#endif

				#ifdef _ALPHATEST_SHADOW_ON
                    DoAlphaTest( surfaceDescription.Alpha, surfaceDescription.AlphaClipThresholdShadow);
				#endif

				#ifdef _DEPTHOFFSET_ON
                    ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
				#endif

				float3 bentNormalWS;
                //BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
                InitBuiltinData(posInput, surfaceDescription.Alpha, bentNormalWS, -fragInputs.tangentToWorld[2], fragInputs.texCoord1, fragInputs.texCoord2, builtinData);

				#ifdef _DEPTHOFFSET_ON
                    builtinData.depthOffset = surfaceDescription.DepthOffset;
				#endif

                #ifdef _ALPHATEST_ON
                    builtinData.alphaClipTreshold = surfaceDescription.AlphaClipThreshold;
                #endif

                #ifdef UNITY_VIRTUAL_TEXTURING
                    builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif

				#ifdef ASE_BAKEDGI
                    builtinData.bakeDiffuseLighting = surfaceDescription.BakedGI;
				#endif

				#ifdef ASE_BAKEDBACKGI
                    builtinData.backBakeDiffuseLighting = surfaceDescription.BakedBackGI;
				#endif

                builtinData.emissiveColor = surfaceDescription.Emission;

				PostInitBuiltinData(V, posInput, surfaceData, builtinData);

            }

			AttributesMesh ApplyMeshModification(AttributesMesh inputMesh, float3 timeParameters, inout PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS )
			{
				_TimeParameters.xyz = timeParameters;
				inputMesh = ApplyMeshModification(inputMesh);
				float4 appendResult993_g55 = (float4(cross( inputMesh.normalOS , float3(0,0,1) ) , -1.0));
				
				outputPackedVaryingsMeshToPS.ase_texcoord7.xy = inputMesh.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				outputPackedVaryingsMeshToPS.ase_texcoord7.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				float3 defaultVertexValue = inputMesh.positionOS.xyz;
				#else
				float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue =   defaultVertexValue ;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif
				inputMesh.normalOS = inputMesh.normalOS;
				inputMesh.tangentOS = appendResult993_g55;
				return inputMesh;
			}

			PackedVaryingsMeshToPS VertexFunction(AttributesMesh inputMesh)
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS = (PackedVaryingsMeshToPS)0;
				AttributesMesh defaultMesh = inputMesh;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( outputPackedVaryingsMeshToPS );

				inputMesh = ApplyMeshModification( inputMesh, _TimeParameters.xyz, outputPackedVaryingsMeshToPS);

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
				float4 VPASSpreviousPositionCS;
				float4 VPASSpositionCS = mul(UNITY_MATRIX_UNJITTERED_VP, float4(positionRWS, 1.0));

				bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
				if (forceNoMotion)
				{
					VPASSpreviousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
				}
				else
				{
					bool hasDeformation = unity_MotionVectorsParams.x > 0.0;
					float3 effectivePositionOS = (hasDeformation ? inputMesh.previousPositionOS : defaultMesh.positionOS);
					#if defined(_ADD_PRECOMPUTED_VELOCITY)
					effectivePositionOS -= inputMesh.precomputedVelocity;
					#endif

					#if defined(HAVE_MESH_MODIFICATION)
						AttributesMesh previousMesh = defaultMesh;
						previousMesh.positionOS = effectivePositionOS ;
						PackedVaryingsMeshToPS test = (PackedVaryingsMeshToPS)0;
						float3 curTime = _TimeParameters.xyz;
						previousMesh = ApplyMeshModification(previousMesh, _LastTimeParameters.xyz, test);
						_TimeParameters.xyz = curTime;
						float3 previousPositionRWS = TransformPreviousObjectToWorld(previousMesh.positionOS);
					#else
						float3 previousPositionRWS = TransformPreviousObjectToWorld(effectivePositionOS);
					#endif

					#ifdef ATTRIBUTES_NEED_NORMAL
						float3 normalWS = TransformPreviousObjectToWorldNormal(defaultMesh.normalOS);
					#else
						float3 normalWS = float3(0.0, 0.0, 0.0);
					#endif

					#if defined(HAVE_VERTEX_MODIFICATION)
						ApplyVertexModification(inputMesh, normalWS, previousPositionRWS, _LastTimeParameters.xyz);
					#endif

					VPASSpreviousPositionCS = mul(UNITY_MATRIX_PREV_VP, float4(previousPositionRWS, 1.0));
				}
				#endif

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.positionRWS.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.normalWS.xyz = normalWS;
				outputPackedVaryingsMeshToPS.tangentWS.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.uv1.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.uv2.xyzw = inputMesh.uv2;

				#ifdef _WRITE_TRANSPARENT_MOTION_VECTOR
					outputPackedVaryingsMeshToPS.vpassPositionCS = float3(VPASSpositionCS.xyw);
					outputPackedVaryingsMeshToPS.vpassPreviousPositionCS = float3(VPASSpreviousPositionCS.xyw);
				#endif
				return outputPackedVaryingsMeshToPS;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float3 positionOS : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl Vert ( AttributesMesh v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.positionOS = v.positionOS;
				o.normalOS = v.normalOS;
				o.tangentOS = v.tangentOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
				float3 cameraPos = 0;
				#else
				float3 cameraPos = _WorldSpaceCameraPos;
				#endif
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), cameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, GetObjectToWorldMatrix(), cameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(float4(v[0].positionOS,1), float4(v[1].positionOS,1), float4(v[2].positionOS,1), edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), cameraPos, _ScreenParams, _FrustumPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			PackedVaryingsMeshToPS DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				AttributesMesh o = (AttributesMesh) 0;
				o.positionOS = patch[0].positionOS * bary.x + patch[1].positionOS * bary.y + patch[2].positionOS * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.tangentOS = patch[0].tangentOS * bary.x + patch[1].tangentOS * bary.y + patch[2].tangentOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].positionOS.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			PackedVaryingsMeshToPS Vert ( AttributesMesh v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(WRITE_NORMAL_BUFFER) && defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target2
			#elif defined(WRITE_NORMAL_BUFFER) || defined(WRITE_MSAA_DEPTH)
			#define SV_TARGET_DECAL SV_Target1
			#else
			#define SV_TARGET_DECAL SV_Target0
			#endif

			void Frag( PackedVaryingsMeshToPS packedInput
						#if defined(SCENESELECTIONPASS) || defined(SCENEPICKINGPASS)
						, out float4 outColor : SV_Target0
						#else
							#ifdef WRITE_MSAA_DEPTH
							, out float4 depthColor : SV_Target0
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target1
								#endif
							#else
								#ifdef WRITE_NORMAL_BUFFER
								, out float4 outNormalBuffer : SV_Target0
								#endif
							#endif

							#if (defined(WRITE_DECAL_BUFFER) && !defined(_DISABLE_DECALS)) || defined(WRITE_RENDERING_LAYER)
							, out float4 outDecalBuffer : SV_TARGET_DECAL
							#endif
						#endif

						#if defined(_DEPTHOFFSET_ON) && !defined(SCENEPICKINGPASS)
						, out float outputDepth : DEPTH_OFFSET_SEMANTIC
						#endif
						
					)
			{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
			UNITY_SETUP_INSTANCE_ID(packedInput);

				float3 positionRWS = packedInput.positionRWS.xyz;

				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);

				input.tangentToWorld = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				input.positionRWS = positionRWS;

				#if _DOUBLESIDED_ON && SHADER_STAGE_FRAGMENT
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false);
				#elif SHADER_STAGE_FRAGMENT
				#if defined(ASE_NEED_CULLFACE)
				input.isFrontFace = IS_FRONT_VFACE( packedInput.cullFace, true, false );
				#endif
				#endif
				half isFrontFace = input.isFrontFace;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				PickingSurfaceDescription surfaceDescription = (PickingSurfaceDescription)0;
				float2 uv_TerrainHolesTexture = packedInput.ase_texcoord7.xy * _TerrainHolesTexture_ST.xy + _TerrainHolesTexture_ST.zw;
				float4 tex2DNode451_g55 = SAMPLE_TEXTURE2D( _TerrainHolesTexture, sampler_TerrainHolesTexture, uv_TerrainHolesTexture );
				
				surfaceDescription.Alpha = 0.5;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold =  ( 1.0 - tex2DNode451_g55 ).r;
				#endif

				outColor = _SelectionID;
			}

            ENDHLSL
		}

        Pass
        {

            Name "FullScreenDebug"
            Tags 
			{ 
				"LightMode" = "FullScreenDebug" 
            }

            Cull [_CullMode]
			ZTest LEqual
			ZWrite Off

            HLSLPROGRAM
            /*ase_pragma_before*/
            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex Vert
			#pragma fragment Frag

            #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
            #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC _TRANSPARENT_REFRACTIVE_SORT
            #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT

			#define SHADERPASS SHADERPASS_FULL_SCREEN_DEBUG

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"

			#ifndef SHADER_UNLIT
			#if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
			#define VARYINGS_NEED_CULLFACE
			#endif
			#endif

            #if _MATERIAL_FEATURE_COLORED_TRANSMISSION
            #undef _MATERIAL_FEATURE_CLEAR_COAT
            #endif

		    #if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
		    #define OUTPUT_SPLIT_LIGHTING
		    #endif

            #if (SHADERPASS == SHADERPASS_PATH_TRACING) && !defined(_DOUBLESIDED_ON) && (defined(_REFRACTION_PLANE) || defined(_REFRACTION_SPHERE))
            #undef  _REFRACTION_PLANE
            #undef  _REFRACTION_SPHERE
            #define _REFRACTION_THIN
            #endif

			#if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
			#if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
				#define WRITE_NORMAL_BUFFER
			#endif
			#endif

            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif

			#ifndef DEBUG_DISPLAY
				#if !defined(_SURFACE_TYPE_TRANSPARENT)
					#if SHADERPASS == SHADERPASS_FORWARD
					#define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
					#elif SHADERPASS == SHADERPASS_GBUFFER
					#define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
					#endif
				#endif
			#endif

            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            #if (defined(_TRANSPARENT_WRITES_MOTION_VEC) || defined(_TRANSPARENT_REFRACTIVE_SORT)) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif

            #ifdef DEBUG_DISPLAY
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
            #endif

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"

            struct AttributesMesh
			{
				float3 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 tangentOS : TANGENT;
				#if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
					uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};

			struct VaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
					uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

			struct VertexDescriptionInputs
			{
				 float3 ObjectSpaceNormal;
				 float3 ObjectSpaceTangent;
				 float3 ObjectSpacePosition;
			};

			struct SurfaceDescriptionInputs
			{
				 float3 TangentSpaceNormal;
			};

			struct PackedVaryingsMeshToPS
			{
				SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
				#if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
					uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};

            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
			{
				PackedVaryingsMeshToPS output;
				ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

			VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
			{
				VaryingsMeshToPS output;
				output.positionCS = input.positionCS;
				#if UNITY_ANY_INSTANCING_ENABLED || defined(ATTRIBUTES_NEED_INSTANCEID)
				output.instanceID = input.instanceID;
				#endif
				return output;
			}

            struct VertexDescription
			{
				float3 Position;
				float3 Normal;
				float3 Tangent;
			};

			VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
			{
				VertexDescription description = (VertexDescription)0;
				description.Position = IN.ObjectSpacePosition;
				description.Normal = IN.ObjectSpaceNormal;
				description.Tangent = IN.ObjectSpaceTangent;
				return description;
			}

            struct SurfaceDescription
			{
				float3 BaseColor;
				float3 Emission;
				float Alpha;
				float3 BentNormal;
				float Smoothness;
				float Occlusion;
				float3 NormalTS;
				float Metallic;
			};

			SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
			{
				SurfaceDescription surface = (SurfaceDescription)0;
				surface.BaseColor = IsGammaSpace() ? float3(0.5, 0.5, 0.5) : SRGBToLinear(float3(0.5, 0.5, 0.5));
				surface.Emission = float3(0, 0, 0);
				surface.Alpha = 1;
				surface.BentNormal = IN.TangentSpaceNormal;
				surface.Smoothness = 0.5;
				surface.Occlusion = 1;
				surface.NormalTS = IN.TangentSpaceNormal;
				surface.Metallic = 0;
				return surface;
			}

			VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
			{
				VertexDescriptionInputs output;
				ZERO_INITIALIZE(VertexDescriptionInputs, output);

				output.ObjectSpaceNormal =                          input.normalOS;
				output.ObjectSpaceTangent =                         input.tangentOS.xyz;
				output.ObjectSpacePosition =                        input.positionOS;
				#if UNITY_ANY_INSTANCING_ENABLED
                #else
                #endif

				return output;
			}

			AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters  )
			{
				VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);

				VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);

				input.positionOS = vertexDescription.Position;
				input.normalOS = vertexDescription.Normal;
				input.tangentOS.xyz = vertexDescription.Tangent;
				return input;
			}

			FragInputs BuildFragInputs(VaryingsMeshToPS input)
			{
				FragInputs output;
				ZERO_INITIALIZE(FragInputs, output);

				output.tangentToWorld = k_identity3x3;
				output.positionSS = input.positionCS; // input.positionCS is SV_Position
				#if UNITY_ANY_INSTANCING_ENABLED
                #else
                #endif

				return output;
			}

			FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
			{
				UNITY_SETUP_INSTANCE_ID(input);
				VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
				return BuildFragInputs(unpacked);
			}

			#define DEBUG_DISPLAY
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/FullScreenDebug.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"

			PackedVaryingsType Vert(AttributesMesh inputMesh)
			{
				VaryingsType varyingsType;
				varyingsType.vmesh = VertMesh(inputMesh);
				return PackVaryingsType(varyingsType);
			}

			#if !defined(_DEPTHOFFSET_ON)
			[earlydepthstencil]
			#endif
			void Frag(PackedVaryingsToPS packedInput)
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
				FragInputs input = UnpackVaryingsToFragInputs(packedInput);

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz);

			#ifdef PLATFORM_SUPPORTS_PRIMITIVE_ID_IN_PIXEL_SHADER
				if (_DebugFullScreenMode == FULLSCREENDEBUGMODE_QUAD_OVERDRAW)
				{
					IncrementQuadOverdrawCounter(posInput.positionSS.xy, input.primitiveID);
				}
			#endif
			}
            ENDHLSL
        }
		
	}
	
	CustomEditor "Rendering.HighDefinition.LitShaderGraphGUI"
	
	Dependency "BaseMapShader"="Toby Fredson/The Toby Foliage Engine/(TTFE) Terrain Snow BasePass"
	Dependency "BaseMapShader"="Toby Fredson/The Toby Foliage Engine/(TTFE) Terrain Snow BasePass"

	Fallback "Off"
}
/*ASEBEGIN
Version=19501
Node;AmplifyShaderEditor.CommentaryNode;1365;-1952,144;Inherit;False;3703.237;2599.825;Settings for season control, specular, and others.;1;1366;Mesh Settings;0,1,0.1098039,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1366;-1856,320;Inherit;False;3531.514;2339.1;;8;1453;1379;1372;1371;1369;1368;1367;1464;Snow Control + Output;0.7490196,1,0,1;0;0
Node;AmplifyShaderEditor.FunctionNode;1483;1296,-832;Inherit;False;(TTFE) Terrain 8 Layer;0;;55;080b7ce91c3d3bc4a92eb5f3ac4e69b6;6,504,1,102,1,976,1,978,1,668,0,669,0;0;9;FLOAT3;0;FLOAT3;14;FLOAT;56;FLOAT;45;FLOAT;200;FLOAT;975;COLOR;979;FLOAT3;998;FLOAT4;999
Node;AmplifyShaderEditor.CommentaryNode;1464;592,2256;Inherit;False;649;242;Comment;4;1466;1459;1461;1460;Ao_Final;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1364;-1904,-816;Inherit;False;1386.182;442.4717;;9;1417;1412;1408;1407;1406;1405;1404;1403;1402;Slope Function;0.5471698,0.2180016,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1367;-1760,448;Inherit;False;2036;1379;;26;1454;1450;1446;1444;1443;1441;1440;1439;1438;1435;1434;1433;1432;1431;1428;1427;1426;1425;1423;1422;1421;1420;1419;1416;1415;1411;Main Maps;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1368;-1472,1952;Inherit;False;580;411;Keeps the same UV scale no matter the object scaling.;5;1418;1414;1413;1410;1409;Scale Independent Position;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1369;-848,2032;Inherit;False;1332;544;Create snow noise and convert height to normals.;8;1452;1445;1437;1436;1430;1429;1424;1456;Noise Generator;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1371;352,1520;Inherit;False;1220;308;;8;1449;1393;1390;1386;1385;1383;1382;1378;Normal_Final;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1372;336,912;Inherit;False;1268;472.3812;;8;1451;1448;1447;1392;1389;1384;1381;1377;Albedo_Final;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1379;592,1904;Inherit;False;672.8528;210.2324;;3;1391;1394;1387;Smoothness_Final;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;1471;1696,-512;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;1473;1680,-432;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;1475;1648,-384;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WireNode;1488;1712,-464;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;1378;608,1664;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;1382;608,1568;Inherit;False;1450;NormalMap_Top;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;1383;784,1744;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BlendNormalsNode;1385;832,1632;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;1386;1008,1744;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;1387;640,1952;Inherit;False;1380;SmoothOut;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1388;752,2160;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;1390;1056,1664;Inherit;False;Property;_TerrainNormalMap;Terrain Normal Map;97;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1393;1312,1664;Inherit;False;NormalSnow_Output;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;1402;-1856,-560;Inherit;False;Constant;_Tangent;Tangent;33;0;Create;True;0;0;0;False;0;False;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TransformDirectionNode;1403;-1664,-688;Inherit;False;Tangent;World;True;Fast;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;1404;-1408,-592;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;1405;-1264,-576;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1406;-1280,-672;Inherit;False;Property;_SlopeMin;Slope Min;100;0;Create;True;0;0;0;False;0;False;0;0.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1407;-1296,-752;Inherit;False;Property;_SlopeMax;Slope Max;99;1;[Header];Create;True;1;(Slope);0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;1408;-1104,-640;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectScaleNode;1409;-1424,2000;Inherit;False;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;1410;-1424,2176;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;1411;-1712,960;Inherit;False;Property;_NormalIntensity;Normal Intensity;96;1;[Header];Create;True;1;(Normal);0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;1412;-928,-544;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;1413;-1232,2032;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;1414;-1232,2208;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1415;-1392,1120;Inherit;True;Property;_HeatMap;Heat Map;91;2;[Header];[NoScaleOffset];Create;True;3;______________(TTFE) TERRAIN SNOW________________;_____________________________________________________;Texture Maps;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SamplerNode;1416;-1408,912;Inherit;True;Property;_NormalMap;Normal Map;92;2;[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;-1;None;b1a639f135b924640960ac71591f2edb;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RegisterLocalVarNode;1417;-752,-544;Inherit;False;Slope;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1418;-1072,2112;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;1419;-1200,1456;Inherit;False;Property;_SnowOffset;Snow Offset;104;0;Create;True;0;0;0;False;0;False;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;1420;-1104,1216;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1421;-976,1056;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1422;-1200,1360;Inherit;False;Property;_SnowScale;Snow Scale;103;0;Create;True;0;0;0;False;0;False;0;7.61;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;1423;-1200,1552;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NoiseGeneratorNode;1424;-800,2080;Inherit;True;Simple;True;False;2;0;FLOAT2;1,1;False;1;FLOAT;400;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;1425;-912,1280;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;3.33;False;2;FLOAT;-1.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;1426;-912,1072;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-4.39;False;2;FLOAT;1.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;1427;-1008,1600;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1428;-1200,1712;Inherit;False;1417;Slope;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1429;-800,2320;Inherit;True;Simple;True;False;2;0;FLOAT2;1,1;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;1430;-560,2208;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.6;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;1431;-928,1440;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;3.33;False;2;FLOAT;-1.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1432;-656,1184;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1433;-848,1680;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1434;-1408,688;Inherit;True;Property;_ColorMapProjection;Color Map Projection;93;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;64654d43d9f0bd94bb10a3d2561ae944;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.FunctionNode;1436;-304,2208;Inherit;False;Normal From Height;-1;;57;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;0.06;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;1437;-304,2336;Inherit;False;Normal From Height;-1;;58;1942fe2c5f1a1f94881a33d532e4afeb;0;2;20;FLOAT;0;False;110;FLOAT;8;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;1438;-432,1440;Inherit;False;Property;_NormalMapBasedSnow;Normal Map Based Snow;105;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1439;-1104,608;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;1440;-688,1600;Inherit;False;Property;_SnowSlope;Snow Slope;101;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1441;-448,1648;Inherit;False;Property;_SnowAmount;Snow Amount;102;1;[Header];Create;True;3;_____________________________________________________;Season Settings;(Snow Control);0;0;False;0;False;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1443;-944,608;Inherit;False;ColorMap_Top;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1444;-144,1520;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1445;208,2272;Inherit;False;Normal Reconstruct Z;-1;;59;63ba85b764ae0c84ab3d698b86364ae9;0;1;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;1446;96,1488;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1449;416,1664;Inherit;False;1374;NormalOut;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1450;-1088,912;Inherit;False;NormalMap_Top;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;1452;-560,2080;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1.4;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1453;272,1920;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1454;-160,1488;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1373;1728,-832;Inherit;False;BaseColorOut;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1374;1728,-736;Inherit;False;NormalOut;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1376;1552,-960;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1395;1968,-832;Inherit;False;1393;NormalSnow_Output;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;1398;1968,-736;Inherit;False;1394;Smoothness_Output;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1399;1968,-928;Inherit;False;1392;AlbedoSnow_Output;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendNormalsNode;1456;-16,2256;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;1391;832,1968;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1462;288,2368;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1377;608,976;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;1381;768,1024;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;1384;944,1120;Inherit;False;Property;_ColorMap;Color Map;95;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;1389;1168,1248;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1392;1328,1248;Inherit;False;AlbedoSnow_Output;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;1447;384,1040;Inherit;False;1373;BaseColorOut;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;1448;384,960;Inherit;False;1443;ColorMap_Top;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1451;384,1120;Inherit;False;Property;_ColorMapIntensity;Color Map Intensity;94;1;[Header];Create;True;3;_____________________________________________________;Texture Settings;(Color Map);0;0;False;0;False;1;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1455;384,1392;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;1460;848,2320;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1461;656,2384;Inherit;False;Constant;_Float11;Float 11;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1459;640,2304;Inherit;False;1457;AoOut;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1394;992,1968;Inherit;False;Smoothness_Output;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1466;1024,2320;Inherit;False;Ao_Output;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1458;1984,-624;Inherit;False;1466;Ao_Output;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1457;1728,-624;Inherit;False;AoOut;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;1472;2208,-528;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;1474;2240,-464;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;1476;2288,-432;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;1375;1264,-960;Inherit;False;Property;_SmoothnessIntensity;Smoothness Intensity;98;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1380;1728,-928;Inherit;False;SmoothOut;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorSpaceDouble;1435;-1344,496;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;1489;2224,-480;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DiffusionProfileNode;1491;1888,-528;Float;False;Property;_DiffusionProfile;Diffusion Profile;90;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1348;2297.334,-957.9177;Float;False;True;-1;2;Rendering.HighDefinition.LitShaderGraphGUI;0;12;Toby Fredson/The Toby Foliage Engine/(TTFE) Terrain Snow;53b46d85872c5b24c8f4f0a1c3fe4c87;True;GBuffer;0;0;GBuffer;34;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;10;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;TerrainCompatible=True;IgnoreProjector=True;SplatCount=8;MaskMapR=Metallic;MaskMapG=AO;MaskMapB=Height;MaskMapA=Smoothness;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefGBuffer;255;False;;255;True;_StencilWriteMaskGBuffer;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;False;True;0;True;_ZTestGBuffer;False;True;8;LightMode=GBuffer;TerrainCompatible=True;IgnoreProjector=True;SplatCount=8;MaskMapR=Metallic;MaskMapG=AO;MaskMapB=Height;MaskMapA=Smoothness;False;False;5;Include;;False;;Native;False;0;0;;Pragma;instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap;False;;Custom;False;0;0;;Pragma;shader_feature_local _TERRAIN_8_LAYERS;False;;Custom;False;0;0;;Pragma;shader_feature_local _NORMALMAP;False;;Custom;False;0;0;;Pragma;shader_feature_local _MASKMAP;False;;Custom;False;0;0;;Off;2;BaseMapShader=Toby Fredson/The Toby Foliage Engine/(TTFE) Terrain Snow BasePass;BaseMapShader=Toby Fredson/The Toby Foliage Engine/(TTFE) Terrain Snow BasePass;0;Standard;38;Surface Type;0;638780661950717292;  Rendering Pass;1;0;  Refraction Model;0;0;    Blending Mode;0;0;    Blend Preserves Specular;1;0;  Back Then Front Rendering;0;0;  Transparent Depth Prepass;0;0;  Transparent Depth Postpass;0;0;  ZWrite;0;0;  Z Test;4;0;Double-Sided;0;638628564781829453;Alpha Clipping;1;638175450059869116;  Use Shadow Threshold;0;0;Material Type,InvertActionOnDeselection;1;638780662126156507;  Energy Conserving Specular;1;0;  Transmission,InvertActionOnDeselection;0;0;Receive Decals;1;0;Receive SSR;1;0;Receive SSR Transparent;0;0;Motion Vectors;1;0;  Add Precomputed Velocity;1;638175450296518292;Specular AA;0;638176074834876762;Specular Occlusion Mode;2;638780661043913298;Override Baked GI;0;0;Depth Offset;0;0;  Conserative;1;0;GPU Instancing;0;638175450488502741;LOD CrossFade;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,True,_TessellationPhong;0;  Type;0;0;  Tess;16,True,_TessellationStrength;0;  Min;10,True,_TessellationDistanceMin;0;  Max;25,True,_TessellationDistanceMax;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position;1;0;0;11;True;True;True;True;True;True;False;False;False;True;True;True;;True;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1349;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;META;0;1;META;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1350;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1351;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;SceneSelectionPass;0;3;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1352;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;DepthOnly;0;4;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1354;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentBackface;0;6;TransparentBackface;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;True;1;False;;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelOne;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelTwo;False;False;False;False;False;True;0;True;_ZWrite;True;0;True;_ZTestTransparent;False;True;1;LightMode=TransparentBackface;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1355;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPrepass;0;7;TransparentDepthPrepass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefDepth;255;False;;255;True;_StencilWriteMaskDepth;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=TransparentDepthPrepass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1356;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;TransparentDepthPostpass;0;8;TransparentDepthPostpass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=TransparentDepthPostpass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1357;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;Forward;0;9;Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;True;2;5;False;;10;False;;0;1;False;;0;False;;False;False;True;1;1;False;;0;True;_DstBlend2;0;1;False;;0;False;;False;False;True;1;1;False;;0;True;_DstBlend2;0;1;False;;0;False;;False;False;False;True;0;True;_CullModeForward;False;False;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelOne;False;True;True;True;True;True;0;True;_ColorMaskTransparentVelTwo;False;False;False;True;True;0;True;_StencilRef;255;False;;255;True;_StencilWriteMask;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;0;True;_ZWrite;True;0;True;_ZTestDepthEqualForOpaque;False;True;1;LightMode=Forward;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1358;2297.334,-957.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;ScenePickingPass;0;10;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;True;3;False;;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1361;2297.334,-571.9177;Float;False;False;-1;2;Rendering.HighDefinition.LightingShaderGraphGUI;0;1;New Amplify Shader;53b46d85872c5b24c8f4f0a1c3fe4c87;True;MotionVectors;0;5;MotionVectors;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;True;7;d3d11;metal;vulkan;xboxone;xboxseries;playstation;switch;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;_CullMode;False;False;False;False;False;False;False;False;False;True;True;0;True;_StencilRefMV;255;False;;255;True;_StencilWriteMaskMV;7;False;;3;False;;0;False;;0;False;;7;False;;3;False;;0;False;;0;False;;False;True;1;False;;False;False;True;1;LightMode=MotionVectors;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.CommentaryNode;1400;-1920,-176;Inherit;False;3647.034;100;;0;MESH SETTINGS;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1401;-1904,-1104;Inherit;False;1382.403;100;;0;GLOBAL FUNCTIONS;1,1,1,1;0;0
WireConnection;1471;0;1483;979
WireConnection;1473;0;1483;998
WireConnection;1475;0;1483;999
WireConnection;1488;0;1483;975
WireConnection;1378;0;1449;0
WireConnection;1378;1;1445;0
WireConnection;1378;2;1446;0
WireConnection;1383;0;1378;0
WireConnection;1385;0;1382;0
WireConnection;1385;1;1378;0
WireConnection;1386;0;1383;0
WireConnection;1388;0;1452;0
WireConnection;1390;0;1386;0
WireConnection;1390;1;1385;0
WireConnection;1393;0;1390;0
WireConnection;1403;0;1402;0
WireConnection;1404;0;1403;0
WireConnection;1404;1;1402;0
WireConnection;1405;0;1404;0
WireConnection;1408;0;1407;0
WireConnection;1408;1;1406;0
WireConnection;1408;2;1405;0
WireConnection;1412;0;1408;0
WireConnection;1413;0;1409;1
WireConnection;1413;1;1409;3
WireConnection;1414;0;1410;1
WireConnection;1414;1;1410;3
WireConnection;1416;5;1411;0
WireConnection;1417;0;1412;0
WireConnection;1418;0;1413;0
WireConnection;1418;1;1414;0
WireConnection;1420;0;1415;3
WireConnection;1421;0;1416;2
WireConnection;1424;0;1418;0
WireConnection;1425;0;1421;0
WireConnection;1425;1;1422;0
WireConnection;1425;2;1419;0
WireConnection;1426;0;1420;0
WireConnection;1427;0;1423;2
WireConnection;1429;0;1418;0
WireConnection;1430;0;1424;0
WireConnection;1431;0;1420;0
WireConnection;1431;1;1422;0
WireConnection;1431;2;1419;0
WireConnection;1432;0;1426;0
WireConnection;1432;1;1425;0
WireConnection;1433;0;1427;0
WireConnection;1433;1;1428;0
WireConnection;1436;20;1430;0
WireConnection;1437;20;1429;0
WireConnection;1438;0;1431;0
WireConnection;1438;1;1432;0
WireConnection;1439;0;1435;0
WireConnection;1439;1;1434;0
WireConnection;1440;0;1427;0
WireConnection;1440;1;1433;0
WireConnection;1443;0;1439;0
WireConnection;1444;0;1438;0
WireConnection;1444;1;1440;0
WireConnection;1444;2;1441;0
WireConnection;1445;1;1456;0
WireConnection;1446;0;1444;0
WireConnection;1450;0;1416;0
WireConnection;1452;0;1424;0
WireConnection;1453;0;1446;0
WireConnection;1454;0;1430;0
WireConnection;1373;0;1483;0
WireConnection;1374;0;1483;14
WireConnection;1376;0;1375;0
WireConnection;1376;1;1483;45
WireConnection;1456;0;1437;40
WireConnection;1456;1;1436;40
WireConnection;1391;0;1387;0
WireConnection;1391;1;1388;0
WireConnection;1391;2;1453;0
WireConnection;1462;0;1446;0
WireConnection;1377;0;1448;0
WireConnection;1377;1;1447;0
WireConnection;1381;0;1447;0
WireConnection;1381;1;1377;0
WireConnection;1381;2;1451;0
WireConnection;1384;0;1447;0
WireConnection;1384;1;1381;0
WireConnection;1389;0;1384;0
WireConnection;1389;1;1454;0
WireConnection;1389;2;1455;0
WireConnection;1392;0;1389;0
WireConnection;1455;0;1446;0
WireConnection;1460;0;1459;0
WireConnection;1460;1;1461;0
WireConnection;1460;2;1462;0
WireConnection;1394;0;1391;0
WireConnection;1466;0;1460;0
WireConnection;1457;0;1483;200
WireConnection;1472;0;1471;0
WireConnection;1474;0;1473;0
WireConnection;1476;0;1475;0
WireConnection;1380;0;1376;0
WireConnection;1489;0;1488;0
WireConnection;1348;0;1399;0
WireConnection;1348;1;1395;0
WireConnection;1348;7;1398;0
WireConnection;1348;8;1458;0
WireConnection;1348;9;1489;0
WireConnection;1348;10;1472;0
WireConnection;1348;62;1491;0
WireConnection;1348;12;1474;0
WireConnection;1348;27;1476;0
ASEEND*/
//CHKSM=BA24DE18394FE44E9CCB73CFD916D2480E2471D5