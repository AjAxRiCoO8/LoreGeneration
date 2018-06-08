using UnityEngine;
using System.Collections;

public class HubLight : MonoBehaviour
{
    public int id;
    
    public Light light;


    private void Awake()
    {
        light.intensity = 0;
    }

    public void UpdateLight(float intensity, Color color)
    {
        light.intensity = intensity;
        light.color = color;
    }
}
