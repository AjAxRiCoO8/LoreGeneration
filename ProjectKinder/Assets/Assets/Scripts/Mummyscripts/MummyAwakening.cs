using UnityEngine;
using System.Collections;

public class MummyAwakening : MonoBehaviour {

    public GameObject[] MummysToAwake = new GameObject[1];

    public bool useOfEventTimer;
    public float timeToYield;

    private int players;

    void start()
    {
        players = 0;
    }

    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {

        if (useOfEventTimer)
            StartCoroutine(WakeUpMummys(GetComponent<EventTimer>().yieldTime));
        else
            StartCoroutine(WakeUpMummys(timeToYield));

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            players--;
        }
    }

    IEnumerator WakeUpMummys(float waitforSeconds)
    {
        GameObject[] mummies = GameObject.FindGameObjectsWithTag("Mummy");
        for (int i = 0; i < mummies.Length; i++)
        {
            mummies[i].GetComponent<MummyVariables>().LoadInPlayerData();
        }

        yield return new WaitForSeconds(waitforSeconds);

        foreach (var mummy in MummysToAwake)
        {
            MummySleeping mummycode = mummy.GetComponent<MummySleeping>();

            mummycode.WakeUp();
        }
    }
	
	
}
