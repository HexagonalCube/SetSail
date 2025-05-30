using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;
using PRISM.Utils;


// Define the Volume Component for the custom post processing effect 
[System.Serializable, VolumeComponentMenu("Cinematic URP Post Processing/CUPP - PRISM DOF")]
public class PRISMDepthOfField : VolumeComponent
{
    [Tooltip("Enable the effect")]
    public BoolParameter enableDoF = new BoolParameter(false);

    public ClampedFloatParameter aperture = new ClampedFloatParameter(1, 0.8f, 16);

    public ClampedFloatParameter focalLength = new ClampedFloatParameter(50f, 12f, 200f);

    [HideInInspector]
    public BoolParameter enableGPUAutofocus = new BoolParameter(false);
    public BoolParameter enableFrontBlur = new BoolParameter(false);

    public BoolParameter enableFastMobileDOF = new BoolParameter(true);
    public BoolParameter enableDebugMode = new BoolParameter(false);
    public BoolParameter useCameraPhysicalProperties = new BoolParameter(false);

    public ClampedFloatParameter autoFocusSpeed = new ClampedFloatParameter(0.5f, 0.0001f, 1.0f);

    [Tooltip("Maximum amount of blur applied.")]
    public ClampedFloatParameter maxBlur = new ClampedFloatParameter(0.7f, 0.0001f, 4f);
    [Tooltip("Focus distance when not using AutoFocus")]
    public ClampedFloatParameter focusDistance = new ClampedFloatParameter(0.3f, 0.3f, 100f);

    [HideInInspector]
    public ClampedFloatParameter bokehIntensity = new ClampedFloatParameter(0.7f, 0.0001f, 2f);

    public ClampedIntParameter bokehRings = new ClampedIntParameter(5, 1, 8);

    [HideInInspector]
    public ClampedIntParameter apertureEdgeCount = new ClampedIntParameter(5, 1, 8);

    public ClampedIntParameter dofSamplesPerRing = new ClampedIntParameter(8, 1, 8);

    public ClampedIntParameter dofSamplesPerEdge = new ClampedIntParameter(4, 1, 8);
}

namespace PRISM.Utils { 

// Define the renderer for the custom post processing effect
public class PRISMDoF_URP : CustomPostProcessingPass<PRISMDepthOfField>
{
    //Needs to be defined the same as in the shader.
    public struct MyCustomData
    {
        public Vector3 something;
        public Vector3 somethingElse;
    }
    MyCustomData[] m_MyCustomDataArr;

    private Material m_Material;

    // The ids of the shader variables
    static class ShaderIDs
    {
        internal readonly static int Input = Shader.PropertyToID("_MainTex");
        internal readonly static int Intermediate = Shader.PropertyToID("_Intermediate");
        internal readonly static int Aperture = Shader.PropertyToID("fAperture");
        internal readonly static int Secondary = Shader.PropertyToID("_SecondaryTex");
        internal readonly static int Tertiary = Shader.PropertyToID("_DoFTex");
        internal readonly static int FocalLength = Shader.PropertyToID("fFocalLength");
        internal readonly static int Threshold = Shader.PropertyToID("BokehDoFTreshold");


        internal readonly static int DoFMaxBlur = Shader.PropertyToID("DoFMaxBlur");
        internal readonly static int BokehIntensity = Shader.PropertyToID("BokehDoFIntensity");
        internal readonly static int DoFRings = Shader.PropertyToID("DoFRings");
        internal readonly static int ApertureEdgeCount = Shader.PropertyToID("BokehDoFEdgeCount");
        internal readonly static int DoFSamplesPerRing = Shader.PropertyToID("DoFSamplesPerRing");
        internal readonly static int BokehDoFSamplePerEdge = Shader.PropertyToID("BokehDoFSamplePerEdge");
        internal readonly static int DofFocusDist = Shader.PropertyToID("DofFocusDist");
        internal readonly static int DofSensorSize = Shader.PropertyToID("DofSensorSize");
        internal readonly static int DofCoefficient = Shader.PropertyToID("DofCoefficient");
        internal readonly static int DofCoefficientFront = Shader.PropertyToID("DofCoefficientFront");

        internal readonly static int ConstantBufferName = Shader.PropertyToID("_MyCustomBuffer");
    }

    #region old
    /*
    // A variable to hold a reference to the corresponding volume component
    private PRISMDepthOfField m_VolumeComponent;

    // The postprocessing material
    private Material m_Material;

    private RenderTargetHandle _intermediate = default;
    public RenderTexture intTexture;
    private RenderTargetHandle _Main = default;



    // By default, the effect is visible in the scene view, but we can change that here.
    public override bool visibleInSceneView
    {
        get
        {
            if (m_VolumeComponent == null) return false;

            return false;
        }   
    }

    /// Specifies the input needed by this custom post process. Default is Color only.
    public override ScriptableRenderPassInput input => ScriptableRenderPassInput.Depth;

    // Initialized is called only once before the first render call
    // so we use it to create our material
    public override void Initialize()
    {
        m_Material = CoreUtils.CreateEngineMaterial("PRISM/PRISMDOF");
        
        ConstructDataBuffers();

        CUPPURPHelpers.CheckShaderKeywordState(m_Material);
    }

    // Called for each camera/injection point pair on each frame. Return true if the effect should be rendered for this camera.
    public override bool Setup(ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMDepthOfField>();

        _intermediate = new RenderTargetHandle();
        _intermediate.Init("_IntermediateTex");

        _Main = new RenderTargetHandle();
        _Main.Init("_MainTex");

        // if blend value > 0, then we need to render this effect. 
        return m_VolumeComponent.enableDoF.value == true;
    }

    ComputeBuffer m_AutofocusComputeBuffer;

    void ConstructDataBuffers()
    {
        if (m_VolumeComponent.enableGPUAutofocus.value == false) return;

        int memalloc = 24;
        int bufferLength = 2;
        m_AutofocusComputeBuffer = new ComputeBuffer(bufferLength, memalloc);//stride == sizeof(MyCustomDataStruct)
        Graphics.SetRandomWriteTarget(1, m_AutofocusComputeBuffer, true);
        m_Material.SetBuffer(ShaderIDs.ConstantBufferName, m_AutofocusComputeBuffer);

        m_MyCustomDataArr = new MyCustomData[bufferLength];
    }


    // Dispose of the allocated resources
    public override void Dispose(bool disposing)
    {
        if (m_VolumeComponent.enableGPUAutofocus.value == true)
            m_AutofocusComputeBuffer.Dispose();

        intTexture.Release();

        base.Dispose(disposing);
    }

    public void OnDisable()
    {
        if (m_VolumeComponent.enableGPUAutofocus.value == true)
            m_AutofocusComputeBuffer.Dispose();
    }

    void OnDestroy()
    {
        if (m_VolumeComponent.enableGPUAutofocus.value == true)
            m_AutofocusComputeBuffer.Dispose();
    }

    void Reset()
    {
        if (m_VolumeComponent.enableGPUAutofocus.value == true)
            m_AutofocusComputeBuffer.Dispose();
    }

    

    // The actual rendering execution is done here
    public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
    {
        float apVal = m_VolumeComponent.aperture.value;
        // set source texture
        m_Material.SetFloat(ShaderIDs.Aperture, apVal);
        m_Material.SetFloat(ShaderIDs.BokehIntensity, m_VolumeComponent.bokehIntensity.value);
        m_Material.SetFloat(ShaderIDs.Threshold, m_VolumeComponent.autoFocusSpeed.value);
        m_Material.SetFloat(ShaderIDs.DoFMaxBlur, m_VolumeComponent.maxBlur.value);

        if (m_VolumeComponent.enableFrontBlur.value == true && m_VolumeComponent.enableFastMobileDOF.value == false)
        {
            m_Material.EnableKeyword("DOF_FRONTBLUR");
        }
        else
        {
            m_Material.DisableKeyword("DOF_FRONTBLUR");
        }

        if (m_VolumeComponent.enableDebugMode.value == true)
        {
            m_Material.EnableKeyword("DOF_DEBUG");
        }
        else
        {
            m_Material.DisableKeyword("DOF_DEBUG");
        }

        if (renderingData.cameraData.camera.usePhysicalProperties == true && m_VolumeComponent.useCameraPhysicalProperties.value == true)
        {
            float sensorHeight = renderingData.cameraData.camera.sensorSize.y * 0.001f;
            m_Material.SetFloat(ShaderIDs.DofSensorSize, sensorHeight);
            m_Material.SetFloat(ShaderIDs.FocalLength, renderingData.cameraData.camera.focalLength * 0.01f);
            //Debug.Log(renderingData.cameraData.camera.focalLength);
        } else
        {
            m_Material.SetFloat(ShaderIDs.FocalLength,  m_VolumeComponent.focalLength.value * 0.01f);
            m_Material.SetFloat(ShaderIDs.DofSensorSize, 0.024f);
        }

        m_Material.SetInteger(ShaderIDs.DoFSamplesPerRing, m_VolumeComponent.dofSamplesPerRing.value);
        m_Material.SetInteger(ShaderIDs.DoFRings, m_VolumeComponent.bokehRings.value);
        m_Material.SetInteger(ShaderIDs.ApertureEdgeCount, m_VolumeComponent.apertureEdgeCount.value);

        #region SETUPSHADERPARAMS
        var fDistance = m_VolumeComponent.focusDistance.value;
        var lensFLength = m_VolumeComponent.focalLength.value / 100f;
        fDistance = Mathf.Max(fDistance, lensFLength);

        m_Material.SetFloat(ShaderIDs.DofFocusDist, fDistance);

        var coeff = lensFLength * lensFLength / (apVal * (fDistance - lensFLength) * 0.024f * 2);
        m_Material.SetFloat(ShaderIDs.DofCoefficient, coeff);

        float frontCoeff = 0.0f;
        frontCoeff = Mathf.Min(lensFLength, fDistance - 0.01f);

        m_Material.SetFloat(ShaderIDs.DofCoefficientFront, coeff);
        
        #endregion


        RenderTextureDescriptor descriptorOne = GetTempRTDescriptor(renderingData);
        cmd.GetTemporaryRT(_Main.id, descriptorOne);
        cmd.Blit(source, ShaderIDs.Input);
        cmd.SetGlobalTexture(ShaderIDs.Input, _Main.id);

        // Create a temporary target
        RenderTextureDescriptor dofDescriptor = GetTempRTDescriptor(renderingData);

        //dofDescriptor.colorFormat = RenderTextureFormat.ARGBHalf;

        int combinePass = 1;
        int prePass = 0;

        float initialDofDownsample = 4f;
        dofDescriptor.height /= (int)initialDofDownsample;
        dofDescriptor.width /= (int)initialDofDownsample;

        if (m_VolumeComponent.enableFastMobileDOF.value == true)
        {
            combinePass = 2;
            prePass = 3;
        }

        cmd.GetTemporaryRT(_intermediate.id, dofDescriptor, FilterMode.Bilinear);
        //CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, _Main.id, _intermediate.Identifier(), m_Material, prePass);
        cmd.Blit(_Main.id, _intermediate.Identifier(), m_Material, prePass);

        //cmd.SetGlobalTexture(ShaderIDs.Tertiary, _intermediate.Identifier());

        //1 = combine
        cmd.SetGlobalTexture(ShaderIDs.Secondary, _intermediate.id);

        //CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, _Main.id, renderingData.cameraData.renderer.cameraColorTarget, m_Material, combinePass);
        //cmd.Blit(_Main.id, renderingData.cameraData.renderer.cameraColorTarget, m_Material, combinePass);
        cmd.Blit(_Main.id, destination, m_Material, combinePass);

        //CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, source, destination, m_Material, combinePass);

        //cmd.Blit(_intermediate.Identifier(), renderingData.cameraData.renderer.cameraColorTarget, m_Material, combinePass);

        cmd.ReleaseTemporaryRT(_intermediate.id);
        cmd.ReleaseTemporaryRT(_Main.id);
    }
    */
    #endregion


    // A variable to hold a reference to the corresponding volume component
    private PRISMDepthOfField m_VolumeComponent;

    protected override string RenderTag => "PRISM DoF";

    protected override void BeforeRender(CommandBuffer commandBuffer, ref RenderingData renderingData)
    {
        // Debug.LogWarning("GOT TO BEFORE RENDERER");

        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMDepthOfField>();

        float apVal = m_VolumeComponent.aperture.value;
        // set source texture
        m_Material.SetFloat(ShaderIDs.Aperture, apVal);
        m_Material.SetFloat(ShaderIDs.BokehIntensity, m_VolumeComponent.bokehIntensity.value);
        m_Material.SetFloat(ShaderIDs.Threshold, m_VolumeComponent.autoFocusSpeed.value);
        m_Material.SetFloat(ShaderIDs.DoFMaxBlur, m_VolumeComponent.maxBlur.value);

        if (m_VolumeComponent.enableFrontBlur.value == true && m_VolumeComponent.enableFastMobileDOF.value == false)
        {
            m_Material.EnableKeyword("DOF_FRONTBLUR");
        }
        else
        {
            m_Material.DisableKeyword("DOF_FRONTBLUR");
        }

        if (m_VolumeComponent.enableDebugMode.value == true)
        {
            m_Material.EnableKeyword("DOF_DEBUG");
        }
        else
        {
            m_Material.DisableKeyword("DOF_DEBUG");
        }

        if (renderingData.cameraData.camera.usePhysicalProperties == true && m_VolumeComponent.useCameraPhysicalProperties.value == true)
        {
            float sensorHeight = renderingData.cameraData.camera.sensorSize.y * 0.001f;
            m_Material.SetFloat(ShaderIDs.DofSensorSize, sensorHeight);
            m_Material.SetFloat(ShaderIDs.FocalLength, renderingData.cameraData.camera.focalLength * 0.01f);
            //Debug.Log(renderingData.cameraData.camera.focalLength);
        }
        else
        {
            m_Material.SetFloat(ShaderIDs.FocalLength, m_VolumeComponent.focalLength.value * 0.01f);
            m_Material.SetFloat(ShaderIDs.DofSensorSize, 0.024f);
        }

        m_Material.SetInteger(ShaderIDs.DoFSamplesPerRing, m_VolumeComponent.dofSamplesPerRing.value);
        m_Material.SetInteger(ShaderIDs.DoFRings, m_VolumeComponent.bokehRings.value);
        m_Material.SetInteger(ShaderIDs.ApertureEdgeCount, m_VolumeComponent.apertureEdgeCount.value);

        #region SETUPSHADERPARAMS
        var fDistance = m_VolumeComponent.focusDistance.value;
        var lensFLength = m_VolumeComponent.focalLength.value / 100f;
        fDistance = Mathf.Max(fDistance, lensFLength);

        m_Material.SetFloat(ShaderIDs.DofFocusDist, fDistance);

        var coeff = lensFLength * lensFLength / (apVal * (lensFLength) * 0.024f * 2);
        m_Material.SetFloat(ShaderIDs.DofCoefficient, coeff);
       // Debug.Log(coeff);
        //Debug.Log(lensFLength);

        float frontCoeff = 0.0f;
        frontCoeff = Mathf.Min(lensFLength, fDistance - 0.01f);

        m_Material.SetFloat(ShaderIDs.DofCoefficientFront, coeff);

        #endregion
    }

    //Do the work here
    protected override void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier dest)
    {
        //commandBuffer.Blit(source, dest, Material, 0);
        //Debug.Log("BLIT ONE"); 
               
        int combinePass = 1;
        int prePass = 0;

        if (m_VolumeComponent.enableFastMobileDOF.value == true)
        {
            combinePass = 2;
            prePass = 3;
        }

        // Create a temporary target, downsampled
        RenderTextureDescriptor dofDescriptor = GetTempRTDescriptor(renderingData);
        float initialDofDownsample = 4f;
        dofDescriptor.height /= (int)initialDofDownsample;
        dofDescriptor.width /= (int)initialDofDownsample;

        //GET RT
        commandBuffer.GetTemporaryRT(ShaderIDs.Intermediate, dofDescriptor);
        RenderTargetIdentifier intermediate_Identifier = new RenderTargetIdentifier(source, 0);

        //BLIT #1
        commandBuffer.Blit(source, ShaderIDs.Intermediate, Material, prePass);

        //1 = combine
        commandBuffer.SetGlobalTexture(ShaderIDs.Secondary, ShaderIDs.Intermediate);
        commandBuffer.Blit(source, dest, m_Material, combinePass);


        commandBuffer.ReleaseTemporaryRT(ShaderIDs.Secondary);

    }

    protected override bool IsActive()
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        m_VolumeComponent = stack.GetComponent<PRISMDepthOfField>();

        return Component.active && m_VolumeComponent.enableDoF.value == true;
    }



    //init
    public PRISMDoF_URP(RenderPassEvent renderPassEvent, Shader shader) : base(renderPassEvent, shader)
    {
        if (!m_Material)
        {
            m_Material = CoreUtils.CreateEngineMaterial("PRISM/PRISMDOF");
            CUPPURPHelpers.CheckShaderKeywordState(m_Material);
            SetMaterial(m_Material);
        }

        //Debug.Log(m_Material.name);

    }




}
}