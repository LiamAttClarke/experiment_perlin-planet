Shader "Custom/Terrain" {
	Properties {
		_Color_Rock ("Rock Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Sand ("Sand Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Grass ("Grass Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color_Snow ("Snow Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Texture", 2D) = "white" {}
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
			uniform float4 _Color_Rock;
			uniform float4 _Color_Sand;
			uniform float4 _Color_Grass;
			uniform float4 _Color_Snow;
			
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
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
				float diffuseAtten = 2.0;
				float rimAtten = 0.3;
				float3 diffuseLight = diffuseAtten * dot (normalDir, lightDir);
				float3 rimLight = rimAtten * (1.0 / dot (normalDir, viewDir));
				float3 light = (_LightColor0 * diffuseLight) + rimLight + UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				// vertex color based on height
				float4 vertColor;
				float vertMag = sqrt ((i.vertex.x * i.vertex.x) + (i.vertex.y * i.vertex.y) + (i.vertex.z * i.vertex.z));
				if (vertMag > 1.15) {
				vertColor = _Color_Snow;
				} else if (vertMag >= 0.97) {
					vertColor = _Color_Grass;
				} else if (vertMag > 0.86) {
				 	vertColor = _Color_Sand;
				} else {
					vertColor = _Color_Rock;
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
