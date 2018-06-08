using UnityEngine;
using System.Collections;

public class MummyWakingUp : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

	void Update ()
    {
	    if (animator.GetCurrentAnimatorStateInfo(0).IsName("awakening") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            GetComponent<MummyStateMachine>().UpdateCurrentState(MummyStateMachine.States.Wandering);
        }
	}
	
}
