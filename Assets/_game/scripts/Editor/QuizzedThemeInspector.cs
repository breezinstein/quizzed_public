using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuizzedTheme))]
public class QuizzedThemeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuizzedTheme myScript = (QuizzedTheme)target;
        
        if (GUILayout.Button("Apply Theme"))
        {
            myScript.ApplyTheme();
        }
    }
}
