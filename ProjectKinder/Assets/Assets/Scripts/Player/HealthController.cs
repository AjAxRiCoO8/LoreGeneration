using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public AudioClip hit;
    public AudioClip dead;
    public UIController ui;
    public Image fadeScreen;
    public Text gameOverText;
    public Text deadText;

    private Rigidbody rb;

    public float maxHealth;
    public float currentHealth;
    public float countdown;
    public float countdownRate;

    private bool played;
    private float countdownSave;

    public GameObject[] invisibleWall;
    public GameObject[] mainArtifacts;

    // awake is called before start. 
    void Awake()
    {
        ui = GetComponent<UIController>();
        countdownSave = countdown;
        currentHealth = maxHealth;

        invisibleWall = new GameObject[2];
        invisibleWall[1] = GameObject.Find("InvisbleWall");
        invisibleWall[0] = GameObject.Find("InvisibleWallEffect");

        mainArtifacts = new GameObject[2];
        mainArtifacts[0] = GameObject.FindGameObjectWithTag("MainArtifactRight");
        mainArtifacts[1] = GameObject.FindGameObjectWithTag("MainArtifactLeft");
    }

    void Update()
    {
        //if the currenthealth is less than or equal to 0 the player will die and respawn.
        //ui for hp is updated. 
        if (currentHealth <= 0)
        {
            Death();
            Respawn();
            ui.UpdateHealthUI(maxHealth, currentHealth);
        }
    }

    /// <summary>
    ///take a certain amount of damage.
    ///blood splatters will show with the DamageEffect method.
    ///hp goes down with the damage.
    ///ui for hp is updated.
    /// </summary>
    public void TakeDamage(int damage)
    {
        DamageEffect();

        currentHealth -= damage;

        ui.UpdateHealthUI(maxHealth, currentHealth);
    }

    /// <summary>
    ///Plays the audio for getting hit and shows the blood spatters
    /// </summary>
    public void DamageEffect()
    {
        Debug.Log("check1");
        GetComponent<AudioSource>().PlayOneShot(hit, 1);
        GetComponent<BloodEffect>().ActivateBloodSpatter();
    }

    /// <summary>
    ///Death will disable the player movement by disabling the fpsController.
    ///The animator is stopped and the FadeScreen is shown
    ///if countdown is more than or equal to 0 the countdown will begin.
    ///RespawnText shows the respawn time
    /// </summary>
    public void Death()
    {
        if (!played)
        {
            played = true;
            GetComponent<AudioSource>().PlayOneShot(dead, 1);
        }
        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<Animator>().StartPlayback();
        FadeScreen(Color.red, 0.2f);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<FirstPersonController>().enabled = false;
            players[i].GetComponent<Animator>().StartPlayback();
            players[i].GetComponent<HealthController>().FadeScreen(Color.red, 0.2f);

            if (players[i].gameObject == gameObject)
            {
                players[i].GetComponent<HealthController>().deadText.text = "You died... \n Going back to main menu in " + Mathf.Round(countdown).ToString() + " seconds";
            }
            else
            {
                players[i].GetComponent<HealthController>().deadText.text = "Your friend died... \n Going back to main menu in " + Mathf.Round(countdown).ToString() + " seconds";
            }
        }

        if (countdown >= 0)
        {
            countdown -= countdownRate * Time.deltaTime;
        }
    }

    /// <summary>
    ///Respawn uses the method of the Respawn Class 'RespawnAtPoint' to spawn at a certain spawn point
    ///The fpsController is enabled
    ///The animator is started
    ///Health is restored to full
    ///The countdown is rest
    /// </summary>
    public void Respawn()
    {
        if (countdown <= 0)
        {
            Destroy(GameObject.FindGameObjectWithTag("NetworkManager"));
            SceneManager.LoadScene("MainMenu Scene");
        }
    }

    /// <summary>
    ///the fadeColor aplha is initialized with startAlpha
    ///fadeScreen color is set to fadeColor
    ///the image fadeScreen is set to active
    /// </summary>
    public void FadeScreen(Color fadeColor, float startAlpha)
    {
        fadeColor.a = startAlpha;
        fadeScreen.color = fadeColor;
        fadeScreen.gameObject.SetActive(true);
    }

    private int ResetCurrentWing()
    {
        HubController hub = GameObject.FindGameObjectWithTag("Hub").GetComponent<HubController>();
        int wing = 0;

        for (int i = 0; i < hub.CodesGuessed.Count; i++)
        {
            if (!hub.CodesGuessed[hub.DoorCodes[hub.Doors[i]]])
            {
                wing = i;
            }
        }

        //RESET MUSIC
        GameObject[] mummies = GameObject.FindGameObjectsWithTag("Mummy");
        for (int i = 0; i < mummies.Length; i++)
        {
            mummies[i].GetComponent<MummyStateMachine>().UpdateCurrentState(MummyStateMachine.States.Asleep);
            mummies[i].transform.position = mummies[i].GetComponent<MummyVariables>().StartingPosition;
            mummies[i].transform.rotation = mummies[i].GetComponent<MummyVariables>().StartingRotation;
        }

        switch (wing)
        {
            case 3:
                currentHealth = 0;

                GameObject gate = GameObject.FindGameObjectWithTag("Wing 3 Gate");
                gate.GetComponent<GateHandler>().trigger.IsClosed = true;
                gate.GetComponent<GateHandler>().OpenDoor();
                GameObject trigger = GameObject.FindGameObjectWithTag("Wing 3 Trigger");
                trigger.GetComponent<EventTimer>().activated = false;
                invisibleWall[0].SetActive(true);
                invisibleWall[1].SetActive(true);
                break;
            case 4:
                currentHealth = 0;

                GameObject gate2 = GameObject.FindGameObjectWithTag("Wing 4 Gate");
                gate2.GetComponent<GateHandler>().trigger.IsClosed = true;
                if (gate2.GetComponent<GateHandler>().trigger.gameObject.GetComponent<Animation>() != null)
                    gate2.GetComponent<GateHandler>().trigger.gameObject.GetComponent<Animation>().Rewind();
                gate2.GetComponent<GateHandler>().CloseDoor();
                GameObject trigger2 = GameObject.FindGameObjectWithTag("PillarTrigger");
                trigger2.GetComponent<PillarTrigger>().activate = false;
                GameObject[] pillars = GameObject.FindGameObjectsWithTag("FallingPillar");
                for (int i = 0; i < pillars.Length; i++)
                {
                    pillars[i].GetComponent<Animation>().Rewind();
                }
                mainArtifacts[0].SetActive(true);
                mainArtifacts[1].SetActive(true);
                break;
            default:
                break;
        }

        return 0;
    }
}
