Shader "URP/DebugWorldPosBasic"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" }
        Pass
        {
            Name "Simple"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings o;
                float3 world = TransformObjectToWorld(input.positionOS.xyz);
                o.worldPos = world;
                o.positionHCS = TransformWorldToHClip(world);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half2 color = i.worldPos.xy * 0.5 + 0.5;
                return half4(color, 0.0, 1.0);
            }

            ENDHLSL
        }
    }
}
