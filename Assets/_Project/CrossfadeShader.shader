Shader "Custom/CrossfadeShader"
{
    Properties
    {
        _MainTex ("Textura Actual", 2D) = "white" {}
        _NextTex ("Siguiente Bioma", 2D) = "white" {}
        _Blend ("Mezcla (0-1)", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MainTex;
            sampler2D _NextTex;
            float4 _MainTex_ST;
            float _Blend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Mezclamos la textura actual con la siguiente usando _Blend
                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_NextTex, i.uv);
                return lerp(col1, col2, _Blend);
            }
            ENDCG
        }
    }
}