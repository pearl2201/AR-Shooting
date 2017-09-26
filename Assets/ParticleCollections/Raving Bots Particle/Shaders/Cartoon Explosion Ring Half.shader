// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/Cartoon Explosion/Ring Half"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Mask ("Mask", Range(0, 1)) = 0.5
		_Divider ("Divider", Range(-1, 1)) = 1
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
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

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

			float _Mask;
			float _Divider;
			
			fixed SampleMask(float2 texcoord, fixed a)
			{
				texcoord = texcoord/_Mask - (0.5/_Mask - 0.5) * float2(1, 1);
				
				if (texcoord.x < 0 || texcoord.x > 1 || texcoord.y < 0 || texcoord.y > 1)
					return 0;
				
				return SampleSpriteTexture(texcoord).a * a;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				if (sign(_Divider) * IN.texcoord.y > sign(_Divider) * 0.5)
					return fixed4(0, 0, 0, 0);
				
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				
				c.a -= _Mask > 0 ? SampleMask(IN.texcoord, IN.color.a) : 0;
				
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
