// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SkyboxShader"
{
	Properties
	{
		_Noise_1("Noise_1", 2D) = "white" {}
		_Noise_2("Noise_2", 2D) = "white" {}
		_Texture3("Texture 3", 2D) = "white" {}
		_UV_Distortion_Mask("UV_Distortion_Mask", 2D) = "white" {}
		_OpacityMask("OpacityMask", 2D) = "white" {}
		_Noise_1_Tiling("Noise_1_Tiling", Vector) = (0,0,0,0)
		_Noise_2_Tiling("Noise_2_Tiling", Vector) = (0,0,0,0)
		_Noise_3_Tiling("Noise_3_Tiling", Vector) = (0,0,0,0)
		_UV_Distortion_Tiling("UV_Distortion_Tiling", Vector) = (1,1,0,0)
		_Noise_1_Panner("Noise_1_Panner", Vector) = (0,0,0,0)
		_Noise_2_Panner("Noise_2_Panner", Vector) = (0,0,0,0)
		_Noise_3_Panner("Noise_3_Panner", Vector) = (0,0,0,0)
		_UV_Distortion_Speed("UV_Distortion_Speed", Vector) = (0,0,0,0)
		_Noise_1_Color("Noise_1_Color", Color) = (0,0,0,0)
		_Noise_2_Color("Noise_2_Color", Color) = (0,0,0,0)
		_VertexOffset("VertexOffset", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float4 _Noise_1_Color;
		uniform sampler2D _Noise_1;
		uniform float2 _Noise_1_Panner;
		uniform sampler2D _UV_Distortion_Mask;
		uniform float2 _UV_Distortion_Speed;
		uniform float2 _UV_Distortion_Tiling;
		uniform float2 _Noise_1_Tiling;
		uniform float4 _Noise_2_Color;
		uniform sampler2D _Noise_2;
		uniform float2 _Noise_2_Panner;
		uniform float2 _Noise_2_Tiling;
		uniform sampler2D _Texture3;
		uniform float2 _Noise_3_Panner;
		uniform float2 _Noise_3_Tiling;
		uniform float _VertexOffset;
		uniform sampler2D _OpacityMask;
		uniform float4 _OpacityMask_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_TexCoord98 = v.texcoord.xy * _UV_Distortion_Tiling;
			float2 panner99 = ( 1.0 * _Time.y * _UV_Distortion_Speed + uv_TexCoord98);
			float4 tex2DNode101 = tex2Dlod( _UV_Distortion_Mask, float4( panner99, 0, 0.0) );
			float2 uv_TexCoord12 = v.texcoord.xy * ( tex2DNode101 * float4( _Noise_1_Tiling, 0.0 , 0.0 ) ).rg;
			float2 panner8 = ( 1.0 * _Time.y * _Noise_1_Panner + uv_TexCoord12);
			float2 uv_TexCoord17 = v.texcoord.xy * ( tex2DNode101 * float4( _Noise_2_Tiling, 0.0 , 0.0 ) ).rg;
			float2 panner9 = ( 1.0 * _Time.y * _Noise_2_Panner + uv_TexCoord17);
			float2 uv_TexCoord19 = v.texcoord.xy * ( tex2DNode101 * float4( _Noise_3_Tiling, 0.0 , 0.0 ) ).rg;
			float2 panner10 = ( 1.0 * _Time.y * _Noise_3_Panner + uv_TexCoord19);
			float4 tex2DNode7 = tex2Dlod( _Texture3, float4( panner10, 0, 0.0) );
			float4 lerpResult32 = lerp( ( _Noise_1_Color * tex2Dlod( _Noise_1, float4( panner8, 0, 0.0) ) ) , ( _Noise_2_Color * tex2Dlod( _Noise_2, float4( panner9, 0, 0.0) ) ) , tex2DNode7);
			v.vertex.xyz += ( lerpResult32 * ( tex2DNode7 * _VertexOffset ) ).rgb;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			v.normal = ase_worldViewDir;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord98 = i.uv_texcoord * _UV_Distortion_Tiling;
			float2 panner99 = ( 1.0 * _Time.y * _UV_Distortion_Speed + uv_TexCoord98);
			float4 tex2DNode101 = tex2D( _UV_Distortion_Mask, panner99 );
			float2 uv_TexCoord12 = i.uv_texcoord * ( tex2DNode101 * float4( _Noise_1_Tiling, 0.0 , 0.0 ) ).rg;
			float2 panner8 = ( 1.0 * _Time.y * _Noise_1_Panner + uv_TexCoord12);
			float2 uv_TexCoord17 = i.uv_texcoord * ( tex2DNode101 * float4( _Noise_2_Tiling, 0.0 , 0.0 ) ).rg;
			float2 panner9 = ( 1.0 * _Time.y * _Noise_2_Panner + uv_TexCoord17);
			float2 uv_TexCoord19 = i.uv_texcoord * ( tex2DNode101 * float4( _Noise_3_Tiling, 0.0 , 0.0 ) ).rg;
			float2 panner10 = ( 1.0 * _Time.y * _Noise_3_Panner + uv_TexCoord19);
			float4 tex2DNode7 = tex2D( _Texture3, panner10 );
			float4 lerpResult32 = lerp( ( _Noise_1_Color * tex2D( _Noise_1, panner8 ) ) , ( _Noise_2_Color * tex2D( _Noise_2, panner9 ) ) , tex2DNode7);
			o.Emission = lerpResult32.rgb;
			float2 uv_OpacityMask = i.uv_texcoord * _OpacityMask_ST.xy + _OpacityMask_ST.zw;
			float4 tex2DNode86 = tex2D( _OpacityMask, uv_OpacityMask );
			o.Alpha = tex2DNode86.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
2567;190;1906;1011;3630.08;556.8131;1.278653;True;False
Node;AmplifyShaderEditor.Vector2Node;96;-4014.527,-727.1658;Float;False;Property;_UV_Distortion_Tiling;UV_Distortion_Tiling;8;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;97;-3793.823,-602.3057;Float;False;Property;_UV_Distortion_Speed;UV_Distortion_Speed;12;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;98;-3786.384,-721.8444;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;99;-3518.127,-722.6085;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;100;-3216.698,-757.8677;Float;True;Property;_UV_Distortion_Mask;UV_Distortion_Mask;3;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector2Node;15;-2415.657,-132.4573;Float;False;Property;_Noise_1_Tiling;Noise_1_Tiling;5;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;16;-2426.93,112.4312;Float;False;Property;_Noise_2_Tiling;Noise_2_Tiling;6;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;101;-2981.655,-757.8464;Float;True;Property;_TextureSample4;Texture Sample 4;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-2209.497,-127.1852;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;18;-2424.351,378.5468;Float;False;Property;_Noise_3_Tiling;Noise_3_Tiling;7;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-2219.727,105.5294;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-2221.004,379.1612;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;21;-2002.922,232.1768;Float;False;Property;_Noise_2_Panner;Noise_2_Panner;10;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-2068.599,-123.3;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-2074.737,116.4739;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;20;-1995.485,-7.597031;Float;False;Property;_Noise_1_Panner;Noise_1_Panner;9;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1147.007,93.25832;Float;True;Property;_Noise_2;Noise_2;1;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector2Node;22;-1995.228,495.7351;Float;False;Property;_Noise_3_Panner;Noise_3_Panner;11;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1151.557,-132.2916;Float;True;Property;_Noise_1;Noise_1;0;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2068.343,380.0322;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-1774.513,-127.0062;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;9;-1789.836,111.8736;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;10;-1784.743,375.4323;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-931.8574,-130.9915;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;26;-631.5568,-141.3912;Float;False;Property;_Noise_1_Color;Noise_1_Color;13;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-630.9066,93.90881;Float;False;Property;_Noise_2_Color;Noise_2_Color;14;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-927.3076,94.55841;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1148.307,335.0583;Float;True;Property;_Texture3;Texture 3;2;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-419.0071,114.7088;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;7;-928.6075,336.3584;Float;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-419.6572,-120.5912;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-132.9636,628.3343;Float;False;Property;_VertexOffset;VertexOffset;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;85;-887.2555,662.2079;Float;True;Property;_OpacityMask;OpacityMask;4;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;255.7469,359.8171;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;32;-56.23458,175.4695;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;86;-667.556,663.5081;Float;True;Property;_TextureSample3;Texture Sample 3;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;94;405.3496,737.0198;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;517.871,329.1293;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;11.52368,435.2577;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;755.2701,-21.39557;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;SkyboxShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;98;0;96;0
WireConnection;99;0;98;0
WireConnection;99;2;97;0
WireConnection;101;0;100;0
WireConnection;101;1;99;0
WireConnection;102;0;101;0
WireConnection;102;1;15;0
WireConnection;103;0;101;0
WireConnection;103;1;16;0
WireConnection;104;0;101;0
WireConnection;104;1;18;0
WireConnection;12;0;102;0
WireConnection;17;0;103;0
WireConnection;19;0;104;0
WireConnection;8;0;12;0
WireConnection;8;2;20;0
WireConnection;9;0;17;0
WireConnection;9;2;21;0
WireConnection;10;0;19;0
WireConnection;10;2;22;0
WireConnection;3;0;2;0
WireConnection;3;1;8;0
WireConnection;5;0;4;0
WireConnection;5;1;9;0
WireConnection;29;0;28;0
WireConnection;29;1;5;0
WireConnection;7;0;6;0
WireConnection;7;1;10;0
WireConnection;27;0;26;0
WireConnection;27;1;3;0
WireConnection;91;0;7;0
WireConnection;91;1;92;0
WireConnection;32;0;27;0
WireConnection;32;1;29;0
WireConnection;32;2;7;0
WireConnection;86;0;85;0
WireConnection;93;0;32;0
WireConnection;93;1;91;0
WireConnection;87;0;7;0
WireConnection;87;1;86;0
WireConnection;0;2;32;0
WireConnection;0;9;86;0
WireConnection;0;11;93;0
WireConnection;0;12;94;0
ASEEND*/
//CHKSM=22F9874E15B2C0631C7C50092ADBCAE8C25F3191