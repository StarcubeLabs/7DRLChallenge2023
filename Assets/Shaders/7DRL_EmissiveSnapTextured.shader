Shader "7DRL/7DRL_EmissiveSnapTextured"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorTint ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _ColorTint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex = vertSnap(v, 160, 120);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);
                float lightIntensity = NdotL > 0 ? 1 : 0;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float alpha = col.a;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                clip(alpha - .001);
                float4 fin = col * _ColorTint;
                fin = lerp(fin * .5, col, lightIntensity);
                return half4(fin.rgb, alpha) * _ColorTint;
            }
            ENDCG
        }
    }
}
