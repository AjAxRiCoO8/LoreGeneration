using UnityEngine;
using System.Collections;
using System;

public class DoorDownwards : Door
{
    [SerializeField][Range(1, 10)]
    private float openingTime;
    private bool isClosed;
    private bool audioTriggered;

    private void Start()
    {
        isClosed = true;
        destination = new Vector3(startingPosition.x, startingPosition.y - transform.localScale.y, startingPosition.z);
    }

    private void Update()
    {
        if (!isClosed)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    /// <summary>
    /// This method activates the audio if it's not activated yet.
    /// </summary>
    private void ActivateAudio()
    {
        if (!audioTriggered)
        {
            audioSource.Play();
            audioTriggered = true;
        }
    }

    /// <summary>
    /// This method deactivates the audio.
    /// </summary>
    private void DeactivateAudio()
    {
        audioSource.Stop();
        audioTriggered = false;
    }

    /// <summary>
    /// This method translates the transform upwards
    /// </summary>
    public override void OpenDoor()
    {
        if (transform.position.y > destination.y)
        {
            transform.Translate((new Vector3(0, -transform.localScale.y, 0) / openingTime) * Time.deltaTime);
            ActivateAudio();
        }
        else
        {
            DeactivateAudio();
        }
    }

    public override void CloseDoor()
    {
        if (transform.position.y < startingPosition.y)
        {
            transform.Translate((new Vector3(0, transform.localScale.y, 0) / openingTime) * Time.deltaTime);
        }
    }

    public override float OpeningTime
    {
        get { return openingTime; }
    }

    public override bool IsClosed
    {
        get { return isClosed; }
        set { isClosed = value; }
    }
}
