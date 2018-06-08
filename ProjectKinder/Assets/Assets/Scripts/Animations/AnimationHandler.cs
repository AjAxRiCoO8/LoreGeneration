using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour
{

    public float animationSpeed;
    // Use this for initialization
    void Start()
    {
        GetComponent<Animator>().speed = animationSpeed;
        GetComponent<Animator>().SetBool("canSwing", false);
    }

    public void StartAnimation()
    {
        Debug.Log("StartAnimation");
        GetComponent<Animator>().SetBool("canSwing", true);
    }

    public void StopAnimation()
    {
        GetComponent<Animator>().SetBool("canSwing", false);
    }
}
