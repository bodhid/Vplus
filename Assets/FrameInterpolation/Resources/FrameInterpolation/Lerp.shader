Shader "LerpMat"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Last ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			v2f vert (float4 vertex : POSITION,float2 uv : TEXCOORD0)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.uv = uv;
				return o;
			}
			sampler2D _MainTex, _Last;
			float4 frag (v2f i) : SV_Target
			{
				float4 newColor = tex2D(_MainTex, i.uv);
				//return newColor;
				return lerp(newColor,tex2D(_Last, i.uv),0.5);
			}
			ENDCG
		}
	}
}
