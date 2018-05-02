Shader "Hidden/Sum"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_A ("Texture", 2D) = "white" {}
		_B("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _A,_B;

			float4 frag (v2f i) : SV_Target
			{
				float4 result = 0;
				float multiplier = 1.0 / 32.0;
				for (int yy = 0;yy < 32;yy++)
				{
					for (int xx = 0;xx < 32;xx++)
					{
						result += abs(tex2D(_A, float2(xx, yy)*multiplier) - tex2D(_B, float2(xx, yy)*multiplier));
					}
				}
				return result;
			}
			ENDCG
		}
	}
}
