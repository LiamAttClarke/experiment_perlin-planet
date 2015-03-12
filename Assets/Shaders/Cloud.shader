Shader "Custom/Cloud" {
		Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_DiffuseGain ("Diffuse Gain", Range(0, 2.0)) = 1.5
      	_RimGain ("Rim Gain", Range(0.5, 5.0)) = 4.0
	}
	SubShader {
		Pass {
			Tags { "Queue" = "Geometry" "LightMode" = "ForwardBase" }

			CGPROGRAM
			// Pragmas
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
						
			// User defined variables
			// Color
			uniform float4 _Color;
			uniform float4 _RimColor;
			// Lighting
			uniform float _DiffuseGain;
			uniform float _RimGain;
			
			// Unity defined variables
			uniform float4 _LightColor0;
			
			// Base input structs
			struct vertexIn {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};
			struct fragmentIn {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};
			
			
			// Vertex function
			fragmentIn vert (vertexIn i) {
				fragmentIn o;

				// vectors
				float3 normalDir = normalize (mul (i.normal, _World2Object).xyz);
				float3 lightDir = normalize (_WorldSpaceLightPos0.xyz);
				float3 viewDir = normalize (_WorldSpaceCameraPos.xyz);
				
				// lighting
				// diffuse
				float3 diffuseLight = _DiffuseGain * dot (normalDir, lightDir);
				// rim
				float rimPower = 3.0;
				half rim = 1.0 - clamp(dot(viewDir, normalDir), 0, 1.0);
				float3 rimLight = _RimGain *_RimColor.rgb * pow(rim, rimPower);
				//final
				float3 light = diffuseLight + rimLight;
				
				o.col = float4 (light * _Color, 1.0);
				o.pos = mul (UNITY_MATRIX_MVP, i.vertex);
				
				return o;
			}
			
			// Fragment funcion
			float4 frag (fragmentIn o) : COLOR {
				return o.col;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}