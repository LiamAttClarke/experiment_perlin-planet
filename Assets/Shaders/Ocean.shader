Shader "Custom/Ocean" {
    Properties {
        _Color("Main Color", Color) = (1, 1, 1, 1) //Color when not intersecting
        _HighlightColor("Highlight Color", Color) = (1, 1, 1, 1) //Color when intersecting
        _HighlightThreshold("Highlight Threshold", Float) = 1 // Difference for intersections
    }
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" "LightMode" = "ForwardBase" }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
 
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            uniform sampler2D _CameraDepthTexture; //Depth Texture
            uniform half3 _Color;
            uniform half3 _HighlightColor;
            uniform float _HighlightThreshold;
 
            struct v2f {
                float4 pos : SV_POSITION;
                float4 norm : NORMAL;
                float4 projPos : TEXCOORD1; //Screen position of pos
            };
 
            v2f vert(appdata_base v) {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);           
                o.projPos = ComputeScreenPos(o.pos);
 
                return o;
            }
 
            half4 frag(v2f i) : COLOR {
            	half4 finalColor = half4(_Color, 0.5);
                
                //Get the distance to the camera from the depth buffer for this point
                float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r);
 
                //Actual distance to the camera
                float partZ = i.projPos.z;
 
                //If the two are similar, then there is an object intersecting with our object
                //float diff = (abs(sceneZ - partZ)) / (_HighlightThreshold * clamp(_SinTime.w, 0, 1.0));
                float diff = (abs(sceneZ - partZ)) / _HighlightThreshold;
                if (diff < 0.2) {
                	finalColor = half4(_HighlightColor,0.9);
                } else if(diff < 0.3) {
                    finalColor = half4(lerp(_HighlightColor, _Color, 0.33), 0.9);
                }
 
                half4 c = finalColor;
 
                return c;
            }
 
            ENDCG
        }
    }
    FallBack "Diffuse"
}