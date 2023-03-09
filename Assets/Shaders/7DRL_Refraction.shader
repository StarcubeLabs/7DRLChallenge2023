Shader "Unlit/7DRL Refraction"
{
    Properties
    {
        _DistortionAmount("Distortion amount", float) = 0
        _ColorTint ("Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Cull Off
        ZWrite Off
        LOD 100

        GrabPass
        {
            "_GrabTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 distortionUV : TEXCOORD1;
                float4 grabPassUV : TEXCOORD2;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                half3 normal : TEXCOORD3;
                float3 worldNormal : NORMAL;
                float4 scrPos: TEXCOORD4;
            };

            float _DistortionAmount;
            sampler2D _DistortionGuide;
            float4 _DistortionGuide_ST;
            sampler2D _GrabTexture;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraDepthNormalsTexture;

            float4 _ColorTint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.distortionUV = TRANSFORM_TEX(v.uv, _DistortionGuide);
                o.grabPassUV = ComputeGrabScreenPos(o.vertex);
                o.color = v.color;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.scrPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float3 viewSpaceNormal;
                float viewDepth;

                float2 screenPosition = (i.scrPos.xy / i.scrPos.w);
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, screenPosition), viewDepth, viewSpaceNormal);
                float3 worldNormal = mul((float3x3)unity_MatrixInvV, float4(viewSpaceNormal, 0.0));
                float3 color = GammaToLinearSpace(worldNormal * 0.5 + 0.5);
                // return float4(color, 1);

                float2 distortion = UnpackNormal(tex2D(_DistortionGuide, i.distortionUV)).xy;
                distortion *= _DistortionAmount * i.color.a;

                distortion = screenPosition.xy + color.xy * _DistortionAmount;
                i.grabPassUV.xy += distortion * i.grabPassUV.z;
                fixed4 col = tex2Dproj(_GrabTexture, i.grabPassUV);
                return col * _ColorTint;
            }
            ENDCG
        }
    }
}
