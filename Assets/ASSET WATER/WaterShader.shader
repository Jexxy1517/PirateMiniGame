Shader "Custom/BigWaveWater"
{
    Properties
    {
        _Color ("Water Color", Color) = (0,0.5,1,0.6)
        _WaveSpeed ("Wave Speed", Range(0,5)) = 1.0
        _WaveHeight ("Wave Height", Range(0,5)) = 2.0   // much taller waves
        _WaveFrequency ("Wave Frequency", Range(0,10)) = 0.5 // wide, smooth ripples
        _RippleOrigin ("Ripple Origin", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _WaveSpeed;
            float _WaveHeight;
            float _WaveFrequency;
            float4 _Color;
            float4 _RippleOrigin;

            v2f vert(appdata v)
            {
                v2f o;

                float t = _Time.y * _WaveSpeed;

                // World position of vertex
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Distance from ripple origin
                float dist = distance(worldPos.xz, _RippleOrigin.xz);

                // BIG circular wave
                float wave = sin(dist * _WaveFrequency - t) * _WaveHeight;

                v.vertex.y += wave;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = worldPos;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
