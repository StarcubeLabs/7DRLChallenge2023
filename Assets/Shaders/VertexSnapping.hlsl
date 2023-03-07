
#include "UnityCG.cginc"

struct v2f
{
    half4 color : COLOR0;
    half4 colorFog : COLOR1;
    float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
    half3 normal : TEXCOORD1;
	float4 vertex : SV_POSITION;
};

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

float4 vertSnap(appdata v, float xSize, float ySize)
{
	v2f o;

	//Vertex snapping
	float4 snapToPixel = mul(UNITY_MATRIX_MVP,v.vertex);
	float4 vertex = snapToPixel;
	vertex.xyz = snapToPixel.xyz / snapToPixel.w;
	vertex.x = floor(xSize * vertex.x) / xSize;
	vertex.y = floor(ySize * vertex.y) / ySize;
	vertex.xyz *= snapToPixel.w;
	return vertex;

}

fixed4 overlay(fixed4 a, fixed4 b)
{
	return a < 0.5 ? (2.0 * a * b) : (1.0 - 2.0 * (1.0 - a) * (1.0 - b));
}

float overlay(float a, float b)
{
	return a < 0.5 ? (2.0 * a * b) : (1.0 - 2.0 * (1.0 - a) * (1.0 - b));
}