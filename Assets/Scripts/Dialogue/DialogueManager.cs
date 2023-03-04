using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [HideInInspector()]
    public Manager manager;

    public Transform dialogueBox;

    private Queue<string> queue;
    private NPC curNpc;
    private Action callback;
    UnityEvent addDialogue = new UnityEvent();

    public bool active = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        queue = new Queue<string>();

        manager = Manager.instance;       
    }

    void Update()
    {
        dialogueBox.gameObject.SetActive(active);
    }

    public void Display()
    {
        if (queue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = queue.Dequeue();

        if (curNpc != null)
        {
            dialogueBox.GetChild(0).gameObject.SetActive(true);
            dialogueBox.GetComponent<Image>().color = curNpc.dialogueColor;
            dialogueBox.GetChild(0).GetComponent<TextMeshProUGUI>().text = !curNpc.displayName.Equals("") ? curNpc.displayName : curNpc.name;
        }
        else
        {
            dialogueBox.GetComponent<Image>().color = Color.white;
            dialogueBox.GetChild(0).gameObject.SetActive(false);
        }
        
        dialogueBox.GetChild(1).GetComponent<TextMeshProUGUI>().text = sentence;

        // show "continue" only if theres more
        dialogueBox.GetChild(2).gameObject.SetActive(queue.Count > 0);

        active = true;
    }

    public void EndDialogue()
    {
        active = false;

        if (curNpc != null)
            curNpc.isTalking = false;

        curNpc = null;

        if (callback != null)
            callback();
    }

    public void StartDialogue(string[] sentences, string npcName, Action callback, bool urgent = false)
    {
        queue.Clear();

        this.curNpc = (!npcName.Equals("") ? manager.getNPC(npcName) : null);
        this.callback = callback;

        if (curNpc != null)
            curNpc.isTalking = true;

        foreach (string sentence in sentences)
            queue.Enqueue(sentence);

        Display();
    }

    private Option[] ParseChoose(string choose)
    {
        // Trim <choose()>
        choose = choose.Substring(8, choose.Length - 2 - 8);

        // ???
        string[] array = choose.Split(',');
        List<Option> options = new List<Option>();

        foreach (string str in array)
        {
            string option = str.Substring(0, str.Length - 1).Split('(')[0];
            string expression = str.Substring(0, str.Length - 1).Split('(')[1];

            if (str.StartsWith("$"))
            {
                string[] comps = expression.Split('?', ':');

                if (Evaluate(comps[0]))
                    options.Add(new Option(int.Parse(comps[1]), option));
                else
                    options.Add(new Option(int.Parse(comps[2]), option));
            }
            else
            {
                options.Add(new Option(int.Parse(expression), option));
            }
        }

        return options.ToArray();
    }

    private bool Evaluate(string expression)
    {
        string[] parts = expression.Split(' ');
        FactRule rule;
        manager.ruleEntries.TryGetValue(parts[0], out rule);
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

    struct Option
    {
        public int dialogueIndex;
        public string name;

        public Option(int dialogueIndex, string name)
        {
            this.dialogueIndex = dialogueIndex;
            this.name = name;
        }
    }
}

// {choose\((.*?)\)}
// (.*?)\(\$(.*?) \? (\d) : (\d)\)||(.*?)\(\d\)
// (.*?)\((.*?)\)