using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractPopup : MonoBehaviour
{
    public GameObject popup;
    public Vector3 offset;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = Manager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.instance.active)
        {
            popup.SetActive(false);
            return;
        }

        NPC closest = null;

        foreach (NPC npc in NPC.closeToPlayer)
        {
            if (closest == null || npc.PlayerDist() < closest.PlayerDist())
                closest = npc;
        }

        if (closest != null)
        {
            Vector3 popupCoords = (1 * offset);
            popupCoords += closest.transform.position;

            Vector2 screenCoords = Camera.main.WorldToScreenPoint(popupCoords);

            // only if looking at direction of npc
            if (Vector3.Dot(player.transform.forward, closest.transform.position - player.transform.position) > 0)
            {
                popup.SetActive(true);
                popup.transform.position = screenCoords;
                popup.transform.GetChild(1).GetComponent<Image>().color = closest.dialogueColor;
            }
        }
        else
        {
            popup.SetActive(false);
        }
    }
}
