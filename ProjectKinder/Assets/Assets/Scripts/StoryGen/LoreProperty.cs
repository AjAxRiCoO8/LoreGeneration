using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class LoreProperty : MonoBehaviour
{
    public LoreManager loreManager;

    [SerializeField]
    new protected int name;

    public int Name
    {
        get { return name; }
        set { name = value; }
    }
}
