using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using System.Linq;
using PRISM.Utils;


// Define the Volume Component for the custom post processing effect 
[System.Serializable, VolumeComponentMenu("Cinematic URP Post Processing/CUPP - PRISM Long Exposure")]
public class PRISMLongExposure : VolumeComponent
{
    [Tooltip("Intensity of the effect")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 4f);

    public ClampedFloatParameter exposureTime = new ClampedFloatParameter(0f, 0f, 0.1f);

    [HideInInspector]
    public BoolParameter debugMode = new BoolParameter(false);
    public BoolParameter showShutterSpeed = new BoolParameter(false);
}

namespace PRISM.Utils { 

// Define the renderer for the custom post processing effect
public class PRISMLongExposure_URP : CustomPostProcessingPass<PRISMLongExposure>
{
    // A variable to hold a reference to the corresponding volume component
    private PRISMLongExposure m_VolumeComponent;

    // The postprocessing material
    private Material m_Material;

    BufferedRTHandleSystem historyHandler = new BufferedRTHandleSystem();
    private float t = 0.0f;

    /// <summary>
    /// Used to store the history of a camera (its previous frame and whether it is valid or not)
    /// </summary>
    private class CameraHistory
    {
        public RenderTexture Frame { get; set; }
        public RenderTexture FrameSecondary { get; set; }
        public float TimeLastUpdated { get; set; }
        public bool Invalidated { get; set; }

        public CameraHistory(RenderTextureDescriptor descriptor)
        {
            Frame = new RenderTexture(descriptor);
            FrameSecondary = null;
            TimeLastUpdated = Time.time;
            //FrameSecondary = new RenderTexture(descriptor);
            Invalidated = false;
        }
    }

    int historyTracker = 0;
    const int maxHistories = 4;

    // We store the history for each camera separately (key is the camera instance id).
    private Dictionary<int, CameraHistory> _histories = null;

    private RTHandle m_AccumMotion;
    public RenderTexture accumMotion;
    private RTHandle m_AccumMotionTemp;
    public RenderTexture accumMotionTEMP;
    private RTHandle m_AccumMotionTempFinal;
    public RenderTexture accumMotionTEMPFinal;

    #region old
    /*
    /// Specifies the input needed by this custom post process. Default is Color only.
    public override ScriptableRenderPassInput input => ScriptableRenderPassInput.Motion;





    private RenderTargetHandle m_AccumMotion;
    public RenderTexture accumMotion;
    private RenderTargetHandle m_AccumMotionTemp;
    public RenderTexture accumMotionTEMP;
    private RenderTargetHandle m_AccumMotionTempFinal;
    public RenderTexture accumMotionTEMPFinal;

    private RenderTargetHandle _intermediate = default;
    private RenderTargetHandle _Main = default;

    // Initialized is called only once before the first render call
    // so we use it to create our material
    public override void Initialize()
    {
        _histories = new Dictionary<int, CameraHistory>();
        m_Material = CoreUtils.CreateEngineMaterial("PRISM/PRISMLongExposure");

        m_AccumMotion = new RenderTargetHandle();
        m_AccumMotion.Init("_MidTex");

        m_AccumMotionTemp = new RenderTargetHandle();
        m_AccumMotionTemp.Init("_AccumMotion");

        m_AccumMotionTempFinal = new RenderTargetHandle();
        m_AccumMotionTempFinal.Init("_MotionTemp");

        CUPPURPHelpers.CheckShaderKeywordState(m_Material);
    }

    // Called for each camera/injection point pair on each frame. Return true if the effect should be rendered for this camera.
    public override bool Setup(ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMLongExposure>();

        bool requireRendering = m_VolumeComponent.intensity.value > 0;

        _intermediate = new RenderTargetHandle();
        _intermediate.Init("_IntermediateTex");

        _Main = new RenderTargetHandle();
        _Main.Init("_MainTex");

        // if blend value > 0, then we need to render this effect. 
        return requireRendering;
    }

    // Dispose of the allocated resources
    public override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        // Release every history RT
        foreach (var entry in _histories)
        {
            entry.Value.Frame.Release();
        }
        // Clear the histories dictionary
        _histories.Clear();

        accumMotion.Release();
        accumMotionTEMP.Release();
        accumMotionTEMPFinal.Release();
    }

    public void IncreaseHistoryTracker()
    {
        //Debug.Log(Time.unscaledDeltaTime);

        if (historyTracker == 0) {
            //Debug.Log("Increased tracker" + Time.time);
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampOne, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.5f, 0.0f, 0.0f, 0.0f));
        }

        if (historyTracker == 1) {
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampTwo, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.0f, 0.5f, 0.0f, 0.0f));
        }

        if (historyTracker == 2) {
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampThree, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.0f, 0.0f, 0.5f, 0.0f));
        }

        if (historyTracker == 3) {
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampFour, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.0f, 0.0f, 0.0f, 0.5f));
        }


        historyTracker++;
        if (historyTracker > maxHistories)
        {
            historyTracker =0;
        }
    }

    public bool CheckIfTextureShouldBeAccumulated(float timeCreated)
    {
        var addedTime = timeCreated + m_VolumeComponent.exposureTime.value;
        if (addedTime < Time.time)
        {
            return true;
        }
        return false;
    }

    public float ValueIfTextureShouldBeAccumulated(float timeCreated)
    {
        var maxTimeItCanStillBeUsed = Time.time - m_VolumeComponent.exposureTime.value;

        if(maxTimeItCanStillBeUsed > timeCreated)
        {
            return 0f;
        }
        return 1f;
    }


   public RenderTextureFormat _vectorRTFormat = RenderTextureFormat.RGHalf;


    // The actual rendering execution is done here
    public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
    {


        // Get camera instance id
        int id = historyTracker;
        IncreaseHistoryTracker();

        // Create a temporary target
        var bHalf = RenderTexture.GetTemporary(descriptor);

        CameraHistory history;

        // See if we already have a history for this camera
        if (_histories.TryGetValue(id, out history))
        {
            //Debug.Log("Found ID#: " + id);
            //Debug.Log("Tiem created: " + history.TimeLastUpdated);

            //Here we need to check: If TimeLastUpdated is so recently that we still should be accumulating this frame into the current sensor readout, then instead of performing a standard backbuffer blit, we perform a lerping 60/40 blit to essentially accumulate two frames.
            //If at frame 160 (TimeLastUpdated) we copied this frame last, and our ExposureTime is 100 frames, and it is now frame 240 (Time.time), then we perform the above, flagging that the next blit on this RT should be a 'fresh' one so as to not re-accumulate old frames.
            if (CheckIfTextureShouldBeAccumulated(history.TimeLastUpdated))
            {
                //Debug.Log("History valid!");
                history.Invalidated = true;
            }
            else
            {
                //Debug.LogWarning("History IS NOT valid!");
                history.Invalidated = false;
            }

            var frame = history.Frame;
            // If the camera target is resized, we need to resize the history too.
            if(frame)
            {
                if (frame.width != descriptor.width || frame.height != descriptor.height)
                {
                    //Debug.Log("ID#: " + id + " not valid dims");

                    RenderTexture newframe = new RenderTexture(descriptor);
                    newframe.name = "_CameraHistoryTexture" + id.ToString();
                    if (history.Invalidated) // if invalidated, blit from source to history
                    {
                        cmd.Blit(source, newframe);//, m_Material, 13);
                    }
                    frame.Release();
                    history.Frame = newframe;
                }
                else if (history.Invalidated)
                {
                    //Debug.Log("ID#: " + id + " invalidated");
                    cmd.Blit(source, frame);// if invalidated, blit from source to history
                }
                else if (!history.Invalidated)//if we good
                {
                    RenderTexture newframe = new RenderTexture(descriptor);
                    newframe.name = "_CameraHistoryTexture" + id.ToString();
                    cmd.SetGlobalTexture(ShaderIDs.LongExpTex, history.Frame);
                    cmd.Blit(source, newframe, m_Material, 5);
                    frame.Release();
                    history.Frame = newframe;
                }
                history.TimeLastUpdated = Time.time;
                history.Invalidated = false;
            }
        }
        else
        {
            //Debug.Log("Didnt find ID#: " + id + " Creating new");
            // If we had no history for this camera, create one for it.
            history = new CameraHistory(descriptor);
            history.TimeLastUpdated = Time.time;
            history.Frame.name = "_CameraHistoryTexture" + id.ToString();
            _histories.Add(id, history);
            cmd.Blit(source, history.Frame);//, m_Material, 13); // Copy frame from source to history
        }

        int initialPass = 1;


        if(accumMotion == null)
        {
            var tempMD = GetTempRTDescriptor(renderingData);
            tempMD.colorFormat = RenderTextureFormat.RG16;
            tempMD.width = tempMD.width / 8;
            tempMD.height = tempMD.height / 8;
            accumMotion = new RenderTexture(tempMD);
        }

        if (accumMotionTEMP == null)
        {
            accumMotionTEMP = new RenderTexture(motionDescriptor);
        }

        if (accumMotionTEMPFinal == null)
        {
            accumMotionTEMPFinal = new RenderTexture(motionDescriptor);
        }

        //Motion vector prepass Tilemax
        //cmd.GetTemporaryRT(m_AccumMotion.id, motionDescriptor);
        GetTemporaryDividedRT(cmd, m_AccumMotion.id, motionDescriptor, 2, RenderTextureFormat.RG16);

        cmd.SetGlobalTexture(ShaderIDs.AccumulatedMotion, accumMotion);
        CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, source, m_AccumMotion.Identifier(), m_Material, 0);

        //Tilemax again
        GetTemporaryDividedRT(cmd, m_AccumMotionTemp.id, motionDescriptor, 4, RenderTextureFormat.RG16);
        CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, m_AccumMotion.Identifier(), m_AccumMotionTemp.Identifier(), m_Material, 6);

        //Tilemax again
        GetTemporaryDividedRT(cmd, m_AccumMotionTempFinal.id, motionDescriptor, 8, RenderTextureFormat.RG16);
        CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, m_AccumMotionTemp.Identifier(), m_AccumMotionTempFinal.Identifier(), m_Material, 6);

        //cmd.Blit(m_AccumMotionTempFinal.Identifier(), accumMotion);
        CUPPURPHelpers.BlitFullscreenMesh(cmd, ShaderIDs.Input, m_AccumMotionTempFinal.Identifier(), accumMotion, m_Material, 7);

        if (m_VolumeComponent.debugMode.value == true)
        {
            cmd.SetGlobalTexture(ShaderIDs.AccumulatedMotion, accumMotion);
            cmd.Blit(source, destination, m_Material, 4);
            return;
        }


        //History.Frame = last frame. We need to blend our NEW frame AND history.frame into new RT
        cmd.SetGlobalTexture(ShaderIDs.Input, source);
        m_Material.SetTexture(ShaderIDs.LongExpTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryOneTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryTwoTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryThreeTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryFourTex, history.Frame);
        cmd.SetGlobalTexture(ShaderIDs.AccumulatedMotion, accumMotion);

        Vector4 timeStampWeights = new Vector4(0f, 0f, 0f, 0f);

        for (int i = 0; i < maxHistories; i++)
        {
            CameraHistory historyTemp;
            // See if we already have a history for this camera
            if (_histories.TryGetValue(i, out historyTemp))
            {
                timeStampWeights[i] = ValueIfTextureShouldBeAccumulated(historyTemp.TimeLastUpdated);

                //Debug.Log("Got to #: " + i);
                if(i == 0) m_Material.SetTexture(ShaderIDs.HistoryOneTex, historyTemp.Frame);
                if (i == 1) m_Material.SetTexture(ShaderIDs.HistoryTwoTex, historyTemp.Frame);
                if (i == 2) m_Material.SetTexture(ShaderIDs.HistoryThreeTex, historyTemp.Frame);
                if (i == 3) m_Material.SetTexture(ShaderIDs.HistoryFourTex, historyTemp.Frame);
            }
        }

       // Debug.LogWarning(timeStampWeights);
        m_Material.SetVector(ShaderIDs.HistoryTimestampWeights, timeStampWeights);


        cmd.Blit(ShaderIDs.Input, destination, m_Material, initialPass);

        //Now, send the new RT as our destination
        //cmd.SetGlobalTexture(ShaderIDs.MainTex, bHalf);
        //cmd.Blit(bHalf, destination);
        
        //cmd.Blit(bHalf, renderingData.cameraData.renderer.cameraColorTarget);

        //Now, blit the new RT back into History.Frame
        m_Material.SetTexture(ShaderIDs.Input, bHalf);
        cmd.Blit(destination, history.Frame);

        RenderTexture.ReleaseTemporary(bHalf);

        cmd.ReleaseTemporaryRT(m_AccumMotion.id);
        cmd.ReleaseTemporaryRT(m_AccumMotionTemp.id);
        cmd.ReleaseTemporaryRT(m_AccumMotionTempFinal.id);
    }

    */

    #endregion


    protected override string RenderTag => "PRISM LongExposure";

    // The ids of the shader variables
    static class ShaderIDs
    {
        internal readonly static int Input = Shader.PropertyToID("_MainTex");
        internal readonly static int Intermediate = Shader.PropertyToID("_Intermediate");

        internal readonly static int AccumulatedMotion = Shader.PropertyToID("_AccumMotion");
        internal readonly static int MotionTemp = Shader.PropertyToID("_MotionTemp");
        internal readonly static int MotionTempFinal = Shader.PropertyToID("_MotionTempFinal");
        internal readonly static int Intensity = Shader.PropertyToID("_Intensity");

        internal readonly static int HistoryOneTex = Shader.PropertyToID("_HistoryOne");
        internal readonly static int HistoryTwoTex = Shader.PropertyToID("_HistoryTwo");
        internal readonly static int HistoryThreeTex = Shader.PropertyToID("_HistoryThree");
        internal readonly static int HistoryFourTex = Shader.PropertyToID("_HistoryFour");

        internal readonly static int HistoryTimestampWeights = Shader.PropertyToID("_HistoryTimestampWeights");

        internal readonly static int HistoryTexExposureTimestampOne = Shader.PropertyToID("_HistoryTexExposureTimestampOne");
        internal readonly static int HistoryTexExposureTimestampTwo = Shader.PropertyToID("_HistoryTexExposureTimestampTwo");
        internal readonly static int HistoryTexExposureTimestampThree = Shader.PropertyToID("_HistoryTexExposureTimestampThree");
        internal readonly static int HistoryTexExposureTimestampFour = Shader.PropertyToID("_HistoryTexExposureTimestampFour");

        internal readonly static int LongExpTex = Shader.PropertyToID("_LongExpTex");
    }

    void GetTemporaryDividedRT(CommandBuffer cmd, int nameId, RenderTextureDescriptor source, int divider, RenderTextureFormat format)
    {
        var w = source.width / divider;
        var h = source.height / divider;
        source.width = w;
        source.height = h;
        cmd.GetTemporaryRT(nameId, source);
    }

    public void IncreaseHistoryTracker()
    {
        //Debug.Log(Time.unscaledDeltaTime);

        if (historyTracker == 0)
        {
            //Debug.Log("Increased tracker" + Time.time);
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampOne, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.5f, 0.0f, 0.0f, 0.0f));
        }

        if (historyTracker == 1)
        {
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampTwo, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.0f, 0.5f, 0.0f, 0.0f));
        }

        if (historyTracker == 2)
        {
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampThree, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.0f, 0.0f, 0.5f, 0.0f));
        }

        if (historyTracker == 3)
        {
            m_Material.SetFloat(ShaderIDs.HistoryTexExposureTimestampFour, Time.time);
            m_Material.SetVector("_LastExposedHistory", new Vector4(0.0f, 0.0f, 0.0f, 0.5f));
        }


        historyTracker++;
        if (historyTracker > maxHistories)
        {
            historyTracker = 0;
        }
    }

    public bool CheckIfTextureShouldBeAccumulated(float timeCreated)
    {
        var addedTime = timeCreated + m_VolumeComponent.exposureTime.value;
        if (addedTime < Time.time)
        {
            return true;
        }
        return false;
    }

    public float ValueIfTextureShouldBeAccumulated(float timeCreated)
    {
        var maxTimeItCanStillBeUsed = Time.time - m_VolumeComponent.exposureTime.value;

        if (maxTimeItCanStillBeUsed > timeCreated)
        {
            return 0f;
        }
        return 1f;
    }

    protected void SetShaderValues(Material longExpMaterial, Camera m_Camera, float height)
    {
        var maxBlurPixels = (m_VolumeComponent.intensity.value * height / 100);
        longExpMaterial.SetFloat(ShaderIDs.Intensity, maxBlurPixels);

        m_Camera.depthTextureMode |= DepthTextureMode.Depth;
        m_Camera.depthTextureMode |= DepthTextureMode.MotionVectors;
    }

    protected override void BeforeRender(CommandBuffer commandBuffer, ref RenderingData renderingData)
    {
        // Debug.LogWarning("GOT TO BEFORE RENDERER");

        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        // Get the corresponding volume component
        m_VolumeComponent = stack.GetComponent<PRISMLongExposure>();

        // set material properties
        m_Material.SetFloat(ShaderIDs.Intensity, m_VolumeComponent.intensity.value);

        float xpTime = m_VolumeComponent.exposureTime.value;
        float deltaTimeMultiplier = Time.unscaledDeltaTime;
        t += deltaTimeMultiplier / xpTime;
        if (t >= xpTime && m_VolumeComponent.debugMode == false)
        {
            //doUpdate = false;
            t = 0f;

        }
        //Debug.Log("T: " + t + ", expWeight: " + (deltaTimeMultiplier / xpTime));
        float expWeight = deltaTimeMultiplier / xpTime;
        m_Material.SetFloat("individualExposureWeight", expWeight);
        m_Material.SetFloat("_ExposureTime", xpTime);
        m_Material.SetFloat("_ExposureTimestamp", Time.time);

        //Debug.Log("set weight to: " + expWeight);

        //Managing the constant accumulated motion texture
        RenderTextureDescriptor motionDescriptor = GetTempRTDescriptor(renderingData);
        motionDescriptor.colorFormat = RenderTextureFormat.RG16;

        SetShaderValues(m_Material, renderingData.cameraData.camera, motionDescriptor.height);
    }

    //Do the work here
    protected override void Render(CommandBuffer commandBuffer, ref RenderingData renderingData, RenderTargetIdentifier source, RenderTargetIdentifier dest)
    {
        //Debug.Log("BLIT ONE");
        //commandBuffer.Blit(source, dest, Material, 0);

        // Get camera instance id
        int id = historyTracker;
        IncreaseHistoryTracker();

        // Create a temporary target
        RenderTextureDescriptor descriptor = GetTempRTDescriptor(renderingData);
        RenderTextureDescriptor motionDescriptor = GetTempRTDescriptor(renderingData);
        motionDescriptor.colorFormat = RenderTextureFormat.RG16;

        var bHalf = RenderTexture.GetTemporary(descriptor);

        CameraHistory history;
        
        // See if we already have a history for this camera
        if (_histories.TryGetValue(id, out history))
        {
            //Debug.Log("Found ID#: " + id);
            //Debug.Log("Tiem created: " + history.TimeLastUpdated);

            //Here we need to check: If TimeLastUpdated is so recently that we still should be accumulating this frame into the current sensor readout, then instead of performing a standard backbuffer blit, we perform a lerping 60/40 blit to essentially accumulate two frames.
            //If at frame 160 (TimeLastUpdated) we copied this frame last, and our ExposureTime is 100 frames, and it is now frame 240 (Time.time), then we perform the above, flagging that the next blit on this RT should be a 'fresh' one so as to not re-accumulate old frames.
            if (CheckIfTextureShouldBeAccumulated(history.TimeLastUpdated))
            {
                //Debug.Log("History valid!");
                history.Invalidated = true;
            }
            else
            {
                //Debug.LogWarning("History IS NOT valid!");
                history.Invalidated = false;
            }

            var frame = history.Frame;
            // If the camera target is resized, we need to resize the history too.
            if (frame)
            {
                if (frame.width != descriptor.width || frame.height != descriptor.height)
                {
                    //Debug.Log("ID#: " + id + " not valid dims");

                    RenderTexture newframe = new RenderTexture(descriptor);
                    newframe.name = "_CameraHistoryTexture" + id.ToString();
                    if (history.Invalidated) // if invalidated, blit from source to history
                    {
                        commandBuffer.Blit(source, newframe);//, m_Material, 13);
                    }
                    frame.Release();
                    history.Frame = newframe;
                }
                else if (history.Invalidated)
                {
                    //Debug.Log("ID#: " + id + " invalidated");
                    commandBuffer.Blit(source, frame);// if invalidated, blit from source to history
                }
                else if (!history.Invalidated)//if we good
                {
                    RenderTexture newframe = new RenderTexture(descriptor);
                    newframe.name = "_CameraHistoryTexture" + id.ToString();
                    commandBuffer.SetGlobalTexture(ShaderIDs.LongExpTex, history.Frame);
                    commandBuffer.Blit(source, newframe, m_Material, 5);
                    frame.Release();
                    history.Frame = newframe;
                }
                history.TimeLastUpdated = Time.time;
                history.Invalidated = false;
            }
        }
        else
        {
            //Debug.Log("Didnt find ID#: " + id + " Creating new");
            // If we had no history for this camera, create one for it.
            history = new CameraHistory(descriptor);
            history.TimeLastUpdated = Time.time;
            history.Frame.name = "_CameraHistoryTexture" + id.ToString();
            _histories.Add(id, history);
            commandBuffer.Blit(source, history.Frame);//, m_Material, 13); // Copy frame from source to history
        }

        int initialPass = 1;


        if (accumMotion == null)
        {
            var tempMD = GetTempRTDescriptor(renderingData);
            tempMD.colorFormat = RenderTextureFormat.RG16;
            tempMD.width = tempMD.width / 8;
            tempMD.height = tempMD.height / 8;
            accumMotion = new RenderTexture(tempMD);
        }

        if (accumMotionTEMP == null)
        {
            accumMotionTEMP = new RenderTexture(motionDescriptor);
        }

        if (accumMotionTEMPFinal == null)
        {
            accumMotionTEMPFinal = new RenderTexture(motionDescriptor);
        }


        GetTemporaryDividedRT(commandBuffer, ShaderIDs.Intermediate, motionDescriptor, 2, RenderTextureFormat.RG16);

        commandBuffer.SetGlobalTexture(ShaderIDs.AccumulatedMotion, accumMotion);
        commandBuffer.Blit(source, ShaderIDs.Intermediate, m_Material, 0);

        //Tilemax again
        GetTemporaryDividedRT(commandBuffer, ShaderIDs.MotionTemp, motionDescriptor, 4, RenderTextureFormat.RG16);
        commandBuffer.Blit(ShaderIDs.Intermediate, ShaderIDs.MotionTemp, m_Material, 6);

        //Tilemax again
        GetTemporaryDividedRT(commandBuffer, ShaderIDs.MotionTempFinal, motionDescriptor, 8, RenderTextureFormat.RG16);
        commandBuffer.Blit(ShaderIDs.MotionTemp, ShaderIDs.MotionTempFinal, m_Material, 6);

        commandBuffer.Blit(ShaderIDs.MotionTempFinal, accumMotion, m_Material, 7);

        if (m_VolumeComponent.debugMode.value == true)
        {
            commandBuffer.SetGlobalTexture(ShaderIDs.AccumulatedMotion, accumMotion);
            commandBuffer.Blit(source, dest, m_Material, 4);
            return;
        }


        //History.Frame = last frame. We need to blend our NEW frame AND history.frame into new RT
        commandBuffer.SetGlobalTexture(ShaderIDs.Input, source);
        m_Material.SetTexture(ShaderIDs.LongExpTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryOneTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryTwoTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryThreeTex, history.Frame);
        m_Material.SetTexture(ShaderIDs.HistoryFourTex, history.Frame);
        commandBuffer.SetGlobalTexture(ShaderIDs.AccumulatedMotion, accumMotion);

        Vector4 timeStampWeights = new Vector4(0f, 0f, 0f, 0f);

        for (int i = 0; i < maxHistories; i++)
        {
            CameraHistory historyTemp;
            // See if we already have a history for this camera
            if (_histories.TryGetValue(i, out historyTemp))
            {
                timeStampWeights[i] = ValueIfTextureShouldBeAccumulated(historyTemp.TimeLastUpdated);

                //Debug.Log("Got to #: " + i);
                if (i == 0) m_Material.SetTexture(ShaderIDs.HistoryOneTex, historyTemp.Frame);
                if (i == 1) m_Material.SetTexture(ShaderIDs.HistoryTwoTex, historyTemp.Frame);
                if (i == 2) m_Material.SetTexture(ShaderIDs.HistoryThreeTex, historyTemp.Frame);
                if (i == 3) m_Material.SetTexture(ShaderIDs.HistoryFourTex, historyTemp.Frame);
            }
        }

        // Debug.LogWarning(timeStampWeights);
        m_Material.SetVector(ShaderIDs.HistoryTimestampWeights, timeStampWeights);


        commandBuffer.Blit(source, dest, m_Material, initialPass);

        //Now, send the new RT as our destination
        //cmd.SetGlobalTexture(ShaderIDs.MainTex, bHalf);
        //cmd.Blit(bHalf, destination);

        //cmd.Blit(bHalf, renderingData.cameraData.renderer.cameraColorTarget);

        //Now, blit the new RT back into History.Frame
        m_Material.SetTexture(ShaderIDs.Input, bHalf);
        commandBuffer.Blit(dest, history.Frame);

        RenderTexture.ReleaseTemporary(bHalf);

        commandBuffer.ReleaseTemporaryRT(ShaderIDs.MotionTempFinal);
        commandBuffer.ReleaseTemporaryRT(ShaderIDs.MotionTemp);
        commandBuffer.ReleaseTemporaryRT(ShaderIDs.Intermediate);
    }

    protected override bool IsActive()
    {
        // Get the current volume stack
        var stack = VolumeManager.instance.stack;
        m_VolumeComponent = stack.GetComponent<PRISMLongExposure>();

        return Component.active && m_VolumeComponent.intensity.value > 0;
    }



    //init
    public PRISMLongExposure_URP(RenderPassEvent renderPassEvent, Shader shader) : base(renderPassEvent, shader)
    {
        if (!m_Material)
        {
            m_Material = CoreUtils.CreateEngineMaterial("PRISM/PRISMLongExposure");
            CUPPURPHelpers.CheckShaderKeywordState(m_Material);
            SetMaterial(m_Material);
        }

        _histories = new Dictionary<int, CameraHistory>();
        //Debug.Log(m_Material.name);

    }



}
}