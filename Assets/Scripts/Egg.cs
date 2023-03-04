using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    Animator anim;

    public IEnumerator Ascend(Rule next)
    {
        anim.SetTrigger("explosion");
        gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        next.Trigger();
    }
}
