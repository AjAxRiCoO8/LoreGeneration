using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class CreateMapFromFile : MonoBehaviour
{
    string[] lines;
    public string path = "Assets/Kinder/map.txt";
    
    string[][] actions;

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public Transform parent;

    void Start()
    {
        lines = File.ReadAllLines(path);
        actions = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            actions[i] = lines[i].Split(' ');
        }

        CreateMap();
    }

    public void CreateMap()
    {
        Transform walls = (Instantiate(new GameObject("Walls"), parent) as GameObject).transform;
        Transform floors = (Instantiate(new GameObject("Floors"), parent) as GameObject).transform;

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < actions[i].Length; j++)
            {
                switch (actions[i][j])
                {
                    case "-":
                        break;
                    case "x":
                        Instantiate(floorPrefab, new Vector3(j * floorPrefab.transform.lossyScale.x, 0, i * floorPrefab.transform.lossyScale.z), Quaternion.identity, floors);
                        break;
                    case "0":
                        Instantiate(wallPrefab, new Vector3(j * wallPrefab.transform.lossyScale.x, wallPrefab.transform.localScale.y / 2, i * wallPrefab.transform.lossyScale.z), Quaternion.identity, walls);
                        break;
                }
            }
        }
    }
}
