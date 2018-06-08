using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkDiscovery : NetworkDiscovery {

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("broadcast from: " + fromAddress);

		NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();

		StopBroadcast ();
    }

}
