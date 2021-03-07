Shader "Custom/TriplanerShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
		[NoScaleOffset] _MOSMap("Main MOS Mask", 2D) = "black" {}

		_TopTex("Up texture", 2D) = "white" {}
		[NoScaleOffset] _TOPMOSMap("Top MOS Mask", 2D) = "black" {}
		
		_Scale("Map scale",float) = 1
		_BlendOfeset("Blend offset",Range(0,0.5)) = 0
		_BlendSharpnes("Blend sharpnes",Range(1,10)) = 2
		_TopOffset("Top offset",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _MOSMap;
		
		sampler2D _TopTex;
		sampler2D _TOPMOSMap;

		float _Scale;
		float _BlendOfeset;
		float _BlendSharpnes;
		float _TopOffset;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
			float3 Normal;
        };

		struct TripMapiningUV 
		{
			float2 x, y, z;
		};

        
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

		TripMapiningUV Trimap(Input IN) 
		{
			TripMapiningUV triUV;

			//Apaling saclae
			float3 p = IN.worldPos * _Scale;

			//Maping evy axis
			triUV.x = p.zy;
			triUV.y = p.xz;
			triUV.z = p.xy;

			return triUV;
		}

		float3 BlendWeight(Input IN) 
		{
			float3 blednWeight = abs(IN.worldNormal);
			blednWeight = saturate(blednWeight - _BlendOfeset); //clamp01 //
			blednWeight = pow(blednWeight , _BlendSharpnes);		//chaneg weight basic on blend sharpnes //

			return blednWeight / (blednWeight.x + blednWeight.y + blednWeight.z);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

			TripMapiningUV triUV = Trimap(IN);
			float3 weights = BlendWeight(IN);

			//albedo
			float3 albedoX = tex2D(_MainTex, triUV.x);
			float3 albedoY = albedoY = tex2D(_MainTex, triUV.y);
			float3 albedoZ = tex2D(_MainTex, triUV.z);

			//mos map
			float4 mosX = tex2D(_MOSMap, triUV.x); 
			float4 mosY = mosY = tex2D(_MOSMap, triUV.y);
			float4 mosZ = tex2D(_MOSMap, triUV.z); 

			if (IN.worldNormal.y > _TopOffset)
			{
				 albedoY = tex2D(_TopTex, triUV.y);
				 mosY = tex2D(_TOPMOSMap, triUV.y);
			}

			//blending
			float3 surfAalbedo = albedoX * weights.x + albedoY * weights.y + albedoZ * weights.z;
			float4 surafaceMos = mosX * weights.x + mosY * weights.y + mosZ * weights.z;

			//Outputing
            o.Albedo     = surfAalbedo;
			o.Metallic = surafaceMos.x;
			o.Smoothness = surafaceMos.a;
			//o.Occlusion = surafaceMos.y;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
