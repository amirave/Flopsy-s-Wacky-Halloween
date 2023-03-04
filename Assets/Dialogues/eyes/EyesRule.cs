using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EyesRule : Rule
{
    public Rule before;
    public Rule notEnough;
    public Rule enough;
    public Rule confirm;

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
        bool allCandy = Manager.instance.AllCandy();

        if (!Manager.instance.badEnding)
            before.Trigger();

        if (allCandy)
        {
            if (Manager.instance.hasConsulted)
            {
                confirm.Trigger();
            }
            else
            {
                enough.Trigger();
            }

            Manager.instance.hasConsulted = true;
        }
        else
        {
            notEnough.Trigger();
        }
    }
}
