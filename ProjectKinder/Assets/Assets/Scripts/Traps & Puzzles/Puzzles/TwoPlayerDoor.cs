using UnityEngine;
using System.Collections;

public class TwoPlayerDoor : MonoBehaviour
{
    [Range(1, 10)]
    public int timeToReachDestination;          //The time it takes for the door to open up.
	public bool bothPlayersNeeded = false;      //Whether both pressure plates need to be triggered at the same time
	public bool shouldStayOpen = false;         //Whether the door should stay opened when the pressure plates are no longer triggered

    private Transform[] doors;
    private Transform[] pressurePlates;
    private Vector3[] offsets;
    private Vector3[] initialPositions;

    private AudioSource doorAudio;
    private bool triggeredDoorAudio;

    void Awake()
    {
        doors = new Transform[2];
        doors[0] = transform.Find("Doors").Find("Door 1");
        doors[1] = transform.Find("Doors").Find("Door 2");

        pressurePlates = new Transform[2];
        pressurePlates[0] = transform.Find("Pressure Plate 1");
        pressurePlates[1] = transform.Find("Pressure Plate 2");

        offsets = new Vector3[2];
        offsets[0] = new Vector3(0, 0, doors[0].localPosition.z + doors[0].localScale.z);
        offsets[1] = new Vector3(0, 0, doors[1].localPosition.z - doors[1].localScale.z);

        initialPositions = new Vector3[2];
        initialPositions[0] = doors[0].localPosition;
        initialPositions[1] = doors[1].localPosition;

        doorAudio = transform.Find("Doors").GetComponent<AudioSource>();
    }

    /// <summary>
    /// Gets called every frame
    /// </summary>
	void Update()
    {
		OpenAndCloseDoors();
    }

    /// <summary>
    /// This method checks whether both players are needed, 
    /// and whether the pressure plates are triggered.
    /// Using this information doors will either open or close.
    /// </summary>
	void OpenAndCloseDoors()
    {
		if (bothPlayersNeeded)
        {
            //Checks if the pressure plate is triggered
            if (pressurePlates[0].GetComponent<PressurePlate>().triggered && pressurePlates[1].GetComponent<PressurePlate>().triggered)
            {
                OpenDoor(0);
                OpenDoor(1);
            }
            else
            {
                if (!shouldStayOpen)
                {
                    CloseDoor(0);
                    CloseDoor(1);
                }
            }
		}
        else
        {
			if (pressurePlates[0].GetComponent<PressurePlate> ().triggered || pressurePlates[1].GetComponent<PressurePlate> ().triggered)
            {
				OpenDoor(0);
                OpenDoor(1);
			}
            else
            {
				if (!shouldStayOpen)
                {
					CloseDoor(0);
                    CloseDoor(1);
				}
			}
		}
    }

    /// <summary>
    /// This method checks whether the door with the given index still has to move 
    /// and opens the door further if that's the case.
    /// </summary>
    /// <param name="index">The index of the door to open</param>
	void OpenDoor(int index)
    {
		if (Mathf.Abs (doors[index].localPosition.z) < Mathf.Abs (offsets[index].z))
        {
			doors[index].Translate (offsets[index] * Time.deltaTime / timeToReachDestination, Space.Self);
			if (!triggeredDoorAudio)
            {
				doorAudio.Play ();
				triggeredDoorAudio = true;
			}
		}
        else
        {
			doorAudio.Stop ();
			triggeredDoorAudio = false;
		}
	}

    /// <summary>
    /// This method checks whether the door with the given index still has to move 
    /// and closes the door further if that's the case.
    /// </summary>
    /// <param name="index">The index of the door to open</param>
	void CloseDoor(int index)
    {
		if (Mathf.Abs(doors[index].localPosition.z) > Mathf.Abs(initialPositions[index].z))
		{
			doors[index].Translate(-offsets[index] * Time.deltaTime / (timeToReachDestination / 2), Space.Self);
			if (!triggeredDoorAudio)
			{
				doorAudio.Play();
				triggeredDoorAudio = true;
			}
		}
		else
		{
			doorAudio.Stop();
			triggeredDoorAudio = false;
		}
	}
}
