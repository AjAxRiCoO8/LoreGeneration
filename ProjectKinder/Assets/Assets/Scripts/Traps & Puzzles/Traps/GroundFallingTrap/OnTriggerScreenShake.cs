using UnityEngine;
using System.Collections;

public class OnTriggerScreenShake : MonoBehaviour {

    public PressurePlate plateOne;
    public PressurePlate plateTwo;

    private bool active;

    // Update is called once per frame
    void Update()
    {
        if (plateOne.triggered && plateTwo.triggered)
        {
            if (!active)
            {
                active = true;

                startScreenShake();
            }
        }
    }

    void startScreenShake()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            player.transform.GetChild(0).GetComponent<CameraShake>().shake = true;
        }
    }
}
