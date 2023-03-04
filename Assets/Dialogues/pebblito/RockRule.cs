using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RockRule : Rule
{
    public Rule quest;
    public Rule noQuest;

    public override bool Check()
    {
        return true;
    }

    public override int Complexity()
    {
        return 100;
    }

    public override void Trigger()
    {
        NPC dad = Manager.instance.getNPC("pebblo");

        if (dad.mode == NPCMode.Quest)
            quest.Trigger();
        else
            noQuest.Trigger();
    }
}
