using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BirdRule : Rule
{
    public Rule saved;
    public Rule egg;

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
        if (Manager.instance.Evaluate("candy.hard == 1"))
            Manager.instance.StartCoroutine(Cutscene());
        else
            egg.Trigger();
    }

    IEnumerator Cutscene()
    {
        Manager m = Manager.instance;
        m.explosionAnim.SetTrigger("explosion");
        NPC.closeToPlayer.Clear();
        m.getNPC("egg").gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        m.getNPC("cluck").enabled = true;
        m.getNPC("chuck").enabled = true;

        saved.Trigger();
    }
}
