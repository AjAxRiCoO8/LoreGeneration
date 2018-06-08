using UnityEngine;
using System.Collections;
using System;

public class ResetLights : MonoBehaviour
{
    private PuzzleLight[] puzzleLights;
	
    private void Start()
    {
        puzzleLights = new PuzzleLight[0];

        GameObject[] tempLights = GameObject.FindGameObjectsWithTag("ColorCodeLight");

        for (int i = tempLights.Length - 1; i >= 0; i--)
        {
            if (tempLights[i].transform.parent == transform.parent)
            {
                Array.Resize(ref puzzleLights, puzzleLights.Length + 1);
                puzzleLights[puzzleLights.Length - 1] = tempLights[i].GetComponent<PuzzleLight>();
            }
        }
    }

    private void FixedUpdate()
    {
        if (GetComponent<LeverAnimation>().changedState)
        {
            Reset();
            GetComponent<LeverAnimation>().changedState = false;
        }
    } 

    private void Reset()
    {
        for (int i = 0; i < puzzleLights.Length; i++)
        {
            transform.parent.GetComponent<ColorCodeController>().UpdateLightInfo(i, false, puzzleLights[i].startingColor);
        }
    }
}
