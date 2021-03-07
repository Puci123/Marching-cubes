Shader "Unlit/TeriainSurafaceTest"
{
    Properties
    {
	
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
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
				float3 normals : NORMAL;
            };

            struct v2f
            {
				float3 normals : TEXCOORD1;

                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
 

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normals = v.normals;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float4 normalsVal = 0;
				normalsVal.xyz = i.normals * 0.5f + 0.5f;
				return normalsVal;
            }
            ENDCG
        }
    }
}
