Shader "Custom/WindowMaskStencilCircle"
{
    Properties
    {
        _Radius ("Radius", Range(0,1)) = 0.48
        _Softness ("Softness", Range(0.001,0.2)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }

        Pass
        {
            ColorMask 0
            ZWrite Off
            Cull Off

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Radius;
            float _Softness;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 centered = i.uv - float2(0.5, 0.5);
                float dist = length(centered);

                float alpha = 1.0 - smoothstep(_Radius, _Radius + _Softness, dist);
                clip(alpha - 0.5);

                return 0;
            }
            ENDCG
        }
    }
}