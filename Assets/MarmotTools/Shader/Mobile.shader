Shader "Custom/MobilePBR" {

	Properties{

	 [Header(Shadow)]

	 [Toggle]_USESHADOW("Use Shadow&Light?(라이팅, 그림자 사용여부)", float) = 0



	 [Header(_______________________________________________________________________________________________________________________________________)]

	 [Header(Albedo)]

	 [HDR]_Color("Color", Color) = (1, 1, 1, 1)

	 _MainTex("Albedo (RGB)", 2D) = "gray" {}

	 [Header(Normal)]

	 [Toggle]_USENORMAL("UseNormalmap?(노멀맵사용)", float) = 0

	 [Normal][NoScaleOffset]_Normal("Normal", 2D) = "bump"{}

	 [Toggle]_USENORMALPOW("UseNormalPow?(노멀강도 사용여부)", float) = 0

	 _NormalPow("NormalPow(노멀강도)", Range(0, 3)) = 1



	 [Header(_______________________________________________________________________________________________________________________________________)]

	 [Header(Specular)]

	 [KeywordEnum(NoSpec, Spec, SpecHDR)]

	 _SPECTYPE("SpecType(스펙큘러 사용여부)", float) = 0

	 [PowerSlider(200)]_SpecPow("SpecPow(스펙큘러 면적)", Range(1, 200)) = 200

	 _SpecMul("SpecMul(스펙큘러 강도)", Range(0, 10)) = 10

	 [NoScaleOffset]_PBR("Metallic(RGB)Smoothness(A)", 2D) = "white"{}



	 [Header(_______________________________________________________________________________________________________________________________________)]

	 [Header(Emission)]

	 [Toggle]_EMISSIONUSE("Use Emission? (Emission 사용)", float) = 0

	 [NoScaleOffset]_Emission("Emission", 2D) = "white" {}

	 [HDR]_EmissionColor("EmissionColor", Color) = (0, 0, 0, 0)



	 [Header(_______________________________________________________________________________________________________________________________________)]

	 [Header(Reflection)]

	 [KeywordEnum(NoRef, Ref, RefHDR)]

	 _REFTYPE("RefType(반사 사용여부)", float) = 0

	 [PowerSlider(20)]_ReflectionPow("RefPow(반사대비)", Range(1, 20)) = 1

	 _ReflectionMul("RefMul(반사밝기)", Range(1, 10)) = 1.5

	 _ReflectionPow_InShadow("RefPow_InShadow(쉐도우 리플렉션)", Range(0, 3)) = 1

	}

		SubShader{

		 Tags { "RenderType" = "Opaque" }

		 LOD 200



		 CGPROGRAM

		 // Physically based Standard lighting model, and enable shadows on all light types

		 #pragma surface surf CumstomLighing fullforwardshadows nolightmap nodynlightmap nodirlightmap nofog nometa noforwardadd nolppv noshadowmask interpolateview



		 // Use shader model 3.0 target, to get nicer looking lighting

		 #pragma target 3.0

		 #pragma multi_compile _USENORMAL_OFF _USENORMAL_ON //노멀 사용여부

		 #pragma multi_compile _USENORMALPOW_OFF _USENORMALPOW_ON //노멀 파워

		 #pragma multi_compile _SPECTYPE_NOSPEC _SPECTYPE_SPEC _SPECTYPE_SPECHDR //스펙큘러

		 #pragma multi_compile _EMISSIONUSE_OFF _EMISSIONUSE_ON //에미션 사용

		 #pragma multi_compile _REFTYPE_NOREF _REFTYPE_REF _REFTYPE_REFHDR //반사

		 #pragma multi_compile _USESHADOW_OFF _USESHADOW_ON //그림자





		 sampler2D _MainTex;

		 sampler2D _Normal;

		 sampler2D _PBR;

		 sampler2D _Emission;



		 struct CustomData {

		  half3 Albedo;

		  half3 Normal;

		  half3 Emission;

		  half Alpha;



		  half3 RefCustomData;

		 };



		 struct Input {

		  float2 uv_MainTex;

		  //half2 uv_Normal;

		  half3 viewDir;



		  #if _REFTYPE_NOREF

		  #elif _REFTYPE_REF

		  half3 worldRefl; //WorldReflectionVector 데이터에 사용됩니다.

		  INTERNAL_DATA //worldRefl 데이터를 가져오기 위해 해당 데이터 구조를 필요로 함.

		  #elif _REFTYPE_REFHDR

		  half3 worldRefl; //WorldReflectionVector 데이터에 사용됩니다.

		  INTERNAL_DATA //worldRefl 데이터를 가져오기 위해 해당 데이터 구조를 필요로 함.

		  #endif



		 };



		 fixed4 _Color;

		 half _NormalPow;

		 half _SpecPow;

		 half _SpecMul;

		 half3 _EmissionColor;

		 half _RefView;

		 half _ReflectionPow;

		 half _ReflectionMul;

		 half _ReflectionPow_InShadow;



		 // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.

		 // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.

		 // #pragma instancing_options assumeuniformscaling

		 UNITY_INSTANCING_BUFFER_START(Props)

			 // put more per-instance properties here

			UNITY_INSTANCING_BUFFER_END(Props)



			void surf(Input IN, inout CustomData o) {





			 //Normal

			 #if _USENORMAL_ON

			 half3 NormalFinal = UnpackNormal(tex2D(_Normal, IN.uv_MainTex));



			  #if _USENORMALPOW_ON

			  o.Normal = lerp(o.Normal, NormalFinal, _NormalPow);

			  #elif _USENORMALPOW_OFF

			  o.Normal = NormalFinal;

			  #endif



			 #elif _USENORMAL_OFF

			 #endif



			 //뷰 디렉션 내적

			 half NdotV = dot(o.Normal, IN.viewDir) * 0.5 + 0.5;



			 //DiffuseColor

			 half3 c = tex2D(_MainTex, IN.uv_MainTex);



			 //Metallic

			 half4 _PBRMask = tex2D(_PBR, IN.uv_MainTex);



			 //Spec

			 #if _SPECTYPE_SPEC //기본 스펙큘러

			 half3 SpecFinal = saturate(pow(NdotV, _SpecPow) * _PBRMask.a * c * _SpecMul); //보간

			 #elif _SPECTYPE_SPECHDR //HDR스펙큘러

			 half3 SpecFinal = pow(NdotV, _SpecPow) * _PBRMask.a * (c * _SpecMul); //HDR

			 #elif _SPECTYPE_NOSPEC //스펙큘러 사용안함

			 #endif



			  //Albedo

			 #if _SPECTYPE_SPEC //스펙큘러 사용

			 half3 AlbedoFinal = c * NdotV + SpecFinal;

			 #elif _SPECTYPE_SPECHDR //스펙큘러 사용

			 half3 AlbedoFinal = c * NdotV + SpecFinal;

			 #elif _SPECTYPE_NOSPEC //스펙큘러 사용안함

			 half3 AlbedoFinal = c * NdotV;

			 #endif



			 //Emission

			 #if _EMISSIONUSE_OFF

			 #elif _EMISSIONUSE_ON

			 half3 EmissionFinal = tex2D(_Emission, IN.uv_MainTex) * _EmissionColor;

			 o.Emission = EmissionFinal;

			 #endif



			 //Reflection

			 #if _REFTYPE_NOREF



			 #elif _REFTYPE_REF

			 half3 Reflection1 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, WorldReflectionVector(IN, o.Normal)) * unity_SpecCube0_HDR.r; //unity_SpecCube0_HDR.r : 리플렉션 큐브의 Intensity값.

			 half3 Reflection2 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0, WorldReflectionVector(IN, o.Normal)) * unity_SpecCube0_HDR.r; //Reflection2는 다른 리플렉션 프로브와의 보간을 처리합니다. 필요없는 경우 제거 하십시오. //unity_SpecCube0_BoxMin.w : 두개의 리플렉션 프로브를 보간합니다.

			 half3 ReflectionFinal = clamp((pow(lerp(Reflection2, Reflection1, unity_SpecCube0_BoxMin.w), _ReflectionPow) * _ReflectionMul), 0, 2); //최대 밝기 보간.



			 #elif _REFTYPE_REFHDR

			 half3 Reflection1 = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, WorldReflectionVector(IN, o.Normal)) * unity_SpecCube0_HDR.r; //unity_SpecCube0_HDR.r : 리플렉션 큐브의 Intensity값.

			 half3 Reflection2 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0, WorldReflectionVector(IN, o.Normal)) * unity_SpecCube0_HDR.r; //Reflection2는 다른 리플렉션 프로브와의 보간을 처리합니다. 필요없는 경우 제거 하십시오. //unity_SpecCube0_BoxMin.w : 두개의 리플렉션 프로브를 보간합니다.

			 half3 ReflectionFinal = pow(lerp(Reflection2, Reflection1, unity_SpecCube0_BoxMin.w), _ReflectionPow) * _ReflectionMul;

			 #endif



			 //Albedo

			 #if _REFTYPE_NOREF

			 o.Albedo = AlbedoFinal * _Color;

			 #elif _REFTYPE_REF

			 o.Albedo = lerp(AlbedoFinal, ReflectionFinal * c * _PBRMask.rgb

			   #if _SPECTYPE_SPEC //스펙큘러 사용

			   + SpecFinal

			   #elif _SPECTYPE_SPECHDR //스펙큘러 사용

			   + SpecFinal

			   #endif

			  , _PBRMask.rgb) * _Color;

			 #elif _REFTYPE_REFHDR

			 o.Albedo = lerp(AlbedoFinal, ReflectionFinal * c * _PBRMask.rgb

			   #if _SPECTYPE_SPEC //스펙큘러 사용

			   + SpecFinal

			   #elif _SPECTYPE_SPECHDR //스펙큘러 사용

			   + SpecFinal

			   #endif

			  , _PBRMask.rgb) * _Color;

			 #endif



			 o.Alpha = 1;



			 //ReflectionData

			 #if _REFTYPE_NOREF

			 #elif _REFTYPE_REF

			 o.RefCustomData = ReflectionFinal * c * _PBRMask.rgb;

			 #elif _REFTYPE_REFHDR

			 o.RefCustomData = ReflectionFinal * c * _PBRMask.rgb;

			 #endif

			}



			half4 LightingCumstomLighing(CustomData s, half3 lightDir, half atten)

			{

			 half4 c;



			 #if _USESHADOW_OFF

			 #elif _USESHADOW_ON

			 half3 shadowMask = atten * _LightColor0; //그림자 + 라이팅컬러

			 #endif



			 #if _REFTYPE_NOREF

			 c.rgb = s.Albedo

				 //쉐도우 적용

				 #if _USESHADOW_OFF

				 #elif _USESHADOW_ON

				 * shadowMask

				 #endif

				 ;

			 #elif _REFTYPE_REF

			  #if _USESHADOW_ON

			  half3 RefColor = lerp(s.RefCustomData * _ReflectionPow_InShadow, shadowMask, saturate(shadowMask)); //그림자 내에서의 반사

			  #elif _USESHADOW_OFF

			  half3 RefColor = s.RefCustomData; //그림자 내에서의 반사

			  #endif

			 c.rgb = s.Albedo * RefColor;



			 #elif _REFTYPE_REFHDR

			  #if _USESHADOW_ON

			  half3 RefColor = lerp(s.RefCustomData * _ReflectionPow_InShadow, shadowMask, saturate(shadowMask)); //그림자 내에서의 반사

			  #elif _USESHADOW_OFF

			  half3 RefColor = s.RefCustomData; //그림자 내에서의 반사

			  #endif

			 c.rgb = s.Albedo * RefColor;

			 #endif



			 c.a = s.Alpha;

			 return c;

			}

			ENDCG

	 }

		 FallBack "Diffuse"

}