using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoreActor : LoreProperty {

    // Use this for initialization
    void Start () {
        Debug.Log("Name: " + loreManager.Actors[name]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
