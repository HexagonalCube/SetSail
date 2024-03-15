using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.Rendering;
using PRISM.Utils;

namespace PRISM.Utils { 

[CustomEditor(typeof(PRISMLongExposure))]
public class PRISMLongExposureEditor : VolumeComponentEditor
{
    public Texture2D editorTex;
    const string editorTextureStringb64 = "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";
    SerializedObject serObj;

    SerializedDataParameter intensity;
    SerializedDataParameter exposureTime;
    SerializedDataParameter showShutterSpeed;
    ShutterSpeedValue xShutter = ShutterSpeedValue.One;

    GUIContent raysCasterContent = new GUIContent("   >Ray Caster Transform", "The transform that the rays should come from (Usuall a directional light)");

    public static bool useCameraExposureTime = false;

    public override void OnEnable()
    {
        serObj = new SerializedObject(target);

        var prism = new PropertyFetcher<PRISMLongExposure>(serializedObject);

        //var prismset = new PropertyModification
        intensity = Unpack(prism.Find(x => x.intensity));
        exposureTime = Unpack(prism.Find(x => x.exposureTime));
        showShutterSpeed = Unpack(prism.Find(x => x.showShutterSpeed));

    }

    public static float SnapTo(float a, float snap)
    {
        return Mathf.Round(a / snap) * snap;
    }



    public static int ConvertIndexToShutterSpeed(int inInt)
    {
        /*OneFourHundredth = 400,
        OneTwoHundredth = 200,
        OneOneSixtyth = 160,
        OneOneTwentyFifth = 125,
        OneOneHundredth = 100,
        OneEightyth = 80,
        OneSixtyth = 60,
        OneFiftyth = 50,
        OneFourtyth = 40,
        OneThirtyth = 30,
        OneTwentyFifth = 25,
        OneTwentieth = 20,
        OneFifteenth = 15,
        OneTenth = 10,
        OneFifth = 5,
        OneHalf = 2,
        One = 1,*/

        switch (inInt)
        {
            case 17:
                return 400;
            case 16:
                return 200;
            default:
                return 1;
        }
    }

    public override void OnInspectorGUI()
    {
        byte[] b64_bytes = System.Convert.FromBase64String(editorTextureStringb64);

        intensity.overrideState.boolValue = true;
        exposureTime.overrideState.boolValue = true;
        showShutterSpeed.overrideState.boolValue = true;

        //   target.SetAllOverridesTo(true);

        editorTex = new Texture2D(562, 32);
        editorTex.LoadImage(b64_bytes);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(editorTex);


        string helpString1 = "Intensity controls the length of the motion blur. Increasing exposure time (in seconds) will increase the number of frames that the motion blur accumulates.";
        EditorGUILayout.HelpBox(helpString1, MessageType.Info);
        EditorGUILayout.EndHorizontal();

        PropertyField(intensity);
        useCameraExposureTime = showShutterSpeed.value.boolValue;
        useCameraExposureTime = EditorGUILayout.Toggle("Use shutter speed values", useCameraExposureTime);
        showShutterSpeed.value.boolValue = useCameraExposureTime;


        if(useCameraExposureTime)
        {

            EditorGUILayout.LabelField("   Shutter Speed (seconds)");

            EditorGUILayout.BeginHorizontal();
            //int sliderFloat = (int)xShutter;
            //sliderFloat = EditorGUILayout.IntSlider(sliderFloat, 0, 400);
            //xShutter = (ShutterSpeedValue)ConvertIndexToShutterSpeed(sliderFloat);
            GUILayout.Space(14f);
            if(GUILayout.Button("<") && xShutter != ShutterSpeedValue.One)
            {
                xShutter = xShutter.Previous();
            }

            if(GUILayout.Button(">") && xShutter != ShutterSpeedValue.OneFourHundredth)
            {
                xShutter = xShutter.Next();
            }

            EditorGUILayout.LabelField("   1/" + xShutter.NumberString() + " sec");
            //xShutter = (ShutterSpeedValue)EditorGUILayout.EnumPopup("   1/" + xShutter.NumberString(), xShutter);
            EditorGUILayout.EndHorizontal();

            int shutterInt = (int)xShutter;
            exposureTime.value.floatValue = (float)(1f / (float)shutterInt);
            //Debug.Log(exposureTime.value.floatValue);
        } else
        {
            PropertyField(exposureTime);
        }


        var prismRef = target as PRISMLongExposure;
        
        if(Camera.main.depthTextureMode == DepthTextureMode.None)
        {
            string helpString = "Your main camera, on the gameobject: ";
            helpString += Camera.main.gameObject.name;
            helpString += " does not have a depth or motion vectors texture enabled. Fixing.";
            EditorGUILayout.HelpBox(helpString, MessageType.Info);

            if (GUILayout.Button("Enable Main Camera Depth & Motion"))
            {
                Camera.main.depthTextureMode = Camera.main.depthTextureMode | DepthTextureMode.Depth;
                Camera.main.depthTextureMode = Camera.main.depthTextureMode | DepthTextureMode.MotionVectors;
            }
        }

        serializedObject.ApplyModifiedProperties();

    }

}

}