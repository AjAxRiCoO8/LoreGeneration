using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class Door : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Vector3 startingPosition;
    [HideInInspector] public Vector3 destination;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startingPosition = transform.position;
    }

    public abstract void OpenDoor();
    public abstract void CloseDoor();

    public abstract float OpeningTime
    {
        get;
    }

    public abstract bool IsClosed
    {
        get; set;
    }
}
