using UnityEngine;
using System.Collections;
using System;

public class MummiesActive : MonoBehaviour
{
    public GameObject[] children;

    private bool active;

    void Awake()
    {
        children = new GameObject[0];
        active = false;
        GetChildren();
    }

    void Update()
    {
        if (active)
        {
            SetChildrenActive(true);
        }
        else
        {
            SetChildrenActive(false);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            active = true;
        }
    }

    void GetChildren()
    {
        GameObject[] childrenTemp = GameObject.FindGameObjectsWithTag("Mummy");
 
        for (int i = 0; i < childrenTemp.Length; i++)
        {
            if (childrenTemp[i].transform.parent == transform)
            {
                Array.Resize(ref children, children.Length + 1);
                children[children.Length - 1] = childrenTemp[i];
            }
        }
    }

    private void SetChildrenActive(bool active)
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetActive(active);
        }
    }
}
