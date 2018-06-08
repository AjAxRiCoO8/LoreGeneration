using UnityEngine;
using System.Collections;

public class ActivateTrap : AbstractQTE {

	[Range(0, 15)]
	public int damagePerSecond = 1;

	public bool isReusable = false;

	bool hasBeenUsed = false;

	bool startQTE = false;

	bool isPlayerStuck = false;
	GameObject player;

	float damageClock = 0;
	int timesDamageDone = 0;

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			if (!hasBeenUsed || isReusable) {
				Debug.Log ("coll");
				GetComponent<Animator> ().SetBool ("Stuck", true);

				isPlayerStuck = true;
				player = other.gameObject;

				other.GetComponent<FirstPersonController>().IsStuck = true;
				other.transform.position = transform.Find ("TrapPoint").transform.position;

				startQTE = true;
				hasBeenUsed = true;

			}
		}
	}

	void Update() {
		if (isPlayerStuck) {
			damageClock += Time.deltaTime;
			if (damageClock / (timesDamageDone + 1) >= 1) {
				Debug.Log ("Damage");
				player.GetComponent<HealthController> ().TakeDamage (damagePerSecond);
				timesDamageDone++;
			}
		}
	}

	public override void SuccessOutcome ()
	{
		ReleasePlayer (player, true);
	}

	public override void FailOutcome ()
	{
		ReleasePlayer (player, false);

	}

	void ReleasePlayer(GameObject player, bool success) {
		startQTE = false;
		isPlayerStuck = false;
		player.GetComponent<FirstPersonController> ().IsStuck = false;
		GetComponent<Animator> ().SetBool ("Stuck", false);

		if (!success) {
			player.GetComponent<HealthController> ().TakeDamage (10);
		}
	}

	public override bool StartQTE {
		get {
			return startQTE; 
		}
	}

	public override GameObject PlayerObject {
		get {
			return player;
		}
	}

	public override float PercentageCompleted {
		get;
		set;
	}
}
