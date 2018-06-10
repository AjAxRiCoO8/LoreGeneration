using UnityEngine;
using System.Collections;

public class ChangeMusicOnTriggerEnter : MonoBehaviour
{

    public MusicController musicController;

    public float waitTime;

    public bool useOfEventTrigger;

    public MusicController.GameMusic gameMusic;

    private EventTimer eventTimer;

    private bool activated;

    void Start()
    {
        eventTimer = GetComponent<EventTimer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (useOfEventTrigger && !eventTimer.activated)
            {
                StartCoroutine(ChangeMusic(eventTimer.yieldTime));
            }
            else if (!activated)
            {
                StartCoroutine(ChangeMusic(waitTime));
                activated = true;
            }
        }
    }

    private IEnumerator ChangeMusic(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        musicController.ChangeMusic(gameMusic);
    }
}
