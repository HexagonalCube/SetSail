using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PRISM.Utils
{
    public class PRISMSharpenRenderFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Settings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            [HideInInspector]
            public Shader shader;
        }

        public Settings settings = new Settings();

        private PRISMSharpen_URP _pass;

        public override void Create()
        {
            this.name = "PRISM Sharpen";
            _pass = new PRISMSharpen_URP(settings.renderPassEvent, settings.shader);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            _pass.Setup(renderer.cameraColorTargetHandle);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_pass);
        }
    }
}