using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidEyes : NPC
{
    private float bounds;
    public float followSpeed = 0.2f;
    public float floatSpeed = 0.1f;

    public float dissapearDist = 5f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        bounds = Manager.instance.bounds;
    }

    // Update is called once per frame
    new void Update()
    {
        bool badEnding = manager.badEnding;

        base.Update();
        Vector3 target = player.transform.position.normalized * (bounds + 4) + Vector3.up * (1 + Mathf.Sin(Time.time * floatSpeed) * 0.5f);

        transform.position = Vector3.Lerp(transform.position, target, followSpeed);

        Color tint = Color.Lerp(Color.white, Color.clear, Mathf.Clamp01(PlayerDist() / dissapearDist));

        if (!badEnding || player.transform.position.y < -7f)
            tint = Color.clear;

        renderer.color = tint;

        interactDistance = (badEnding ? 2 : 0);
    }
}
