using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform follow;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t = follow.transform.position;
        transform.LookAt(new Vector3(t.x, transform.position.y, t.z));
    }
}
