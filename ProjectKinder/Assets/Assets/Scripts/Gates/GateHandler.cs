using UnityEngine;
using System.Collections;

public class GateHandler : MonoBehaviour
{

	public Transform gate;
	//The transform of Door 2

	public Trigger trigger;
	//The Pressure Plate Object that triggers the doors to open.

	[Range (1, 10)]
	public int timeToReachDestination;
	//The time it takes for the door / wall to open up.

	public bool startAsOpen = false;

	private AudioSource doorAudio;

	//private Vector3 offsetGate;
	public Vector3 openPosition;
	private Vector3 closedPosition;

	private Vector3 initialGatePos;

	private bool triggeredDoorAudio;
	private bool triggeredWallAudio;

	public bool doorIsClosed = false;

	void Awake ()
	{
		doorAudio = GetComponent<AudioSource> ();

//		if (startAsOpen) {
//			offsetGate = new Vector3 (0, gate.localPosition.y - 10, 0);
//		} else {
//			offsetGate = new Vector3 (0, gate.localPosition.y + 10, 0);
//		}

		closedPosition = gate.localPosition;
		openPosition = new Vector3 (gate.localPosition.x, gate.localPosition.y + 10, gate.localPosition.z);

		//initialGatePos = gate.localPosition;

		doorIsClosed = !startAsOpen;

	}

	void Start() {
		if (startAsOpen) {
			gate.localPosition = openPosition;
		}
	}

	public virtual void Update ()
	{
		if (((!startAsOpen && trigger.IsClosed) || (startAsOpen && !trigger.IsClosed))  && !doorIsClosed) {
			// Close door
			CloseDoor ();
		} else if (((!startAsOpen && !trigger.IsClosed) || (startAsOpen && trigger.IsClosed)) && doorIsClosed) {
			// open door
			OpenDoor ();
		}
	}

	/// <summary>
	/// This method checks whether the first pressure plate is triggered.
	/// If this is the case, the doors will open up.
	/// If this is not the case and the doors are still open, the doors will close again.
	/// All the position checks are done with absolute values, so we don't have to think about when the position is negative.
	/// </summary>
	/// 
	public void OpenDoor ()
	{
		//Checks whether the position of Door 1 is lower then the offset destination of Door 1
		if ((gate.localPosition.y) < openPosition.y) {
			//If thats te case the door will be translated towards the destination, till the destination is reached.
			gate.Translate (new Vector3 (0, gate.localScale.y, 0) * Time.deltaTime / timeToReachDestination, Space.Self);

			ActivateAudio ();
		} else {
			DeactivateAudio ();
			doorIsClosed = false;
		}
	}

	public void CloseDoor ()
	{
		if ((gate.localPosition.y) > closedPosition.y) {
			gate.Translate (new Vector3 (0, -gate.localScale.y, 0) * Time.deltaTime / timeToReachDestination, Space.Self);

			ActivateAudio ();
		} else {
			DeactivateAudio ();
			doorIsClosed = true;
		} 
	}

	void ActivateAudio ()
	{
		if (!triggeredDoorAudio) {
			doorAudio.Play ();
			triggeredDoorAudio = true;
		}
	}

	void DeactivateAudio ()
	{
		doorAudio.Stop ();
		triggeredDoorAudio = false;
	}

}
