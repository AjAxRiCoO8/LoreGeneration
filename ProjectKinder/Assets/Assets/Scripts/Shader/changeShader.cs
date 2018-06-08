using UnityEngine;
using System.Collections;

public class changeShader : MonoBehaviour {

    // De shader waar hij heen verandert.
    public Shader outlineShader;

    // true if you want to change the materials of the children.
    public bool changeChildren;

    //De shader die je in het begin op het object plaatst. 
    Shader startShader;

    Material[] materials;

	// Use this for initialization
	void Start () {
     startShader = GetComponent<MeshRenderer>().material.shader;

        if (changeChildren)
        {
            materials = GetComponentInChildren<MeshRenderer>().materials;
        }
        else
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
    }
	
	// Update is called once per frame
	public void Outline () {

        foreach (var material in materials)
        {
            material.shader = outlineShader;
        }
    }

    public void StandartShader()
    {
        foreach (var material in materials)
        {
            material.shader = startShader;
        }
    }

}
