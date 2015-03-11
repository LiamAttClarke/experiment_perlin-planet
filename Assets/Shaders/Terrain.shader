Shader "Custom/Terrain" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color_Sand ("Sand Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Grass ("Grass Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Snow ("Snow Color", Color) = (1.0, 1.0, 1.0, 1.0)
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
			
			#include "UnityCG.cginc"
			
			// User defined variables
			// Color
			uniform float4 _Color_Sand;
			uniform float4 _Color_Grass;
			uniform float4 _Color_Snow;
			
			// Texture
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			// Lighting
			uniform float _DiffuseGain;
			uniform float4 _RimColor;
			uniform float _RimGain;
			
			// Unity defined variables
			uniform float4 _LightColor0;
			
			// Base input structs
			struct vertexIn {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			struct fragmentIn {
				float4 pos : SV_POSITION;
				float4 col : COLOR;
				half2 uv : TEXCOORD0;
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
				
				// vertex color based on altitude
				float4 vertColor;
				float vertMag = sqrt ((i.vertex.x * i.vertex.x) + (i.vertex.y * i.vertex.y) + (i.vertex.z * i.vertex.z));
				if (vertMag > 1.15) {
				vertColor = _Color_Snow;
				} else if (vertMag >= 0.97) {
					vertColor = _Color_Grass;
				} else {
				 	vertColor = _Color_Sand;
				}
				
				o.col = float4 (light * vertColor, 1.0);
				o.pos = mul (UNITY_MATRIX_MVP, i.vertex);
				o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
				
				return o;
			}
			
			// Fragment funcion
			float4 frag (fragmentIn o) : COLOR {
				return tex2D(_MainTex, o.uv) * o.col;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
