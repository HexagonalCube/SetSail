
#define s2(a, b)				temp = a; a = min(a, b); b = max(temp, b);
#define mn3(a, b, c)			s2(a, b); s2(a, c);
#define mx3(a, b, c)			s2(b, c); s2(a, c);

//Roman Galashov (RomBinDaHouse) operator - https://www.shadertoy.com/view/MssXz7
half3 filmicTonemapRomBOld(in float3 col, in float gammaval)
{
	//return exp(_ToneParams.x / (_ToneParams.y * col.rgb + _ToneParams.z));
	gammaval = max(0.01, gammaval);
	col = saturate(exp(-1.0 / (2.72*col + gammaval)));
	//col = pow(col, (float3)(gammaval / 1.));
	
	return col;
}

// Compatibility function
float fastReciprocal(float value)
{
	return 1. / value;
}

// Tonemapper from http://gpuopen.com/optimized-reversible-tonemapper-for-resolve/
float4 FastToneMapPRISM(in float4 color)
{
	return color;
	float v[2];
	v[0] = color.r;
	v[1] = color.g;
	v[2] = color.b;//

	float3 temp;
	mx3(v[0], v[1], v[2]);

	return float4(color.rgb * fastReciprocal(
		v[1]
		+ 1.)
		, color.a
		);
}