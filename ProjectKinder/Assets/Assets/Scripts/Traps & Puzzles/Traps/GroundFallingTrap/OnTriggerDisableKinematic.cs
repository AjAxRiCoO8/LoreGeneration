using UnityEngine;
using System.Collections;

public class OnTriggerDisableKinematic : MonoBehaviour {

    public PressurePlate plateOne;

    public GameObject[] floors = new GameObject[0];

    public float interval;

    private bool active;

	// Update is called once per frame
	void Update () {
	
        if (plateOne.triggered)
        {
            if (!active)
            {
                active = true;

                StartCoroutine(disableKinemetic());
            }
        }
	}

    IEnumerator disableKinemetic()
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(interval);

        foreach (var floor in floors)
        {
            floor.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


}
