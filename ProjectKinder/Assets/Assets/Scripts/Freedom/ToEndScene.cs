using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ToEndScene : MonoBehaviour {

	CustomLobbyManager manager;

	void Start() {
		manager = GameObject.FindGameObjectWithTag ("NetworkManager").GetComponent<CustomLobbyManager>();
	}

    void OnTriggerEnter(Collider other)
	{
        if(other.CompareTag("Player"))
        {
            Destroy(GameObject.FindGameObjectWithTag("NetworkManager"));
            SceneManager.LoadScene("CreditScene");
        }
    }
}
