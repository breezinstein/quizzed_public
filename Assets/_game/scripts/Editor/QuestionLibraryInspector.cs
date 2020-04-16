using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestionLibrary))]
public class QuestionLibraryInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestionLibrary myScript = (QuestionLibrary)target;
        if (GUILayout.Button("Build Question Set"))
        {
            myScript.GenerateQuestions();
        }
        if (GUILayout.Button("Shuffle Questions"))
        {
            myScript.ShuffleQuestions();
        }
        if (GUILayout.Button("Clear Stats"))
        {
            myScript.ClearStats();
        }
    }
}
