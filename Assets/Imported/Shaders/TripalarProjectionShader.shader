Shader "Unlit/TripalarProjectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_TexturScale("Texture Scale",float) = 1
		_TripalnarBlenderSharpnes("Blend Sharpnes",float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			
			float _TexturScale;
			float _TripalnarBlenderSharpnes;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = mul(unity_ObjectToWorld, v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
				//Caluclate Uvs for each axis
				float2 xUv = i.worldPos.xz / _TexturScale;
				float2 yUv = i.worldPos.zy / _TexturScale;
				float2 zUv = i.worldPos.xy / _TexturScale;

				//SampleTexture for each axis
				float3 xText = tex2D(_MainTex, xUv);
				float3 yText = tex2D(_MainTex, yUv);
				float3 zText = tex2D(_MainTex, zUv);


				//Caluclalte Blend weight for nomrals
				//Greater blend value more sharper
				float3 blendsWeights = pow(abs(i.normal), _TripalnarBlenderSharpnes);
				blendsWeights = blendsWeights / (blendsWeights.x + blendsWeights.y + blendsWeights.z);  //Normalizde <-1,1>

				//Blending 
				float3 col = (xText * blendsWeights.x, yText * blendsWeights.y, zText * blendsWeights.z);


                return float4(col.xyz,1);
            }
            ENDCG
        }
    }
}
