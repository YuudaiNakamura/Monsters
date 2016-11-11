// This is just a copy of the additive particle shader with the queue increased by 1 so that it will
// appear in front of the smoke.  If this is causing problems you could remove the smoke trail and use
// the regular additive particle shader.
Shader "Necromancer/FireballParticleShader"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}

	Category
	{
		Tags
		{
			"Queue" = "Transparent+1"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Color (0,0,0,0) }
	
		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				fixed4 _TintColor;
						
				float4 _MainTex_ST;
				
				sampler2D _CameraDepthTexture;
				float _InvFade;

				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD1;
					#endif
				};

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos (o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
			
				fixed4 frag (v2f i) : COLOR
				{
					#ifdef SOFTPARTICLES_ON
					float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
					float partZ = i.projPos.z;
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
					#endif
				
					return 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				}
				ENDCG 
			}
		}	
	}
}
