using UnityEngine;
using System.Collections;

public class AmbientColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Color color;
        color = Color.black;
        float ambientColor = PlayerPrefs.GetFloat("ambientColor");

        color.r = ambientColor;
        color.g = ambientColor;
        color.b = ambientColor;

        RenderSettings.ambientLight = color;
        Debug.Log("checkAmbient");
	}
	
}
