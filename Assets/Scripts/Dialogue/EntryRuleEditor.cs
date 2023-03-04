using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(EntryRule))]
public class TestScriptableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (EntryRule) target;

        if (GUILayout.Button("Create Next Entry", GUILayout.Height(40)))
        {
            script.CreateEntry();
        }

    }
}

#endif