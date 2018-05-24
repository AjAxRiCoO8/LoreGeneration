using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreProcessedRule {

    List<int> consumedProperties = new List<int>();

    List<List<int>> producedProperties = new List<List<int>>();
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

    public void AddRule(LoreRule rule)
    {
        if (consumedProperties.Count == 0)
        {
            consumedProperties.AddRange(rule.ConsumedProperties);
            producedProperties.Add(rule.ProducedProperties);
            producedStory.Add(rule.RuleStory);
        }
        else
        {
            if (rule.HasSameConsumedProperties(this.consumedProperties))
            {
                for (int i = 0; i < producedProperties.Count; i++)
                {
                    if (rule.HasSameProducedProperties(producedProperties[i]))
                    {
                        return;
                    }
                }

                producedProperties.Add(rule.ProducedProperties);
                producedStory.Add(rule.RuleStory);
            }
        }
    }

    public bool IsValidRule(List<int> storyState)
    {
        List<int> temp = new List<int>(consumedProperties);

        temp.Sort();
        storyState.Sort();

        int amountOfHits = 0;

        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = 0; j < storyState.Count; j++)
            {
               // Debug.Log("Consumed: " + temp[i] + " // storyState: " + storyState[j]);
                if (temp[i] == storyState[j])
                {
                    amountOfHits++;
                }
            }
        }

        return temp.Count == amountOfHits;
    }

    public void DoRule(List<int> storyState)
    {
        for (int i = 0; i < consumedProperties.Count; i++)
        {
            for (int j = 0; j < storyState.Count; j++)
            {
                storyState.Remove(consumedProperties[i]);
            }
        }

        if (!HasSimilarRules)
        {
            for (int i = 0; i < producedProperties[0].Count; i++)
            {
                storyState.Add(producedProperties[0][i]);
            }
            Debug.Log(producedStory[0]);
        }
        else
        {
            int randomOutcome = (Random.Range(0, 100) / (int)RuleChance);

            if (randomOutcome >= producedProperties.Count || randomOutcome < 0)
            {
                Debug.Log("Something went wrong with randomOutcome in LoreProcessedRule");
            }

            for (int i = 0; i < producedProperties[randomOutcome].Count; i++)
            {
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
