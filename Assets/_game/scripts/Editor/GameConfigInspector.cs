using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameConfig))]
public class GameConfigInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameConfig myScript = (GameConfig)target;
        if (GUILayout.Button("Set Build Settings"))
        {
            myScript.SetBuildSettings();
        }
        if (GUILayout.Button("Apply Theme"))
        {
            myScript.ApplyTheme();
        }
    }
}
