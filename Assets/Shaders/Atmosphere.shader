Shader "Custom/Atmosphere" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Range ("Range", Range(0,10)) = 1
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" "LightMode" = "ForwardBase"}
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Front
            
			CGPROGRAM
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			
			
			// User defined variables
			uniform float3 _Color;
			uniform float _Range;
			
			// Base input structs
			struct vertexIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
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
				float3 viewDir = normalize (_WorldSpaceCameraPos.xyz);
				
				// lighting
				float alpha = _Range * dot (normalDir, -viewDir);
				
				o.col = float4 (_Color, alpha);
				o.pos = mul (UNITY_MATRIX_MVP, i.vertex);
								
				return o;
			}
			
			// Fragment funcion
			half4 frag (fragmentIn o) : COLOR {
				return o.col;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
