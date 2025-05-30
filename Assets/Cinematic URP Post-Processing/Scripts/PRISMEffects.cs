using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using PRISM.Utils;


// Define the Volume Component for the custom post processing effect 
[System.Serializable, VolumeComponentMenu("Cinematic URP Post Processing/CUPP - PRISM Effects")]
public class PRISMEffects : VolumeComponent
{
    [Tooltip("Controls the blending between the original and the grayscale color.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0, 1);
    public BoolParameter fastMode = new BoolParameter(false);
    public BoolParameter showInSceneView = new BoolParameter(false);

    [Header("PRISM.DEVELOP")]
    [InspectorName("Color Temperature (Warm>Cool)"), Tooltip("Controls the temperature of the color, used to offset light color for a neutral scene")]
    public ClampedFloatParameter colorTemperature = new ClampedFloatParameter(6500f, 1000f, 40000f);
    [InspectorName("Color Tint (Magenta>Green)"), Tooltip("Controls the tint of the color, used to make fine corrections")]
    public ClampedFloatParameter colorTint = new ClampedFloatParameter(0, -1, 1);
    [InspectorName("Exposure Compensation"), Tooltip("Adjusts the brightness of the image, in stops of brightness")]
    public ClampedFloatParameter exposure = new ClampedFloatParameter(0f, -5f, 20f);
    public BoolParameter useLinearSpace = new BoolParameter(false);


    public FloatRangeParameter lightness = new FloatRangeParameter(new Vector2(0f, 1f), 0f, 2f);
    [Range(-0.5f, 0.5f)] public ClampedFloatParameter lightnessSlope = new ClampedFloatParameter(0, 0, 1);
    [InspectorName("Green & Magenta Chromacity"), Tooltip("Controls the vibrance of green & magenta hues.")]
    [Range(-1f, 1f)] public ClampedFloatParameter labA_GreenAndMagenta = new ClampedFloatParameter(0, -1f, 1);
    [InspectorName("Yellow & Blue Chromacity"), Tooltip("Controls the vibrance of yellow & blue hues.")]
    [Range(-1f, 1f)] public ClampedFloatParameter labB_YellowAndBlue = new ClampedFloatParameter(0, -1f, 1);

    //Chromatic Aberration
    [Space]
    [Header("PRISM.DISTORTIONS")]
    public BoolParameter useLensDistortion = new BoolParameter(false);
    public ClampedFloatParameter barrelDistortion = new ClampedFloatParameter(0f, 0f, 1f);
    public BoolParameter useChromaticAberration = new BoolParameter(false);
    [Range(0.0f, 0.2f)] public FloatParameter glassDispersion = new FloatParameter(0f);
    public ClampedFloatParameter petzvalDistortionValue = new ClampedFloatParameter(0f, 0f, 2f);
    public Vector3Parameter chromaticColorWeights = new Vector3Parameter(new Vector3(0.0f, 0.0f, 1f));

    //Bloom
    [Space]
    [Header("PRISM.BLOOM")]
    public BoolParameter useBloom = new BoolParameter(false);
    [Range(0f, 5f)] public ClampedFloatParameter bloomStrength = new ClampedFloatParameter(0.05f, 0, 1);
    [Range(0f, 1f)] public ClampedFloatParameter bloomThreshold = new ClampedFloatParameter(2f, 0f, 1f);
    [Range(0f, 1f)] public ClampedFloatParameter bloomSaturation = new ClampedFloatParameter(1.5f, 0f, 2f);

    [InspectorName("Bloom Radius"), Tooltip("Actually adjusts the resolution decrease down of each bloom texture")]
    public ClampedFloatParameter bloomRadius = new ClampedFloatParameter(2f, 0.2f, 3f);
    [Space]
    public BoolParameter useLensDirt = new BoolParameter(false);

    [InspectorName("Lens Dirt Texture"), Tooltip("Simulates a dirty lens")]
    public TextureParameter bloomDirtTexture = new TextureParameter(null);

    [InspectorName("Lens Dirt Strength"), Tooltip("Adjusts the brightness of the lens dirt, in stops of brightness")]
    public ClampedFloatParameter lensDirtExposure = new ClampedFloatParameter(1, 0, 1);


    [Space]
    public BoolParameter increaseBloomClarity = new BoolParameter(true);
    public BoolParameter bloomFastMode = new BoolParameter(false);
    public BoolParameter bloomUseLessPassesForMobile = new BoolParameter(false);
    public BoolParameter bloomGradualThreshold = new BoolParameter(false);

    [Space]
    public BoolParameter bloomDebug = new BoolParameter(false);

    //Noise
    [Space]
    [Header("PRISM.NOISE")]
    public BoolParameter useFilmicNoise = new BoolParameter(false);
    [Range(0, 10)] public IntParameter sensorNoise = new IntParameter(4);
    [Range(0f, 1f)] public ClampedFloatParameter noiseRedSaturation = new ClampedFloatParameter(1f, 0f, 1f);
    [Range(0f, 1f)] public ClampedFloatParameter noiseGreenSaturation = new ClampedFloatParameter(1f, 0f, 1f);
    [Range(0f, 1f)] public ClampedFloatParameter noiseBlueSaturation = new ClampedFloatParameter(1f, 0f, 1f);
    [Range(0f, 1f)] public ClampedFloatParameter sensorPixelSize = new ClampedFloatParameter(0.2f, 0, 1);

    //TV Noise: Width, Animation Speed, Animation Angle
    public BoolParameter useTVNoise = new BoolParameter(false);
    [Range(0f, 1f)] public ClampedFloatParameter noiseTVSpeed = new ClampedFloatParameter(0.2f, 0, 1);
    [Range(0f, 1f)] public ClampedFloatParameter noiseTVWidth = new ClampedFloatParameter(0.2f, 0, 1);
    [Range(0f, 1f)] public ClampedFloatParameter noiseTVAngle = new ClampedFloatParameter(0f, 0, 1);

    public BoolParameter useDither = new BoolParameter(false);
    [Range(2, 10)] public IntParameter ditherBitDepth = new IntParameter(10);

    //Vig
    [Space]
    [Header("PRISM.VIGNETTE")]
    [Range(0f, 2f)] public ClampedFloatParameter vignetteStrength = new ClampedFloatParameter(0.5f, 0f, 1f);

    //Whitepoitn
    [Space]
    [Header("PRISM.COLOR")]
    public BoolParameter useColorCorrection = new BoolParameter(false);

    public ColorParameter shadows = new ColorParameter(Color.black);

    public ColorParameter midtones = new ColorParameter(Color.black);

    public ColorParameter highlights = new ColorParameter(Color.black);

    [InspectorName("Gain"), Tooltip("Controls the lightest portions of the color.")]
    //[PRISMTrackball(PRISMTrackballAttribute.Mode.Gain)]
    public Color gain = new Color(1f, 1f, 1f, 0f);

    [InspectorName("Saturation R | G | B"), Tooltip("Controls the saturation of colours.")]
    public Vector3Parameter saturations = new Vector3Parameter(new Vector3(0f, 0f, 0f));
    [Space]
    [Range(0f, 0.2f)] public ClampedFloatParameter colorVibrance = new ClampedFloatParameter(0f, 0f, 0.2f);

    [Space]
    [Range(-1f, 1f)] public ClampedFloatParameter contrast = new ClampedFloatParameter(0f, -1f, 1f);

    //[Space]
    //START LUT VARIABLES==================================
    //[Header("PRISM.LOOKUP TEXTURES")]
    [InspectorName("  >Use cube LUTs"), Tooltip("Use Cube 3d textures instead of 2d")]
    public BoolParameter useCubeLUTs = new BoolParameter(false);

    public BoolParameter useLut = new BoolParameter(false);
    [InspectorName("  2D LUT (Lookup Texture)"), Tooltip("See the textures folder in PRISM for examples.")]
    public TextureParameter twoDLookupTex = new TextureParameter(null);
    [InspectorName("  First LUT Lerp Amount"), Tooltip("How much the LUT should contribute")]
    [Range(-1f, 1f)] public ClampedFloatParameter lutLerpAmount = new ClampedFloatParameter(0f, -1f, 1f);

    [InspectorName("  Use second LUT"), Tooltip("See the textures folder in PRISM for examples.")]
    public BoolParameter useSecondLut = new BoolParameter(false);
    [InspectorName("  Second 2D LUT (Lookup Texture)"), Tooltip("See the textures folder in PRISM for examples.")]
    public TextureParameter secondaryTwoDLookupTex = new TextureParameter(null);

    [InspectorName("  Second LUT Lerp Amount"), Tooltip("How much the LUT should contribute")]
    [Range(-1f, 1f)] public ClampedFloatParameter secondaryLutLerpAmount = new ClampedFloatParameter(0, 0, 1);

    //END LUT VARIABLES====================================

    //Tonemap
    [Space]
    [Header("PRISM.TONEMAPPING")]
    public BoolParameter tonemap = new BoolParameter(false);
    [Range(-0.4f, 0.4f)] public ClampedFloatParameter gammaOffset = new ClampedFloatParameter(0.0f, -0.8f, 0.8f);


    [InspectorName("Fullscreen Overlay & Blur Texture"), Tooltip("Simulates a dirty visor (REQUIRES SHARPEN TO BE ON)")]
    public TextureParameter fsOverlayTexture = new TextureParameter(null);
    [Range(0f, 1f)] public ClampedFloatParameter fsOverlayColorContribution = new ClampedFloatParameter(0f, 0f, 1f);
}

namespace PRISM.Utils { 

// Define the renderer for the custom post processing effect
public class PRISMStack_URP : CustomPostProcessingPass<PRISMEffects>
{
    // The postprocessing materials
    private Material m_Material;

    // A variable to hold a reference to the corresponding volsume component
    private PRISMEffects m_VolumeComponent;

    protected override string RenderTag => "PRISM Stack";

    // The ids of the shader variables
    static class ShaderIDs
    {
        internal readonly static int Input = Shader.PropertyToID("_MainTex");
        internal readonly static int Intermediate = Shader.PropertyToID("_IntermediateTex");

        internal readonly static int Other = Shader.PropertyToID("_SecondaryTex");

        internal readonly static int Intensity = Shader.PropertyToID("_Intensity");

        internal static readonly int Strength = Shader.PropertyToID("_Strength");
        internal static readonly int ColorTemperature = Shader.PropertyToID("_ColorTemperature");
        internal static readonly int ColorTint = Shader.PropertyToID("_ColorTint");
        internal static readonly int Lightness = Shader.PropertyToID("_Lightness");
        internal static readonly int LightnessSlope = Shader.PropertyToID("_LightnessSlope");
        internal static readonly int A_LabValue = Shader.PropertyToID("_a_LabValue");
        internal static readonly int B_LabValue = Shader.PropertyToID("_b_LabValue");
        internal static readonly int Tonemap = Shader.PropertyToID("_Tonemap");
        internal static readonly int GammaOffset = Shader.PropertyToID("_Gamma");
        internal static readonly int GlassDispersion = Shader.PropertyToID("_GlassDispersion");
        internal static readonly int LensDistortion = Shader.PropertyToID("_GlassAngle");
        internal static readonly int NegativeBarrel = Shader.PropertyToID("_NegativeBarrel");
        internal static readonly int PetzvalDistortion = Shader.PropertyToID("_PetzvalDistortion");

        internal static readonly int Exposure = Shader.PropertyToID("_Exposure");

        internal static readonly int ChromaticColorWeights = Shader.PropertyToID("_ColorDistort");

        //Noise
        internal static readonly int SensorNoise = Shader.PropertyToID("_GrainIntensity");
        internal static readonly int SensorNoiseScale = Shader.PropertyToID("_GrainScale");

        internal static readonly int SensorNoiseSaturations = Shader.PropertyToID("_GrainSaturations");
        internal static readonly int GrainTVValues = Shader.PropertyToID("_GrainTVValues");

        //FSTex
        internal static readonly int FSOverlayTexture = Shader.PropertyToID("_FSOverlayTex");
        internal static readonly int FSOverlayColorContribution = Shader.PropertyToID("_FSOverlayColorContribution");


        internal static readonly int DitherBitDepth = Shader.PropertyToID("_DitherBitDepth");
        internal static readonly int UseDither = Shader.PropertyToID("_UseDither");

        //Bloom
        internal static readonly int BloomStrength = Shader.PropertyToID("_BloomStrength");
        internal static readonly int BloomThresh = Shader.PropertyToID("_BloomThreshold");
        internal static readonly int BloomRadius = Shader.PropertyToID("_BloomRadius");
        internal static readonly int BloomSaturation = Shader.PropertyToID("_BloomSaturation");
        internal static readonly int BloomDirtTex = Shader.PropertyToID("_BloomDirtTex");
        internal static readonly int LensDirtExposure = Shader.PropertyToID("_LensDirtExposure");
        internal static readonly int BloomTex = Shader.PropertyToID("_BloomTex");
        internal static readonly int BloomQuarterTex = Shader.PropertyToID("_BloomTexQuarter");
        internal static readonly int BloomEighthTex = Shader.PropertyToID("_BloomTexEighth");
        internal static readonly int BloomSixTeenthTex = Shader.PropertyToID("_BloomTexSixteenth");
        internal static readonly int BloomThirtySecondTex = Shader.PropertyToID("_BloomTexThirtySecond");
        internal static readonly int BloomTexSixtyFourth = Shader.PropertyToID("_BloomTexSixtyFourth");
        internal static readonly int BloomTexelSize = Shader.PropertyToID("texelSize");
        internal static readonly int BloomStabilityTex = Shader.PropertyToID("_BloomTexLast");


        //Vignette
        internal static readonly int VignetteStrength = Shader.PropertyToID("_VignetteStrength");

        //Color
        internal static readonly int ColorVibrance = Shader.PropertyToID("_ColorVibrance");
        internal static readonly int Shadows = Shader.PropertyToID("_ShadowValue");
        internal static readonly int Midtones = Shader.PropertyToID("_MiddleValue");
        internal static readonly int Gain = Shader.PropertyToID("_GainValue");
        internal static readonly int Highlights = Shader.PropertyToID("_HighlightsValue");
        internal static readonly int Saturations = Shader.PropertyToID("_SaturationValues");

        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");

        internal static readonly int LutScale = Shader.PropertyToID("_LutScale");
        internal static readonly int LutOffset = Shader.PropertyToID("_LutOffset");
        internal static readonly int LutTex = Shader.PropertyToID("_LutTex");
        internal static readonly int LutLerpAmount = Shader.PropertyToID("_LutAmount");

        internal static readonly int SecondLutScale = Shader.PropertyToID("_SecondLutScale");
        internal static readonly int SecondLutOffset = Shader.PropertyToID("_SecondLutOffset");
        internal static readonly int SecondLutTex = Shader.PropertyToID("_SecondLutTex");
        internal static readonly int SecondLutAmount = Shader.PropertyToID("_SecondLutAmount");

        internal static readonly int DownsampleAmount = Shader.PropertyToID("_DownsampleAmount");

    }

    //START LUT TEXTURES
    public Texture3D threeDLookupTex = null;
    public Texture3D secondaryThreeDLookupTex = null;

    public string basedOnTempTex = "";
    public string secondaryBasedOnTempTex = "";

    public Texture lastLut2d = null;
    public Texture lastSecondLut2d = null;
    //END LUT TEXTURES

    /// <summary>
    /// Used to store the history of a camera (its previous frame and whether it is valid or not)
    /// </summary>
    private class CameraHistory
    {
        public RenderTexture Frame { get; set; }
        public bool Invalidated { get; set; }

        public CameraHistory(RenderTextureDescriptor descriptor)
        {
            Frame = new RenderTexture(descriptor);
            Invalidated = false;
        }
    }

    // We store the history for each camera separately (key is the camera instance id).
    private Dictionary<int, CameraHistory> _histories = null;

    // A temporary render target in case we need it (if destination is the camera render target and can't be used as source).
    private RTHandle _main = default;
    /*private RenderTargetHandle _bloomEighth = default;
    private RenderTargetHandle _bloomSixteenth = default;
    private RenderTargetHandle _bloomThirtySecond = default;
    private RenderTargetHandle _bloomSixtyFourth = default;*/

    Vector4 noiseSaturations = Vector4.one;
    Vector4 tvValues = Vector4.one;

    public void SetPrimaryLut(Texture newPrimaryLut)
    {
        m_VolumeComponent.twoDLookupTex.value = newPrimaryLut;

        Convert(newPrimaryLut as Texture2D, false);

        if (m_VolumeComponent.useCubeLUTs.value == false)
        {
            Convert(newPrimaryLut as Texture2D, false);
        }
        else if (newPrimaryLut.dimension == TextureDimension.Tex3D)
        {
            threeDLookupTex = newPrimaryLut as Texture3D;
        }
    }

    public void SetSecondaryLut(Texture newSecondaryLut)
    {
        m_VolumeComponent.secondaryTwoDLookupTex.value = newSecondaryLut;


        Convert(newSecondaryLut as Texture2D, true);

        if (m_VolumeComponent.useCubeLUTs.value == false)
        {
            Convert(newSecondaryLut as Texture2D, true);
        }
        else if (newSecondaryLut.dimension == TextureDimension.Tex3D)
        {
            threeDLookupTex = newSecondaryLut as Texture3D;
        }
    }

    void OnDestroy()
    {
        if (threeDLookupTex)
            GameObject.DestroyImmediate(threeDLookupTex);
        threeDLookupTex = null;
        basedOnTempTex = "";
        m_VolumeComponent.twoDLookupTex.value = null;

        if (secondaryThreeDLookupTex)
            GameObject.DestroyImmediate(secondaryThreeDLookupTex);
        secondaryThreeDLookupTex = null;
        secondaryBasedOnTempTex = "";
        m_VolumeComponent.secondaryTwoDLookupTex.value = null;
    }

        void SetPrimaryLutShaderValues()
        {
            if (m_VolumeComponent.useLut.value == true && m_VolumeComponent.twoDLookupTex.value && threeDLookupTex)
            {
                m_Material.EnableKeyword("LUT");
                int lutSize = threeDLookupTex.width;
                threeDLookupTex.wrapMode = TextureWrapMode.Clamp;

                m_Material.SetFloat(ShaderIDs.LutScale, (lutSize - 1) / (1.0f * lutSize));
                m_Material.SetFloat(ShaderIDs.LutOffset, 1.0f / (2.0f * lutSize));
                m_Material.SetTexture(ShaderIDs.LutTex, threeDLookupTex);
                m_Material.SetFloat(ShaderIDs.LutLerpAmount, m_VolumeComponent.lutLerpAmount.value);
            }
            else if (m_VolumeComponent.useLut.value == false)
            {
                m_Material.SetFloat(ShaderIDs.LutLerpAmount, 0f);

                if (m_VolumeComponent.useSecondLut.value == false)
                {
                    m_Material.DisableKeyword("LUT");
                }
            }
            else
            {
                if (!threeDLookupTex && m_VolumeComponent.twoDLookupTex.value != null)
                {
                    Convert(m_VolumeComponent.twoDLookupTex.value as Texture2D, false);
                }

                if (!m_VolumeComponent.twoDLookupTex.value)
                {

                    Debug.Log("No Primary LUT is set or LUT is null, will not use LUT effect this frame");   
                    m_Material.SetFloat(ShaderIDs.LutLerpAmount, 0f);

                    if (m_VolumeComponent.useSecondLut.value == false)
                    {
                        m_Material.DisableKeyword("LUT");
                    }
                }
                else if (!threeDLookupTex)
                {
                    Debug.LogError("Disabling LUT due to issue");
                    m_Material.SetFloat(ShaderIDs.LutLerpAmount, 0f);

                    if (m_VolumeComponent.useSecondLut.value == false)
                    {
                        m_Material.DisableKeyword("LUT");
                    }
                }


            }
        }

    void SetSecondaryLutShaderValues()
    {
        if (m_VolumeComponent.useSecondLut.value == true && m_VolumeComponent.secondaryTwoDLookupTex.value && secondaryThreeDLookupTex)
        {
            m_Material.EnableKeyword("LUT");
            int lutTwoSize = secondaryThreeDLookupTex.width;
            secondaryThreeDLookupTex.wrapMode = TextureWrapMode.Clamp;

            m_Material.SetFloat(ShaderIDs.SecondLutScale, (lutTwoSize - 1) / (1.0f * lutTwoSize));
            m_Material.SetFloat(ShaderIDs.SecondLutOffset, 1.0f / (2.0f * lutTwoSize));
            m_Material.SetTexture(ShaderIDs.SecondLutTex, secondaryThreeDLookupTex);
            m_Material.SetFloat(ShaderIDs.SecondLutAmount, m_VolumeComponent.secondaryLutLerpAmount.value);
        }
        else
        {
            m_Material.SetFloat(ShaderIDs.SecondLutAmount, 0f);
        }
    }

    void CalculateWeights()
    {
        float[] weights;

        // Only 5, 9, 13 and 17 taps are supported in the fast shader
        int sampledTaps = 17;

        float[] offsets;
        GaussianKernel.GetFastWeights(sampledTaps, GaussianKernel.IdealStandardDeviation(7),
            out weights, out offsets);

        if (sampledTaps != 5)
            m_Material.SetFloat("centerTapWeight", weights[0]);

        switch (sampledTaps)
        {
            case 5:
                m_Material.SetVector("tapWeights", new Vector4(weights[1], weights[0], weights[1], 0f));
                m_Material.SetVector("tapOffsets", new Vector4(-offsets[1], offsets[0], offsets[1], 0f));
                break;
            case 9:
                m_Material.SetVector("tapWeights", new Vector4(weights[2], weights[1], weights[1], weights[2]));
                m_Material.SetVector("tapOffsets", new Vector4(-offsets[2], -offsets[1], offsets[1], offsets[2]));
                break;
            case 13:
                m_Material.SetVector("tapWeights", new Vector4(weights[1], weights[2], weights[3], 0f));
                m_Material.SetVector("tapOffsets", new Vector4(offsets[1], offsets[2], offsets[3], 0f));
                break;
            case 17:
                m_Material.SetVector("tapWeights", new Vector4(weights[1], weights[2], weights[3], weights[4]));
                m_Material.SetVector("tapOffsets", new Vector4(offsets[1], offsets[2], offsets[3], offsets[4]));
                break;
        }
    }

    float GetNoiseTV()
    {
        if (m_VolumeComponent.useTVNoise.value == true) return 1.0f;

        return 0.0f;
    }

    Vector2 directionBloomBlur = new Vector2(0f, 1f);
    static Vector2 bloomDirVert = new Vector2(0f, 1f);
    static Vector2 bloomDirHoriz = new Vector2(1f, 0f);

    [ContextMenu("Check if DEVELOP keyword is active")]
    bool ShouldEnableDevelopKeyword()
    {
        if (m_VolumeComponent.sensorNoise.value > 0f && m_VolumeComponent.useDither.value == true) return true;

        if (m_VolumeComponent.useColorCorrection.value == true) return true;

        return false;
    }

    [ContextMenu("Check if using any effects from SENSOR pass")]
    bool IsUsingAnyEffectsFromStackSensorPass()
    {
        if (m_VolumeComponent.sensorNoise.value > 0f) return true;

        if (m_VolumeComponent.useDither.value == true) return true;

        if (m_VolumeComponent.useFilmicNoise.value == true) return true;

        if (m_VolumeComponent.useLut.value == true) return true;

        if (m_VolumeComponent.useSecondLut.value == true) return true;

        if (m_VolumeComponent.useColorCorrection.value == true) return true;

        return false;
    }

    protected override void BeforeRender(CommandBuffer commandBuffer, ref RenderingData renderingData)
    {
        // Debug.LogWarning("GOT TO BEFORE RENDERER");
        //Set these otherwise build on android no worky
        if (m_VolumeComponent.useLut.value == true && m_VolumeComponent.twoDLookupTex.value)
        {
            SetPrimaryLut(m_VolumeComponent.twoDLookupTex.value);
                SetPrimaryLutShaderValues();
            }

        if (m_VolumeComponent.useSecondLut.value == true)
        {
            SetSecondaryLut(m_VolumeComponent.secondaryTwoDLookupTex.value);
                SetSecondaryLutShaderValues();
            }

        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMEffects>();

        m_Material.SetFloat(ShaderIDs.Intensity, m_VolumeComponent.intensity.value);


        m_Material.SetFloat(ShaderIDs.Intensity, m_VolumeComponent.intensity.value);
        m_Material.SetInt(ShaderIDs.DownsampleAmount, 1);

#if UNITY_EDITOR
        if (m_VolumeComponent.twoDLookupTex.value != null && lastLut2d != m_VolumeComponent.twoDLookupTex.value)
        {
            SetPrimaryLut(m_VolumeComponent.twoDLookupTex.value as Texture2D);
            lastLut2d = m_VolumeComponent.twoDLookupTex.value;
        }

        if (m_VolumeComponent.secondaryTwoDLookupTex.value != null && lastSecondLut2d != m_VolumeComponent.secondaryTwoDLookupTex.value)
        {
            SetSecondaryLut(m_VolumeComponent.secondaryTwoDLookupTex.value as Texture2D);
            lastSecondLut2d = m_VolumeComponent.secondaryTwoDLookupTex.value;
        }
#endif

        #region valuesetting
        m_Material.SetFloat(ShaderIDs.ColorTemperature, m_VolumeComponent.colorTemperature.value);
        m_Material.SetFloat(ShaderIDs.ColorTint, m_VolumeComponent.colorTint.value + 1f);
        m_Material.SetFloat(ShaderIDs.A_LabValue, m_VolumeComponent.labA_GreenAndMagenta.value + 1f);
        m_Material.SetFloat(ShaderIDs.B_LabValue, m_VolumeComponent.labB_YellowAndBlue.value + 1f);
        m_Material.SetFloat(ShaderIDs.Lightness, m_VolumeComponent.lightness.value.x);
        m_Material.SetFloat(ShaderIDs.LightnessSlope, m_VolumeComponent.lightness.value.y);
        m_Material.SetFloat(ShaderIDs.Exposure, m_VolumeComponent.exposure.value);


        float tonemap = 0f;
        if (m_VolumeComponent.tonemap.value == true)
        {
            tonemap = 1f;
        }
        m_Material.SetFloat(ShaderIDs.Tonemap, tonemap);
        m_Material.SetFloat(ShaderIDs.GammaOffset, m_VolumeComponent.gammaOffset.value);

        //Barrel and chromab
        m_Material.SetFloat(ShaderIDs.GlassDispersion, m_VolumeComponent.glassDispersion.value);
        m_Material.SetFloat(ShaderIDs.LensDistortion, m_VolumeComponent.barrelDistortion.value > 0f ? m_VolumeComponent.barrelDistortion.value : m_VolumeComponent.barrelDistortion.value * 0.5f);
        m_Material.SetFloat(ShaderIDs.NegativeBarrel, m_VolumeComponent.barrelDistortion.value > 0f ? 0.0f : 1.0f);
        m_Material.SetVector(ShaderIDs.ChromaticColorWeights, m_VolumeComponent.chromaticColorWeights.value);
        m_Material.SetFloat(ShaderIDs.PetzvalDistortion, m_VolumeComponent.petzvalDistortionValue.value);

        //Bloom
        m_Material.SetFloat(ShaderIDs.BloomStrength, m_VolumeComponent.bloomStrength.value);
        m_Material.SetFloat(ShaderIDs.BloomThresh, m_VolumeComponent.increaseBloomClarity.value == true ? m_VolumeComponent.bloomThreshold.value : m_VolumeComponent.bloomThreshold.value);
        m_Material.SetFloat(ShaderIDs.BloomRadius, m_VolumeComponent.bloomRadius.value);
        m_Material.SetFloat(ShaderIDs.BloomSaturation, m_VolumeComponent.bloomSaturation.value);
        m_Material.SetFloat(ShaderIDs.LensDirtExposure, m_VolumeComponent.lensDirtExposure.value);


        m_Material.SetFloat(ShaderIDs.SensorNoise, m_VolumeComponent.sensorNoise.value * 0.025f);

        //Save GC on new Vector
        if (noiseSaturations.x != m_VolumeComponent.noiseRedSaturation.value || noiseSaturations.y != m_VolumeComponent.noiseGreenSaturation.value || noiseSaturations.z != m_VolumeComponent.noiseBlueSaturation.value || noiseSaturations.w != GetNoiseTV())
        {
            noiseSaturations = new Vector4(m_VolumeComponent.noiseRedSaturation.value, m_VolumeComponent.noiseGreenSaturation.value, m_VolumeComponent.noiseBlueSaturation.value, GetNoiseTV());
        }

        m_Material.SetVector(ShaderIDs.SensorNoiseSaturations, noiseSaturations);

        //Save GC on new Vector
        if (tvValues.x != m_VolumeComponent.noiseTVAngle.value || tvValues.y != m_VolumeComponent.noiseTVSpeed.value || tvValues.z != m_VolumeComponent.noiseTVWidth.value)
        {
            tvValues = new Vector4(m_VolumeComponent.noiseTVAngle.value, m_VolumeComponent.noiseTVSpeed.value, m_VolumeComponent.noiseTVWidth.value, GetNoiseTV());
        }
        m_Material.SetVector(ShaderIDs.GrainTVValues, tvValues);

        //Bloom Cont
        CalculateWeights();

        float useDither = 0f;
        if (m_VolumeComponent.useDither.value == true)
        {
            useDither = 1f;
            m_Material.SetFloat(ShaderIDs.DitherBitDepth, Mathf.Pow(2f, (float)m_VolumeComponent.ditherBitDepth.value) - 1.0f);
        }
        m_Material.SetFloat(ShaderIDs.UseDither, useDither);
        if (m_VolumeComponent.sensorPixelSize.value == 0f)
        {
            m_Material.SetFloat(ShaderIDs.SensorNoiseScale, 0f);
        }
        else
        {
            m_Material.SetFloat(ShaderIDs.SensorNoiseScale, Mathf.Lerp(1700f, 300f, m_VolumeComponent.sensorPixelSize.value));
        }

        //vignette on both
        m_Material.SetFloat(ShaderIDs.VignetteStrength, m_VolumeComponent.vignetteStrength.value);

        //color
        m_Material.SetFloat(ShaderIDs.ColorVibrance, m_VolumeComponent.colorVibrance.value);
        m_Material.SetVector(ShaderIDs.Shadows, m_VolumeComponent.shadows.value);// m_VolumeComponent.shadows.value);

            Color midTonesBlackCorrected = Color.Lerp(Color.black, m_VolumeComponent.midtones.value, m_VolumeComponent.midtones.value.a);
            Color highlightsBlackCorrected = Color.Lerp(Color.black, m_VolumeComponent.highlights.value, m_VolumeComponent.highlights.value.a);

            m_Material.SetVector(ShaderIDs.Midtones, midTonesBlackCorrected);//.value
        m_Material.SetVector(ShaderIDs.Gain, m_VolumeComponent.gain);
        m_Material.SetVector(ShaderIDs.Highlights, highlightsBlackCorrected);
        m_Material.SetVector(ShaderIDs.Saturations, m_VolumeComponent.saturations.value);

        m_Material.SetFloat(ShaderIDs.Contrast, m_VolumeComponent.contrast.value);

        //LUT

        //Debug.Log("Shadows: " + m_VolumeComponent.shadows.value);

        if (m_VolumeComponent.useColorCorrection.value == true) m_Material.EnableKeyword("COLORCORRECT");
        if (m_VolumeComponent.useColorCorrection.value == false) m_Material.DisableKeyword("COLORCORRECT");

        if (m_VolumeComponent.useBloom.value == true)
        {
            m_Material.EnableKeyword("BLOOM");
            m_Material.DisableKeyword("BLOOMLENSDIRT");
        }
        if (m_VolumeComponent.useBloom.value == false)
        {
            m_Material.DisableKeyword("BLOOM");
            m_Material.DisableKeyword("BLOOMLENSDIRT");
            m_Material.DisableKeyword("BLOOMSTAB");
        }

        if (m_VolumeComponent.useLensDirt.value == true && m_VolumeComponent.useBloom.value == true)
        {
            m_Material.EnableKeyword("BLOOMLENSDIRT");
            m_Material.DisableKeyword("BLOOM");
        }
        if (m_VolumeComponent.useLensDirt.value == false)
        {
            m_Material.DisableKeyword("BLOOMLENSDIRT");
        }

        if (m_VolumeComponent.useLensDirt.value == true)
        {
            if (m_VolumeComponent.bloomDirtTexture != null)
            {
                m_Material.SetTexture(ShaderIDs.BloomDirtTex, m_VolumeComponent.bloomDirtTexture.value);
            }
        }

        if (m_VolumeComponent.increaseBloomClarity.value == true && m_VolumeComponent.useBloom.value == true) m_Material.EnableKeyword("BLOOMCLARITY");
        if (m_VolumeComponent.increaseBloomClarity.value == false || m_VolumeComponent.useBloom.value == false) m_Material.DisableKeyword("BLOOMCLARITY");

        if (m_VolumeComponent.useLinearSpace.value == true) m_Material.EnableKeyword("GAMMATOLINEAR");
        if (m_VolumeComponent.useLinearSpace.value == false) m_Material.DisableKeyword("GAMMATOLINEAR");

        if (m_VolumeComponent.useFilmicNoise.value == true) m_Material.EnableKeyword("GRAIN");
        if (m_VolumeComponent.useFilmicNoise.value == false) m_Material.DisableKeyword("GRAIN");

        if (m_VolumeComponent.tonemap.value == true) m_Material.EnableKeyword("CUPP_TONEMAP");
        if (m_VolumeComponent.tonemap.value == false) m_Material.DisableKeyword("CUPP_TONEMAP");


        bool shouldEnableDevelopKeyword = ShouldEnableDevelopKeyword();
        bool isUsingAnyEffectsFromStackSensorPass = IsUsingAnyEffectsFromStackSensorPass();

        if (shouldEnableDevelopKeyword == true) m_Material.EnableKeyword("DEVELOP");
        if (shouldEnableDevelopKeyword == false) m_Material.DisableKeyword("DEVELOP");

        //Lensdistort
        if (m_VolumeComponent.useLensDistortion.value == true)
        {
            m_Material.EnableKeyword("BARRELDIST");

            if (m_VolumeComponent.glassDispersion.value > 0f && m_VolumeComponent.useChromaticAberration.value == false) m_Material.EnableKeyword("FRINGING");
            if (m_VolumeComponent.glassDispersion.value == 0f || m_VolumeComponent.useChromaticAberration.value == true) m_Material.DisableKeyword("FRINGING");

            if (m_VolumeComponent.useChromaticAberration.value == true) m_Material.EnableKeyword("CHROMAB");
            if (m_VolumeComponent.useChromaticAberration.value == false) m_Material.DisableKeyword("CHROMAB");
        }
        else
        {
            m_Material.DisableKeyword("BARRELDIST");
            m_Material.DisableKeyword("FRINGING");
            m_Material.DisableKeyword("CHROMAB");
        }
        //BARRELDIST
        #endregion
    }

    //Do the work here
    protected override void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier destination)
    {
       // Debug.Log("BLIT ONE");

        // set material properties
        if (m_Material == null)
        {
            Debug.LogError("Material null in PRISM!");
            return;
        }


        // set source texture
        RenderTextureDescriptor descriptorNormal = GetTempRTDescriptor(renderingData);

        _main = RTHandles.Alloc(descriptorNormal);
        //commandBuffer.GetTemporaryRT(_main.id, descriptorNormal);
        //commandBuffer.Blit(source, ShaderIDs.Input);

        commandBuffer.SetGlobalTexture(ShaderIDs.Input, renderingData.cameraData.renderer.cameraColorTargetHandle);

       // Debug.LogError(m_VolumeComponent.useBloom.value + " <Bloom?");

        if (m_VolumeComponent.useBloom.value == true)
        {
            
                //HQ Bloom
                //Use bloom
                float initialBloomSize = m_VolumeComponent.bloomRadius.value;// 4f;

                RenderTextureDescriptor bloomInitialDescriptor = GetTempRTDescriptor(renderingData);
                bloomInitialDescriptor.height /= (int)initialBloomSize;
                bloomInitialDescriptor.width /= (int)initialBloomSize;

                RenderTextureDescriptor bloomQuarterDescriptor = GetTempRTDescriptor(renderingData);
                initialBloomSize *= 2f;
                bloomQuarterDescriptor.height /= (int)initialBloomSize;
                bloomQuarterDescriptor.width /= (int)initialBloomSize;

                RenderTextureDescriptor bloomEighthDescriptor = GetTempRTDescriptor(renderingData);
                initialBloomSize *= 2f;
                bloomEighthDescriptor.height /= (int)initialBloomSize;
                bloomEighthDescriptor.width /= (int)initialBloomSize;

                RenderTextureDescriptor bloomSixteenthDescriptor = GetTempRTDescriptor(renderingData);
                initialBloomSize *= 2f;
                bloomSixteenthDescriptor.height /= (int)initialBloomSize;
                bloomSixteenthDescriptor.width /= (int)initialBloomSize;

                RenderTextureDescriptor bloomThirtySecondDescriptor = GetTempRTDescriptor(renderingData);
                initialBloomSize *= 2f;
                bloomThirtySecondDescriptor.height /= (int)initialBloomSize;
                bloomThirtySecondDescriptor.width /= (int)initialBloomSize;

                RenderTextureDescriptor bloomSixtyFourthDescriptor = GetTempRTDescriptor(renderingData);
                initialBloomSize *= 2f;
                bloomSixtyFourthDescriptor.height /= (int)initialBloomSize;
                bloomSixtyFourthDescriptor.width /= (int)initialBloomSize;


                bloomInitialDescriptor.sRGB = true;
                bloomQuarterDescriptor.sRGB = true;
                bloomEighthDescriptor.sRGB = true;
                bloomSixteenthDescriptor.sRGB = true;
                bloomThirtySecondDescriptor.sRGB = true;
                bloomSixtyFourthDescriptor.sRGB = true;

                /*_bloomQuarter.id = 44;
                _bloomEighth.id = 48;
                _bloomSixteenth.id = 52;
                _bloomThirtySecond.id = 56;
                _bloomSixtyFourth.id = 68;*/

                var bHalf = RenderTexture.GetTemporary(bloomInitialDescriptor);
                var bQuart = RenderTexture.GetTemporary(bloomQuarterDescriptor);
                var bEighth = RenderTexture.GetTemporary(bloomEighthDescriptor);
                var bSixteenth = RenderTexture.GetTemporary(bloomSixteenthDescriptor);
                var bThirtySecond = RenderTexture.GetTemporary(bloomThirtySecondDescriptor);
                var bSixtyFourth = RenderTexture.GetTemporary(bloomSixtyFourthDescriptor);

                //Blurs
                //cmd.GetTemporaryRT(_bloomHalf.id + 1, bloomInitialDescriptor, FilterMode.Bilinear);
                var bHalf1 = RenderTexture.GetTemporary(bloomInitialDescriptor);

                //Blit source into half texture... TODO threshold
                //Downsample Source to halfres
                //cmd.Blit(source, _bloomHalf.id + 1, m_Material, 2);
                commandBuffer.Blit(source, bHalf1, m_Material, 2);

                //Median and threshold
                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bHalf1);

            int medianPass = 5;
            //Switch to mobile median if required
            if(m_VolumeComponent.fastMode.value == true)
            {
                medianPass = 14;
            }
                commandBuffer.Blit(bHalf1, bHalf, m_Material, medianPass);

                int loops = 2;

                //Gaussian blurs
                for (int i = 0; i < loops; i++)
                {
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bHalf);
                    commandBuffer.Blit(bHalf, bHalf1, m_Material, 6);
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bHalf1);
                    commandBuffer.Blit(bHalf1, bHalf, m_Material, 7);
                }

                //Downsample the _BloomHalf into Bloomquarter
                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bHalf);
                commandBuffer.Blit(bHalf, bQuart, m_Material, 2);

                //Quarter
                // cmd.GetTemporaryRT(_bloomQuarter.id + 1, bloomQuarterDescriptor, FilterMode.Bilinear);
                var bQuart1 = RenderTexture.GetTemporary(bloomQuarterDescriptor);

                //Blurs
                for (int i = 0; i < loops; i++)
                {
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bQuart);
                    commandBuffer.Blit(bQuart, bQuart1, m_Material, 6);
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bQuart1);
                    commandBuffer.Blit(bQuart1, bQuart, m_Material, 7);
                }

                //Eighth
                // cmd.GetTemporaryRT(_bloomEighth.id + 1, bloomEighthDescriptor, FilterMode.Bilinear);
                var bEighth1 = RenderTexture.GetTemporary(bloomEighthDescriptor);

                //Downsample quarter into 8th
                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bQuart);
                commandBuffer.Blit(bQuart, bEighth, m_Material, 2);

                for (int i = 0; i < loops; i++)
                {
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bEighth);
                    commandBuffer.Blit(bEighth, bEighth1, m_Material, 9);
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bEighth1);
                    commandBuffer.Blit(bEighth1, bEighth, m_Material, 10);
                }

                //Sixteenth
                //cmd.GetTemporaryRT(_bloomSixteenth.id + 1, bloomSixteenthDescriptor, FilterMode.Bilinear);
                var bSixteenth1 = RenderTexture.GetTemporary(bloomSixteenthDescriptor);

                //Downsample quarter into 16th
                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bEighth);
                commandBuffer.Blit(bEighth, bSixteenth, m_Material, 2);

                //Blur
                for (int i = 0; i < loops; i++)
                {
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bSixteenth);
                    commandBuffer.Blit(bSixteenth, bSixteenth1, m_Material, 11);
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bSixteenth1);
                    commandBuffer.Blit(bSixteenth1, bSixteenth, m_Material, 12);
                }

                //ThirtySecond
                //cmd.GetTemporaryRT(_bloomThirtySecond.id + 1, bloomThirtySecondDescriptor, FilterMode.Bilinear);
                var bThirtySecond1 = RenderTexture.GetTemporary(bloomThirtySecondDescriptor);

                //Downsample quarter into 16th
                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bSixteenth);
                commandBuffer.Blit(bSixteenth, bThirtySecond, m_Material, 2);

                //Blur
                for (int i = 0; i < loops; i++)
                {
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bThirtySecond);
                    commandBuffer.Blit(bThirtySecond, bThirtySecond1, m_Material, 11);
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bThirtySecond1);
                    commandBuffer.Blit(bThirtySecond1, bThirtySecond, m_Material, 12);
                }

                var bSixtyFourth1 = RenderTexture.GetTemporary(bloomSixtyFourthDescriptor);

                //Downsample into 64th
                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bThirtySecond);
                commandBuffer.Blit(bThirtySecond, bSixtyFourth, m_Material, 2);

                //Blur
                for (int i = 0; i < loops; i++)
                {
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bSixtyFourth);
                    commandBuffer.Blit(bSixtyFourth, bSixtyFourth1, m_Material, 11);
                    commandBuffer.SetGlobalTexture(ShaderIDs.Input, bSixtyFourth1);
                    commandBuffer.Blit(bSixtyFourth1, bSixtyFourth, m_Material, 12);
                }

                //NOW _bloomHalf
                commandBuffer.SetGlobalTexture(ShaderIDs.BloomTex, bHalf);
                commandBuffer.SetGlobalTexture(ShaderIDs.BloomQuarterTex, bQuart);
                commandBuffer.SetGlobalTexture(ShaderIDs.BloomEighthTex, bEighth);
                commandBuffer.SetGlobalTexture(ShaderIDs.BloomSixTeenthTex, bSixteenth);
                commandBuffer.SetGlobalTexture(ShaderIDs.BloomThirtySecondTex, bThirtySecond);
                commandBuffer.SetGlobalTexture(ShaderIDs.BloomTexSixtyFourth, bSixtyFourth);


                commandBuffer.SetGlobalTexture(ShaderIDs.Input, bHalf);


                commandBuffer.SetGlobalTexture(ShaderIDs.Input, source);

                RenderTexture.ReleaseTemporary(bHalf);
                RenderTexture.ReleaseTemporary(bQuart);
                RenderTexture.ReleaseTemporary(bEighth);
                RenderTexture.ReleaseTemporary(bSixteenth);
                RenderTexture.ReleaseTemporary(bThirtySecond);
                RenderTexture.ReleaseTemporary(bSixtyFourth);
                RenderTexture.ReleaseTemporary(bHalf1);
                RenderTexture.ReleaseTemporary(bQuart1);
                RenderTexture.ReleaseTemporary(bEighth1);
                RenderTexture.ReleaseTemporary(bSixteenth1);
                RenderTexture.ReleaseTemporary(bThirtySecond1);
                RenderTexture.ReleaseTemporary(bSixtyFourth1);

            //Display debug bloom and GTFO
            if(m_VolumeComponent.bloomDebug.value == true)
            {
                commandBuffer.Blit(bHalf, destination);
                goto PostRender;
            }

            // Create a temporary target
            //GET RT
            RenderTextureDescriptor descriptorLarge = GetTempRTDescriptor(renderingData);
            commandBuffer.GetTemporaryRT(ShaderIDs.Intermediate, descriptorLarge);
            RenderTargetIdentifier intermediate_Identifier = new RenderTargetIdentifier(source, 0);

            //BLIT #1
            commandBuffer.Blit(source, ShaderIDs.Intermediate, m_Material, 0);
            //CUPPURPHelpers.BlitFullscreenMesh(commandBuffer, ShaderIDs.Input, source, _intermediate.Identifier(), m_Material, 0);

            //commandBuffer.Blit(_intermediate.Identifier(), destination, m_Material, 1);
            //CUPPURPHelpers.BlitFullscreenMesh(commandBuffer, ShaderIDs.Other, _intermediate.Identifier(), destination, m_Material, 1);
            commandBuffer.Blit(ShaderIDs.Intermediate, destination, m_Material, 1);

        PostRender:
            // Release the temporary target
            commandBuffer.ReleaseTemporaryRT(ShaderIDs.Intermediate);
            _main.Release();
            //commandBuffer.ReleaseTemporaryRT(_main.id);
        }
        else
        {
            RenderTextureDescriptor descriptorLarge = GetTempRTDescriptor(renderingData);
            commandBuffer.GetTemporaryRT(ShaderIDs.Intermediate, descriptorLarge);
            RenderTargetIdentifier intermediate_Identifier = new RenderTargetIdentifier(source, 0);

            //BLIT #1
            commandBuffer.Blit(source, ShaderIDs.Intermediate, m_Material, 0);
            //CUPPURPHelpers.BlitFullscreenMesh(commandBuffer, ShaderIDs.Input, source, _intermediate.Identifier(), m_Material, 0);

            commandBuffer.Blit(ShaderIDs.Intermediate, destination, m_Material, 1);
            // CUPPURPHelpers.BlitFullscreenMesh(commandBuffer, ShaderIDs.Other, _intermediate.Identifier(), destination, m_Material, 1);

            // Release the temporary target
            commandBuffer.ReleaseTemporaryRT(ShaderIDs.Intermediate);
            _main.Release();
            //commandBuffer.ReleaseTemporaryRT(_main.id);
        }

    }

    protected override bool IsActive()
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        m_VolumeComponent = stack.GetComponent<PRISMEffects>();

        return Component.active && m_VolumeComponent.intensity.value > 0;
    }



    //init
    public PRISMStack_URP(RenderPassEvent renderPassEvent, Shader shader) : base(renderPassEvent, shader)
    {
        if (!m_Material)
        {
            m_Material = CoreUtils.CreateEngineMaterial("PRISM/PRISMStack");
            CUPPURPHelpers.CheckShaderKeywordState(m_Material);
            SetMaterial(m_Material);
        }

        _histories = new Dictionary<int, CameraHistory>();

        //_main.Init("_MainTex");

        
        //Debug.Log(m_Material.name);

    }
    //TODO DISPOSE HISTORIES


    #region lut
    //Editor mainly	
    /// <summary>
    /// Helper function for the Lookup textures creation
    /// </summary>
    public void SetIdentityLut(bool secondary = false)
    {
        int dim = 16;
        Color[] newC = new Color[dim * dim * dim];
        float oneOverDim = 1.0f / (1.0f * dim - 1.0f);

        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j < dim; j++)
            {
                for (int k = 0; k < dim; k++)
                {
                    newC[i + (j * dim) + (k * dim * dim)] = new Color((i * 1.0f) * oneOverDim, (j * 1.0f) * oneOverDim, (k * 1.0f) * oneOverDim, 1.0f);
                }
            }
        }

        if (!secondary)
        {
            if (threeDLookupTex)
                GameObject.DestroyImmediate(threeDLookupTex);

            threeDLookupTex = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
            threeDLookupTex.SetPixels(newC);
            threeDLookupTex.Apply();
            basedOnTempTex = "";
        }
        else
        {
            if (secondaryThreeDLookupTex)
                GameObject.DestroyImmediate(secondaryThreeDLookupTex);

            secondaryThreeDLookupTex = new Texture3D(dim, dim, dim, TextureFormat.ARGB32, false);
            secondaryThreeDLookupTex.SetPixels(newC);
            secondaryThreeDLookupTex.Apply();
            secondaryBasedOnTempTex = "";
        }
    }

      

        /// <summary>
        /// Helper function for the Lookup textures creation
        /// </summary>
        public bool ValidDimensions(Texture2D tex2d)
    {
        if (!tex2d)
            return false;

        if (tex2d.height != Mathf.FloorToInt(Mathf.Sqrt(tex2d.width)))
        {
            //if(tex2d.height != 512 && tex2d.width != 512)
            return false;
        }

        return true;
    }

    public bool IsSquare(Texture2D tex2d)
    {
        if (!tex2d)
            return false;

        if (tex2d.height != 512 && tex2d.width != 512)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Helper function for the Lookup textures creation
    /// </summary>
    public void Convert(Texture2D temp2DTex, bool secondaryLut = false)
    {
#if UNITY_EDITOR
        //Debug.Log("Converting LUT internally (this message is editor-only) - Is secondary? " + secondaryLut);
#endif
        // conversion fun: the given 2D texture needs to be of the format
        //  w * h, wheras h is the 'depth' (or 3d dimension 'dim') and w = dim * dim

        if (temp2DTex)
        {
            int dim = temp2DTex.width * temp2DTex.height;
            dim = temp2DTex.height;
            int dimH = temp2DTex.height;
            //Debug.Log("Dim: " + dim);

            bool sq = false;

            if (ValidDimensions(temp2DTex) == false && sq == false)
            {
                Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
                if (!secondaryLut)
                {
                    secondaryBasedOnTempTex = "";
                }
                else basedOnTempTex = "";
                return;
            }

            if (sq)
            {
                dim = 64;
                dimH = 8;
            }

            Color[] c = temp2DTex.GetPixels();
            Color[] newC = new Color[c.Length];

            if (sq)
            {
                int tempDim = 64;
                for (int i = 0; i < tempDim; i++)
                {
                    for (int j = 0; j < tempDim; j++)
                    {
                        for (int k = 0; k < tempDim; k++)
                        {
                            int j_ = tempDim - j - 1;
                            newC[i + (j * tempDim) + (k * tempDim * tempDim)] = c[k * tempDim + i + j_ * tempDim * tempDim];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        for (int k = 0; k < dim; k++)
                        {
                            int j_ = dim - j - 1;
                            newC[i + (j * dim) + (k * dim * dim)] = c[k * dim + i + j_ * dim * dim];
                        }
                    }
                }
            }



            if (!secondaryLut)
            {
                if (threeDLookupTex)
                    GameObject.DestroyImmediate(threeDLookupTex);
                threeDLookupTex = new Texture3D(dim, dim, dimH, TextureFormat.ARGB32, false);
                threeDLookupTex.SetPixels(newC);
                threeDLookupTex.Apply();
                basedOnTempTex = temp2DTex.name;
                m_VolumeComponent.twoDLookupTex.value = temp2DTex;
            }
            else
            {
                if (secondaryThreeDLookupTex)
                    GameObject.DestroyImmediate(secondaryThreeDLookupTex);
                secondaryThreeDLookupTex = new Texture3D(dim, dim, dimH, TextureFormat.ARGB32, false);
                secondaryThreeDLookupTex.SetPixels(newC);
                secondaryThreeDLookupTex.Apply();
                secondaryBasedOnTempTex = temp2DTex.name;
                m_VolumeComponent.secondaryTwoDLookupTex.value = temp2DTex;
            }

        }
        else if(temp2DTex == null)
        {
                m_VolumeComponent.intensity.value = 0;
            }
        else
        {
            // error, something went terribly wrong
            Debug.LogError("Couldn't color correct with 3D LUT texture. PRISM will be disabled.");
            m_VolumeComponent.intensity.value = 0;
        }
    }
    #endregion




}
}