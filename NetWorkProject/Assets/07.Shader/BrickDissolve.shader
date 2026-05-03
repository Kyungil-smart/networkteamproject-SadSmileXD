Shader "UI/BrickDissolve_Color"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}

        _Progress ("Progress", Range(0,1)) = 0
        _EdgeWidth ("Edge Width", Range(0,0.2)) = 0.05

        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _EdgeColor ("Edge Color", Color) = (1,0.5,0,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
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
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv  : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;

            float _Progress;
            float _EdgeWidth;

            float4 _TintColor;
            float4 _EdgeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 중앙 기준 퍼짐
                float centerDist = abs(uv.x - 0.5) * 2;

                // 노이즈
                float noise = tex2D(_NoiseTex, uv * 5).r;

                float dissolve = centerDist + noise * 0.3;

                // 기본 알파
                float alpha = step(dissolve, _Progress);

                // 🔥 경계 영역 (edge)
                float edge = smoothstep(_Progress - _EdgeWidth, _Progress, dissolve);

                fixed4 col = tex2D(_MainTex, uv) * _TintColor;

                // 👉 경계 색상 섞기
                col.rgb = lerp(_EdgeColor.rgb, col.rgb, edge);

                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}