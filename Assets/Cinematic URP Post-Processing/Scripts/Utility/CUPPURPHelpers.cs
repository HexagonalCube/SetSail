using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PRISM.Utils
{

    public class CUPPURPHelpers
    {
        public static bool ENABLE_EXTRA_DEBUG = true;
        public const string SUNSHAFTS_HIGHQUAL_KEYWORD = "ULTRA_QUALITY_SUNSHAFTS";
        public const string SUNSHAFTS_OFF_KEYWORD = "NOT_ULTRA_QUALITY_SUNSHAFTS";



            public static void SetKeywordEnabled(string keyword, bool enabled) {
              if (enabled) {
                Shader.EnableKeyword(keyword);
              } else {
                Shader.DisableKeyword(keyword);
              }
            }



        public static void CheckShaderKeywordState(Material m_Material)
        {
            if (!CUPPURPHelpers.ENABLE_EXTRA_DEBUG) return;

            // Get the instance of the Shader class that the material uses
            var shader = m_Material.shader;

            // Get all the local keywords that affect the Shader
            var keywordSpace = shader.keywordSpace;

            // Iterate over the local keywords
            foreach (var localKeyword in keywordSpace.keywords)
            {
                // If the local keyword is overridable,
                // and a global keyword with the same name exists and is enabled,
                // then Unity uses the global keyword state
                if (localKeyword.isOverridable && Shader.IsKeywordEnabled(localKeyword.name))
                {
                    //Debug.Log("Local keyword with name of " + localKeyword.name + " is overridden by a global keyword, and is enabled");
                }
                // Otherwise, Unity uses the local keyword state
                else
                {
                    var state = m_Material.IsKeywordEnabled(localKeyword) ? "enabled" : "disabled";
                    //Debug.Log("Local keyword with name of " + localKeyword.name + " is " + state);
                }
            }
        }

        public static void SetKeywordEnabled(string keyword, bool enabled, Material specificMaterial)
        {
            if (enabled)
            {
                specificMaterial.EnableKeyword(keyword);
            }
            else
            {
                specificMaterial.DisableKeyword(keyword);
            }
        }

    }

}
