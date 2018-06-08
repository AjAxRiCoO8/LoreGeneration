using UnityEngine;
using System.Collections;

public class LightPuzzleGate : Gate {

    public LightBlockController boundedLightBlock1;
    public LightBlockController boundedLightBlock2;

    // Use this for initialization
    void Start () {
        StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("LightBlock"))
            {
                if (boundedLightBlock1 == null)
                {
                    boundedLightBlock1 = hitColliders[i].gameObject.GetComponent<LightBlockController>();
                }
                else if (boundedLightBlock2 == null)
                {
                    boundedLightBlock2 = hitColliders[i].gameObject.GetComponent<LightBlockController>();
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    public override void Update () {

        if (boundedLightBlock1 != null && boundedLightBlock2)
        {
            if (boundedLightBlock1.active || boundedLightBlock2.active)
            {
                base.Update();
            }
        }
        else if (boundedLightBlock1 != null)
        {
            if (boundedLightBlock1.active)
            {
                base.Update();
            }
        }
	}

    
}
