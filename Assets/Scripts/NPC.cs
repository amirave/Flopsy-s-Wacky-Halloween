using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LookAtCamera))]
public class NPC : MonoBehaviour
{
    public static List<NPC> closeToPlayer = new List<NPC>();
    public static Manager manager;
    protected SpriteRenderer renderer;

    public string name = "";
    public string displayName = "";

    public NPCMode mode;

    public Rule quest;
    public Rule[] midQuest;
    public Rule eye;
    public Rule random;

    public Color dialogueColor;

    public Sprite[] sprites;
    public Sprite[] spritesOpenMouth;

    public float spriteCycleSpeed = 1f;
    public bool fullBright = false;
    public float interactDistance = 1;

    [HideInInspector()]
    public bool isTalking = false;

    private float spriteIndex = 0;

    public static Player player
    {
        get { return manager.player; }
    }

    public void Start()
    {
        manager = Manager.instance;
        renderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        if (PlayerDist() <= manager.NpcInteractDist * interactDistance)
        {
            if (!closeToPlayer.Contains(this))
                closeToPlayer.Add(this);
        }
        else
        {
            closeToPlayer.Remove(this);
        }

        spriteIndex += spriteCycleSpeed * Time.deltaTime;
        int index = (int)spriteIndex;

        if (renderer != null)
        {
            if (isTalking)
                renderer.sprite = (index % 2 == 0 ? spritesOpenMouth[index % spritesOpenMouth.Length] : sprites[index % sprites.Length]);
            else
                renderer.sprite = sprites[((int)spriteIndex) % sprites.Length];
        }

        // darken color if farther than fog
        if (!fullBright)
        {
            Color tint = Color.Lerp(Color.white, manager.fogColor, Mathf.Clamp01(PlayerDist() / manager.fogDistance));
            renderer.color = tint;
        }
    }

    public float PlayerDist()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    public void TriggerDialogue()
    {
        if (mode == NPCMode.Special)
        {
            random.Trigger();
            return;
        }

        if (mode == NPCMode.PreQuest)
            quest.Trigger();

        else if (mode == NPCMode.Quest)
            Rule.TriggerBest(midQuest);

        else if (manager.badEnding)
            eye.Trigger();

        else
            random.Trigger();
    }

    public void CreateEntry()
    {
        /*EntryRule entry = ScriptableObject.CreateInstance<EntryRule>();
        EventRule eventRule = ScriptableObject.CreateInstance<EventRule>();

        string path = "Assets\\Dialogues\\" + name + "\\";

        if (!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets\\Dialogues", name);

        AssetDatabase.CreateAsset(entry, AssetDatabase.GenerateUniqueAssetPath(path + "talk_entry.asset"));
        AssetDatabase.CreateAsset(eventRule, AssetDatabase.GenerateUniqueAssetPath(path + "talk_event.asset"));

        entry.dispatch = eventRule;
        entry.npcName = name;
        talkEvent = eventRule;

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = entry;*/
    }
}

public enum NPCMode
{
    NoQuest,
    PreQuest,
    Quest,
    Finished,
    Special
}