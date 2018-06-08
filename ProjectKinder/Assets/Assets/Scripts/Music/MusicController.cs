using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

    public enum GameMusic{
        approachingDarkness,
        demonAssault,
        hauntedManor,
        abandonedCellar,
        victoryMusic
    }

    public AudioClip[] music = new AudioClip[0];

    public float timeToChangeMusic;

    private AudioSource audioSource;

    private IEnumerator coroutine;

    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeMusic(GameMusic sountrack)
    {
        //StopMusic();
        //audioSource.clip = music[(int)sountrack];
        //audioSource.Play();

        coroutine = FadeOut(audioSource, timeToChangeMusic, sountrack);
        StartCoroutine(coroutine);

    }

    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime, GameMusic sountrack)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = music[(int)sountrack];


        coroutine = FadeIn(audioSource, timeToChangeMusic, sountrack, startVolume);
        StartCoroutine(coroutine);

        
    }

    private IEnumerator FadeIn(AudioSource audioSource, float inTime, GameMusic sountrack, float startVolume)
    {
        audioSource.Play();

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / inTime;

            yield return null;
        }
    }

}
