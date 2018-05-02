// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Headjack/YUV"
{
	Properties
	{
		_MainTex ("empty", 2D) = "black" {}
		_YTex("Y", 2D) = "black" {}
		_UTex("U", 2D) = "gray" {}
		_VTex("V", 2D) = "gray" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			sampler2D _MainTex,_YTex,_UTex,_VTex;
			float4 _MainTex_ST;
			v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.uv = TRANSFORM_TEX(uv, _MainTex);
				return o;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				float2 inv_uv = float2(i.uv.x, 1.0 - i.uv.y);
				float Y = tex2D(_YTex, inv_uv).a;
				float U = tex2D(_UTex, inv_uv).a-0.5;
				float V = tex2D(_VTex, inv_uv).a-0.5;
				//return 0;
				return pow(fixed4( ((float3((Y+ 1.39765625 * V),(Y - 0.390625 * U - 0.8125 * V),(Y + 2.015625 * U))-0.062745)/219)*255 , 1.0),1);
			}
			ENDCG
		}
	}
}
