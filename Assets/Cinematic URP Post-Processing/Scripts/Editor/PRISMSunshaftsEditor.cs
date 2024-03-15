using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.Rendering;

namespace PRISM.Utils { 

[CustomEditor(typeof(PRISMSunshafts))]
public class PRISMSunshaftsEditor : VolumeComponentEditor
{
    public Texture2D editorTex;
    const string editorTextureStringb64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAACXBIWXMAAA7EAAAOxAGVKw4bAAABrUlEQVRYw81XO27CQBCdrNwAXcpIoXJ6hBDHcEkaak7AbSyOwQloUuUCpkCKSOd0Kwp2HmkGtFr8W9vJMtJIlu157+3M2LNL1NIALABcxBdtcVTbwL7sMQUYY577JirDvBPAzDOlVAZg1Rc5gJVgzmpVAsit5lpVgMYAluJxFbmFl9dm1wm4dMlEayw3kJnnvuTMPO+0EEvE2gGeAkgBZADO4hmAlJmnDsa6UxaZ+dY0xpghgI2zqiLfGGOGRRitTch3DtERwFb86Dzb2SI6m7PyIzMnWuvbJ6y1VsycOEI2vZBLzW3yccW7Y1uE2xNtV59agEkDwYklOK17/6lkkn0qpfYiICOimIi+T6fT62g0QhWg1loNBoMvInohor1S6k1wYiK6z0hJJy+t52e5t/XI2lZizta9ZRFX8GkYEdF7UQms64OUYKK1Vk1KQEQTK/ZqHyVcten80yasteCfoWQh3I+IKOCv2B7DHYaR9yi/pj3cOC7YkHiPVGae9bIb+tct2UNsSiV1eZ1an6OZiMgbl7LJwcT3bNj4YEJEFEXRTyOlHlaGGXwaBhfwC1FkyrW/xwc4AAAAAElFTkSuQmCC";

    SerializedObject serObj;

    SerializedDataParameter intensity;
    SerializedDataParameter rayDecay;
    SerializedDataParameter rayDensity;
    SerializedDataParameter sunTransform;
    SerializedDataParameter sunTransformPosition;
    SerializedDataParameter useUltraQuality;
    SerializedDataParameter useDownsampling, sunColor;


     GUIContent raysCasterContent = new GUIContent("   >Ray Caster Transform", "The transform that the rays should come from (Usuall a directional light)");



    public override void OnEnable()
    {
        serObj = new SerializedObject(target);

        var prism = new PropertyFetcher<PRISMSunshafts>(serializedObject);

        //var prismset = new PropertyModification
        intensity = Unpack(prism.Find(x => x.rayIntensity));
        rayDecay = Unpack(prism.Find(x => x.rayDecay));
        rayDensity = Unpack(prism.Find(x => x.rayDensity));
        sunTransformPosition = Unpack(prism.Find(x => x.sunTransformPosition));
        useUltraQuality = Unpack(prism.Find(x => x.useUltraQuality));
        useDownsampling = Unpack(prism.Find(x => x.useMobileDownsampling));
        sunColor
             = Unpack(prism.Find(x => x.sunColor));

    }

    public static float SnapTo(float a, float snap)
    {
        return Mathf.Round(a / snap) * snap;
    }

    public override void OnInspectorGUI()
    {
        byte[] b64_bytes = System.Convert.FromBase64String(editorTextureStringb64);

        editorTex = new Texture2D(562, 32);
        editorTex.LoadImage(b64_bytes);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(editorTex);

        string helpString = "Tip: Try and use a low Intensity for smoother looking rays where possible. \n Ray density controls the total reach of your rays across the screen, Ray decay controls how fast they fall off.";
        EditorGUILayout.HelpBox(helpString, MessageType.Info);
        EditorGUILayout.EndHorizontal();

       // target.SetAllOverridesTo(true);


        PropertyField(intensity);
        PropertyField(rayDecay);
        PropertyField(rayDensity);
        PropertyField(useUltraQuality);
        PropertyField(useDownsampling);
        PropertyField(sunColor);


        var prismRef = target as PRISMSunshafts;

        EditorGUI.BeginChangeCheck();
        Transform raysT = (Transform)EditorGUILayout.ObjectField(raysCasterContent, prismRef.sunTransform.value, typeof(Transform), true);
        if (EditorGUI.EndChangeCheck())
        {
            PRISMSunshafts_URP.SetSunTransform(raysT);
            Undo.RecordObject(target, "Rays transform");
            prismRef.sunTransform.value = raysT;
            sunTransformPosition.value.vector3Value = raysT.transform.position;

            if (sunTransformPosition.value.vector3Value != raysT.transform.position)
            {
                sunTransformPosition.value.vector3Value = raysT.transform.position;
            }
        }



        if (GUILayout.Button("Set Rays Transform To Directional Light"))
        {
            var sunsInScene = Object.FindObjectsOfType((typeof(Light))) as Light[];
            foreach (var v in sunsInScene)
            {
                if (v.type == LightType.Directional)
                {
                    prismRef.sunTransform.value = v.transform;
                }
            }
        }

        /*
        if(Camera.main.depthTextureMode == DepthTextureMode.None)
        {
            string helpString = "Your main camera, on the gameobject: ";
            helpString += Camera.main.gameObject.name;
            helpString += " does not have a Depth Texture enabled. Ensure the depth texture is enabled for Godrays to work.";
            EditorGUILayout.HelpBox(helpString, MessageType.Info);

            if (GUILayout.Button("Enable Main Camera Depth"))
            {
                Camera.main.depthTextureMode = DepthTextureMode.Depth;
            }
        }*/



        /* prism.useRays = EditorGUILayout.Toggle(raysContent, prism.useRays);
         if (EditorGUI.EndChangeCheck())
         {
             if (!prism.rayTransform)
             {

             }
         }*/
         


        serializedObject.ApplyModifiedProperties();

    }
}
}