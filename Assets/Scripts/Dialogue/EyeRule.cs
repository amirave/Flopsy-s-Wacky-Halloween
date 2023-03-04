using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EyeRule : Rule
{
    public Rule noHardCandy;

    public string[] hasHardCandy;

    public Rule afterTrigger;

    public override bool Check()
    {
        return true;
    }

    public override int Complexity()
    {
        return 10000;
    }

    public override void Trigger()
    {
        Manager m = Manager.instance;

        if (m.Evaluate("candy.hard == 1"))
        {
            DialogueManager.instance.StartDialogue(hasHardCandy, "forbidden_eye", TriggerBadEnding);
        }
        else
        {
            noHardCandy.Trigger();
        }
    }

    public void TriggerBadEnding()
    {
        Manager m = Manager.instance;

        m.cageBefore.SetActive(false);
        m.cageAfter.SetActive(true);

        // TODO - explosion?
        m.StartCoroutine(m.getNPC("forbidden_eye").GetComponent<ForbiddenEye>().Ascend(afterTrigger));
        m.badEnding = true;
    }
}
