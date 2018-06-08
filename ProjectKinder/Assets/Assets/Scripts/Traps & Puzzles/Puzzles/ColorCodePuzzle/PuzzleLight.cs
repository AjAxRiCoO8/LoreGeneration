using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLight : MonoBehaviour
{
    public Light light;
    [HideInInspector]
    public Color startingColor;
    public Color currentColor;
    public Color targetColor;

    private ColorCodeController cc;

    private void Awake()
    {
        cc = GetComponentInParent<ColorCodeController>();

        StartCoroutine(InitializeLightVariables());
    }	

    private IEnumerator InitializeLightVariables()
    {
        yield return new WaitForSeconds(0.5f);

        currentColor = cc.colors[cc.colorIDs[Random.Range(0, cc.colors.Count)]];
        startingColor = currentColor;
        light.color = currentColor;
    }
}
