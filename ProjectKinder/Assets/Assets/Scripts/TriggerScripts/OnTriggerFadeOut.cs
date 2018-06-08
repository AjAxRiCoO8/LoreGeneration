using UnityEngine;
using System.Collections;

public class OnTriggerFadeOut : MonoBehaviour {

    public Trigger trigger;

    public AudioSource audioToFade;

    public float TimeToFloat;

    private bool activated;

    private IEnumerator coroutine;

    public virtual void Update()
    {
        if (!trigger.IsClosed && !activated)
        {
            coroutine = FadeOut(audioToFade, TimeToFloat);
            StartCoroutine(coroutine);
            activated = true;
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}
