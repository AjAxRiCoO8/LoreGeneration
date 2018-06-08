using UnityEngine;
using System.Collections;

public class OnTriggerEnterDestroy : MonoBehaviour {

    public GameObject[] itemsTodestroy = new GameObject[0];

    public bool useOfEventTimer;

    public float yieldTime;

    private EventTimer eventTimer;

    void Start()
    {
        eventTimer = GetComponent<EventTimer>();
    }

	void OnTriggerEnter()
    {
        if (useOfEventTimer && !eventTimer.activated)
        {
            StartCoroutine(DestroyObjects(eventTimer.yieldTime));
        }
        else if (!eventTimer.activated)
        {
            StartCoroutine(DestroyObjects(yieldTime));
        }
    }

    IEnumerator DestroyObjects(float waitForSeconds)
    {
        yield return new WaitForSeconds(waitForSeconds);

        foreach (var item in itemsTodestroy)
        {
            item.gameObject.SetActive(false);
        }
    }
}
