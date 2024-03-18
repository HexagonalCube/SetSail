Shader "Hidden/PrismFlares" {
	Properties{
		_MainTex("", 2D) = "white" {}
	}

		HLSLINCLUDE
#include "CUPPCore.hlsl"
#include "PRISMCG.hlsl"
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0



		//UNITY_DECLARE_TEX2D(_MainTex);
		//float4 _MainTex_TexelSize;

		//UNITY_DECLARE_TEX2D(_FlareStarburstTex);
		//float4 _FlareStarburstTex_TexelSize;


		//UNITY_DECLARE_TEX2D(_FinalFlareTex_ST);
		//float4 _FinalFlareTex_TexelSize;
		uniform sampler2D _MainTex;
	half4 _MainTex_TexelSize;


	const float4 ZEROES = float4(0.0, 0.0, 0.0, 0.0);


	uniform float _FlareGhostDispersal;
	uniform int _FlareNumberOfGhosts;
	//uniform sampler2D _FlareRadialColorTex;
	//half4 _FlareRadialColorTex_ST;
	uniform sampler2D _FlareStarburstTex;
	half4 _FlareStarburstTex_TexelSize;
	uniform sampler2D _FinalFlareTex;
	half4 _FinalFlareTex_ST;
	half4 _FinalFlareTex_TexelSize;
	uniform float _FlareHaloWidth;
	uniform float _FlareChromaticDistortion;
	uniform float _FlareStrength;
	uniform float _FlareDirtIntensity;

	// fixed for Single Pass stereo rendering method
	inline float3 sampleTexChromatic(sampler2D tex, half4 tex_ST, float2 uv, float2 direction, float3 distortion)
	{
		return float3(
			tex2D(tex, (uv + direction * distortion.r)).r,
			tex2D(tex, (uv + direction * distortion.g)).g,
			tex2D(tex, (uv + direction * distortion.b)).b
			);
	}

	float4 KawaseBlur(sampler2D s, float2 uv, int iteration)//float2 pixelSize, 
	{
		//Dont need anymorefloat2 texCoordSample = 0;
		float2 pixelSize = _MainTex_TexelSize.xy * 0.5;//(float2)(1.0 / _ScreenParams.xy); //_MainTex_TexelSize.wx;
		float2 halfPixelSize = pixelSize / 2.0;
		float2 dUV = (pixelSize.xy * float(iteration)) + halfPixelSize.xy;
		float4 col;
		//We probably save like 1 operation from this, lol
		float4 cheekySample = float4(uv.x, uv.x, uv.y, uv.y) + float4(-dUV.x, dUV.x, dUV.y, dUV.y);
		float4 cheekySample2 = float4(uv.x, uv.x, uv.y, uv.y) + float4(dUV.x, -dUV.x, -dUV.y, -dUV.y);

		// Sample top left pixel
		col = tex2D(s, cheekySample.rb);
		// Sample top right pixel
		col += tex2D(s, cheekySample.ga);
		// Sample bottom right pixel
		col += tex2D(s, cheekySample.rb);
		// Sample bottom left pixel
		col += tex2D(s, cheekySample.ga);
		// Average
		col *= 0.25f;
		//return tex2D(s, uv);

#if 1
		float val = (col.x + col.y + col.z) / 3.0;
		return col * smoothstep(1.0 - 0.1, 1.0 + 0.1, val);
#endif

		return col;
	}

	float4 fragMedRGB(PostProcessVaryings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);

		float2 ooRes = _MainTex_TexelSize.xy;//_ScreenParams.w;

		float4 midCol = tex2D(_MainTex, uv);

		// -1  1  1
		// -1  0  1 
		// -1 -1  1  //3x3=9	
		float3 v[9];

		// Add the pixels which make up our window to the pixel array.
		UNITY_UNROLL
		for (int dX = -1; dX <= 1; ++dX)
		{
		UNITY_UNROLL
			for (int dY = -1; dY <= 1; ++dY)
			{
				float2 ofst = float2(float(dX), float(dY));

				// If a pixel in the window is located at (x+dX, y+dY), put it at index (dX + R)(2R + 1) + (dY + R) of the
				// pixel array. This will fill the pixel array, with the top left pixel of the window at pixel[0] and the
				// bottom right pixel of the window at pixel[N-1].
				v[(dX + 1) * 3 + (dY + 1)] = (float3)tex2D(_MainTex, uv + ofst * ooRes).rgb;
			}
		}

		float3 temp;

		// Starting with a subset of size 6, remove the min and max each time
		mnmx6(v[0], v[1], v[2], v[3], v[4], v[5]);
		mnmx5(v[1], v[2], v[3], v[4], v[6]);
		mnmx4(v[2], v[3], v[4], v[7]);
		mnmx3(v[3], v[4], v[8]);

		return float4(v[4].rgb, midCol.a);
	}

		float4 fragDSThresh(PostProcessVaryings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);

		return KawaseBlur(_MainTex, uv, 1);
	}

		//http://john-chapman-graphics.blogspot.com.au/2013/02/pseudo-lens-flare.html//
		float4 fragFlareFeatures(PostProcessVaryings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);

		float2 pixelSize = _MainTex_TexelSize.xy;

		// ghost vector to image centre:
		float2 ghostVec = (float2(0.5,0.5) - uv) * _FlareGhostDispersal;
		//TODO PIXELSIZE THIS
		//Chromatic params
		float3 distortion = float3(-pixelSize.x * _FlareChromaticDistortion, 0.0, pixelSize.x * _FlareChromaticDistortion);

		float2 direction = normalize(ghostVec);

		// sample ghosts:  
		float4 col = ZEROES;

		for (int i = 0; i < _FlareNumberOfGhosts; ++i) {

			float2 offs = frac(uv + ghostVec * (float)i);

			//Bright spots from centre only!
			float weight = length(float2(0.5,0.5) - offs) / length(float2(0.5,0.5));
			weight = pow(1.0 - weight, 10.0);

			col.rgb += sampleTexChromatic(_MainTex, _MainTex_TexelSize, offs, direction, distortion).rgb * weight;
		}

		//Color the flare 
		//length(float2(0.5,0.5)) - uv
		col.rgb *= sampleTexChromatic(_FlareStarburstTex, _FlareStarburstTex_TexelSize, uv, direction, distortion).rgb / length(float2(0.5,0.5));

		// sample halo:// 
		float2 haloVec = normalize(ghostVec) * _FlareHaloWidth;
		float haloWeight = length(float2(0.5,0.5) - frac(uv + haloVec)) / length(float2(0.5,0.5));
		haloWeight = pow(1.0 - haloWeight, 5.0);

		col.rgb += sampleTexChromatic(_MainTex, _MainTex_TexelSize, uv + haloVec, direction, distortion).rgb * haloWeight;
		col.a = 1.0;



		//return tex2D(_MainTex, uv + haloVec);

		return col;
	}

	uniform float _CamRotation;

	inline float3 CombineFlare(float2 uv, float3 col, float4 dirt)
	{
		float2 starUV = uv;
		//Rotate starburst
#if !SHADER_API_METAL
		starUV.xy -= 0.5; // shift the center of the coordinates to (0,0)
		float s, c;
		sincos(radians(_CamRotation), s, c); // compute the sin and cosine
		float2x2 rotationMatrix = float2x2(c, -s, s, c);
		starUV = mul(starUV, rotationMatrix);
		starUV += 0.5; // shift the center of the coordinates back to (0.5,0.5)
#endif

//Scale flare tex
		float2 scaleCenter = float2(0.5f, 0.5f);
		starUV = (starUV - scaleCenter) * 0.7 + scaleCenter;

		float4 lensStarburst = tex2D(_FlareStarburstTex, starUV);
		float4 lensFlare = tex2D(_FinalFlareTex, uv) * _FlareStrength * lensStarburst;
		lensFlare += dirt * lensFlare * _FlareDirtIntensity;
		return col.rgb + lensFlare.rgb;
	}

	inline float3 CombineFlareLight(float2 uv, float3 col)
	{
		float4 lensFlare = tex2D(_FinalFlareTex, uv);
		col.rgb += lensFlare.rgb * _FlareStrength;
		return col;
	}

	float4 fragFlareAdd(PostProcessVaryings input) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);

		float4 col = tex2D(_MainTex, uv);
		//
		col.rgb = CombineFlare(uv,col.rgb, ZEROES);

		return col;
	}


		ENDHLSL

		SubShader {

		ZTest Always Cull Off ZWrite Off

			//0 - Flare Features
			Pass{
				HLSLPROGRAM
					#pragma vertex FullScreenTrianglePostProcessVertexProgram
					#pragma fragment fragFlareFeatures
				ENDHLSL
		}

			//1 - Flare Add
			Pass{
				HLSLPROGRAM
					#pragma vertex FullScreenTrianglePostProcessVertexProgram
					#pragma fragment fragFlareAdd
				ENDHLSL
		}

			//2 - DownsampleAndThresh
			Pass{
				HLSLPROGRAM
					#pragma vertex FullScreenTrianglePostProcessVertexProgram
					#pragma fragment fragDSThresh
				ENDHLSL
		}

			//3 - Med
			Pass{
				HLSLPROGRAM
					#pragma vertex FullScreenTrianglePostProcessVertexProgram
					#pragma fragment fragMedRGB
				ENDHLSL
		}

			////





	}
















	FallBack off
}
