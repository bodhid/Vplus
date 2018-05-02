Shader "Unlit/Interpolate"
{
	Properties
	{
		_Next("Next",2D)="black"{}
		_Previous("Previous",2D)="black"{}
		_Motion("Motion",2D)="blue"{}
		_TestShift("Test Shift",range(0,1))=0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		cull off
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
			v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.uv = uv;
				return o;
			}
			sampler2D _Previous, _Next, _Motion;
			half    _TestShift;
			fixed4 frag (v2f i) : SV_Target
			{
				half2 FinalUvShift=(((tex2D(_Motion,i.uv).rg)));
				return lerp(tex2D(_Previous, i.uv-(FinalUvShift*_TestShift)),tex2D(_Next, i.uv+(FinalUvShift*(1-_TestShift))),_TestShift);
			}
			ENDCG
		}
	}
}
