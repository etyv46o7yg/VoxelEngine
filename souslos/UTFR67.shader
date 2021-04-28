// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RayShUYTU"
    {
    Properties
        {
        _MainTex("Texture", 2D) = "white" {}
        }
    SubShader
        {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
            {
            CGPROGRAM
            // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members screenPos)
            #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
                {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                };

            struct v2f
                {
                float2 uv        : TEXCOORD0;
                float2 screenPos : TEXCOORD1;
                float3 ray       : COLOR;
                UNITY_FOG_COORDS(1)
                float4 vertex    : SV_POSITION;
                };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4x4 matx;

            v2f vert(appdata v)
                {
                v2f o;
                o.vertex    = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                float3 scrP = ComputeScreenPos(o.vertex);
                o.ray       = mul ( float4(scrP, 1.0f),  matx  );
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
                }

            fixed4 frag(v2f i) : SV_Target
                {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return fixed4(i.ray, 1.0f);
                //return fixed4(1.0f, i.screenPos.y, 1.0f, 1);
                }
            ENDCG
            }
        }
    FallBack "Diffuse"
    }
