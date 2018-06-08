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
           bool isLocalPlayer = other.GetComponent<FirstPersonController>().isLocalPlayer;
           if (isLocalPlayer)
           {
                LoreManager loreManger = other.GetComponentInChildren<LoreManager>();
                loreManger.AddNewActiveActor(0);
           }
        }
    }
}

