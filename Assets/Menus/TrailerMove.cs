using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerMove : MonoBehaviour
{
    public float acc = 0.1f;
    public bool mult = false;
    public float speed = 150;

    void Start()
    {
        /*if (mult) 
            speed = 1f;*/
    }

    void Update()
    {
        if (Time.time < 3)
            return;

        speed = (mult ? speed / acc : speed + acc * Time.deltaTime);
        print(speed);
        transform.position += -speed * Time.deltaTime * transform.forward;
    }
}
