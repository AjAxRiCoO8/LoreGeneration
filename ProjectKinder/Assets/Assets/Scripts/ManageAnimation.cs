using UnityEngine;
using System.Collections;

public class ManageAnimation : MonoBehaviour {

    public AnimationHandler[] animations;

	void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartAnimations();
            Debug.Log("#1");
        }
    }

    void StartAnimations()
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].StartAnimation();
        }
    }

    void StopAnimations()
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].StopAnimation();
        }
    }
}
