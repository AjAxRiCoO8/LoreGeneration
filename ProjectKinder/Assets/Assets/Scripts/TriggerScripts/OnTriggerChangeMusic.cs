using UnityEngine;
using System.Collections;

public class OnTriggerChangeMusic : MonoBehaviour {

    public MusicController musicController;

    public Trigger trigger;

    public AudioSource audioToFade;

    public MusicController.GameMusic gameMusic;

    public float TimeToFloat;

    private bool activated;

    private IEnumerator coroutine;

    public virtual void Update()
    {
        if (!trigger.IsClosed && !activated)
        {
            coroutine = ChangeMusic(0);
            StartCoroutine(coroutine);
            activated = true;
        }
    }

    private IEnumerator ChangeMusic(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        musicController.ChangeMusic(gameMusic);
    }
}
