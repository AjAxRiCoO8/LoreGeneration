using UnityEngine;
using System.Collections;

public class HubButton : MonoBehaviour
{
    public int id;
    public bool canPress;
    public bool isResetting;
    public HubController controller;

    private HubLight hublight;
    private AudioSource audiosource;
    private int[] code;

    private float timer = 0.5f;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
        canPress = true;
    }

    private void Update()
    {
        if (!canPress)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                canPress = true;
                timer = 0.5f;
            }
        }
    }

    public void ButtonPressed()
    {
        GetLight(ref hublight);

        if (hublight != null)
        {
            controller.PlayerCode[hublight.id] = id;
            hublight.UpdateLight(4f, Color.cyan);   

            if (hublight.id == controller.Lights.Length - 1)
            {
                StartCoroutine(controller.ResetLights());
            }
        }

        audiosource.Play();
        canPress = false;
    }

    private void GetLight(ref HubLight hubLight)
    {
        for (int i = 0; i < controller.Lights.Length; i++)
        {
            if (controller.Lights[i].GetComponent<HubLight>().light.intensity == 0)
            {
                hubLight = controller.Lights[i].GetComponent<HubLight>();
                break;
            }
            else
            {
                hubLight = null;
            }
        }
    }
}
