using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Colors
{
    Red,
    Yellow,
    Green,
    LightBlue,
    Blue,
    Pink
}

public class ColorCodeController : MonoBehaviour
{
    public GameObject linkedTo;
    public Vector3 destination;

    public Dictionary<int, Colors> colorIDs = new Dictionary<int, Colors>();
    public Dictionary<Colors, Color> colors = new Dictionary<Colors, Color>();
    
    public GameObject[] lights;
    public GameObject[] buttons;
    private Color[,] lightInfo;
    private bool[] rightColor;

    private void Awake()
    {
        InitializeColors();

        GameObject[] tempLights = GameObject.FindGameObjectsWithTag("ColorCodeLight");
        GameObject[] tempButtons = GameObject.FindGameObjectsWithTag("ColorCodeButton");

        for (int i = tempLights.Length-1; i >= 0; i--)
        { 
            if (tempLights[i].transform.parent == transform)
            {
                Array.Resize(ref lights, lights.Length + 1);
                lights[lights.Length-1] = tempLights[i];
            }
        }

        for (int i = tempButtons.Length-1; i >= 0; i--)
        {
            if (tempButtons[i].transform.parent == transform)
            {
                Array.Resize(ref buttons, buttons.Length + 1);
                buttons[buttons.Length - 1] = tempButtons[i];
            }
        }

        lightInfo = new Color[lights.Length, 2];
        rightColor = new bool[lights.Length];

        StartCoroutine(GenerateTargetColors());

        StartCoroutine(GetLightInfo());
    }

    private void Update()
    {
        if (GetRightColors())
        {
            OpenDoor();
        }
    }

    public bool GetRightColors()
    {
        for (int i = 0; i < rightColor.Length; i++)
        {
            if (!rightColor[i])
            {
                return false;
            }
        }

        return true;
    }

    public void UpdateLightInfo(int id, bool targetColor, Color color)
    {
        PuzzleLight pz = lights[id].GetComponent<PuzzleLight>();

        if (targetColor)
        {
            lightInfo[id, 1] = color;
            pz.targetColor = lightInfo[id, 1];
        }
        else
        {
            lightInfo[id, 0] = color;

            pz.currentColor = lightInfo[id, 0];
            pz.light.color = pz.currentColor;
        }

        if (lightInfo[id, 0] == lightInfo[id, 1])
        {
            rightColor[id] = true;
        }
        else
        {
            rightColor[id] = false;
        }
    }

    private void OpenDoor()
    {
        if (linkedTo.transform.position.y > destination.y)
        {
            linkedTo.transform.position = Vector3.MoveTowards(linkedTo.transform.position, destination, 0.01f);
        }
    }

    private void InitializeColors() 
    {
        colorIDs.Add(0, Colors.Red);
        colorIDs.Add(1, Colors.Yellow);
        colorIDs.Add(2, Colors.Green);
        colorIDs.Add(3, Colors.LightBlue);
        colorIDs.Add(4, Colors.Blue);   
        colorIDs.Add(5, Colors.Pink);

        colors.Add(Colors.Red, new Color(255, 0, 0));
        colors.Add(Colors.Yellow, new Color(255, 255, 0));
        colors.Add(Colors.Green, new Color(0, 255, 0));
        colors.Add(Colors.LightBlue, new Color(0, 255, 255));
        colors.Add(Colors.Blue, new Color(0, 0, 255));
        colors.Add(Colors.Pink, new Color(255, 0, 255));
    }

    private IEnumerator GetLightInfo()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < lights.Length; i++)
        {
            PuzzleLight pz = lights[i].GetComponent<PuzzleLight>();

            lightInfo[i, 0] = pz.currentColor;
        }
    }

    private IEnumerator GenerateTargetColors()
    {
        yield return new WaitForSeconds(0.8f);

        Color[] targetColors = new Color[lights.Length];
        ChangeLights[] bt = new ChangeLights[buttons.Length];

        for (int i = 0; i < bt.Length; i++)
        {
            bt[i] = buttons[i].GetComponent<ChangeLights>();
        }

        for (int i = 0; i < (Math.Pow(colors.Count, lights.Length) / colors.Count); i++)
        {
            targetColors = bt[UnityEngine.Random.Range(0, 3)].GenerateTargetColors();
        }

        for (int i = 0; i < targetColors.Length; i++)
        {
            UpdateLightInfo(i, true, targetColors[i]);
        }
    }
}
