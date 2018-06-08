using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ipAdress : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(Network.player.ipAddress);
	}

}
