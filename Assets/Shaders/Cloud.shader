Shader "Custom/Cloud" {
	Properties {
		_Color ("White", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Tags { "LightMode" = "ForwardBase" }
		Pass {
			CGPROGRAM
			// Pragmas
			#pragma vertex vert
			#pragma fragment frag
			
			// User defined variables
			uniform float4 _Color;
			
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
				float3 viewDir = normalize (_WorldSpaceCameraPos.xyz - i.vert.xyz);
				float diffAtten = 1.25;
				float rimAtten = 0.25;
				float3 diffuseLight = diffAtten * (0.0, dot (normalDir, lightDir));
				float3 rimLight = rimAtten * (1 / dot (normalDir, viewDir) + 0.1);
				float3 lightFinal = (_LightColor0 * diffuseLight) + rimLight + UNITY_LIGHTMODEL_AMBIENT.xyz;
				o.col = float4 (lightFinal * _Color, 1.0);
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