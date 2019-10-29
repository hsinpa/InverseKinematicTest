Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Scale("Scale", Range(0,4)) = 1

		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

    }
    SubShader
    {
		Tags
		{
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}
		LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

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
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			float4 _AmbientColor;
			int _Scale;

			sampler2D _CameraDepthTexture;
			float4 _CameraDepthTexture_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float halfScaleFloor = floor(_Scale * 0.5);
				float halfScaleCeil = ceil(_Scale * 0.5);

				float4 bottomLeftUV = float4(i.screenPosition.xy - float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleFloor, i.screenPosition.zw);
				float4 topRightUV = float4(i.screenPosition.xy + float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleCeil, i.screenPosition.zw);
				float4 bottomRightUV = float4(i.screenPosition.xy + float2(_CameraDepthTexture_TexelSize.x * halfScaleCeil, -_CameraDepthTexture_TexelSize.y * halfScaleFloor), i.screenPosition.zw);
				float4 topLeftUV = float4(i.screenPosition.xy + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, _CameraDepthTexture_TexelSize.y * halfScaleCeil), i.screenPosition.zw);

				float depth0 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
				float depth1 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(topRightUV)).r;
				float depth2 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(bottomRightUV)).r;
				float depth3 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(topLeftUV)).r;

				float existingDepthLinear = LinearEyeDepth(depth0);

				float depthDifference = existingDepthLinear - (i.screenPosition.w);

				return i.screenPosition.w;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

				//Diffuse
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);
				float4 light = NdotL * _LightColor0;



                return (col + _AmbientColor) *(light);
            }
            ENDCG
        }
    }
}
