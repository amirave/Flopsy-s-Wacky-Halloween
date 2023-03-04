using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenEye : MonoBehaviour
{
    Color color = new Color(0, 0.7f, 0.7f);
    public float scrollSpeed = 0.05f;

    public Animator anim;

    void Update()
    {
        color.r += scrollSpeed;
        color.r %= 1;

        //GetComponent<SpriteRenderer>().color = color;
    }

    public IEnumerator Ascend(Rule entry)
    {
        anim.SetTrigger("explosion");

        yield return new WaitForSeconds(2);

        float speed = 0.0001f;

        while (transform.position.y < 100)
        {
            speed += 0.0001f;
            transform.position += new Vector3(0, speed, 0);
            yield return null;
        }

        GetComponent<SpriteRenderer>().color = Color.clear;

        yield return new WaitForSeconds(3);

        entry.Trigger();
    }
}
