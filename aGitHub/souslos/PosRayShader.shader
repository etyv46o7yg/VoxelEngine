
Shader "Unlit/PosRayShader"
    {
    Properties
        {
        _MainTex ("Texture", 2D) = "white" {}
        }
    SubShader
        {
        Cull off
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
            {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #define UNITY_SHADER_NO_UPGRADE 1

            struct appdata
                {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                };

            struct v2f
                {
                float2 uv       : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex   : SV_POSITION;
                float4 worldPos : COLOR;
                };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
                {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
                }

            fixed4 frag(v2f i) : SV_Target
                {
                // sample the texture
                float3 ray = normalize(_WorldSpaceCameraPos.rgb - i.worldPos.rgb);
                //float4 ray = i.worldPos;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                fixed4 res = fixed4(ray * 0.5f + fixed3(0.5f, 0.5f, 0.5f), 1);
                //fixed4 res = fixed4(ray, 1);
                return res;
                }
            ENDCG
            }
        }
    }
