// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GravityShader"
{
	Properties
	{
		_FresnelPower("FresnelPower", Float) = 1
		_CounterFresnelPower("CounterFresnelPower", Float) = 1
		_Alpha("Alpha", Float) = 1
		_FresnelScale("FresnelScale", Float) = 1
		_CounterFresnelScale("CounterFresnelScale", Float) = 1
		_FresnelBias("FresnelBias", Float) = 1
		_CounterFresneBias("CounterFresneBias", Float) = 1
		_MainColor("MainColor", Color) = (0,0,0,0)
		_EmissiveValue("EmissiveValue", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float _EmissiveValue;
		uniform float4 _MainColor;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _CounterFresneBias;
		uniform float _CounterFresnelScale;
		uniform float _CounterFresnelPower;
		uniform float _Alpha;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV1, _FresnelPower ) );
			o.Emission = ( _EmissiveValue * ( _MainColor * fresnelNode1 ) ).rgb;
			float fresnelNdotV11 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode11 = ( _CounterFresneBias + _CounterFresnelScale * pow( 1.0 - fresnelNdotV11, _CounterFresnelPower ) );
			float clampResult21 = clamp( fresnelNode11 , 0.0 , 1.0 );
			float temp_output_7_0 = ( fresnelNode1 * _Alpha );
			float clampResult10 = clamp( temp_output_7_0 , 0.0 , 1.0 );
			o.Alpha = ( ( 1.0 - clampResult21 ) * clampResult10 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

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
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
2574;208;1906;987;1375.283;518.8906;1.372545;True;False
Node;AmplifyShaderEditor.RangedFloatNode;2;-1332.432,457.2605;Float;False;Property;_FresnelPower;FresnelPower;0;0;Create;True;0;0;False;0;1;2.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-962.4927,-347.6598;Float;False;Property;_CounterFresneBias;CounterFresneBias;6;0;Create;True;0;0;False;0;1;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-969.1827,-267.3774;Float;False;Property;_CounterFresnelScale;CounterFresnelScale;4;0;Create;True;0;0;False;0;1;1.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-975.3537,-192.2251;Float;False;Property;_CounterFresnelPower;CounterFresnelPower;1;0;Create;True;0;0;False;0;1;6.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1326.261,382.1082;Float;False;Property;_FresnelScale;FresnelScale;3;0;Create;True;0;0;False;0;1;1.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1319.571,301.8259;Float;False;Property;_FresnelBias;FresnelBias;5;0;Create;True;0;0;False;0;1;-0.33;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;11;-543.5194,-222.3037;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-835.8719,437.89;Float;False;Property;_Alpha;Alpha;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-979,202;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;21;-269.0117,-326.7345;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-955.2836,-27.40271;Float;False;Property;_MainColor;MainColor;7;0;Create;True;0;0;False;0;0,0,0,0;1,0.1556604,0.7107285,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-619.0104,252.5963;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;16;-94.69849,-238.7739;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;10;-197.6387,262.2043;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;71.37939,-92.02905;Float;False;Property;_EmissiveValue;EmissiveValue;8;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-605.2845,119.4595;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;439.2214,-34.38214;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;9;-421.3637,263.5768;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;53.53642,211.421;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;743.9194,-126.2741;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;GravityShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;0;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.267;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;1;19;0
WireConnection;11;2;18;0
WireConnection;11;3;20;0
WireConnection;1;1;4;0
WireConnection;1;2;3;0
WireConnection;1;3;2;0
WireConnection;21;0;11;0
WireConnection;7;0;1;0
WireConnection;7;1;8;0
WireConnection;16;0;21;0
WireConnection;10;0;7;0
WireConnection;6;0;5;0
WireConnection;6;1;1;0
WireConnection;22;0;23;0
WireConnection;22;1;6;0
WireConnection;9;0;7;0
WireConnection;17;0;16;0
WireConnection;17;1;10;0
WireConnection;0;2;22;0
WireConnection;0;9;17;0
ASEEND*/
//CHKSM=AFA812600EE041583EE975EE565C98E9FD7B8802