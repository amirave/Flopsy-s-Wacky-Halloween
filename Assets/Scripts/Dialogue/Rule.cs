using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Rule : ScriptableObject
{
    /*public EventRule dispatch;
    public string[] reqs;

    public string[] changes;
    public Rule[] next;*/

    public abstract int Complexity();

    public abstract bool Check();

    public abstract void Trigger();

    public static void TriggerBest(Rule[] rules)
    {
        Array.Sort(rules, new RuleComparer());

        foreach (Rule r in rules)
        {
            if (r.Check())
            {
                r.Trigger();
                return;
            }
        }
    }
}

public class RuleComparer : IComparer<Rule>
{
    public int Compare(Rule a, Rule b)
    {
        if (a.Complexity() > b.Complexity())
            return -1;
        if (a.Complexity() < b.Complexity())
            return 1;

        return 0;
    }
}