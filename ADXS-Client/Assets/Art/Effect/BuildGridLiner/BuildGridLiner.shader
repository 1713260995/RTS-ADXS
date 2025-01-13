// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hexu/BuildLiner"
{
    Properties
    {
        _Width("Width", float ) = 0.01
        _ScaleOffset("Scale Offset", Vector) = (10,10,0,0)
        [HDR] _Color("Tint", Color) = (1,1,1,1)
        _min("Min",float) = 0
        _max("Max",float) = 0.5
    }
    Subshader
    {
        Tags
        {
            "Queue" = "Transparent"
        }
        Pass
        {
            ZWrite Off
            ColorMask RGB
            Blend SrcAlpha One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(2)
                float4 pos : SV_POSITION;
            };

            float4x4 unity_Projector;

            v2f vert(const float4 vertex : POSITION)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(vertex);
                o.uv0 = mul(unity_Projector, vertex);
                return o;
            }

            float4 _Color;
            float _Width;
            float4 _ScaleOffset;
            float _min;
            float _max;
            fixed4 frag(const v2f i) : SV_Target
            {
                const float temp_output_2_0_g3 = 1 - _Width;
                const float2 appendResult10_g4 = float2(temp_output_2_0_g3, temp_output_2_0_g3);
                const float2 temp_output_11_0_g4 = abs(frac(i.uv0 * _ScaleOffset.xy + _ScaleOffset.zw) * 2.0 + -1.0) -
                    appendResult10_g4;
                const float2 break16_g4 = 1.0 - temp_output_11_0_g4 / fwidth(temp_output_11_0_g4);
                float4 res = 1 - saturate(min(break16_g4.x, break16_g4.y)).xxxx;
                const float len = length(i.uv0 - float2(0.5,0.5));
                res *= step(len, 0.5);
                res *= smoothstep( 1-len, _min, _max);
        
                return res * _Color;
            }
            ENDCG
        }
    }
}