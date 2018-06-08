using System.Collections;
using UnityEngine;

public class LightBlockController : MonoBehaviour {

    public new Light light;
    public ParticleSystem particle;

    Vector2 StartLightSize;

    public enum Sides
    {
        Right,
        left,
        Above,
        Under
    }

    public string id;
    public bool active;
    public bool[] hasSides = new bool[4];

    void Start()
    {
       // StartLightSize = light.areaSize;
    }


    void Update()
    {
        if (active)
        {
            particle.Play();
            light.gameObject.SetActive(true);
        }

        else
        {
            particle.Stop();
            light.gameObject.SetActive(false);
        }
            
    }

}
