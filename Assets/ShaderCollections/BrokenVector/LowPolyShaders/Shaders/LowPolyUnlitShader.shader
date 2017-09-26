Shader "LowPolyShaders/LowPolyUnlitShader" {
	Properties {
		_MainTex ("Color Scheme", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		PASS{
		Lighting Off
		
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		         #include "UnityCG.cginc"
		sampler2D _MainTex;
		fixed4 _Color;

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

            float4 SoftLight (float4 a, float4 b) {
    float4 r = float4(0,0,0,1);
    if (b.r > 0.5) { r.r = a.r*(1-(1-a.r)*(1-2*(b.r))); }
    else { r.r = 1-(1-a.r)*(1-(a.r*(2*b.r))); }
    if (b.g > 0.5) { r.g = a.g*(1-(1-a.g)*(1-2*(b.g))); }
    else { r.g = 1-(1-a.g)*(1-(a.g*(2*b.g))); }
    if (b.b > 0.5) { r.b = a.b*(1-(1-a.b)*(1-2*(b.b))); }
    else { r.b = 1-(1-a.b)*(1-(a.b*(2*b.b))); }
    return r;
}

float4 HardLight (float4 a, float4 b) {
    float4 r = float4(0,0,0,1);
    if (b.r > 0.5) { r.r = 1-(1-a.r)*(1-2*(b.r)); }
    else { r.r = a.r*(2*b.r); }
    if (b.g > 0.5) { r.g = 1-(1-a.g)*(1-2*(b.g)); }
    else { r.g = a.g*(2*b.g); }
    if (b.b > 0.5) { r.b = 1-(1-a.b)*(1-2*(b.b)); }
    else { r.b = a.b*(2*b.b); }
    return r;
}

float4 Multiply (float4 a, float4 b) { return (a * b); }

float4 Lighten (float4 a, float4 b) { return float4(max(a.rgb, b.rgb), 1); }
float4 ColorBurn (float4 a, float4 b) { return (1-(1-a)/b); }
           
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               
                return o;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
             
                return Multiply(col,_Color);
            }

		ENDCG
		}
	}
	FallBack "Unlit/Texture"
}
