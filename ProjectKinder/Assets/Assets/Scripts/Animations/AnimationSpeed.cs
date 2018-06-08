using UnityEngine;
using System.Collections;

public class AnimationSpeed : MonoBehaviour {

    public float animationSpeed;
    // Use this for initialization
    void Start () {
        GetComponent<Animator>().speed = animationSpeed;
    }
}
