Shader "Custom/Terrain" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color_Sand ("Sand Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Grass ("Grass Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Dirt ("Dirt Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Snow ("Snow Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimColor ("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_DiffuseGain ("Diffuse Gain", Range(0, 2.0)) = 1.5
      	_RimGain ("Rim Gain", Range(0.5, 5.0)) = 4.0
	}
	SubShader {
		Pass {
			Tags { "Queue" = "Geometry" "LightMode" = "ForwardBase" }

			CGPROGRAM
			// Pragma
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			// User defined variables
			// Color
			uniform half3 _Color_Sand;
			uniform half3 _Color_Grass;
			uniform half3 _Color_Dirt;
			uniform half3 _Color_Snow;
			
			// Texture
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			// Lighting
			uniform float _DiffuseGain;
			uniform half3 _RimColor;
			uniform float _RimGain;
			
			// Base input structs
			struct fragmentIn {
				float4 pos 			: SV_POSITION;
				half3 col 			: COLOR;
				half2 uv 			: TEXCOORD0;
				float3 fragPos 		: TEXCOORD2;
			};
			
			
			// Vertex function
			fragmentIn vert (appdata_base i) {
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
								
				o.fragPos = i.vertex;
				o.col = float4 (light, 1.0);
				o.pos = mul (UNITY_MATRIX_MVP, i.vertex);
				o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
				
				return o;
			}
			
			// Fragment funcion
			half4 frag (fragmentIn o) : COLOR {
				half3 vertColor;
				float vertMag = sqrt ((o.fragPos.x * o.fragPos.x) + (o.fragPos.y * o.fragPos.y) + (o.fragPos.z * o.fragPos.z));
				if (vertMag > 1.15) {
				vertColor = _Color_Snow;
				} else if (vertMag >= 0.97) {
					vertColor = _Color_Grass;
				} else if (vertMag >= 0.95) {
					vertColor = _Color_Dirt;
				} else {
				 	vertColor = _Color_Sand;
				}
				o.col *= vertColor;
				return tex2D(_MainTex, o.uv) * half4(o.col.rgb,1.0);
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
