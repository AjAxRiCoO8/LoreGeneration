﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

//[ExecuteInEditMode]
public class LoreManager : MonoBehaviour
{
    public static int LOOP_COUNTER = 0;
    public static int TOTAL_LOOPS = 0;

    private const int MaxLengthStory = 3;

    [SerializeField]
    public List<string> actors = new List<string>();

    [SerializeField]
    [HideInInspector]
    List<LoreRule> rules = new List<LoreRule>(); // List in the inspector

    List<LoreProcessedRule> processedRules = new List<LoreProcessedRule>(); // hidden list with processedRule objects. This contains rules which have the same entry, but different outcomes.

    [SerializeField]
    [HideInInspector]
    List<int> init = new List<int>(); // Start list

    [SerializeField]
    [HideInInspector]
    List<int> storyState;

    [HideInInspector]
    Queue<string> story = new Queue<string>();

    //[HideInInspector]
    //string story = "";

    [HideInInspector]
    static LoreManager instance;

    [HideInInspector]
    public bool StoryComplete = false;

    [HideInInspector]
    public int lastChosenRule;

    StoryUIManager storyUIManager;


    // Use this for initialization
    void Start()
    {
        instance = this;

        storyState = new List<int>(init);

        storyUIManager = GetComponent<StoryUIManager>();

        // Process all rules, checking for similar rules.
        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i].hasBeenProcessed)
            {
                continue;
            }

            LoreProcessedRule processedRule = new LoreProcessedRule(rules[i], i);
            rules[i].hasBeenProcessed = true;

            for (int j = i + 1; j < rules.Count; j++)
            {
                if (rules[i].HasSameConsumedProperties(rules[j].ConsumedProperties))
                {
                    processedRule.AddRule(rules[j], j);
                    rules[j].hasBeenProcessed = true;
                }
            }

            processedRules.Add(processedRule);
        }

        StartCoroutine(UpdateStory());
    }

    IEnumerator UpdateStory()
    {
        yield return new WaitForSeconds(5);
        UpdateRules();
        StartCoroutine(UpdateStory());
    }

    public void UpdateRules()
    {
        int i = 0;

        List<LoreProcessedRule> validRules = new List<LoreProcessedRule>();

        // Check if a rule is valid in this frame
        for (i = 0; i < processedRules.Count; i++)
        {
            LoreManager.LOOP_COUNTER++;
            if (processedRules[i].IsValidRule(storyState))
            {
                // if so do the rule and end the frame
                validRules.Add(processedRules[i]);
            }
        }
        if (validRules.Count > 0)
        {
            int randomInputRule = Random.Range(0, validRules.Count);

            lastChosenRule = validRules[randomInputRule].DoRule(storyState);
        }

        //Debug.Log("Iterations needed for rule: " + LOOP_COUNTER);
        TOTAL_LOOPS += LOOP_COUNTER;
        LOOP_COUNTER = 0;
    }

    public static LoreManager GetInstance()
    {
        return instance;
    }

    public void UpdateStoryText(string newLine)
    {
        if (story.Count >= MaxLengthStory)
        {
            story.Dequeue();
        }

        string currentStory = "";

        foreach (var item in story)
        {
            currentStory += item + " ";
        }

        story.Enqueue(newLine);
        currentStory += "<b><i>" + newLine + "</i></b>";

        storyUIManager.SetStoryText(currentStory);
    }

    public void AddNewActiveActor(int newActor)
    {
        if (!storyState.Contains(newActor))
        {
            storyState.Add(newActor);
        }
    }

    public List<string> Actors
    {
        get { return actors; }
        set { actors = value; }
    }

    public List<LoreRule> Rules
    {
        get { return rules; }
        set { rules = value; }
    }

    public List<int> Init
    {
        get { return init; }
        set { init = value; }
    }

    public List<int> StoryState
    {
        get { return storyState; }
        set { storyState = value; }
    }
}
