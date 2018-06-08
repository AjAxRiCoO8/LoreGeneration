using UnityEngine;
using System.Collections;

public class PillarTrigger : MonoBehaviour {

    public GameObject[] pillars;
    public bool activate;
    public bool delay;
    public float delayFalling;

    private float delayFallingSave;
    private int pillarCount = 0;

	// Use this for initialization
	void Start () {
        delayFallingSave = delayFalling;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (activate && !delay)
        {
            for (int i = 0; i < pillars.Length; i++)
            {
                pillars[i].GetComponent<FallingPillar>().StartFalling(true);
            }
        }
        else if(activate && delay)
        {
            if(pillarCount != pillars.Length)
            delayFalling -= Time.deltaTime;

            if (delayFalling <= 0 && pillarCount != pillars.Length)
            { 
                pillars[pillarCount].GetComponent<FallingPillar>().StartFalling(true);
                delayFalling = delayFallingSave;
                pillarCount++;
   
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            activate = true;
        }
    }
}
