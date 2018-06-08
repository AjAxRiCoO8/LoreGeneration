using UnityEngine;
using System.Collections;

public class GateTrigger : Trigger {

	public bool isOpen = true;

	int players = 2;
	int enteredPlayers = 0;

	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.CompareTag ("Player")) {
            //Debug.LogError("NumbOfPlayers: " + players);
			if (++enteredPlayers / 2 >= players) {
				isOpen = false;
			}
		}
	}

	void OnTriggerExit(Collider collision) {
		if (collision.gameObject.CompareTag ("Player")) {
			enteredPlayers--;
		}
	}

	public override bool IsClosed {
		get {
			return isOpen;
		}
        set
        {
            isOpen = value;
        }
	}
}
