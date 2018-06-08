using UnityEngine;
using System.Collections;

public class ChangeLights : MonoBehaviour
{
    public KeyCode pressKey;

    private ColorCodeController cc;
    private float timer = 1f;
    private bool canPress = true;

    [SerializeField] private int[] effectOnLights;

    private void Awake()
    {
        cc = GetComponentInParent<ColorCodeController>();

        StartCoroutine(InitializeEffects());
    }

    private void Update()
    {
        if (!canPress)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 1f;
                canPress = true;
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (Input.GetKeyDown(pressKey) && canPress)
            {
                ButtonPressed();
            }
        }
    }

    public Color[] GenerateTargetColors()
    {
        Color[] newColors = new Color[effectOnLights.Length];

        for (int i = 0; i < effectOnLights.Length; i++)
        {
            PuzzleLight pz = cc.lights[i].GetComponent<PuzzleLight>();

            for (int j = 0; j < cc.colorIDs.Count; j++)
            {
                if (cc.colors[cc.colorIDs[j]] == pz.currentColor)
                {
                    newColors[i] = cc.colors[cc.colorIDs[((j + effectOnLights[i]) % cc.colors.Count + cc.colors.Count) % cc.colors.Count]];
                }
            }
        }

        return newColors;
    }

    private void ButtonPressed()
    {
        for (int i = 0; i < effectOnLights.Length; i++)
        {
            PuzzleLight pz = cc.lights[i].GetComponent<PuzzleLight>();

            for (int j = 0; j < cc.colorIDs.Count; j++)
            {
                if (cc.colors[cc.colorIDs[j]] == pz.currentColor)
                {
                    Color newColor = cc.colors[cc.colorIDs[((j + effectOnLights[i]) % cc.colors.Count + cc.colors.Count) % cc.colors.Count]];
                    cc.UpdateLightInfo(i, false, newColor);

                    break;
                }
            }
        }

        canPress = false;
    }

    private IEnumerator InitializeEffects()
    {
        yield return new WaitForSeconds(0.5f);

        effectOnLights = new int[cc.lights.Length];

        int noEffectLights = 0;

        for (int i = 0; i < effectOnLights.Length; i++)
        {
            effectOnLights[i] = Random.Range(-3, 4);

            if (effectOnLights[i] == 0)
            {
                noEffectLights++;
            }
        }

        if (noEffectLights > 1)
        {
            StartCoroutine(InitializeEffects());
        } 
    }
}
