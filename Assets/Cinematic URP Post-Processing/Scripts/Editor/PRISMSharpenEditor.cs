using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.Rendering;

namespace PRISM.Utils { 
[CustomEditor(typeof(PRISMSharpen))]
public class PRISMSharpenEditor : VolumeComponentEditor
{
    public Texture2D editorTex;
    const string editorTextureStringb64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAOHSURBVFhH7ZXbb0xRFIfnqu20oXULioaUSngQqUQEEW1IRISEkirCGxGPXj34Azw3QtJIPBC3UG0icSfigZYHiURcow3VkjEtc/X9ztlzOnM6M71k6skv+Watvc/svddae599PP/1X2NRMpk8BSk3iUSi1vxlwvIZm1eRSET/abFb2fJ6vfuNO3kiyw25sje8DYfDoyZRSKMOJsuDxs2lxaFQaK3xi69YLBYiy5+urN20mr8XX0ze5FosAg9dfQPRaLTUDBm3RtsCd/k7wZ1xZSAQ2Gb84ikej88hu5gr22a2pQIbdvVfN8M8VEOVy0kqlXJsWnkr4PP59mICdstSlMG3gsHgL/wrdpejrQQ8Uw6H1upAcjS/5hB+FnYepoMotAXud/wOpf4hh8HnrZ5hBRSwJjUB6GcKVMFcmAfToQS8o1aAd38FZpXdcnTVWJX5HuaL3XJknRcTgDLWgutBlWyGBpgFQXCUM4AcNxzVT14zvqesrCyOuWC3HNXzn2WMVQTKdCHshibYJchcV3cIrHVViREBDA0NKXpFnKlHlP+r8S0xuM24jlh7H0ZzVvC8DqsFZ4MyrwZVdhroPGDsvcoS5W9kotummdYbeGm7WdoO2uu0PjC+zu/3a7FjsBGUkCqmFZ/COXgNgyrWiAAoozI7YLfGLzJrYOL3uMehHj5CDJZCH8/P8Pwx/gA26cdxpHec03wWNzOrcUmTYjpYKIhfi9WB7cWvwZZDP/47bBiSWWeA0u3AVNitCWunTiyLdLH4M2w3qAp9UIKvSuhMKMnhW0NinPa+0W5ZUjaH4LfVyq0tcNh2LbFuqoWFOrE12Er6FsEmWAk99LfS/wD/O9jiJptPAAnIvGI7zOO8yjOuHco5kKuxbXADnsMneAEnoQ5KnC0wV6/7tdR5KChez88YfaQytZnJZ5DlIL5Ov7ZV1VQldQ8sAb2epdaCZKEyHZGfKTLQARqL7hqblp+EjmK/Ue5LWH072qEL/kA1/dqWKi+RXsZZA7qv3dK7rzvgNBM+sXpcMuO1v0oiUwlQZV7BTdDls46q6KDrAN6HDgUw/GXIrz0EcNH4WSowXv36cupOOAEKSN+DdLA90O3e82Iq/U3QKzcVFhj6oRd0JpaPuAmLJc6PDrY+vbqQyrH6NCsYfQt0PSuAxGQHoEuB9b0+rBYVWbfvvwhA17NljSZtzQnI4/kLmcwsIfTGrcgAAAAASUVORK5CYII=";

    SerializedObject serObj;

    SerializedDataParameter intensity;
   // SerializedDataParameter useMultiPassSharpen;
    SerializedDataParameter useDepthAwareSharpen;


    public override void OnEnable()
    {
        serObj = new SerializedObject(target);

        var prism = new PropertyFetcher<PRISMSharpen>(serializedObject);

        //var prismset = new PropertyModification
        intensity = Unpack(prism.Find(x => x.intensity));
       //useMultiPassSharpen = Unpack(prism.Find(x => x.useMultiPassSharpen));
        useDepthAwareSharpen = Unpack(prism.Find(x => x.useDepthAwareSharpen));

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

        string helpString = "Tip: When sharpening scenes with grass or foliage which may have pixels from the background directly behind foliage pixels, use depth-aware sharpen to avoid artifacts.";
        EditorGUILayout.HelpBox(helpString, MessageType.Info);
        EditorGUILayout.EndHorizontal();

        //target.SetAllOverridesTo(true);


        PropertyField(intensity);
       // PropertyField(useMultiPassSharpen);
        PropertyField(useDepthAwareSharpen);

        serializedObject.ApplyModifiedProperties();

    }
}
}