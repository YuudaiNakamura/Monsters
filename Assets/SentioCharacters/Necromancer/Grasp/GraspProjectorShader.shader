// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'

Shader "Necromancer/GraspProjectorShader"
{ 
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_Multiplier ("Multiplier", Range(0.0, 3.0)) = 3.0
		_Decal ("Decal", 2D) = "" {}
		_Falloff ("Falloff", 2D) = "" {}
		_AspectMultiplier ("Aspect Multiplier", Range(0.0, 1.0)) = 1.0
		_PixelMultiplier ("Pixel Multiplier", Range(0.0, 4.0)) = 4.0
		_Distance ("Distance", Range(0.0, 1.0)) = 0.0
		_AnimationDistance ("Animation Distance", Range(0.0, 1.0)) = 0.0
		_Offset ("Offset", Range(0.0, 2.0)) = 0.0
	}
	 
	Subshader
	{
		Tags { "Queue" = "Transparent" }

		Pass
		{
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha One
			Offset -1, -1
	 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			fixed4 _Color;
			float _Multiplier;
			sampler2D _Decal;
			sampler2D _Falloff;

			float _AspectMultiplier;
			float _PixelMultiplier;
			float _Distance;
			float _AnimationDistance;
			float _Offset;
			
			float4x4 unity_Projector;
			
			struct v2f
			{
				float4 pos : POSITION;
				float4 uvDecal : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
			};
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uvDecal = (mul(unity_Projector, v.vertex) - float4((_Offset + _Distance) / _PixelMultiplier, 0, 0, 0)) * float4(_AspectMultiplier, 1, 1, 1);
				o.uvFalloff = mul(unity_Projector, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 tDecalTex = tex2Dproj(_Decal, UNITY_PROJ_COORD(i.uvDecal));
				fixed4 tFalloffTex = tex2Dproj(_Falloff, UNITY_PROJ_COORD(i.uvFalloff));

				float tDistance = _AnimationDistance + (i.uvFalloff.x * _PixelMultiplier - _Distance);

				float tFirstExpand = saturate(tDecalTex.r * (tDistance - 0));
				float tSecondExpand = saturate(tDecalTex.g * (tDistance - 1) * 0.5);
				float tThirdExpand = saturate(tDecalTex.b * (tDistance - 2) * 0.25);

				fixed4 tColor;
				tColor.rgb = _Color.rgb * tDecalTex.a;
				tColor.a = _Color.a * tDecalTex.a * tFalloffTex.r * (tFirstExpand + tSecondExpand + tThirdExpand);
				
				tColor = saturate(tColor * _Multiplier);

				return tColor;
			}

			ENDCG
		}
	}
}
