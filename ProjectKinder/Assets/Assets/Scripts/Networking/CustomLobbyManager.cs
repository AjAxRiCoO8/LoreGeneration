using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomLobbyManager : NetworkLobbyManager {

    public GameObject[] lobbyPlayerPosition;
    public Canvas theCanvas;

    public Text ipView;
    public Text connectToIp;

    public UIManager uiManager;

	private NetworkDiscovery broadcast;

    private NetworkClient player1;

    private int lobbyPlayerPositionNumber = 0;

	private bool arePlayersReady = false;
	private bool sceneHasChanged = false;

    void Start()
    {
		broadcast = GetComponent<CustomNetworkDiscovery> ();
		Debug.Log (Network.player.ipAddress);
    }

    public void StartWithHost()
    {
        player1 = StartHost();
        DontDestroyOnLoad(gameObject);
    }

    public void StartBroadCastAsClient()
    {
        broadcast.Initialize();
        broadcast.StartAsClient();
    }

    public void ViewIP()
    {
        if (ipView.text == ("XXX.XXX.XXX.XX"))
        {
            ipView.text = Network.player.ipAddress;
        }
        else
        {
            ipView.text = "XXX.XXX.XXX.XX";
        }
    }

    public void ConnectToIp()
    {
        networkAddress = connectToIp.text;
        StartClient();
        DontDestroyOnLoad(gameObject);
    }

    public void ReorderGameObject(GameObject lobbyPlayer)
    {
        if (theCanvas == null) return;

        lobbyPlayer.transform.SetParent(theCanvas.transform);

        if (lobbyPlayerPositionNumber > lobbyPlayerPosition.Length)
        {
            lobbyPlayerPositionNumber = 0;
        }

        lobbyPlayer.GetComponent<CustomLobbyPlayer>().ChangePlayerName("Player " + (lobbyPlayerPositionNumber + 1));

        lobbyPlayer.transform.position = lobbyPlayerPosition[lobbyPlayerPositionNumber++].transform.position;
        lobbyPlayer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public override void OnStartHost()
    {
		/*
        broadcast.Initialize();
        broadcast.StartAsServer();
        */
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        uiManager.DisableConnectButton();

        base.OnClientConnect(conn);
    }

    public override void OnClientSceneChanged (NetworkConnection conn)
    {
		NetworkManagerHUD hudManager = GetComponent<NetworkManagerHUD> ();
		hudManager.showGUI = false;

        //Debug.LogError("SceneChanges");

		arePlayersReady = true;

		base.OnClientSceneChanged (conn);
	} 

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (lobbyPlayerPositionNumber > lobbyPlayerPosition.Length)
        {
            lobbyPlayerPositionNumber = 0;
        }

        //Debug.LogError("Spawn Host");

        GameObject newLobbyPlayer = (GameObject)Instantiate(lobbyPlayerPrefab.gameObject, lobbyPlayerPosition[lobbyPlayerPositionNumber].transform.position, Quaternion.identity);
        newLobbyPlayer.transform.SetParent(theCanvas.transform);

        return newLobbyPlayer;
    }

	public override void OnLobbyServerPlayersReady ()
	{
        //Debug.LogError ("PlayersReady");

		arePlayersReady = true;

		base.OnLobbyServerPlayersReady ();
	}

    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {

        base.OnLobbyClientSceneChanged(conn);
    }

    /* public override void OnLobbyStartClient(NetworkClient lobbyClient)
     {
         if (lobbyPlayerPositionNumber > lobbyPlayerPosition.Length)
         {
             lobbyPlayerPositionNumber = 0;
         }

         Debug.LogError("Spawn Client");

         lastSpawnWasLobby = true;

         base.OnLobbyStartClient(lobbyClient);
     }*/

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		//Debug.LogError ("PLAYER READY - id: " + playerControllerId);

		base.OnServerAddPlayer (conn, playerControllerId);

		if (arePlayersReady) {
			GameObject newPlayer = (GameObject)Instantiate (gamePlayerPrefab, GetStartPosition().position, Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, newPlayer, playerControllerId);
		}
	}

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //Debug.LogWarning("NotConnected");

        base.OnClientDisconnect(conn);
    }

    public override void OnLobbyClientAddPlayerFailed ()
	{
		if (!NetworkServer.active) {
			StartCoroutine ("WaitForServerToActive");
		}
		//ClientScene.Ready (client.connection);

		//base.OnLobbyClientAddPlayerFailed ();
	}

	IEnumerator WaitForServerToActive() {
		if (NetworkServer.active) {
			GameObject newPlayer = (GameObject)Instantiate (gamePlayerPrefab, GetStartPosition ().position, Quaternion.identity);
			NetworkServer.AddPlayerForConnection (client.connection, newPlayer, 1);
		}

		yield return null;
	}
}
