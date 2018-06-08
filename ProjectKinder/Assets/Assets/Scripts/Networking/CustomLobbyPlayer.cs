using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomLobbyPlayer : NetworkLobbyPlayer {

    public Text playerName;
    public Button readyButton;

    private Image background;

	// Use this for initialization
	void Start () {
        background = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!readyToBegin)
        {
            background.color = Color.red;
        } else
        {
            background.color = Color.green;
        }
	}

    public void ClientIsReady()
    {
        if (isLocalPlayer)
        {
            SendReadyToBeginMessage();
        }
    }

    public override void OnClientEnterLobby()
    {
        //Debug.LogError("1234556");

        GameObject lobbyObject = GameObject.FindGameObjectWithTag("NetworkManager");

        lobbyObject.GetComponent<CustomLobbyManager>().ReorderGameObject(gameObject);

        base.OnClientEnterLobby();
    }

    public void ChangePlayerName(string playername)
    {
        playerName.text = playername;
    }
}
