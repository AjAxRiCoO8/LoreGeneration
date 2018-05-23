using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoreActor : MonoBehaviour {
    
    public LoreManager loreManager = LoreManager.GetInstance();

    public int name;

    public int state;


    // Use this for initialization
    void Start () {
        Debug.Log("Name: " + loreManager.Actors[name] + ", State: " + loreManager.States[state]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
