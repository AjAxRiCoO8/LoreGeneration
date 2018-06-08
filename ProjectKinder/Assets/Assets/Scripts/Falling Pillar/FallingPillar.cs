using UnityEngine;
using System.Collections;

public class FallingPillar : MonoBehaviour {

    Animation anim;
    bool triggered = false;
    

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void StartFalling(bool fall)
    {
        if(fall && !triggered)
        {
            anim.Play("PillarFalling");
            triggered = true;
        }
        
    }

}
