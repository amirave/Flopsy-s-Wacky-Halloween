using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Dialogue : ScriptableObject
{
    public string npcName;
    [TextArea(3, 0)]
    public string[] sentences;
}
