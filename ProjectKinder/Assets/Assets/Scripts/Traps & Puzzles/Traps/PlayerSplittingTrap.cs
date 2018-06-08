using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Handles all the logic behind the player splitting trap.
/// All of the calculations are done in local space, so it also functions when the object as a whole is rotated.
/// </summary>
public class PlayerSplittingTrap : MonoBehaviour
{
	[Range(1, 10)]
	public int timeToReachDestination;  //The time it takes for the door / wall to open up.

    private AudioSource doorAudio;
    private AudioSource wallAudio;

    private Transform[] doors;              //Transforms of the objects to open
    private Transform[] pressurePlates;     //Transforms of the pressure plates
    private Vector3[] offsets;              //Vector3, positions of the destination of the objects
    private Vector3[] initialPositions;     //Vector3, initial positions of the objects

    private bool triggeredDoorAudio;
    private bool triggeredWallAudio;
    private bool wallIsOpened;

    void Awake ()
	{
        doors = new Transform[3];
        doors[0] = transform.Find("Doors").Find("Door 1");
        doors[1] = transform.Find("Doors").Find("Door 2");
        doors[2] = transform.Find("WallToOpen");

        pressurePlates = new Transform[2];
        pressurePlates[0] = transform.Find("Pressure Plate 1");
        pressurePlates[1] = transform.Find("Pressure Plate 2");

        offsets = new Vector3[3];
        offsets[0] = new Vector3(0, 0, doors[0].localPosition.z + (doors[0].localScale.z - 1));
        offsets[1] = new Vector3(0, 0, doors[1].localPosition.z - (doors[1].localScale.z - 1));
        offsets[2] = new Vector3(0, doors[2].localPosition.y - doors[2].localScale.y, 0);

        initialPositions = new Vector3[3];
        initialPositions[0] = doors[0].localPosition;
        initialPositions[1] = doors[1].localPosition;
        initialPositions[2] = doors[2].localPosition;

        pressurePlates[1].GetComponent<PressurePlate>().isOneTimeTrigger = true;

        doorAudio = transform.Find("Doors").GetComponent<AudioSource>();
        wallAudio = transform.Find("WallToOpen").GetComponent<AudioSource>();

        wallIsOpened = false;
	}

    /// <summary>
    /// Gets called every frame
    /// </summary>
	void Update ()
	{
        if (pressurePlates[0].GetComponent<PressurePlate>().triggered && !wallIsOpened)
        {
            OpenDoor(0);
            OpenDoor(1);
        }
        else
        {
            CloseDoor(0);
            CloseDoor(1);
        }

        if (pressurePlates[1].GetComponent<PressurePlate>().triggered)
        {
            OpenWall();
        }
	}

    /// <summary>
    /// This method checks whether the door with the given index still has to move 
    /// and opens the door further if that's the case.
    /// </summary>
    /// <param name="index">The index of the door to open</param>
	void OpenDoor(int index)
    {
        if (Mathf.Abs(doors[index].localPosition.z) < Mathf.Abs(offsets[index].z))
        {
            doors[index].Translate(offsets[index] * Time.deltaTime / timeToReachDestination, Space.Self);
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
    
    /// <summary>
    /// This method translates the wall down, until the wall is at it's destination
    /// </summary>
    void OpenWall()
    {
        wallIsOpened = true;

        if (doors[2].position.y > offsets[2].y)
        {
            doors[2].Translate(offsets[2] * Time.deltaTime / timeToReachDestination, Space.Self);
            if (!triggeredWallAudio)
            {
                wallAudio.Play();
                triggeredWallAudio = true;
            }
        }
        else
        {
            wallAudio.Stop();
            triggeredWallAudio = false;
        }
    }
}
