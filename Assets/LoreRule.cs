using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoreRule {

    [SerializeField]
    private string ruleStory;

    [SerializeField]
    private List<int> consumedProperties;
    [SerializeField]
    private List<int> producedProperties;

    [HideInInspector]
    public bool hasBeenProcessed = false;

    public LoreRule()
    {
        hasBeenProcessed = false;
    }

    public LoreRule(List<int> consumedProperties, List<int> producedProperties)
    {
        this.consumedProperties = consumedProperties;
        this.producedProperties = producedProperties;
    }

    public bool HasSameConsumedProperties(List<int> consumedProperties)
    {
        if (this.consumedProperties.Count != consumedProperties.Count)
        {
            return false;
        }
        else
        {
            this.consumedProperties.Sort();
            consumedProperties.Sort();

            for (int i = 0; i < this.consumedProperties.Count; i++)
            {
                if (this.consumedProperties[i] != consumedProperties[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    public bool HasSameProducedProperties(List<int> producedProperties)
    {
        if (this.producedProperties.Count != producedProperties.Count)
        {
            return false;
        }
        else
        {
            this.producedProperties.Sort();
            producedProperties.Sort();

            for (int i = 0; i < this.producedProperties.Count; i++)
            {
                if (this.producedProperties[i] != producedProperties[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    public List<int> ConsumedProperties
    {
        get { return consumedProperties; }
        set { consumedProperties = value; }
    }
    public List<int> ProducedProperties
    {
        get { return producedProperties; }
        set { producedProperties = value; }
    }

    public string RuleStory
    {
        get { return ruleStory; }
    }
}
