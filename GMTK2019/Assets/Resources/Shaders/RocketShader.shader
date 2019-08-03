// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RocketShader"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_Emissive("Emissive", 2D) = "white" {}
		_DiffuseColor("DiffuseColor", Color) = (0,0,0,0)
		_EmissiveColor("EmissiveColor", Color) = (0,0,0,0)
		_OutlineColor("OutlineColor", Color) = (0,0,0,0)
		_OutlineMask("OutlineMask", 2D) = "white" {}
		_OutlineWidth("OutlineWidth", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input
		{
			float3 worldPos;
		};
		uniform float _OutlineWidth;
		uniform sampler2D _OutlineMask;
		uniform float4 _OutlineMask_ST;
		uniform float4 _OutlineColor;
		
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float fresnelNdotV17 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode17 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV17, 5.0 ) );
			float2 uv_OutlineMask = v.texcoord * _OutlineMask_ST.xy + _OutlineMask_ST.zw;
			float outlineVar = ( fresnelNode17 * ( _OutlineWidth * 0.01 ) * tex2Dlod( _OutlineMask, float4( uv_OutlineMask, 0, 0.0) ) ).r;
			v.vertex.xyz += ( v.normal * outlineVar );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _OutlineColor.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _DiffuseColor;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _EmissiveColor;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			o.Albedo = ( _DiffuseColor * tex2D( _MainTexture, uv_MainTexture ) ).rgb;
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			float4 temp_output_9_0 = ( tex2D( _Emissive, uv_Emissive ) * _EmissiveColor );
			o.Emission = temp_output_9_0.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16900
2574;208;1906;987;632.1324;372.5107;1.428822;True;False
Node;AmplifyShaderEditor.RangedFloatNode;23;-472.1036,777.6905;Float;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-483.534,699.1054;Float;False;Property;_OutlineWidth;OutlineWidth;8;0;Create;True;0;0;False;0;1;1.74;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;29;-1074.353,1297.068;Float;True;Property;_OutlineMask;OutlineMask;7;0;Create;True;0;0;False;0;None;d3e77d6094efb944a973e046bac8bf39;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1087.586,-30.71528;Float;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;False;0;None;d3e77d6094efb944a973e046bac8bf39;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.FresnelNode;17;-410.6641,479.0668;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1085.069,271.8876;Float;True;Property;_Emissive;Emissive;1;0;Create;True;0;0;False;0;None;d3e77d6094efb944a973e046bac8bf39;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-289.2144,691.9615;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-775.7285,1295.639;Float;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-695.0009,-262.4917;Float;False;Property;_DiffuseColor;DiffuseColor;3;0;Create;True;0;0;False;0;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-693.5721,511.9296;Float;False;Property;_EmissiveColor;EmissiveColor;4;0;Create;True;0;0;False;0;0,0,0,0;0.2719384,0.4411775,0.5943396,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-782.1591,271.8876;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-789.6078,-29.44108;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-212.0578,199.0177;Float;False;Property;_OutlineColor;OutlineColor;5;0;Create;True;0;0;False;0;0,0,0,0;0,1,0.8844981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-102.0385,470.4938;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;1161.04,320.4677;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;40;332.3224,740.5414;Float;False;Property;_FresnelScale;FresnelScale;12;0;Create;True;0;0;False;0;1;-1.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;330.8937,661.9564;Float;False;Property;_FresnelBias;FresnelBias;11;0;Create;True;0;0;False;0;1;1.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;35;520.9272,637.6663;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;42;753.8248,656.2409;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;759.5397,729.1107;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;12;-1089.355,999.1581;Float;True;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;None;d3e77d6094efb944a973e046bac8bf39;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1093.641,766.2604;Float;True;Property;_Metallic;Metallic;2;0;Create;True;0;0;False;0;None;d3e77d6094efb944a973e046bac8bf39;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;37;622.3734,421.9142;Float;False;Constant;_FresnelColor;FresnelColor;11;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-790.7308,777.6909;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-64.88943,751.972;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-30.5979,940.5766;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-343.5095,1044.881;Float;False;Property;_SmoothnessValue;SmoothnessValue;10;0;Create;True;0;0;False;0;1;0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-326.3639,956.2936;Float;False;Property;_Metalness;Metalness;9;0;Create;True;0;0;False;0;1;0.82;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-790.7309,997.7293;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;1048.163,503.357;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OutlineNode;15;60.84702,217.5924;Float;False;0;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-417.8096,-31.02256;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-424.9535,274.745;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;39;330.8938,823.4133;Float;False;Property;_FresnelPower;FresnelPower;13;0;Create;True;0;0;False;0;1;0.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1480.26,-62.86817;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;RocketShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;19;0
WireConnection;22;1;23;0
WireConnection;30;0;29;0
WireConnection;5;0;3;0
WireConnection;2;0;1;0
WireConnection;18;0;17;0
WireConnection;18;1;22;0
WireConnection;18;2;30;0
WireConnection;32;0;9;0
WireConnection;32;1;38;0
WireConnection;35;1;41;0
WireConnection;35;2;40;0
WireConnection;35;3;39;0
WireConnection;42;0;35;0
WireConnection;43;0;42;0
WireConnection;11;0;10;0
WireConnection;24;0;11;0
WireConnection;24;1;25;0
WireConnection;27;0;13;0
WireConnection;27;1;26;0
WireConnection;13;0;12;0
WireConnection;38;0;37;0
WireConnection;38;1;43;0
WireConnection;15;0;20;0
WireConnection;15;1;18;0
WireConnection;7;0;6;0
WireConnection;7;1;2;0
WireConnection;9;0;5;0
WireConnection;9;1;8;0
WireConnection;0;0;7;0
WireConnection;0;2;9;0
WireConnection;0;11;15;0
ASEEND*/
//CHKSM=A512CB69D54DA424F92117D64C609B83AEBB8DD3