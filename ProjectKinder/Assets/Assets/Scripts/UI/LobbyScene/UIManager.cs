using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public string goBackToScene;

    public Button serverHost;
    public Button findGame;

    public Button searchGame;
    public Button findManual;

    public Image ipImageView;
    public Button connectWithIP;

    public Button backButton;

    public Image ipDisplay;
    public Image ipText;

    public Image otherPlayerIPText;

    private bool pressedHost;
    private bool pressedClient;

    private bool pressedSearch;
    private bool pressedFind;

    private bool backToMainMenu = true;

    public void PressFirstChoice()
    {
        serverHost.gameObject.SetActive(false);
        findGame.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    public void FindGame()
    {
        backButton.gameObject.SetActive(true);
        pressedClient = true;
        backToMainMenu = false;
    }

    public void FindManually()
    {
        ipImageView.gameObject.SetActive(true);
        connectWithIP.gameObject.SetActive(true);
        otherPlayerIPText.gameObject.SetActive(true);

        ipDisplay.gameObject.SetActive(false);
        ipText.gameObject.SetActive(false);
    }

    public void GoBack()
    {
        if (backToMainMenu)
        {
            SceneManager.LoadScene(goBackToScene, LoadSceneMode.Single);
        } else if (pressedHost || pressedClient)
        {
            serverHost.gameObject.SetActive(true);
            findGame.gameObject.SetActive(true);
            ipDisplay.gameObject.SetActive(true);
            ipText.gameObject.SetActive(true);

			ipImageView.gameObject.SetActive (false);
			connectWithIP.gameObject.SetActive (false);
            otherPlayerIPText.gameObject.SetActive(false);

            pressedHost = false;
            pressedFind = false;
            backToMainMenu = true;
        }
        else if (pressedFind || pressedSearch)
        {
            ipImageView.gameObject.SetActive(false);
            connectWithIP.gameObject.SetActive(false);

            pressedFind = false;
            pressedSearch = false;

            pressedClient = true;
        }
    }

    public void DisableConnectButton()
    {
        connectWithIP.gameObject.SetActive(false);
        ipImageView.gameObject.SetActive(false);
    }
}