Shader "Ukemi/UkemiCharShader"
{
    Properties
    {
        _MainTex ("Detail Texture", 2D) = "white" {}
        _ColorTexture("Color Guide Texture", 2D) = "white" {}
        _Color1("Color 1", Color) = (1,1,1,0)
        _Color2("Color 2", Color) = (1,1,1,0)
        _Color3("Color 3", Color) = (1,1,1,0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
        Cull Off

        Pass
        {



            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "VertexSnapping.hlsl"
            

            sampler2D _MainTex, _ColorTexture;
            float4 _MainTex_ST;

            float4 _Color1, _Color2, _Color3;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.vertex = vertSnap(v, 160, 120);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 map = tex2D(_ColorTexture, i.uv);

                if (map.r == 1)
                {
                    col = overlay(col, _Color1);
                }
                else if (map.g == 1)
                {
                    col = overlay(col, _Color2);
                }
                else if (map.b == 1)
                {
                    col = overlay(col, _Color3);
                }
                clip(col.a - .001);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
