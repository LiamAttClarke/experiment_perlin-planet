Shader "Custom/Ocean" {
	Properties {
		_Color ("White", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader {
		Tags { "LightMode" = "ForwardBase" "Queue"="Transparent" }
		Pass {
        	Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
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
				float atten = 1.0;
				float rimAtten = 0.25;
				float3 lightFinal = (_LightColor0 * (atten * max (0.0, dot (normalDir, lightDir)))) + (rimAtten * (1 / max (0.0, dot (normalDir, lightDir)))) + UNITY_LIGHTMODEL_AMBIENT.xyz;
				o.col = float4 (lightFinal * _Color, _Color.a);
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