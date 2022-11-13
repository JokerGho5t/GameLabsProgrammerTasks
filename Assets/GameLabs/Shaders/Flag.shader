Shader "Custom/Flag" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Amplitude ("Amplitude", Float) = 1
		_WaveLength ("WaveLength", Float) = 10
		_Speed ("Speed", Float) = 1
		_ScrollSpeed("ScrollSpeed", Vector) = (0, 0, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		half4 _ScrollSpeed;
		fixed4 _Color;
		float _Amplitude, _WaveLength, _Speed;

		void vert(inout appdata_full vertexData)
		{
			float3 p = vertexData.vertex.xyz;

			float k = 2 * UNITY_PI / _WaveLength;
			float f = k * (p.x - _Speed * _Time.y);
			p.y = -vertexData.texcoord.x * _Amplitude * sin(f);

			float3 tangent = normalize(float3(1, k * _Amplitude * cos(f), 0));
			float3 normal = float3(-tangent.y, tangent.x, 0);

			vertexData.vertex.xyz = p;
			vertexData.normal = normal;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed2 offset = _ScrollSpeed * _Time;
			fixed2 mainUV = IN.uv_MainTex + offset;
				
			fixed4 c = tex2D (_MainTex, mainUV) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}