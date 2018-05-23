using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(LoreState))]
public class LoreActor : LoreProperty {

    [SerializeField]
    LoreState state;

    private void OnValidate()
    {
        state = GetComponent<LoreState>();
    }


    // Use this for initialization
    void Start () {
        Debug.Log("Name: " + loreManager.Actors[name] + ", State: " + loreManager.States[state.Name]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public LoreState State
    {
        get { return state; }
        set { state = value; }
    }
}
