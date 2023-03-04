using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class CandyRule : Rule
{
    public string candy = "";
    public string displayName = "";

    public Sprite candySprite;
    public override int Complexity()
    {
        return int.MaxValue;
    }

    public override bool Check()
    {
        return true;
    }

    public override void Trigger()
    {
        Manager manager = Manager.instance;

        manager.Modify("candy." + candy + " = 1");

        manager.candyOpen = true;
        Transform ui = manager.candyUI;
        ui.gameObject.SetActive(true);
        ui.GetComponent<Animator>().SetTrigger("open");
        ui.GetChild(1).GetComponent<Image>().sprite = candySprite;
        ui.GetChild(3).GetComponent<TextMeshProUGUI>().text = displayName;
    }
}
