using UnityEngine;

/// <summary>
/// Checks whether the player enters the trigger of the pressure plate, 
/// And if this is the case that information will be further used in the <see cref="PlayerSplittingTrap"/> script.
/// </summary>
public class PressurePlate : MonoBehaviour
{
    public new Light light;

    public bool triggered;          //Whether the pressure plate is currently triggered.
    public bool isOneTimeTrigger;   //Whether the pressure plate only needs to be triggered once or not, if this is true, the player only needs to enter the trigger once.

    private AudioSource audioSource;
    private Color fancyRed;
    private Color fancyGreen;

    void Awake()
    {
        triggered = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        fancyRed = new Color(143, 10, 10);
        fancyGreen = new Color(10, 143, 61);
        light.color = fancyRed;
    }

    /// <summary>
    /// Checks whether a Collider enters the Pressure Plate's trigger.
    /// </summary>
    /// <param name="collider">The collider that enters the trigger</param>
    void OnTriggerEnter(Collider collider)
    {
        //ONLY if the collider has the player tag
        if (collider.CompareTag("Player"))
        {
            triggered = true;
            audioSource.Play();
            light.color = fancyGreen;
        }
    }

    /// <summary>
    /// Checks whether a Collider exits the Pressure Plate's trigger.
    /// </summary>
    /// <param name="collider">The collider that exits the trigger</param>
    void OnTriggerExit(Collider collider)
    {
        //ONLY if the collider has the player tag AND is NOT a one time trigger
		if (collider.CompareTag ("Player") && !isOneTimeTrigger)
        {
            //The pressure plate will not be triggered anymore.
			triggered = false;
            light.color = fancyRed;
        }
    }
}
