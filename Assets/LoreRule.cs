using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreRule {

    private List<ILoreProperty> consumedProperties;
    private List<ILoreProperty> producedProperties;

    LoreRule(List<ILoreProperty> consumedProperties, List<ILoreProperty> producedProperties)
    {
        this.consumedProperties = consumedProperties;
        this.producedProperties = producedProperties;
    }

    public List<ILoreProperty> GetConsumedProperties() { return consumedProperties; }
    public List<ILoreProperty> GetProducedProperties() { return producedProperties; }
}
