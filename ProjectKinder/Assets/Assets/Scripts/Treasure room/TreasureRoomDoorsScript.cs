using UnityEngine;
using System.Collections;

public class TreasureRoomDoorsScript : MonoBehaviour
{
    //variables
    public bool PTPU;
    bool WallArtifact;
    public Animation doors;
    PlayMode mode;
    public GameObject ArtifactRight;
    public bool open = false;
    public float doorSpeed;
    bool ByTable = false;

    //make sure that when the player is near the table the doors can be opened
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ByTable = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //finding the artifacts
        ArtifactRight = GameObject.FindGameObjectWithTag("MainArtifactRight");
      

        //If the artifacts are null and the player has been near the table the door opens
        if (ArtifactRight == null && ByTable == true)
        {
            doors = this.GetComponent<Animation>();
            if (open == false)
            {
                doors["DoorsOpen"].speed = doorSpeed;
                doors.Play("DoorsOpen", mode = PlayMode.StopSameLayer);
                open = true;
            }
        }
        

    }
}
