using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour {

    public string[] credits;

    public Text someText;
    public GameObject textSpawn;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < credits.Length; i++)
        {
            Text creditText = Instantiate(someText, gameObject.transform) as Text;
            creditText.text = credits[i];
			creditText.transform.position = new Vector3(textSpawn.transform.position.x, textSpawn.transform.position.y - (i * 90), textSpawn.transform.position.z - (i * 30));

			if (i == credits.Length - 1) {
				creditText.GetComponent<CreditTextController> ().isLastUI = true;
			}
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.Space)) {
            Application.Quit();
		}
	}
}
