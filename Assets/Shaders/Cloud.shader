Shader "Custom/Cloud" {
	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
      	_OverHang ("Overhang", Range(0, 2.0)) = 1.0
		_ShadeColor1 ("Shade Color 1", Color) = (1,1,1,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_DiffuseThreshold ("Diffuse Threshold", Range(-10,10)) = 0.5
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" "LightMode" = "ForwardBase"}
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
			CGPROGRAM
			// Pragma
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			// User defined variables
			uniform half3 _Color;
			uniform half3 _ShadeColor1;
			uniform half _OverHang;
			uniform half _DiffuseThreshold;
			
			// Input structs
			struct vertIN {
				float4 vertex 		: POSITION;
				float4 normal 		: NORMAL;
			};
			struct fragIN {
				float4 pos 			: SV_POSITION;
				float4 norm			: NORMAL;
				float3 viewDir 		: TEXCOORD1;
				half alpha			: TEXCOORD2;
			};
			
			// Vertex function
			fragIN vert (vertIN v){
				fragIN f;
				
				f.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				f.norm = mul (v.normal, _World2Object);
				f.viewDir = normalize (WorldSpaceViewDir(v.vertex).xyz);
				f.alpha = clamp(dot(normalize(v.vertex).xyz, -f.viewDir) + _OverHang, 0, 0.75);
				
				return f;
			}
			
			// Fragment function
			half4 frag (fragIN f) : COLOR {
				float3 lightDir = normalize (_WorldSpaceLightPos0.xyz);

				half diffuseLight = dot (f.norm, lightDir);
				half outline = dot (f.norm, f.viewDir);
				
				half3 finalColor;
				if (diffuseLight > _DiffuseThreshold) {
					finalColor = _Color;
				} else if (diffuseLight > _DiffuseThreshold) {
					finalColor = _ShadeColor1;
				}
				
				return half4(finalColor, f.alpha);
			}
			
			ENDCG
			
		}
	}
}