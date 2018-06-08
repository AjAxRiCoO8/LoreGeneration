using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pillar : MonoBehaviour {

    RaycastHit hit;
    AudioSource audioRumble;

    public float timeTillEnd;
    public float downSpeed;

	public GameObject treasure;

    private float duration;
    private bool treasureIsHere = true;
    private bool activated = false;
    private bool won = false;

	// Use this for initialization
	void Start () {
        audioRumble = GetComponent<AudioSource>();
        duration = timeTillEnd;
        
	}
	
	// Update is called once per frame
	void Update () {

		// checking if reference of the treasure is still there, if not, treasureIsHere is turned to false.
		if (treasure == null) {
			treasureIsHere = false;
		}

        //if the treasure is taken and the pillar is activated for the first time
        if(!treasureIsHere && !activated)
        {
			ActivatePillar ();
        }
        
        // pillar goes down when the treasure is not present
        if(!treasureIsHere)
        {
            PillarGoDown();
        }

        // if the treasure is taken you will go to the end scene within a certain time
        if (won)
        {
            duration -= Time.deltaTime;
            if(duration <= 0f)
            {
				SwitchToEndSceen ();
            }
            
        }

       

        Debug.DrawRay(transform.position, Vector3.up * 2f, Color.red);
    }


    void PillarGoDown()
    {
        if(duration > 3f)
        {
            transform.localPosition += Vector3.down * downSpeed * Time.deltaTime;
        }
        else
        {
            transform.localPosition = transform.localPosition;
        }    
    }

	void ActivatePillar() {
		//audio will play + some sort of screen shake
		audioRumble.Play();
		activated = true;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			player.GetComponentInChildren<CameraShake> ().shake = true;
		}
		won = true;
	}

	void SwitchToEndSceen() {
		SceneManager.LoadScene ("End scene");
	}
    
}
