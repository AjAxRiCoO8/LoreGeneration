using UnityEngine;
using System.Collections;

public class QteLever : AbstractQTE {

	public Transform leverHandle;

	Animation anim;
	int totalClips = 60;
	string lastClipPlayed = "";
	float lastPercentage = 0;

	bool startQTE;
	GameObject playerObject;
	float percentageTriggered;
	bool qteHasBeenDone = false;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animation> ();

		for (int i = 0; i < totalClips; i++) {
			anim.AddClip (anim.GetClip ("LeverAnimation"), "Clip" + i, i * (60 / totalClips), i * (60 / totalClips) + (60 / totalClips), false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("QTE: " + startQTE);
		if (startQTE) {
			if (lastPercentage < percentageTriggered && lastClipPlayed != "Clip" + (int)(totalClips * (percentageTriggered / 100))) {
				anim.Play ("Clip" + (int)(totalClips * (percentageTriggered / 100)));
				lastClipPlayed = "Clip" + (int)(totalClips * (percentageTriggered / 100));
			} else if (lastPercentage > percentageTriggered && lastClipPlayed != "Clip" + (int)(totalClips * (percentageTriggered / 100))) {

				anim ["LeverAnimation"].speed = -0.085f;
				if (!anim.isPlaying) {
					Debug.Log (anim ["LeverAnimation"].length);
					anim ["LeverAnimation"].time = anim ["LeverAnimation"].length * (percentageTriggered / 100);
				}
				anim.Play ("LeverAnimation");
			}
		}

		lastPercentage = percentageTriggered;
	}

	public void PullLever(GameObject player) {
		if (!qteHasBeenDone) {
			playerObject = player;
			startQTE = true;
			Debug.Log ("12345675432565756432");
		}
	}

	public override void SuccessOutcome ()
	{
		startQTE = false;
		Debug.Log ("SUCCESS");
		qteHasBeenDone = true;
	}

	public override void FailOutcome ()
	{
		
	}

	public override bool StartQTE {
		get {
			return startQTE;
		}
	}

	public override GameObject PlayerObject {
		get {
			return playerObject;
		}
	}

	public override float PercentageCompleted {
		get {
			return percentageTriggered;
		}
		set {
			percentageTriggered = value;
		}
	}

}
