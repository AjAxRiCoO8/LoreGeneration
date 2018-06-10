using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAddActor : MonoBehaviour {

    [SerializeField]
    private int ActorToAdd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           if (other.GetComponent<FirstPersonController>().isLocalPlayer)
           {
                LoreManager loreManger = other.GetComponentInChildren<LoreManager>();
                loreManger.AddNewActiveActor(ActorToAdd);
                gameObject.SetActive(false);
           }
        }
    }
}

