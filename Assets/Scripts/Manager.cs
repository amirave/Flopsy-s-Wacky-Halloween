using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public DialogueManager dm;
    public Transform candyUI;
    public Player player;
    public Transform npcs;

    public Slider volumeSlider;
    public AudioSource audio;

    public GameObject billboardBefore;
    public GameObject billboardAfter;

    public GameObject cageBefore;
    public GameObject cageAfter;

    public Animator explosionAnim;

    [HideInInspector()]
    public bool badEnding = false;
    [HideInInspector()]
    public bool hasConsulted = false;
    [HideInInspector()]
    public bool talkedToSphere = false;

    public string[] ruleEntryArray;
    [HideInInspector()]
    public Dictionary<string, FactRule> ruleEntries = new Dictionary<string, FactRule>();

    [HideInInspector]
    public Dictionary<string, NPC> NPCs = new Dictionary<string, NPC>();

    public float NpcInteractDist = 2f;

    public bool candyOpen = false;
    public bool mainMenuOpen = false;

    public Animator mainMenuAnim;
    public Rule flopsyStartRule;

    public TextMeshProUGUI credits;
    public GameObject[] endingPics;
    
    public bool playerControl
    {
        get { return !candyOpen && !dm.active; }
    }

    public float fogDistance = 10;
    public Color fogColor;
    public float bounds = 65;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        foreach (string ruleString in ruleEntryArray)
        {
            string[] parts = ruleString.Replace(" ", "").Split('=');
            FactRule rule = new FactRule(parts[0], int.Parse(parts[1]));
            ruleEntries.Add(rule.key, rule);
        }

        for (int i = 0; i < npcs.childCount; i++)
        {
            NPC npc = npcs.GetChild(i).GetComponent<NPC>();
            NPCs.Add(npc.name, npc);
        }
    }

    void Update()
    {
        audio.volume = volumeSlider.value;
    }

    public NPC getNPC(string name)
    {
        NPC npc;
        NPCs.TryGetValue(name, out npc);
        return npc;
    }

    public bool Evaluate(string expression)
    {
        string[] parts = expression.Split(' ');

        FactRule rule;
        ruleEntries.TryGetValue(parts[0], out rule);

        int num = int.Parse(parts[2]);

        switch (parts[1])
        {
            case "<":
                return rule.Get() < num;
            case "<=":
                return rule.Get() <= num;
            case "==":
                return rule.Get() == num;
            case "!=":
                return rule.Get() != num;
            case ">=":
                return rule.Get() >= num;
            case ">":
                return rule.Get() > num;
        }

        return false;
    }

    public void Modify(string expression)
    {
        string[] parts = expression.Split(' ');

        FactRule rule;
        ruleEntries.TryGetValue(parts[0], out rule);

        int num = int.Parse(parts[2]);

        switch (parts[1])
        {
            case "=":
                rule.Set(num);
                break;
            case "+=":
                rule.Add(num);
                break;
        }
    }

    public bool AllCandy()
    {
        foreach (KeyValuePair<string, FactRule> exp in ruleEntries)
        {
            if (exp.Value.key.StartsWith("candy.") && !Evaluate(exp.Value.key + " == 1"))
                return false;
        }

        return true;
    }

    public KeyValuePair<int, int> CandyProgress()
    {
        int total = 0;
        int got = 0;

        foreach (KeyValuePair<string, FactRule> exp in ruleEntries)
        {
            if (exp.Value.key.StartsWith("candy."))
            {
                total += 1;
                if (Evaluate(exp.Value.key + " == 1"))
                    got += 1;
            }

        }

        return new KeyValuePair<int, int>(got, total);
    }
}
