using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LeverAnimation : Trigger {

    public Animation anim;
    private AudioSource audioSource;

    public bool triggered = false;
    public bool isReusable;

    [SerializeField] private AudioClip leverPull; 

	public bool useQTE = false;

    [HideInInspector]
    public bool changedState;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

	public void LeverDown()
    {
        anim["LeverAnimation"].speed = 1;
        anim.Play("LeverAnimation");
    }

    public void LeverUp()
    {
        anim["LeverAnimation"].speed = -1;
        if (!anim.isPlaying)
        {
            anim["LeverAnimation"].time = anim["LeverAnimation"].length;
        }
        anim.Play("LeverAnimation");
    }

    public void PlayAnimation()
    {
        if (!triggered)
        {
            LeverDown();
            //leverAudio();
        }
        else
            LeverUp();
    }

	public void Trigger() {
        if (!isReusable)
        {
            PlayAnimation();
            triggered = true;
            changedState = true;
        }
        else
        {
            PlayAnimation();
            Debug.Log("Triggered = " + triggered + " =D");
            triggered = !triggered;
            changedState = true;

        }
	}


	public override bool IsClosed {
		get {
			return !triggered;
		}
        set
        {
            triggered = value;
        }
	}

    public void leverAudio()
    {
        audioSource.clip = leverPull;
        audioSource.Play();
        audioSource.volume = 0.3f;
    }
}
