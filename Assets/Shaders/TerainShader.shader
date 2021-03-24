Shader "Custom/Terein Shader"
{
    Properties
    {
		[NoScaleOffset]_MainTex ("Main Texture", 2D) = "white" {}
		[NoScaleOffset] _MOSMap("Main MOS Mask", 2D) = "white" {}
		[NoScaleOffset] _NormalMap("Main Normal map", 2D) = "white" {}

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

		//main 
        sampler2D _MainTex;
		sampler2D _MOSMap;
		sampler2D _NormalMap;

		

		//top
		sampler2D _TopTex;
		sampler2D _TOPMOSMap;
		sampler2D _TopNormalMap;


		
		float _Scale;
		float _BlendOfeset;
		float _BlendSharpnes;
		float _TopOffset;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal; INTERNAL_DATA
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

			if (IN.Normal.x < 0) 
			{
				triUV.x.x = -triUV.x.x;
			}
			if (IN.Normal.y < 0)
			{
				triUV.y.x = -triUV.y.x;
			}
			if (IN.Normal.z >= 0)
			{
				triUV.z.x = -triUV.z.x;
			}
			

			triUV.x.y += 0.5f;
			triUV.z.x += 0.5f;

			return triUV;
		}

		float3 BlendSutrfaceWithWorldNormals(float3 mappedNormal, float3 surfaceNormal)
		{
			float3 n;
			n.xy = mappedNormal.xy + surfaceNormal.xy;
			n.z = mappedNormal.z * surfaceNormal.z;
			return n;
		}

		float3 BlendWeight(Input IN) 
		{
			float3 blednWeight = abs(IN.worldNormal);
			blednWeight = saturate(blednWeight - _BlendOfeset);		//clamp01 //
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
			float4 mosY = tex2D(_MOSMap, triUV.y);
			float4 mosZ = tex2D(_MOSMap, triUV.z); 

			//Normal map
			float3 tangentNormalX = UnpackNormal(tex2D(_NormalMap, triUV.x));
			float3 tangentNormalY = UnpackNormal(tex2D(_NormalMap, triUV.y));
			float3 tangentNormalZ = UnpackNormal(tex2D(_NormalMap, triUV.z));


			float3 worldNormalX = BlendSutrfaceWithWorldNormals(tangentNormalX, IN.Normal.zyx).zyx;
			float3 worldNormalY = BlendSutrfaceWithWorldNormals(tangentNormalY, IN.Normal.xzy).xzy;
			float3 worldNormalZ = BlendSutrfaceWithWorldNormals(tangentNormalZ, IN.Normal);
			//o.Normal = normalize(worldNormalX * weights.x + worldNormalY * weights.y + worldNormalZ * weights.z);

			//blending
			float3 surfAalbedo = albedoX * weights.x + albedoY * weights.y + albedoZ * weights.z;
			float4 surafaceMos = mosX * weights.x + mosY * weights.y + mosZ * weights.z;

			//Outputing
			o.Albedo = surfAalbedo;
			o.Metallic = surafaceMos.x;
			o.Smoothness = surafaceMos.a;
			o.Occlusion = surafaceMos.y;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
