// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "E3D/War3R-Ground/Cliff-2L-Spm"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		[HDR]_MainColor("MainColor", Color) = (1,1,1,1)
		_BumpsMap("BumpMap", 2D) = "bump" {}
		_SpecularMap("SpecularMap", 2D) = "white" {}
		_Smooth1Instensity("Smooth1-Instensity", Range( 0 , 1)) = 0.5
		_Metallic1Instensity("Metallic1-Instensity", Range( 0 , 1)) = 1
		_MainTex2("MainTex2", 2D) = "white" {}
		_MainColor2("MainColor2", Color) = (1,1,1,0)
		_BumpsMap1("BumpMap2", 2D) = "bump" {}
		_SpecularMap2("SpecularMap2", 2D) = "white" {}
		_Smooth2Instensity("Smooth2-Instensity", Range( 0 , 1)) = 0.5
		_Metallic2Instensity("Metallic2-Instensity", Range( 0 , 1)) = 1
		[Toggle(_ONLYNORMAL_ON)] _OnlyNormal("OnlyNormal", Float) = 0
		[Toggle(_NONENORMAL_ON)] _NoneNormal("NoneNormal", Float) = 0
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _ONLYNORMAL_ON
		#pragma shader_feature _NONENORMAL_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float2 uv2_texcoord2;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _MainTex2;
		uniform float4 _MainTex2_ST;
		uniform float4 _MainColor2;
		uniform float4 _MainColor;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform sampler2D _BumpsMap1;
		uniform float4 _BumpsMap1_ST;
		uniform sampler2D _BumpsMap;
		uniform float4 _BumpsMap_ST;
		uniform float _Metallic2Instensity;
		uniform sampler2D _SpecularMap2;
		uniform float4 _SpecularMap2_ST;
		uniform float _Metallic1Instensity;
		uniform sampler2D _SpecularMap;
		uniform float4 _SpecularMap_ST;
		uniform float _Smooth2Instensity;
		uniform float _Smooth1Instensity;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			SurfaceOutputStandard s2 = (SurfaceOutputStandard ) 0;
			float2 uv_MainTex2 = i.uv_texcoord * _MainTex2_ST.xy + _MainTex2_ST.zw;
			float2 uv2_MainTex = i.uv2_texcoord2 * _MainTex_ST.xy + _MainTex_ST.zw;
			float4 tex2DNode1 = tex2D( _MainTex, uv2_MainTex );
			float Alpha168 = tex2DNode1.a;
			float4 lerpResult223 = lerp( ( tex2D( _MainTex2, uv_MainTex2 ) * _MainColor2 ) , ( _MainColor * tex2DNode1 ) , Alpha168);
			float4 color41 = IsGammaSpace() ? float4(0.5073529,0.5073529,0.5073529,0) : float4(0.2209101,0.2209101,0.2209101,0);
			#ifdef _ONLYNORMAL_ON
				float4 staticSwitch40 = color41;
			#else
				float4 staticSwitch40 = lerpResult223;
			#endif
			float4 Albedo173 = staticSwitch40;
			s2.Albedo = Albedo173.rgb;
			float2 uv_BumpsMap1 = i.uv_texcoord * _BumpsMap1_ST.xy + _BumpsMap1_ST.zw;
			float2 uv2_BumpsMap = i.uv2_texcoord2 * _BumpsMap_ST.xy + _BumpsMap_ST.zw;
			float3 lerpResult231 = lerp( UnpackNormal( tex2D( _BumpsMap1, uv_BumpsMap1 ) ) , UnpackNormal( tex2D( _BumpsMap, uv2_BumpsMap ) ) , Alpha168);
			float4 color44 = IsGammaSpace() ? float4(0,0,1,0) : float4(0,0,1,0);
			#ifdef _NONENORMAL_ON
				float4 staticSwitch43 = color44;
			#else
				float4 staticSwitch43 = float4( lerpResult231 , 0.0 );
			#endif
			float4 Normal175 = staticSwitch43;
			s2.Normal = WorldNormalVector( i , Normal175.rgb );
			s2.Emission = float3( 0,0,0 );
			float2 uv_SpecularMap2 = i.uv_texcoord * _SpecularMap2_ST.xy + _SpecularMap2_ST.zw;
			float4 tex2DNode233 = tex2D( _SpecularMap2, uv_SpecularMap2 );
			float2 uv2_SpecularMap = i.uv2_texcoord2 * _SpecularMap_ST.xy + _SpecularMap_ST.zw;
			float4 tex2DNode71 = tex2D( _SpecularMap, uv2_SpecularMap );
			float lerpResult237 = lerp( saturate( ( _Metallic2Instensity * tex2DNode233.b ) ) , saturate( ( _Metallic1Instensity * tex2DNode71.b ) ) , Alpha168);
			float Matel177 = lerpResult237;
			s2.Metallic = Matel177;
			float lerpResult188 = lerp( ( tex2DNode233.r * _Smooth2Instensity ) , ( tex2DNode71.r * _Smooth1Instensity ) , Alpha168);
			float Smooth181 = saturate( lerpResult188 );
			s2.Smoothness = Smooth181;
			s2.Occlusion = 1.0;

			data.light = gi.light;

			UnityGI gi2 = gi;
			#ifdef UNITY_PASS_FORWARDBASE
			Unity_GlossyEnvironmentData g2 = UnityGlossyEnvironmentSetup( s2.Smoothness, data.worldViewDir, s2.Normal, float3(0,0,0));
			gi2 = UnityGlobalIllumination( data, s2.Occlusion, s2.Normal, g2 );
			#endif

			float3 surfResult2 = LightingStandard ( s2, viewDir, gi2 ).rgb;
			surfResult2 += s2.Emission;

			#ifdef UNITY_PASS_FORWARDADD//2
			surfResult2 -= s2.Emission;
			#endif//2
			float4 color206 = IsGammaSpace() ? float4(1.5,1.5,1.5,0) : float4(2.440062,2.440062,2.440062,0);
			float3 clampResult205 = clamp( surfResult2 , float3( 0,0,0 ) , color206.rgb );
			c.rgb = clampResult205;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.zw = customInputData.uv2_texcoord2;
				o.customPack1.zw = v.texcoord1;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_texcoord2 = IN.customPack1.zw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17101
153;395;1419;625;-1611.659;1773.566;1.247682;True;True
Node;AmplifyShaderEditor.CommentaryNode;229;1862.8,-2040.007;Inherit;False;1264.485;1210.846;Comment;9;225;227;228;226;1;23;171;168;223;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;141;1242.784,-31.69993;Inherit;False;2547.941;755.7902;Specular;10;181;166;188;187;158;186;156;71;233;240;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;220;2313.624,-559.8542;Inherit;False;1477.828;508.3019;Metall;9;199;204;218;203;177;234;237;239;236;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;1912.8,-1226.428;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;None;2a35763507951d34cbdce51dfbdfe73c;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;71;1239.463,98.22131;Inherit;True;Property;_SpecularMap;SpecularMap;4;0;Create;True;0;0;False;0;None;03c1430809e4b1a418c4a25e64af2ef2;True;1;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;233;1254.689,391.0211;Inherit;True;Property;_SpecularMap2;SpecularMap2;10;0;Create;True;0;0;False;0;None;2b6e142501a49de4d945f6893941436a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;204;2366.545,-505.327;Float;False;Property;_Metallic1Instensity;Metallic1-Instensity;6;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;199;2504.71,-383.267;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;186;1982.027,245.7645;Inherit;False;Property;_Smooth1Instensity;Smooth1-Instensity;5;0;Create;True;0;0;False;0;0.5;0.37;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;227;1943.561,-1990.008;Inherit;True;Property;_MainTex2;MainTex2;7;0;Create;True;0;0;False;0;None;8113dadccb7dda4439a264d580ff939e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;23;1994.641,-1395.993;Float;False;Property;_MainColor;MainColor;2;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;234;2373.241,-264.5712;Float;False;Property;_Metallic2Instensity;Metallic2-Instensity;12;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;168;2323.169,-1134.001;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;156;1981.323,513.0824;Inherit;False;Property;_Smooth2Instensity;Smooth2-Instensity;11;0;Create;True;0;0;False;0;0.5;0.445;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;236;2511.406,-142.5113;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;225;2036.899,-1805.269;Inherit;False;Property;_MainColor2;MainColor2;8;0;Create;True;0;0;False;0;1,1,1,0;0.9191176,0.8493915,0.4595588,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;191;2631.7,739.5118;Inherit;False;1183.177;899.4144;Normal;7;6;175;43;44;231;230;232;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;232;2757.568,1521.179;Inherit;False;168;Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;2319.507,408.5134;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;240;2523.237,450.7258;Inherit;False;168;Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;218;2731.974,-493.8542;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;235;2738.67,-253.0985;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;2322.892,-1374.856;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;2307.369,126.5394;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;2658.14,1274.92;Inherit;True;Property;_BumpsMap;BumpMap;3;0;Create;False;0;0;False;0;None;956ebcc51a898ea4faf552856474bb7f;True;1;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;2345.457,-1847.616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;230;2654.837,1046.53;Inherit;True;Property;_BumpsMap1;BumpMap2;9;0;Create;False;0;0;False;0;None;35bcbb2dc20ee474dbbc779051b10271;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;188;2761.807,267.2321;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;223;2862.285,-1468.814;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;231;3049.13,1157.424;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;41;3260.582,-1293.564;Float;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;0.5073529,0.5073529,0.5073529,0;0.4191176,0.4191176,0.4191176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;44;3014.685,789.5118;Float;False;Constant;_Color1;Color 1;6;0;Create;True;0;0;False;0;0,0,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;239;3008.369,-275.7704;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;203;2993.054,-489.5786;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;238;2998.185,-189.8342;Inherit;False;168;Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;43;3269.905,858.0555;Float;False;Property;_NoneNormal;NoneNormal;14;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;237;3263.996,-336.4007;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;166;2978.937,259.8857;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;40;3694.161,-1460.839;Float;False;Property;_OnlyNormal;OnlyNormal;13;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;175;3547.085,873.4778;Inherit;False;Normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;173;3967.558,-1453.843;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;181;3228.68,244.5786;Inherit;False;Smooth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;219;4095.973,-247.6125;Inherit;False;1218.238;740.8817;PBR-Combine;11;174;176;182;178;206;2;205;214;201;169;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;177;3604.452,-484.6639;Inherit;False;Matel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;182;4150.973,261.3601;Inherit;False;181;Smooth;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;174;4154.703,5.729163;Inherit;False;173;Albedo;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;178;4149.372,149.9805;Inherit;False;177;Matel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;4155.069,75.09892;Inherit;False;175;Normal;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomStandardSurface;2;4388.052,84.69734;Inherit;False;Metallic;Tangent;6;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,1;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;206;4398.88,273.3026;Inherit;False;Constant;_Color2;Color 2;14;1;[HDR];Create;True;0;0;False;0;1.5,1.5,1.5,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;205;4812.88,86.30263;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;169;4794.842,-8.546619;Inherit;False;168;Alpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;214;4835.844,238.771;Inherit;False;-1;;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;226;2002.899,-1609.269;Inherit;False;Constant;_Color4;Color 4;14;0;Create;True;0;0;False;0;0.3455882,0.1512737,0.0254109,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;201;4398.266,492.0679;Inherit;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;61;5051.211,-197.6125;Float;False;True;2;ASEMaterialInspector;0;0;CustomLighting;E3D/War3R-Ground/Cliff-2L-Spm;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;199;0;71;3
WireConnection;168;0;1;4
WireConnection;236;0;233;3
WireConnection;158;0;233;1
WireConnection;158;1;156;0
WireConnection;218;0;204;0
WireConnection;218;1;199;0
WireConnection;235;0;234;0
WireConnection;235;1;236;0
WireConnection;171;0;23;0
WireConnection;171;1;1;0
WireConnection;187;0;71;1
WireConnection;187;1;186;0
WireConnection;228;0;227;0
WireConnection;228;1;225;0
WireConnection;188;0;158;0
WireConnection;188;1;187;0
WireConnection;188;2;240;0
WireConnection;223;0;228;0
WireConnection;223;1;171;0
WireConnection;223;2;168;0
WireConnection;231;0;230;0
WireConnection;231;1;6;0
WireConnection;231;2;232;0
WireConnection;239;0;235;0
WireConnection;203;0;218;0
WireConnection;43;1;231;0
WireConnection;43;0;44;0
WireConnection;237;0;239;0
WireConnection;237;1;203;0
WireConnection;237;2;238;0
WireConnection;166;0;188;0
WireConnection;40;1;223;0
WireConnection;40;0;41;0
WireConnection;175;0;43;0
WireConnection;173;0;40;0
WireConnection;181;0;166;0
WireConnection;177;0;237;0
WireConnection;2;0;174;0
WireConnection;2;1;176;0
WireConnection;2;3;178;0
WireConnection;2;4;182;0
WireConnection;205;0;2;0
WireConnection;205;2;206;0
WireConnection;61;13;205;0
ASEEND*/
//CHKSM=17FCCD4B82A6F3BF6FE07BBFB0B91B266CE07AD3