using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class EndingRule : Rule
{
    public int endingIndex;
    public float revealSpeed = 5f;

    public override bool Check()
    {
        return true;
    }

    public override int Complexity()
    {
        return 100;
    }

    public override void Trigger()
    {
        Image img = Manager.instance.endingPics[endingIndex].GetComponent<Image>();
        TextMeshProUGUI credits = Manager.instance.credits;

        Manager.instance.StartCoroutine(Reveal(img, credits));
    }

    IEnumerator Reveal(Image img, TextMeshProUGUI credits)
    {
        Manager.instance.player.enabled = false;

        img.color = Color.clear;

        float a = 0;
        while (a < 1)
        {
            a += revealSpeed / 100;
            img.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(3f);

        float a2 = 0;
        while (a2 < 0.7f)
        {
            a2 += revealSpeed / 100;
            credits.transform.parent.GetComponent<Image>().color = new Color(0.3f, 0.035f, 0.29f, a2);
            yield return new WaitForSeconds(0.01f);
        }

        float a3 = 0;
        while (a3 < 1)
        {
            a3 += revealSpeed / 100;
            credits.color = new Color(1, 1, 1, a3);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(10f);
        Application.Quit();
    }
}
