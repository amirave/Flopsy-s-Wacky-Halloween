using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (NPC) target;

        if (GUILayout.Button("Create Talk Entry", GUILayout.Height(40)))
        {
            script.CreateEntry();
        }

    }
}

#endif