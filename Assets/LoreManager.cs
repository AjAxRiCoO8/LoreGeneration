using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LoreManager : MonoBehaviour {

    [SerializeField]
    List<string> states = new List<string>();
    [SerializeField]
    List<string> actors = new List<string>();

    static LoreManager instance;

	// Use this for initialization
	void Start () 
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static LoreManager GetInstance()
    {
        return instance;
    }

    public List<string> States
    {
        get { return states; }
        set { states = value; }
    }

    public List<string> Actors
    {
        get { return actors; }
        set { actors = value; }
    }
}
