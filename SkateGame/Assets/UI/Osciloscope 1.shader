Shader "Hobscure/Osciloscope"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BGColor("BGColor", Color) = (0,0,0,0)

		_Value ("Value", float) = 0.
		_Timer ("Timer", float) = 0.
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		GrabPass
		{
			"_PreviousFrame"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			float _Value;

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float3 _BGColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 pf_uv = float2(i.uv.x+0.1, i.uv.y);
				half4 bgcolor = tex2D(_MainTex, pf_uv);
				float4 col = float4(_BGColor, 1.);

				if (i.uv.x >= _Value-0.05 && i.uv.x <= _Value + 0.05) {
					col = float4(1.0, 1.0, 1.0, 1.0);
				}

				// apply fog
				
				return col;
			}
			ENDCG
		}
	}
}
