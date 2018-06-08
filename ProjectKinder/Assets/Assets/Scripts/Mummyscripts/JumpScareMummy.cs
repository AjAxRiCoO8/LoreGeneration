using UnityEngine;
using System.Collections;

public class JumpScareMummy : MonoBehaviour {

    public GameObject ScareMummy;

    public float time;

    private AudioSource jumpScareSound;

    private float despawnTime;
    private bool spawned = false;
    private bool readyToDestroy = false;
    

    // Use this for initialization
    void Start () {

        // set the object to false to conceal it and set the despawnTime equal to time
        SetObjectFalse(ScareMummy);
        despawnTime = time;
        jumpScareSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        /* if the scaremummy is ready to destroy the despawnTime will go down
        and if the despawnTime is less than 0 the scareMummy is set to false using the method SetObjectFalse
        readyToDestroy is set to false to stop the timer.
        */
        
        if (readyToDestroy)
        {
            despawnTime -= Time.deltaTime;

            if(despawnTime < 0)
            {
                SetObjectFalse(ScareMummy);
                readyToDestroy = false;
            }
   
        }

	}
    void OnTriggerEnter(Collider other)
    {
        /* if the player steps into the trigger and spawned is false, spawned is set to true
        and the scareMummy is set to active. readyToDestroy is true.
        */
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;
            SetObjectActive(ScareMummy);
            jumpScareSound.Play();
            readyToDestroy = true; 
        }
    }

    // method to set an object to active
    void SetObjectActive(GameObject theObject)
    {
        theObject.SetActive(true);
        
    }

    // method to set an object to false
    void SetObjectFalse(GameObject theObject)
    {
        theObject.SetActive(false);
    }

    

}
