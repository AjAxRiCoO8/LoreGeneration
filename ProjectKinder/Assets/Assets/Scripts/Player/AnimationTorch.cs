using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class AnimationTorch : NetworkBehaviour
{

    Animator anim;
    bool run;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            return;
        }
            // checks for walking input
            float moveV = Input.GetAxis("Vertical");
            float moveH = Input.GetAxis("Horizontal");
        
        
        // if the player presses the left shift he is running or else he is not
        if(Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

            // directing the variables to the animator
            anim.SetFloat("MovingH", moveH);
            anim.SetFloat("MovingV", moveV);
            anim.SetBool("Run", run);
        }


    
}
