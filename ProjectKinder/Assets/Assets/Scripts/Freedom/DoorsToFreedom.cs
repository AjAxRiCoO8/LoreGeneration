using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DoorsToFreedom : NetworkBehaviour
{


	public Transform transferPointRight;
	public Transform transferPointLeft;
    public Transform prefab;
	public GameObject artifactRight;
	public GameObject artifactLeft;
	public new Light light;

	private Animation anim;
	private bool activated = false;
	private int artifactCount = 0;

	// Use this for initialization
	void Start ()
	{

		// get the animation component and set the speed of the animation to 0.1
		anim = GetComponent<Animation> ();
		anim ["LeftDoor"].speed = 0.1f;

		// light intensity set to 0 for it to stay dark
		light.intensity = 0;

		Debug.Log (artifactLeft);

	}
	
	// Update is called once per frame
	void Update ()
	{

		/* // temporary code for placing the artifacts untill the inventory system works
        if (Input.GetKeyDown(KeyCode.E))
        {
            //set the position of the artifacts to the correct positions on the walls 
            artifactRight.transform.position = transferPointRight.transform.position;
            Instantiate(artifactRight);
            artifactLeft.transform.position = transferPointLeft.transform.position;
            Instantiate(artifactLeft);
            
        }
        */

		// if 2 artifacts are present and the doors are not activated the doors will open
		if (artifactCount == 2 && !activated) {
			OpenDoors ();      
		}
	}

	void OnTriggerEnter (Collider other)
	{
		// check for the artifacts by tag and add to the artifactCount
		if (other.tag.Contains ("MainArtifact")) {
			artifactCount++;
		}
	}

	void OpenDoors ()
	{
		// play the animation and set activated to true
		anim.Play ();
		activated = true;

		// light intensity set to 8 for the light of freedom effect
		light.intensity = 8;
	}

	public void CreateArtifact (string position)
	{
		if (position.Equals ("left")) {
			Instantiate (artifactLeft, transferPointLeft.position, prefab.transform.rotation);
		} else {
			Instantiate (artifactRight, transferPointRight.position, prefab.transform.rotation);
		}
	}

}
