using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.Rendering;

namespace PRISM.Utils { 
[CustomEditor(typeof(PRISMDirectionalLocalisedAntiAliasing))]
public class PRISMDLAAEditor : VolumeComponentEditor
{
    public Texture2D editorTex;
    const string editorTextureStringb64 = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAANQSURBVFhH7ZdfaE5hHMf37v8fwozZTJgSIZT8aTHREBMaSpr/RVZSu5FWXKxQJBdc7GKl/FlzJU1Zil0o4mKhcDEKKWkXlAvx7vX5nvN7zs5p/97tHW5869Nz3u/v2fM95znP87zv0v61YtaOhjLj8XgpbU9GRsYntZ47hNKtTVW5UBaLxTbRroIcmcko1RvIhtKenp4GuMF1DTcxgTauYjJK5Qb0+mYRvJZ2J0yBVuiAn/DHpOBygrfDS/gA9bAcP8Pr4T9YHmtiDK3z+tVwZ0CDjSNsI+0eyIT29PT0O/Ccazf1JfRZRrsIJnpOitKNjmXQfXAduqAFb4F8dTBNxq+Cb5BgBs7BPKulpGIGrISHoCm/AFrx4dWei3cA2iFhVOMX+uWRqZBBlkIbvIcm0ILLA3eGZMuDVnDBR/DnQkrnTA4DVYOeVk/9ACrxi/2yJ03/YvwXoOAf0IRXAlofgXgV6+xySGUxyBo4DXrqJ7ABf6pf9qQZmIOvV+Ke+hbeDIgcQvh14Imb0JoZVPkwm77N8BpuQx3eeHDbSe1K/DPgwhvxKqwWCL8l1GfQo1l3PZ0+l6AT7sMJvEngplI7YQn+SXCDXgZNbWS68fTaXJ+E2QOLTnrqg/AO3kAtdngRZYGOXM3ML9DAnXgLQYdNIHwtWhf+EbZaKVAikSiyS08x3k0NHfU0h0CHhzugNKVFeOfhC2jQt3grQK8lEH6Z1R1HrRSI4M3gpCPblxYHVHE5DcKLSGd9bWjQu7AXX99+gfB2Qbf1Ef0uNgt2uml2oPCe1dNsCw2onXAMvwCCfng6nNwWFOutFJEFBjJ7QOlEOw7PQgNvwQ9vQ0+humg2OyLyuvxYT4/N7l+8hgKoCA26G7vcr/YK/3uozz2zIyKszc/09BWS+lLSV6hOtqvQwGf9UbC38fJBO8GFq08f+Zm9MjtpaQdoIUa+qglrDAVfgUhdIuuzH+nL7NRFWEcovNvsiMg77McG2m+lkYvXof0veeFmR0TQaj8v0FkrpS5u4BHECX8K+oUTEWFNfmag+VYaPZE/0y4DEVTv5wXaYaW/Iwt1emX2iDSsXyxKs0spwf8AfXbCcJXUAMqFU/ZRujYa4cMWN3HRLv9rlJSW9htqIoRDyKkAnQAAAABJRU5ErkJggg==";

    SerializedObject serObj;

    SerializedDataParameter enableDLAA;
    SerializedDataParameter retainSharpness;


    public override void OnEnable()
    {
        serObj = new SerializedObject(target);

        var prism = new PropertyFetcher<PRISMDirectionalLocalisedAntiAliasing>(serializedObject);

        //var prismset = new PropertyModification
        enableDLAA = Unpack(prism.Find(x => x.enableDLAA));

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

        string helpString = "Tip: DLAA is useful for reducing specular aliasing as well.";
        EditorGUILayout.HelpBox(helpString, MessageType.Info);
        EditorGUILayout.EndHorizontal();
        
        //target.SetAllOverridesTo(true);

        PropertyField(enableDLAA);

        serializedObject.ApplyModifiedProperties();

    }
}
}