using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager {

	private NetworkDiscovery customDiscovery;

	void Start() {
		customDiscovery = GetComponent<CustomNetworkDiscovery> ();
        // Show ip adress in the console.
		Debug.Log (Network.player.ipAddress);
	}

	public override void OnStartServer ()
	{
		customDiscovery.Initialize ();

		Debug.Log ("data: " + customDiscovery.broadcastData);

		Debug.Log ("port: " + customDiscovery.broadcastPort);

		customDiscovery.StartAsServer ();
	}

	public override void OnStartClient (NetworkClient client)
	{
		//Debug.Log ("address: " + client.connection.address);
		Debug.Log ("clientPort: " + customDiscovery.broadcastPort);

		customDiscovery.showGUI = false;
	}

	public void ChangeScene(string sceneName) {
		ServerChangeScene (sceneName);
	}
}

