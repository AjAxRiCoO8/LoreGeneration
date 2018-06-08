using UnityEngine;
using System.Collections;

public class AxeTrap : MonoBehaviour {

    public int Damage;

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<HealthController>().TakeDamage(Damage);
        }
    }
}
