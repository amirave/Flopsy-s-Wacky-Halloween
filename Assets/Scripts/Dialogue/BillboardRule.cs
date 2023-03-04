using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BillboardRule : Rule
{
    public Rule done;
    public Rule offQuest;
    public Rule already;
    public Rule eye;

    public override bool Check()
    {
        return true;
    }

    public override int Complexity()
    {
        return 1000;
    }

    public override void Trigger()
    {
        Manager manager = Manager.instance;

        NPCMode mode = manager.getNPC("wailey").mode;

        if (mode == NPCMode.PreQuest)
        {
            offQuest.Trigger();
        }
        else if (mode == NPCMode.Quest)
        {
            if (manager.Evaluate("billboard == 1"))
                already.Trigger();
            else
            {
                manager.Modify("billboard = 1");

                manager.billboardBefore.SetActive(false);
                manager.billboardAfter.SetActive(true);

                done.Trigger();
            }
        }
        else if (manager.badEnding)
            eye.Trigger();
    }
}
