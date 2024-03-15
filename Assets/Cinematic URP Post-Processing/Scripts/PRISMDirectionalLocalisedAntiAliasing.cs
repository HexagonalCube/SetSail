using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;
using PRISM.Utils;

// Define the Volume Component for the custom post processing effect 
[System.Serializable, VolumeComponentMenu("Cinematic URP Post Processing/CUPP - PRISM DLAA")]
public class PRISMDirectionalLocalisedAntiAliasing : VolumeComponent
{
    [Tooltip("Controls the blending between the original and the grayscale color.")]
    public BoolParameter enableDLAA = new BoolParameter(false);
}
namespace PRISM.Utils { 

// Define the renderer for the custom post processing effect
public class PRISMDLAA_URP : CustomPostProcessingPass<PRISMDirectionalLocalisedAntiAliasing>
{
    // A variable to hold a reference to the corresponding volume component
    private PRISMDirectionalLocalisedAntiAliasing m_VolumeComponent;

    // The postprocessing material
    private Material m_Material;

    protected override string RenderTag => "PRISM DLAA";

    // The ids of the shader variables
    static class ShaderIDs
    {
        internal readonly static int Input = Shader.PropertyToID("_MainTex");
        internal readonly static int Intermediate = Shader.PropertyToID("_Intermediate");
    }

    protected override void BeforeRender(CommandBuffer commandBuffer, ref RenderingData renderingData)
    {
        // Debug.LogWarning("GOT TO BEFORE RENDERER");

        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMDirectionalLocalisedAntiAliasing>();

    }

    //Do the work here
    protected override void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier dest)
    {

        RenderTextureDescriptor descriptor = GetTempRTDescriptor(renderingData);
        commandBuffer.GetTemporaryRT(ShaderIDs.Intermediate, descriptor);

        //Debug.Log("BLIT ONE");
        RenderTargetIdentifier intermediate_Identifier = new RenderTargetIdentifier(source, 0);
        commandBuffer.Blit(source, ShaderIDs.Intermediate, Material, 0);

        commandBuffer.Blit(intermediate_Identifier, dest, Material, 1);

        commandBuffer.ReleaseTemporaryRT(ShaderIDs.Intermediate);
    }

    protected override bool IsActive()
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        m_VolumeComponent = stack.GetComponent<PRISMDirectionalLocalisedAntiAliasing>();

        return Component.active && m_VolumeComponent.enableDLAA == true;
    }


    //init
    public PRISMDLAA_URP(RenderPassEvent renderPassEvent, Shader shader) : base(renderPassEvent, shader)
    {
        if (!m_Material)
        {
            m_Material = CoreUtils.CreateEngineMaterial("PRISM/PRISMDLAA");
            CUPPURPHelpers.CheckShaderKeywordState(m_Material);
            SetMaterial(m_Material);
        }

    }

}
}