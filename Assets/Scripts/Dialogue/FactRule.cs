using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactRule
{
    public string key = "";
    private int value = 0;

    public FactRule(string key, int value)
    {
        this.key = key;
        this.value = value;
    }

    public void Add(int x)
    {
        value += x;
    }

    public void Set(int x)
    {
        value = x;
    }

    public int Get()
    {
        return value;
    }
}