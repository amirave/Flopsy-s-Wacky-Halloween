using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{
    public float cooldown = 5;

    private float lastWarp = -1000;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;

        Collider[] collider = GetComponentsInChildren<Collider>();

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject != gameObject)
            {
                ColliderBridge cb = collider[i].gameObject.AddComponent<ColliderBridge>();
                cb.Initialize(this, i);
            }
        }   
    }

    void Update()
    {

    }

    public void MyTriggerEnter(Collider c, int index)
    {
        if (lastWarp + cooldown < Time.time)
        {
            lastWarp = Time.time;

            StartCoroutine(DisablePlayer());
            player.transform.position += transform.GetChild(1 - index).position - transform.GetChild(index).position;
        }
    }

    IEnumerator DisablePlayer()
    {
        player.enabled = false;
        yield return new WaitForSeconds(0.3f);
        player.enabled = true;
    }

    public void MyTriggerLeave(Collider c, int index)
    {

    }
}
