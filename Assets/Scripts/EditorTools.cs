using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class EditorTools : MonoBehaviour
{
    Manager manager;

    void OnDrawGizmos()
    {
        if (manager == null)
            manager = GetComponent<Manager>();

        Gizmos.DrawWireSphere(Vector3.zero, manager.bounds);
    }

}
