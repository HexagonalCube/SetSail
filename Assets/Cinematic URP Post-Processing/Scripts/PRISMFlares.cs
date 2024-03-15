using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using PRISM.Utils;


// Define the Volume Component for the custom post processing effect 
[System.Serializable, VolumeComponentMenu("Cinematic URP Post Processing/CUPP - PRISM Flares")]
public class PRISMFlares : VolumeComponent
{

    [Tooltip("Use flares")]
    public BoolParameter useFlare = new BoolParameter(false);// (0, 0, 1);
    public FloatParameter bloomFlareStrength = new FloatParameter(1.9f);


    public FloatParameter bloomFlareHaloWidth = new FloatParameter(0.5f);

    public FloatParameter bloomFlareGhostDispersal = new FloatParameter(0.11f);

    public FloatParameter bloomFlareChromaticDistortion = new FloatParameter(1.03f);

    public FloatParameter bloomFlareLensDirtIntensityMultiplier = new FloatParameter(1.0f);

    public FloatParameter bloomFlareRadius1 = new FloatParameter(1f);

    public FloatParameter bloomFlareRadius2 = new FloatParameter(1f);

    public FloatParameter bloomFlareRadius3 = new FloatParameter(1f);

    public IntParameter bloomFlareNumberGhosts = new IntParameter(6);

    public TextureParameter flareTexture = new TextureParameter(null);
}

namespace PRISM.Utils { 

// Define the renderer for the custom post processing effect
public class PRISMFlares_URP : CustomPostProcessingPass<PRISMFlares>
{
    // The postprocessing material
    private Material m_Material;

    // A variable to hold a reference to the corresponding volume component
    private PRISMFlares m_VolumeComponent;


    #region old
    /*
    // The postprocessing material
    private Material m_FlaresMaterial;

    private RenderTargetHandle _intermediate = default;
    private RenderTargetHandle _secondary = default;
    private RenderTargetHandle _tertiary = default;
    private RenderTargetHandle _Main = default;

    public float bloomFlareStrength = 1.9f;
    public float bloomFlareHaloWidth = 0.5f;
    public float bloomFlareGhostDispersal = 0.11f;
    public float bloomFlareChromaticDistortion = 1.03f;
    public float bloomFlareLensDirtIntensityMultiplier = 1.0f;
    public int bloomFlareNumberGhosts = 6;
    public float bloomFlareRadius1 = 1f;
    public float bloomFlareRadius2 = 1f;
    public float bloomFlareRadius3 = 1f;

    // The ids of the shader variables
    static class ShaderIDs
    {
        public static int Input = Shader.PropertyToID("_MainTex");
        public static int Intensity = Shader.PropertyToID("_Intensity");
        public static int FlareTex = Shader.PropertyToID("_FinalFlareTex");

        public static int DownsampleAmount = Shader.PropertyToID("_DownsampleAmount");
    }

    // By default, the effect is visible in the scene view, but we can change that here.
    public override bool visibleInSceneView => true;

    /// Specifies the input needed by this custom post process. Default is Color only.
    public override ScriptableRenderPassInput input => ScriptableRenderPassInput.Depth;

    // Initialized is called only once before the first render call
    // so we use it to create our material
    public override void Initialize()
    {
        m_FlaresMaterial = CoreUtils.CreateEngineMaterial("Hidden/PrismFlares");

        CUPPURPHelpers.CheckShaderKeywordState(m_FlaresMaterial);
    }

    // Called for each camera/injection point pair on each frame. Return true if the effect should be rendered for this camera.
    public override bool Setup(ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMFlares>();

        _Main = new RenderTargetHandle();
        _Main.Init("_MainTex");

        _intermediate = new RenderTargetHandle();
        _intermediate.Init("_IntermediateTex");

        _secondary = new RenderTargetHandle();
        _secondary.Init("_SecondaryTex");

        _tertiary = new RenderTargetHandle();
        _tertiary.Init("_TertiaryTex");

        // if blend value > 0, then we need to render this effect. 
        return m_VolumeComponent.useFlare.value == true;
    }

    // The actual rendering execution is done here
    public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
    {
        // set material properties
        if (m_FlaresMaterial != null)
        {
            m_FlaresMaterial.SetFloat(ShaderIDs.Intensity, 1f);
            m_FlaresMaterial.SetFloat("_FlareGhostDispersal", bloomFlareGhostDispersal);
            m_FlaresMaterial.SetFloat("_FlareChromaticDistortion", bloomFlareChromaticDistortion);
            m_FlaresMaterial.SetFloat("_FlareHaloWidth", bloomFlareHaloWidth);
            m_FlaresMaterial.SetFloat("_FlareStrength", bloomFlareStrength);
            m_FlaresMaterial.SetFloat("_CamRotation", renderingData.cameraData.camera.transform.rotation.eulerAngles.y);
            m_FlaresMaterial.SetInt("_FlareNumberOfGhosts", bloomFlareNumberGhosts);

            if(m_VolumeComponent.flareTexture.value)
            {
                m_FlaresMaterial.SetTexture("_FlareStarburstTex", m_VolumeComponent.flareTexture.value);
            }
        }

        //if(SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore)
        {
            RenderTextureDescriptor descriptorOne = GetTempRTDescriptor(renderingData);
            cmd.GetTemporaryRT(_Main.id, descriptorOne);
            cmd.Blit(source, ShaderIDs.Input);
        }


        // set source texture
        cmd.SetGlobalTexture(ShaderIDs.Input, source);
        //m_FlaresMaterial.SetTexture(ShaderIDs.Input, null);

        if (m_VolumeComponent.useFlare.value == true)
        {
            // Create a temporary target
            RenderTextureDescriptor descriptor = GetTempRTDescriptor(renderingData);

            float initialDofDownsample = 2f;
            descriptor.height /= (int)initialDofDownsample;
            descriptor.width /= (int)initialDofDownsample;

            descriptor.colorFormat = RenderTextureFormat.ARGBHalf;
            cmd.GetTemporaryRT(_intermediate.id, descriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(_tertiary.id, descriptor, FilterMode.Trilinear);

            descriptor.height /= (int)initialDofDownsample;
            descriptor.width /= (int)initialDofDownsample;


            descriptor.height /= (int)initialDofDownsample;
            descriptor.width /= (int)initialDofDownsample;

            cmd.GetTemporaryRT(_secondary.id, descriptor, FilterMode.Bilinear);



            //Flares
            //Downsample and threshold
            //Graphics.Blit(null, _intermediate.id _intermediate.Identifier(), m_FlaresMaterial, 2);
            cmd.Blit(source, _intermediate.Identifier(), m_FlaresMaterial, 2);

            //Median
            cmd.Blit(_intermediate.Identifier(), _secondary.id, m_FlaresMaterial, 3);

            //Upscale and features
            cmd.Blit(_secondary.Identifier(), _tertiary.id, m_FlaresMaterial, 0);

            cmd.SetGlobalTexture(ShaderIDs.FlareTex, _tertiary.id);

            cmd.Blit(ShaderIDs.Input, destination, m_FlaresMaterial, 1);


            cmd.ReleaseTemporaryRT(_intermediate.id);
            cmd.ReleaseTemporaryRT(_secondary.id);
            cmd.ReleaseTemporaryRT(_tertiary.id);
        }
        else
        {
            CoreUtils.DrawFullScreen(cmd, m_FlaresMaterial, destination);
        }

    }
    */
    #endregion

    protected override string RenderTag => "PRISM Flares";

    // The ids of the shader variables
    static class ShaderIDs
    {
        internal readonly static int Input = Shader.PropertyToID("_MainTex");
        internal readonly static int Intermediate = Shader.PropertyToID("_Intermediate");
        internal readonly static int Secondary = Shader.PropertyToID("_Secondary");
        internal readonly static int Tertiary = Shader.PropertyToID("_Tertiary");
        internal readonly static int Intensity = Shader.PropertyToID("_Intensity");
        public static int FlareTex = Shader.PropertyToID("_FinalFlareTex");

        public static int DownsampleAmount = Shader.PropertyToID("_DownsampleAmount");
    }

    protected override void BeforeRender(CommandBuffer commandBuffer, ref RenderingData renderingData)
    {
        // Debug.LogWarning("GOT TO BEFORE RENDERER");

        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMFlares>();

        m_Material.SetFloat(ShaderIDs.Intensity, 1f);
        m_Material.SetFloat("_FlareGhostDispersal", m_VolumeComponent.bloomFlareGhostDispersal.value);
        m_Material.SetFloat("_FlareChromaticDistortion", m_VolumeComponent.bloomFlareChromaticDistortion.value);
        m_Material.SetFloat("_FlareHaloWidth", m_VolumeComponent.bloomFlareHaloWidth.value);
        m_Material.SetFloat("_FlareStrength", m_VolumeComponent.bloomFlareStrength.value);
        m_Material.SetFloat("_CamRotation", renderingData.cameraData.camera.transform.rotation.eulerAngles.y);
        m_Material.SetInt("_FlareNumberOfGhosts", m_VolumeComponent.bloomFlareNumberGhosts.value);

        if (m_VolumeComponent.flareTexture.value)
        {
            m_Material.SetTexture("_FlareStarburstTex", m_VolumeComponent.flareTexture.value);
        }
    }

    //Do the work here
    protected override void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier dest)
    {
        //Debug.Log("BLIT ONE");

        //Flares
        //Downsample and threshold
        //GET RT
        RenderTextureDescriptor descriptor = GetTempRTDescriptor(renderingData);
        commandBuffer.GetTemporaryRT(ShaderIDs.Intermediate, descriptor);
        RenderTargetIdentifier intermediate_Identifier = new RenderTargetIdentifier(source, 0);

        //BLIT #1
        commandBuffer.Blit(source, ShaderIDs.Intermediate, Material, 2);

        float initialDofDownsample = 2f;
        descriptor.height /= (int)initialDofDownsample;
        descriptor.width /= (int)initialDofDownsample;
        commandBuffer.GetTemporaryRT(ShaderIDs.Secondary, descriptor);

        //Median
        commandBuffer.Blit(ShaderIDs.Intermediate, ShaderIDs.Secondary, m_Material, 3);

        //Upscale and features
        commandBuffer.Blit(ShaderIDs.Secondary, ShaderIDs.Intermediate, m_Material, 0);

        commandBuffer.SetGlobalTexture(ShaderIDs.FlareTex, ShaderIDs.Intermediate);
        commandBuffer.Blit(source, dest, Material, 1);


        commandBuffer.ReleaseTemporaryRT(ShaderIDs.Secondary);
        commandBuffer.ReleaseTemporaryRT(ShaderIDs.Intermediate);
    }

    protected override bool IsActive()
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        m_VolumeComponent = stack.GetComponent<PRISMFlares>();

        return Component.active && m_VolumeComponent.useFlare.value == true;
    }



    //init
    public PRISMFlares_URP(RenderPassEvent renderPassEvent, Shader shader) : base(renderPassEvent, shader)
    {
        if (!m_Material)
        {
            m_Material = CoreUtils.CreateEngineMaterial("Hidden/PrismFlares");
            CUPPURPHelpers.CheckShaderKeywordState(m_Material);
            SetMaterial(m_Material);
        }

        //Debug.Log(m_Material.name);

    }


}
}