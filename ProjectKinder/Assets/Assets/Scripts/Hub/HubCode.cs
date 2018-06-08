using UnityEngine;
using System.Collections;

public class HubCode : MonoBehaviour
{
    [Tooltip("The Gameobjects of the hieroglyphs of the hub code in this particular order")]
    [SerializeField] private GameObject[] hieroglyphs;

    [Space]
    [Tooltip("The objects the materials should be shown on")]
    [SerializeField] private GameObject[] objects;

    [Space]
    [Tooltip("The door this code is linked to")]
    [SerializeField] private GameObject door;

    [Space]
    [Tooltip("The hubcontroller of the hub in this level")]
    [SerializeField] private HubController hub;

    private int[] code;

    private void Start()
    {
        code = hub.DoorCodes[door];

        for (int i = 0; i < objects.Length; i++)
        {
            GameObject go = new GameObject("hieroglyph");
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            go.transform.SetParent(objects[i].transform);

            Vector3 scale = new Vector3(0, 0, 0);
            switch (code[i])
            {
                case 0:
                    scale = new Vector3(4, 4, 4);
                    break;
                case 1:
                    scale = new Vector3(20, 20, 20);
                    break;
                case 2:
                    scale = new Vector3(2.5f, 2.5f, 2.5f);
                    break;
                case 3:
                    scale = new Vector3(3, 3, 3);
                    break;                
            }
            go.transform.localScale = scale;
            go.transform.localPosition = new Vector3(0, 0.51f, 0);
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);

            go.GetComponent<MeshFilter>().mesh = hieroglyphs[code[i]].GetComponent<MeshFilter>().sharedMesh;
            go.GetComponent<MeshRenderer>().material = hieroglyphs[code[i]].GetComponent<MeshRenderer>().sharedMaterial;
      
        }
    }
}
