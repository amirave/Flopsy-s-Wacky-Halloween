using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FlopsyRule : Rule
{
    public Rule notEnoughGood;
    public Rule notEnoughBad;

    public Rule enoughGood;
    public Rule enoughBad;

    public Rule confirmBad;

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
        bool badEnding = Manager.instance.badEnding;
        bool allCandy = Manager.instance.AllCandy();

        if (allCandy)
        {
            if (badEnding)
            {
                if (Manager.instance.hasConsulted)
                {
                    confirmBad.Trigger();
                }
                else
                {
                    enoughBad.Trigger();
                }
            }
            else if (!badEnding)
            {
                enoughGood.Trigger();
            }

            Manager.instance.hasConsulted = true;
        }
        else
        {
            if (badEnding)
            {
                notEnoughBad.Trigger();
            }
            else
            {
                notEnoughGood.Trigger();
            }
        }
    }
}
