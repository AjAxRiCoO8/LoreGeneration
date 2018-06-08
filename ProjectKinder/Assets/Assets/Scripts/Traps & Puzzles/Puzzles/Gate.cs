using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

    public Transform gate;             //The transform of Door 2   

    public GameObject pressurePlate1;   //The Pressure Plate Object that triggers the doors to open.
    public GameObject pressurePlate2;   //The Pressure Plate Object that triggers the wall to open.

    [Range(1, 10)]
    public int timeToReachDestination;  //The time it takes for the door / wall to open up.

    private AudioSource doorAudio;

    private Vector3 offsetGate;

    private Vector3 initialGatePos;

    private bool triggeredDoorAudio;
    private bool triggeredWallAudio;

    void Awake()
    {
        doorAudio = GetComponent<AudioSource>();

        offsetGate = new Vector3(0, gate.localPosition.y + 10, 0);

        initialGatePos = gate.localPosition;

    }

    public virtual void Update()
    {
        OpenDoor();
        CloseDoor();
    }


    /// <summary>
    /// This method checks whether the first pressure plate is triggered.
    /// If this is the case, the doors will open up.
    /// If this is not the case and the doors are still open, the doors will close again.
    /// All the position checks are done with absolute values, so we don't have to think about when the position is negative.
    /// </summary>
    /// 
    void OpenDoor()
    {
        if (pressurePlate1.GetComponent<PressurePlate>().triggered || pressurePlate2.GetComponent<PressurePlate>().triggered)
        {
            //Checks whether the position of Door 1 is lower then the offset destination of Door 1
            if ((gate.localPosition.y) < Mathf.Abs(offsetGate.y))
            {
                //If thats te case the door will be translated towards the destination, till the destination is reached.
                gate.Translate(offsetGate * Time.deltaTime / timeToReachDestination, Space.Self);

                ActivateAudio();
            }
            else
            {
                DeactivateAudio();
            }
        }
    }

    void CloseDoor()
    {
        if (!pressurePlate1.GetComponent<PressurePlate>().triggered && !pressurePlate2.GetComponent<PressurePlate>().triggered)
        {
            if ((gate.localPosition.y) > Mathf.Abs(initialGatePos.y))
            {
                gate.Translate(-offsetGate * Time.deltaTime / (timeToReachDestination / 2), Space.Self);
                if (!triggeredDoorAudio)
                {
                    ActivateAudio();
                }
            }
            else
            {
                DeactivateAudio();
            }
        }

    }

    void ActivateAudio()
    {
        if (!triggeredDoorAudio)
        {
            doorAudio.Play();
            triggeredDoorAudio = true;
        }
    }

    void DeactivateAudio()
    {
        doorAudio.Stop();
        triggeredDoorAudio = false;
    }
}

    
