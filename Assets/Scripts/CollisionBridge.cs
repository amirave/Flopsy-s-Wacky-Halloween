using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    WarpZone _listener;
    int _index;

    public void Initialize(WarpZone l, int index)
    {
        _listener = l;
        _index = index;
    }

    void OnTriggerEnter(Collider other)
    {
        _listener.MyTriggerEnter(other, _index);
    }
    void OnTriggerLeave(Collider other)
    {
        _listener.MyTriggerLeave(other, _index);
    }
}