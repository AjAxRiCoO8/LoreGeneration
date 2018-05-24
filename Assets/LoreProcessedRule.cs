using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreProcessedRule {

    List<int> consumedProperties = new List<int>();

    // List within a list. Has all the different outcomes for the same consumed propery list.
    List<List<int>> producedProperties = new List<List<int>>();
    // Stores the stories relevant to the produced property. Are on the same index.
    List<string> producedStory = new List<string>();

    public LoreProcessedRule() { }

    public LoreProcessedRule(LoreRule rule)
    {
        AddRule(rule);
    }

    public LoreProcessedRule(List<LoreRule> rules)
    {
        foreach (LoreRule rule in rules)
        {
            AddRule(rule);
        }
    }

    // Adds a rule to the lists.
    public void AddRule(LoreRule rule)
    {
        if (consumedProperties.Count == 0)
        {
            // Checks if there is no other rule yet, which means it will store everything right away.
            consumedProperties.AddRange(rule.ConsumedProperties);
            producedProperties.Add(rule.ProducedProperties);
            producedStory.Add(rule.RuleStory);
        }
        else
        {
            // Checks if the given rule has the same entry requirements as the one saved earlier.
            if (rule.HasSameConsumedProperties(this.consumedProperties))
            {
                // if so, check if this rule doesn't exist yet.
                for (int i = 0; i < producedProperties.Count; i++)
                {
                    if (rule.HasSameProducedProperties(producedProperties[i]))
                    {
                        return;
                    }
                }

                // if not add it to the list.
                producedProperties.Add(rule.ProducedProperties);
                producedStory.Add(rule.RuleStory);
            }
        }
    }

    // Check if the rule is valid with the current story state.
    public bool IsValidRule(List<int> storyState)
    {
        // copy the list in order to do things with it without affecting the original one.
        List<int> temp = new List<int>(consumedProperties);

        temp.Sort();
        storyState.Sort();

        int amountOfHits = 0;

        // Check how many hits the lists has in comparison to the story state.
        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = 0; j < storyState.Count; j++)
            {
                LoreManager.LOOP_COUNTER++;
                if (temp[i] == storyState[j])
                {
                    amountOfHits++;
                }
            }
        }

        // if the amount of hits is similar to the count, it means all requirements are met.
        return temp.Count == amountOfHits;
    }

    // Activate the rule.
    public void DoRule(List<int> storyState)
    {
        // Remove all consumed properties from the storystate.
        for (int i = 0; i < consumedProperties.Count; i++)
        {
            for (int j = 0; j < storyState.Count; j++)
            {
                LoreManager.LOOP_COUNTER++;
                storyState.Remove(consumedProperties[i]);
            }
        }

        // if only one rule
        if (!HasSimilarRules)
        {
            // Add produced properties to the storystate
            for (int i = 0; i < producedProperties[0].Count; i++)
            {
                LoreManager.LOOP_COUNTER++;
                storyState.Add(producedProperties[0][i]);
            }
            Debug.Log(producedStory[0]);
        }
        else
        {
            // Create a random number depending on how many different outcomes there are for this rule.
            int randomOutcome = (Random.Range(0, 100) / (int)RuleChance);

            if (randomOutcome >= producedProperties.Count || randomOutcome < 0)
            {
                Debug.Log("Something went wrong with randomOutcome in LoreProcessedRule");
            }

            // add produced properties to the storystate.
            for (int i = 0; i < producedProperties[randomOutcome].Count; i++)
            {
                LoreManager.LOOP_COUNTER++;
                storyState.Add(producedProperties[randomOutcome][i]);
            }
            Debug.Log(producedStory[randomOutcome]);
        }
    }

    public bool HasSimilarRules
    {
        get
        {
            return producedProperties.Count > 1;
        }
    }

    public int AmountOfRules
    {
        get
        {
            return producedProperties.Count;
        }
    }

    public float RuleChance
    {
        get
        {
            return 100 / AmountOfRules;
        }
    }
}
