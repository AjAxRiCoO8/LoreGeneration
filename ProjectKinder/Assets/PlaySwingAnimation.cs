using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlaySwingAnimation : NetworkBehaviour {

	public AxeTrapTrigger triggerScript;
	public GameObject axeTrap;

	[SyncVar(hook = "PlayAnimation")]
	bool playAnimation = false;

	public void PlayAnimation(bool playAnimation) {
		if (playAnimation) {	
			if (axeTrap.GetComponent<AxeTrapAnimation> ().anim.IsPlaying ("Axe_trap")) {
				return;
			} else {
				axeTrap.GetComponent<AxeTrapAnimation> ().anim.Play ();
				triggerScript.trapActivated = true;
			}
		}
	}

	public void SwingAxe() {
		playAnimation = true;
	}
}
