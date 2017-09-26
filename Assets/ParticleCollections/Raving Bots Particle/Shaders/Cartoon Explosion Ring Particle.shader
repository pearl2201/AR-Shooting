// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/Cartoon Explosion/Ring Particle"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_TintColor ("Tint", Color) = (1,1,1,0.5)
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _TintColor;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _TintColor;

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			
			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				//color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed SampleMask(float2 texcoord, fixed a)
			{
				texcoord = texcoord/a - (0.5/a - 0.5) * float2(1, 1);
				
				if (texcoord.x < 0 || texcoord.x > 1 || texcoord.y < 0 || texcoord.y > 1)
					return 0;
				
				return SampleSpriteTexture(texcoord).a;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed a = IN.color.a < 1 ? SampleMask(IN.texcoord, 1 - IN.color.a) : 0;
				IN.color.a = 1;
			
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				
				c.a -= a;
				
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
