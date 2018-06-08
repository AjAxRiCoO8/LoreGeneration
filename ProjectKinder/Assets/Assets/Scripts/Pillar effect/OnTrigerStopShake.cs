using UnityEngine;
using System.Collections;

public class OnTrigerStopShake : MonoBehaviour {

	void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            CameraShake shake = player.transform.GetChild(0).GetComponent<CameraShake>();

            if (shake.shake)
            {
                shake.shake = false;
            }
        }
    }
}
