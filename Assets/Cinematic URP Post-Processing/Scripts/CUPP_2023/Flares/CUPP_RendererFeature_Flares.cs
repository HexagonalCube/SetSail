using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PRISM.Utils
{
    public class PRISMFlaresRendererFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Settings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            [HideInInspector]
            public Shader shader;
        }

        public Settings settings = new Settings();

        private PRISMFlares_URP _pass;

        public override void Create()
        {
            this.name = "PRISM Flares";
            _pass = new PRISMFlares_URP(settings.renderPassEvent, settings.shader);
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