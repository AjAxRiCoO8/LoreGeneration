using UnityEngine;
using System.Collections;

public class AxeTrapAnimation : MonoBehaviour {


    public Animation anim;


	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
        anim["Axe_trap"].speed = 0.6f;
	}

    
 
}
