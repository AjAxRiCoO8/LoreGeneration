using UnityEngine;
using System.Collections;

public class SwitchableGate : GateHandler {

    public GameObject sphere;
    public GameObject sphere2;
    public Material redGloom;
    public Material greenGloom;

    public bool doorActive;

    private bool materialGreen;

    void Start()
    {
        if (doorActive)
            materialGreen = true;
    }


	// Update is called once per frame
	public override void Update () {
	    if (doorActive)
        {
            base.OpenDoor();

            if (!materialGreen)
            {
                materialGreen = true;
                sphere.GetComponent<Renderer>().material = greenGloom;
                sphere2.GetComponent<Renderer>().material = greenGloom;
            }
        }
        else
        {
            // Close door
            base.CloseDoor();

            if (materialGreen)
            {
                materialGreen = false;
                sphere.GetComponent<Renderer>().material = redGloom;
                sphere2.GetComponent<Renderer>().material = redGloom;
            }
        }
    }
}
