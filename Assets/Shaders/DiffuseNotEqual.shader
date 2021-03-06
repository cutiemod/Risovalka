﻿Shader "Custom/Diffuse NotEquals" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_StencilNum("Stencil number", Int) = 1
	}
		SubShader{
			Tags { "RenderType" = "Opaque" "Queue" = "Geometry"}
			LOD 200

		Stencil
		{
			Ref [_StencilNum]
			Comp notequal
			Pass keep
		}

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

		//Fallback "Legacy Shaders/VertexLit"
}