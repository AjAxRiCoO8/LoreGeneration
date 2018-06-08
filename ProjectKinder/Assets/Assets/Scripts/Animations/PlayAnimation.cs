using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour {

    public string playAnimation;
    private Animation animation;
	
    /// <summary>
    /// Play animation named as the string given in the inspector of the object
    /// </summary>
    public void PlayThisAnimation()
    {
        this.animation.Play(playAnimation);
    }

    /// <summary>
    /// Plays animation named the same as the string given.
    /// </summary>
    /// <param name="animation"></param>
	public void PlayThisAnimation (string animation) {
        this.animation.Play(animation);
    }

}
