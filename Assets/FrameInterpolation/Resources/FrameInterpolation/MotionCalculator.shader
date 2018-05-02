Shader "Unlit/MotionCalculator"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Next("Next",2D)="black"{}
		_Previous("Previous",2D)="black"{}
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

			sampler2D _Previous, _Next;
			float4 _MainTex_TexelSize;
			
			v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.uv = uv;
				return o;
			}
			fixed4 c;
			half PixV(sampler2D Target, half2 Shift, fixed2 iuv)
			{
				c=tex2D(Target,iuv);
				c += tex2D(Target, iuv + Shift+ (half2(1, 0)*_MainTex_TexelSize.xy));
				c += tex2D(Target, iuv + Shift + (half2(0, 1)*_MainTex_TexelSize.xy));
				c += tex2D(Target, iuv + Shift + (half2(-1, 0)*_MainTex_TexelSize.xy));
				c += tex2D(Target, iuv + Shift + (half2(0, -1)*_MainTex_TexelSize.xy));
				return c.r + c.g + c.b;
			}
			half CheckValue, FirstValue, PrevValue, LastMatch;
			float2 FinalUvShift,CheckUvShift;
			fixed3 cn, cp;
			fixed4 frag (v2f i) : SV_Target
			{
				PrevValue=PixV(_Previous,half2(0,0),i.uv);
				LastMatch=abs(PrevValue-PixV(_Next,half2(0,0),i.uv));			
				for(int Circle=1;Circle<4;++Circle)
				{
					for(int j=0;j<4*Circle;++j)
					{
						if ((CheckValue=abs(PrevValue-PixV(_Next, CheckUvShift=half2(sin((FirstValue = 6.28 / (4*Circle))*j),cos(FirstValue*j))*_MainTex_TexelSize.xy*Circle, i.uv)))<LastMatch)
						{
							LastMatch=CheckValue;
							FinalUvShift=CheckUvShift;
						}
					}
				}
				return fixed4( FinalUvShift.xy,0,1);
				
			}
			ENDCG
		}
	}
}
