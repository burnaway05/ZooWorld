Shader "Custom/DefaultColorfulShader"
{
    Properties {
        _Color ("Base Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #pragma multi_compile_instancing

        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
        UNITY_INSTANCING_BUFFER_END(Props)

        struct Input {
            float3 worldNormal;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            o.Albedo = color.rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.5;
        }
        ENDCG
    }

    Fallback "Diffuse"
}
