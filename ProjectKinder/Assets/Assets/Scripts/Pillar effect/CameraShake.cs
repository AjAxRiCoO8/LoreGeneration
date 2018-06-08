using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    Camera mainCamera;

    public float amplitude = 0.1f;
    public float durationTime;
    public bool shake;

    private Vector3 originalPos;
    
    private float duration;


	// Use this for initialization
	void Start () {
        mainCamera = GetComponent<Camera>();
        originalPos = mainCamera.transform.localPosition;
        duration = durationTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if shake is true it will shake the camera for a certain amount of time
        if(shake)
        {
            duration -= Time.deltaTime;

            if (duration > 0f)
            {
                mainCamera.transform.localPosition = mainCamera.transform.localPosition + Random.insideUnitSphere * amplitude;
            }
            else
            {
                mainCamera.transform.localPosition = originalPos;
                duration = durationTime;
            }
        }
        
        
	}
}
