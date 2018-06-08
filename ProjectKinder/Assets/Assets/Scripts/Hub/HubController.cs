using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using System.Collections;

public class HubController : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors = new List<GameObject>();
    [SerializeField]
    private GameObject[] buttons;
    public GameObject[] lights;
    private int[] playerCode;

    private Dictionary<GameObject, int[]> doorCodes = new Dictionary<GameObject, int[]>();
    private Dictionary<int[], bool> codesGuessed = new Dictionary<int[], bool>();

    private void Awake()
    {
        #region Initializing Lights & Buttons
        lights = new GameObject[0];
        GameObject[] tempL = GameObject.FindGameObjectsWithTag("HubLight");
        for (int i = tempL.Length - 1; i >= 0; i--)
        {
            if (tempL[i].transform.parent.Equals(transform))
            {
                Array.Resize(ref lights, lights.Length + 1);
                lights[lights.Length - 1] = tempL[i];
            }
        }
        #endregion

        playerCode = new int[lights.Length];

        doorCodes.Add(doors[0], new int[4] { 0, 3, 1, 0 });
        codesGuessed.Add(doorCodes[doors[0]], false);

        doorCodes.Add(doors[1], new int[4] { 2, 0, 1, 3 });
        codesGuessed.Add(doorCodes[doors[1]], false);

        doorCodes.Add(doors[2], new int[4] { 1, 2, 0, 2 });
        codesGuessed.Add(doorCodes[doors[2]], false);

        doorCodes.Add(doors[3], new int[4] { 3, 1, 2, 1 });
        codesGuessed.Add(doorCodes[doors[3]], false);

        for (int i = 0; i < doorCodes.Count; i++)
        {
            string s = "";
            for (int j = 0; j < doorCodes[doors[i]].Length; j++)
            {
                s += doorCodes[doors[i]][j];
            }
            //Debug.LogError("Code " + i + ": " + s);
        }
    }
    
    /// <summary>
    /// This method generates a random code for the given <see cref="GameObject"/> and
    /// adds the information to the <see cref="DoorCodes"/> and <see cref="CodesGuessed"/> dictionaries.
    /// </summary>
    /// <param name="go"></param>
    private void GenerateDoorCode(GameObject go)
    {
        int[] doorCode = { Random.Range(0, buttons.Length), Random.Range(0, buttons.Length), Random.Range(0, buttons.Length), Random.Range(0, buttons.Length) };

        if (!doorCodes.ContainsValue(doorCode))
        {
            doorCodes.Add(go, doorCode);
            codesGuessed.Add(doorCodes[go], false);
        }
        else
        {
            GenerateDoorCode(go);
        }
    }

    /// <summary>
    /// This method resets the lights and checks whether the player code was right.
    /// If the code was right the lights will blink green and when they are wrong they will blink red while resetting.
    /// </summary>
    /// <returns>WaitForSeconds</returns>
    public IEnumerator ResetLights()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<HubButton>().isResetting = true;
        }

        string s = "";
        for (int j = 0; j < playerCode.Length; j++)
        {
            s += playerCode[j];
        }
        //Debug.LogError("Code: " + s);

        yield return new WaitForSeconds(0.5f);

        int x = 0;

        Color color = PlayerCodeIsRight() ? Color.green : Color.red;

        while (x < 7)
        {
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < lights.Length; i++)
            {
                HubLight hublight = lights[i].GetComponent<HubLight>();
                if (x % 2 == 0)
                {
                    hublight.UpdateLight(0f, color);
                }
                else
                {
                    hublight.UpdateLight(4f, color);
                }
            }

            x++;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<HubButton>().isResetting = false;
        }
    }

    /// <summary>
    /// This method checks whether the player pressed the buttons in the right order.
    /// If the buttons are pressed in the right order the next code can be used.
    /// </summary>
    /// <returns>true or false</returns>
    private bool PlayerCodeIsRight()
    {
        int[] code = new int[4];
        GetCode(ref code);

        for (int i = 0; i < playerCode.Length; i++)
        {
            if (playerCode[i] != code[i])
            {
                return false;
            }   
        }

        for (int i = 0; i < doors.Count; i++)
        {
            if (!codesGuessed[doorCodes[doors[i]]])
            {
                codesGuessed[doorCodes[doors[i]]] = true;
                doors[i].GetComponent<Door>().IsClosed = false;
                Debug.Log(i);

                break;
            }
        }

        string s = "";
        for (int i = 0; i < doorCodes[doors[1]].Length; i++)
        {
            s += doorCodes[doors[1]][i];
        }

        Debug.Log(s);

        return true;
    }

    /// <summary>
    /// Gets the currently active code for the player to guess and open that door.
    /// </summary>
    /// <param name="code">The reference to the code</param>
    private void GetCode(ref int[] code)
    {
        for (int i = 0; i < doors.Count; i++)
        {
            if (!codesGuessed[doorCodes[doors[i]]])
            {
                code = doorCodes[doors[i]];
                break;
            }
        }
    }

    #region Getters
    public List<GameObject> Doors
    {
        get { return doors; }
    }

    public Dictionary<GameObject, int[]> DoorCodes
    {
        get { return doorCodes; }
    }

    public Dictionary<int[], bool> CodesGuessed
    {
        get { return codesGuessed; }
    }

    public GameObject[] Lights
    {
        get { return lights; }
    }

    public int[] PlayerCode
    {
        get { return playerCode; }
        set { playerCode = value; }
    }
    #endregion
}