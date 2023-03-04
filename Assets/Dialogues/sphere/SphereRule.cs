using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SphereRule : Rule
{
    public string[] tips;

    private List<string> tipsUse;
    public Rule first;

    public override bool Check()
    {
        return true;
    }

    public override int Complexity()
    {
        return 1;
    }

    public override void Trigger()
    {
        if (tipsUse == null) tipsUse = new List<string>();

        if (tipsUse.Count == 0)
            foreach (string tip in tips)
                tipsUse.Add(tip);

        if (!Manager.instance.talkedToSphere)
        {
            first.Trigger();
            Manager.instance.talkedToSphere = true;
            return;
        }

        string[] text;
        int x = Random.Range(0, tipsUse.Count);

        if (x < 0)
        {
            KeyValuePair<int, int> nums = Manager.instance.CandyProgress();
            text = new string[] { "you have " + nums.Key + " candy(ies) out of " + nums.Value };
        }
        else
        {
            KeyValuePair<int, int> nums = Manager.instance.CandyProgress();
            string[] candies = new string[] { "you have " + nums.Key + " candy(ies) out of " + nums.Value };

            text = AddArrays(candies, tipsUse[x].Split(';'));
            tipsUse.RemoveAt(x);
        }

        DialogueManager.instance.StartDialogue(text, "sphere", null);
    }

    private string[] AddArrays(string[] a, string[] b)
    {
        string[] x = new string[a.Length + b.Length];

        for (int i = 0; i < a.Length; i++)
            x[i] = a[i];

        for (int i = 0; i < b.Length; i++)
            x[a.Length + i] = b[i];

        return x;
    }
}
