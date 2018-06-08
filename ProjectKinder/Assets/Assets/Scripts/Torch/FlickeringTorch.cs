using UnityEngine;
using System.Collections;

public class FlickeringTorch : MonoBehaviour {

    private new Light light;

    public float scale;
    public float speed;
    public float timerLenght;
    public float maxDifference;

    private float intensityLight;
    private float intensityLightOffset;
    private float rangeLight;
    private float rangeLightOffset;
    private float changedIntensitie;
    private float changedRange;
    private float Timer = 0;

    private bool setRangeAndIntensity;

    private Material pilarMatirial;

    // Use this for initialization
    void Start () {
        light = GetComponent<Light>();

        scale *= 0.1f;
        speed *= 0.02f;
        timerLenght *= 0.01f;

        intensityLightOffset = light.intensity * scale;
        rangeLightOffset = light.range * scale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Timer >= 0.02f)
        {

            Timer = 0f;
            IntensityAndRange();
        }

        Timer += Time.deltaTime;
    }

    private void IntensityAndRange()
    {
        float intensityDelta = 0;
        float rangeDelta = 0;

        intensityLightOffset = light.intensity * scale;
        rangeLightOffset = light.range * scale;

        intensityDelta = Random.Range(-intensityLightOffset, intensityLightOffset) * speed;
        rangeDelta = Random.Range(-rangeLightOffset, rangeLightOffset) * speed;

        if (Mathf.Abs(changedIntensitie) > maxDifference)
        {
            intensityDelta = CheckChanged(intensityDelta, changedIntensitie);
            light.intensity += intensityDelta;
            changedIntensitie += intensityDelta;
        }
        else
        {
            light.intensity += intensityDelta;
            changedIntensitie += intensityDelta;
        }

        if (Mathf.Abs(changedRange) > maxDifference)
        {
            rangeDelta = CheckChanged(rangeDelta , changedRange);
            light.range += rangeDelta;
            changedRange += rangeDelta;
        }
        else
        {
            light.range += rangeDelta;
            changedRange += rangeDelta;
        }
        
        //Debug.Log(changedIntensitie);
        //Debug.Log(changedRange);

        //rangeTarget = rangeOrigin + Random.Range(-rangeOffset, rangeOffset);
        //rangeDelta = (rangeTarget - light.range) * speed;

    }

    private float CheckChanged(float LightAmount, float changedAmount)
    {
        if (changedAmount > maxDifference)
        {
            if (LightAmount > 0)
            {
                return -LightAmount;
            }
            else
            {
                return LightAmount;
            }
        }
        else if (changedAmount < -maxDifference)
        {
            if (LightAmount < 0)
            {
                return -LightAmount;
            }
            else
            {
                return LightAmount;
            }

        }
        else
        {
            return LightAmount;
        }
    }
}
