using UnityEngine;
using System.Collections;

public class AxeTrapTrigger : MonoBehaviour {

   
    public GameObject axeTrap;
    public bool trapActivated = false;

	void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag ("Player") && !trapActivated) {
			transform.parent.GetComponent<PlaySwingAnimation> ().SwingAxe ();
            //GetComponent<AudioSource>().Play();
		}
    }
}
