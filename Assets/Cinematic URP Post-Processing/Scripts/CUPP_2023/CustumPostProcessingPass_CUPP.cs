﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PRISM.Utils
{
    public abstract class CustomPostProcessingPass<T> : ScriptableRenderPass where T : VolumeComponent
    {
        protected static readonly int TempColorBufferId = UnityEngine.Shader.PropertyToID("_MainTex");

        protected Shader Shader;
        protected Material Material;
        protected T Component;

        private RenderTargetIdentifier _renderTargetIdentifier;
        private RenderTargetIdentifier _tempRenderTargetIdentifier;

        protected abstract string RenderTag { get; }

        /// <summary>
        /// Create a descriptor for intermediate render targets based on the rendering data.
        /// Mainly used to create intermediate render targets.
        /// </summary>
        /// <returns>a descriptor similar to the camera target but with no depth buffer or multisampling</returns>
        public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            descriptor.msaaSamples = 1;
            return descriptor;
        }

        public void SetMaterial(Material inputMaterial)
        {
            Material = inputMaterial;
        }

        public CustomPostProcessingPass(RenderPassEvent renderPassEvent, Shader shader)
        {
            this.renderPassEvent = renderPassEvent;
            Shader = shader;

            if (shader == null)
            {
                return;
            }

            //Material = CoreUtils.CreateEngineMaterial(shader);
        }

        public virtual void Setup(in RenderTargetIdentifier renderTargetIdentifier)
        {
            _renderTargetIdentifier = renderTargetIdentifier;
            _tempRenderTargetIdentifier = new RenderTargetIdentifier(TempColorBufferId);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (Material == null || !renderingData.cameraData.postProcessEnabled)
            {
                return;
            }

            var volumeStack = VolumeManager.instance.stack;
            Component = volumeStack.GetComponent<T>();
            if (Component == null || !Component.active || !IsActive())
            {
                return;
            }

            var commandBuffer = CommandBufferPool.Get(RenderTag);
            Render(commandBuffer, ref renderingData);
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        private void Render(CommandBuffer commandBuffer, ref RenderingData renderingData)
        {
            var source = _renderTargetIdentifier;
            var dest = _tempRenderTargetIdentifier;

            SetupRenderTexture(commandBuffer, ref renderingData);

            BeforeRender(commandBuffer, ref renderingData);

            CopyToTempBuffer(commandBuffer, ref renderingData, source, dest);

            Render(commandBuffer, ref renderingData, dest, source);

            CleanupRenderTexture(commandBuffer, ref renderingData);
        }

        protected virtual void SetupRenderTexture(CommandBuffer commandBuffer, ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;

            var desc = new RenderTextureDescriptor(cameraData.camera.scaledPixelWidth, cameraData.camera.scaledPixelHeight);
            desc.colorFormat = cameraData.isHdrEnabled ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;

            //Get temp RT
            commandBuffer.GetTemporaryRT(TempColorBufferId, desc);
        }

        protected virtual void CleanupRenderTexture(CommandBuffer commandBuffer, ref RenderingData renderingData)
        {
            //Release RT
            commandBuffer.ReleaseTemporaryRT(TempColorBufferId);
        }

        protected virtual void CopyToTempBuffer(CommandBuffer commandBuffer, ref RenderingData renderingData,
            RenderTargetIdentifier source, RenderTargetIdentifier dest)
        {
            commandBuffer.Blit(source, dest);
        }

        protected abstract void Render(CommandBuffer commandBuffer, ref RenderingData renderingData,
            RenderTargetIdentifier source, RenderTargetIdentifier dest);

        protected abstract void BeforeRender(CommandBuffer commandBuffer, ref RenderingData renderingData);

        protected abstract bool IsActive();

    }
}