﻿Shader "Custom/Terrain" {
	Properties {
		_Color_DeepSand ("Color_Rock", Color) = (0, 0, 0, 0)
		_Color_Sand ("Color_Sand", Color) = (0, 0, 0, 0)
		_Color_Grass ("Color_Grass", Color) = (0, 0, 0, 0)
		_Color_Snow ("Color_Snow", Color) = (0, 0, 0, 0)
	}
	SubShader {
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			// Pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			// User defined variables
			uniform float4 _Color_DeepSand;
			uniform float4 _Color_Sand;
			uniform float4 _Color_Grass;
			uniform float4 _Color_Snow;
			
			// Unity defined variables
			uniform float4 _LightColor0;
			
			// Base input structs
			struct vertexIn {
				float4 vert : POSITION;
				float4 norm : NORMAL;
			};
			struct vertexOut {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};
			
			// Vertex function
			vertexOut vert (vertexIn i) {
				vertexOut o;
				
				// vectors
				float3 normalDir = normalize (mul (i.norm, _World2Object).xyz);
				float3 lightDir = normalize (_WorldSpaceLightPos0.xyz);
				float3 viewDir = normalize (_WorldSpaceCameraPos.xyz);
				
				// lighting
				float diffuseAtten = 1.5;
				float rimAtten = 0.25;
				float3 diffuseLight = diffuseAtten * dot (normalDir, lightDir);
				float3 rimLight = rimAtten * (1.0 / dot (normalDir, viewDir));
				float3 light = (_LightColor0 * diffuseLight) + rimLight + UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				// vertex color based on height
				float4 vertColor;
				float vertMag = sqrt ((i.vert.x * i.vert.x) + (i.vert.y * i.vert.y) + (i.vert.z * i.vert.z));
				if (vertMag > 1.15) {
				vertColor = _Color_Snow;
				} else if (vertMag >= 0.97) {
					vertColor = _Color_Grass;
				} else if (vertMag > 0.86) {
				 	vertColor = _Color_Sand;
				} else {
					vertColor = _Color_DeepSand;
				}
				
				o.col = float4 (light * vertColor, 1.0);
				o.pos = mul (UNITY_MATRIX_MVP, i.vert);
				
				return o;
			}
			
			// Fragment funcion
			float4 frag (vertexOut o) : COLOR {
				return o.col;
			}
			ENDCG
		}
	} 
	//FallBack "Diffuse"
}