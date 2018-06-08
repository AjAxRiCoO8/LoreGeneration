using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSelecter : MonoBehaviour {

    [SerializeField]
    string ifAmbientColorIsSet = "LobbyScene";
    [SerializeField]
    string ifAmbientColorIsNotSet = "LightBar";
    [SerializeField]
    bool startForTheFirstTime;

    
    // Use this for initialization
    void Start () {

        if (startForTheFirstTime)
        {
            PlayerPrefs.DeleteAll();
        }

	    if (PlayerPrefs.GetFloat("ambientColor") != 0)
        {
            Debug.Log("check");
            SceneManager.LoadScene(ifAmbientColorIsSet, LoadSceneMode.Single);
       
        }
        else
        {
            SceneManager.LoadScene(ifAmbientColorIsNotSet, LoadSceneMode.Single);
        }
	}
}
