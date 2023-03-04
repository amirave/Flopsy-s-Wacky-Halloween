using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class EntryRule : Rule
{
    public string[] reqs;

    public string npcName;
    [TextArea(3, 5)]
    public string[] sentences;

    public string[] changes;
    public List<Rule> next;

    public override int Complexity()
    {
        return reqs.Length;
    }

    public override bool Check()
    {
        foreach (string req in reqs)
        {
            if (!Manager.instance.Evaluate(req))
                return false;
        }

        return true;
    }

    public override void Trigger()
    {
        DialogueManager.instance.StartDialogue(sentences, npcName, Done);
    }

    public void Done()
    {
        foreach (string c in changes)
        {
            string[] parts = c.Split(' ');
            if (parts[0].Equals("mode"))
                Manager.instance.getNPC(npcName).mode = (NPCMode) Enum.Parse(typeof(NPCMode), parts[2]);
            else
                Manager.instance.Modify(c);
        }

        Rule[] arr = next.ToArray();
        TriggerBest(arr);
    }

    // Editor stuff
#if UNITY_EDITOR

    public void CreateEntry()
    {
        EntryRule entry = ScriptableObject.CreateInstance<EntryRule>();
        EventRule eventRule = ScriptableObject.CreateInstance<EventRule>();

        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)) + "\\";
        AssetDatabase.CreateAsset(entry, AssetDatabase.GenerateUniqueAssetPath(path + "talk_entry.asset")); 
        AssetDatabase.CreateAsset(eventRule, AssetDatabase.GenerateUniqueAssetPath(path + "talk_event.asset"));

        entry.npcName = npcName;
        next.Add(entry);

        AssetDatabase.SaveAssets();
            
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = entry;
    }

#endif
}

