using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditTextController : MonoBehaviour {

	public bool isLastUI;

	// Update is called once per frame
	void Update () {
		if (isLastUI) {
			if (transform.localPosition.y > 130) {
                Application.Quit();
			}
		}
	}
}
