using UnityEngine;
using System.Collections;

public class DeactivateInvisibleWall : MonoBehaviour
{

    public GameObject[] itemsTodestroy = new GameObject[2];

    public bool useOfEventTimer;

    public float yieldTime;

    private EventTimer eventTimer;

    void Awake()
    {
        itemsTodestroy[1] = GameObject.Find("InvisbleWall");
        itemsTodestroy[0] = GameObject.Find("InvisibleWallEffect");
    }

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
