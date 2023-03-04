using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PebbloRule : Rule
{
    public Rule next;

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
        NPC kid = Manager.instance.getNPC("pebblito");
        NPC dad = Manager.instance.getNPC("pebblo");

        kid.transform.position = dad.transform.position + new Vector3(-1, -1.31f, -1);

        next.Trigger();
    }
}
