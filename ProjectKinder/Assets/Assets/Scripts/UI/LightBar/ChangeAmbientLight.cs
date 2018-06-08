using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeAmbientLight : MonoBehaviour {

    public Slider slider;
    public string sceneToLoad;

    /* the range the light can be changed */
    public float ambientColor;

    Color currentColor;

    private float lastFrameAmbientColer;

	// Use this for initialization
	void Start () {
        currentColor.r = ambientColor;
        currentColor.g = ambientColor;
        currentColor.b = ambientColor;

        SetAmbientColor(currentColor);
        
    }

    void SetAmbientColor(Color newAmbientColor)
    {
        RenderSettings.ambientLight = currentColor;
    }

    public void valueCheck(float number)
    {
        ambientColor = number;

        currentColor.r = ambientColor;
        currentColor.g = ambientColor;
        currentColor.b = ambientColor;

        SetAmbientColor(currentColor);
    }


    public void saveLightSettings()
    {
        PlayerPrefs.SetFloat("ambientColor", ambientColor);
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        
        
    }

}
