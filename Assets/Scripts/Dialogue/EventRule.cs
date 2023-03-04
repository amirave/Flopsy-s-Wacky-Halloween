using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EventRule : ScriptableObject
{
    public List<Rule> listening = new List<Rule>();

    public void Listen(Rule entry)
    {
        Insert(entry);
    }

    private void Insert(Rule entry)
    {
        if (listening.Contains(entry))
            return;

        for (int i = listening.Count - 1; i >= 0; i--)
        {
            if (entry.Complexity() > listening[i].Complexity())
            {
                listening.Insert(i, entry);
                return;
            }
        }

        listening.Insert(0, entry);
    }

    public void Invoke()
    {
        foreach (Rule entry in listening)
        {
            if (entry.Check())
            {
                entry.Trigger();
            }
        }
    }
}