using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LoreManager : MonoBehaviour {

    [SerializeField]
    List<string> actors = new List<string>();

    [SerializeField]
    [HideInInspector]
    List<LoreRule> rules = new List<LoreRule>();

    List<LoreProcessedRule> processedRules = new List<LoreProcessedRule>();

    [SerializeField]
    [HideInInspector]
    List<int> init = new List<int>();

    [HideInInspector]
    static LoreManager instance;

	// Use this for initialization
	void Start () 
    {
        instance = this;

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
        foreach (LoreProcessedRule rule in processedRules)
        {
            if (rule.IsValidRule(init))
            {
                rule.DoRule(init);
                break;
            }
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
