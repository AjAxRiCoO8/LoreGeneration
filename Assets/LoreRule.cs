using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoreRule {

    [SerializeField]
    private List<string> consumedProperties;
    [SerializeField]
    private List<string> producedProperties;

    LoreRule(List<string> consumedProperties, List<string> producedProperties)
    {
        this.consumedProperties = consumedProperties;
        this.producedProperties = producedProperties;
    }

    public List<string> GetConsumedProperties() { return consumedProperties; }
    public List<string> GetProducedProperties() { return producedProperties; }
}
