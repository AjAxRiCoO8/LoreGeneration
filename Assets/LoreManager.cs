using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LoreManager : MonoBehaviour {

    public static int LOOP_COUNTER = 0;
    public static int TOTAL_LOOPS = 0;

    [SerializeField]
    List<string> actors = new List<string>();

    [SerializeField]
    [HideInInspector]
    List<LoreRule> rules = new List<LoreRule>(); // List in the inspector

    List<LoreProcessedRule> processedRules = new List<LoreProcessedRule>(); // hidden list with processedRule objects. This contains rules which have the same entry, but different outcomes.

    [SerializeField]
    [HideInInspector]
    List<int> init = new List<int>(); // Start list

    [HideInInspector]
    static LoreManager instance;

	// Use this for initialization
	void Start () 
    {
        instance = this;

        // Process all rules, checking for similar rules.
        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i].hasBeenProcessed)
            {
                continue;
            }

            LoreProcessedRule processedRule = new LoreProcessedRule(rules[i]);
            rules[i].hasBeenProcessed = true;

            for (int j = i + 1; j < rules.Count; j++)
            {
                if (rules[i].HasSameConsumedProperties(rules[j].ConsumedProperties))
                {
                    processedRule.AddRule(rules[j]);
                    rules[j].hasBeenProcessed = true;
                }
            }

            processedRules.Add(processedRule);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        int i = 0;

        // Check if a rule is valid in this frame
        for (i = 0; i < processedRules.Count; i++)
        {
            LoreManager.LOOP_COUNTER++;
            if (processedRules[i].IsValidRule(init))
            {
                // if so do the rule and end the frame
                processedRules[i].DoRule(init);
                break;
            }
        }

        //Debug.Log("Iterations needed for rule: " + LOOP_COUNTER);
        TOTAL_LOOPS += LOOP_COUNTER;
        LOOP_COUNTER = 0;
        
        if (i == processedRules.Count)
        {
            //Debug.Log("Iterations needed: " + LoreManager.TOTAL_LOOPS);
            enabled = false;
        }
	}

    public static LoreManager GetInstance()
    {
        return instance;
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
}
