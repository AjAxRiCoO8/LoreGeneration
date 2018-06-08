using UnityEngine;
using System.Collections;

public class EventTimer : MonoBehaviour {

    public float yieldTime;
    public bool activated;

    void OnTriggerEnter()
    {
        activated = true;
    }
}
