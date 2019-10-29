Shader "Unlit/ForceField"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise Tex", 2D) = "white" {}

		_Offset("Offset", Range(-1, 1)) = 0
		_VertDistort("Vert Distort", Range(0, 0.15)) = 0
		_FragDistort("Frag Distort", Range(0, 0.15)) = 0

		[HDR]
		_Emission("Emission", Color) = (0.4,0.4,0.4,1)

		_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
		_FresnelIntensity("Fresnel Intensity", Range(0.5,1.5)) = 1
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		

        Pass
        {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Cull off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float2 depth : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
			sampler2D _NoiseTex;

            float4 _MainTex_ST;
			float4 _NoiseTex_ST;

			half _Offset;
			half _VertDistort;
			half _FragDistort;

			float4 _Emission;

			half4 _FresnelColor;
			half _FresnelIntensity;

			sampler2D _CameraDepthTexture;

            v2f vert (appdata v)
            {
                v2f o;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				float4 vertex = mul(UNITY_MATRIX_MV,v.vertex);
				float4 tex = tex2Dlod(_NoiseTex, float4(v.uv.x + _Time.x, v.uv.y + _Time.x, 0, 0)) * _VertDistort;
				vertex = float4(vertex.x + tex.x * - o.worldNormal.x,
					vertex.y + tex.x * -o.worldNormal.y,
					vertex.z + tex.x * -o.worldNormal.z, 
					vertex.w);

				vertex = mul(UNITY_MATRIX_P, vertex);
                o.vertex = vertex;

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);

                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				half2 noise = tex2D(_NoiseTex, i.uv).xy;
				noise = ((noise * 2) - 1) * _FragDistort * sin(_Time.y);

				//half2 noiseUV = half2((i.uv.x) + cos(noise * _Time.y), (i.uv.y) + sin(noise * _Time.y));
				half4 mainTex = tex2D(_MainTex, i.uv + noise);

				//Get Depth Diff
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
				float existingDepthLinear = LinearEyeDepth(existingDepth01);

				float depthDifference = existingDepthLinear - (i.screenPosition.w - _Offset);
				
				float smoothAlpha = smoothstep(0, 0.15, 1 - depthDifference);

				//Add Fresnel
				float3 worldNormal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);

				float fresnel = dot(worldNormal, viewDir);
				fresnel = saturate(1 - fresnel);
				fresnel = pow(fresnel, _FresnelIntensity);
				float4 fresnelColor = fresnel * _FresnelColor;

				half4 result = half4(_Emission.rgb, smoothAlpha) + fresnelColor;

				return  (result) * mainTex;
			}
            ENDCG
        }
    }
}
