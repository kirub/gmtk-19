// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:Particles/Additive,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5955882,fgcg:0.5955882,fgcb:0.5955882,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:9167,x:34067,y:32553,varname:node_9167,prsc:2|alpha-6600-OUT,refract-9405-OUT;n:type:ShaderForge.SFN_Tex2d,id:2539,x:32807,y:32833,varname:node_2539,prsc:2,ntxv:0,isnm:False|TEX-27-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:27,x:32628,y:32833,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:node_27,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Lerp,id:8762,x:33022,y:32858,varname:node_8762,prsc:2|A-7149-OUT,B-2539-RGB,T-6627-A;n:type:ShaderForge.SFN_Vector3,id:7149,x:33022,y:32765,varname:node_7149,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_VertexColor,id:6627,x:32628,y:33009,varname:node_6627,prsc:2;n:type:ShaderForge.SFN_ComponentMask,id:7828,x:33187,y:32858,varname:node_7828,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8762-OUT;n:type:ShaderForge.SFN_Vector1,id:6600,x:33187,y:32781,varname:node_6600,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:2400,x:33467,y:32877,varname:node_2400,prsc:2|A-7828-OUT,B-6627-A;n:type:ShaderForge.SFN_Multiply,id:9405,x:33759,y:32833,varname:node_9405,prsc:2|A-2400-OUT,B-6903-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6903,x:33467,y:33046,ptovrint:False,ptlb:DistortionValue,ptin:_DistortionValue,varname:node_6903,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;proporder:27-6903;pass:END;sub:END;*/

Shader "Custom/Distortion_NormalMap" {
    Properties {
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _DistortionValue ("DistortionValue", Float ) = 5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _DistortionValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 node_2539 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap)));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + ((lerp(float3(0,0,1),node_2539.rgb,i.vertexColor.a).rg*i.vertexColor.a)*_DistortionValue);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                return fixed4(lerp(sceneColor.rgb, finalColor,0.0),1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
