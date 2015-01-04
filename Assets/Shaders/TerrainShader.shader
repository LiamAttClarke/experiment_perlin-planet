Shader "Custom/TerrainShader" {
	Properties {
		_Color_DeepSand ("Color_Rock", Color) = (0, 0, 0, 0)
		_Color_Sand ("Color_Sand", Color) = (0, 0, 0, 0)
		_Color_Grass ("Color_Grass", Color) = (0, 0, 0, 0)
		_Color_Snow ("Color_Snow", Color) = (0, 0, 0, 0)
	}
	SubShader {
		Pass {
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
				float3 normalDir = normalize (mul (i.norm, _World2Object).xyz);
				float3 lightDir = normalize (_WorldSpaceLightPos0.xyz);
				float atten = 2.0;
				float3 lightFinal = (atten * _LightColor0 * max (0.0, dot (normalDir, lightDir))) + UNITY_LIGHTMODEL_AMBIENT.xyz;
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
				// North & South Poles
//				if (i.vert.y > 0.8 || i.vert.y < -0.8) {
//					vertColor = _Color_Snow;
//				}
				o.col = float4 (lightFinal * vertColor, 1.0);
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
