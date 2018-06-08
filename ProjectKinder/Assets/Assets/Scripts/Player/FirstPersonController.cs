using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonController : NetworkBehaviour
{
    [Header("Stamina")]
    [SerializeField] private Image staminaImage;
    [SerializeField] private float staminaDrainPerSecond;
    [SerializeField] private float staminaRegainSpeed;
    [SerializeField] private float leastToRun;
    private float staminaValue;

    [Header("Movement")]
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isStuck;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [Space]
    [SerializeField] private float stepInterval;

    private Vector3 moveDirection = Vector3.zero;
    private float stepCycle;
    private float nextStep;
    private float runStepLengthen;
    private bool canSprint;

    [Header("Jumping")]
    [SerializeField] private float jumpSpeed;

    private bool previouslyGrounded;
    private bool jump;
    private bool jumping;

    [Header("Gravity")] 
    [SerializeField] private float stickToGroundForce;
    [SerializeField] private float gravityMultiplier;

    [Header("Camera")]
    [SerializeField] private MouseLook mouseLook;

    private Camera theCamera;
    private Vector3 originalCameraPosition;

    [Header("Sounds")] 
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;

    [Header("PlayerModel")]
    [SerializeField] private GameObject spLeftHand;
    [SerializeField] private GameObject spRightHand;
    [SerializeField] private GameObject mpLeftHand;
    [SerializeField] private GameObject mpRightHand;


    private AudioSource audioSource;
    private AudioSource pantingSound;
    private CharacterController characterController;
    private CollisionFlags collisionFlags;
    private Vector2 input;
    private bool isPanting;

    #region Built-in Methods
    private void Start()
    {

        //If the player is not a local player, disable the camera and audiolistener.
        if (!isLocalPlayer)
        {
            transform.GetComponentInChildren<Camera>().enabled = false;
            transform.GetComponentInChildren<AudioListener>().enabled = false;

            // disable player hands that only client player should see
            spLeftHand.SetActive(false);
            spRightHand.SetActive(false);
            return;
        }

        //disable the meshrenderer and glasses so you don't see yourself.
        transform.Find("Body").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("Body").Find("Glasses").GetComponent<MeshRenderer>().enabled = false;
        transform.Find("Body").Find("Cowboy Hat versie 2").Find("default").GetComponent<MeshRenderer>().enabled = false;

        //making parts of the torch invisible
        transform.Find("Body").Find("MultiplayerHands").
                  Find("Multiplayer hand Right").Find("MultiplayerTorch").
                  GetComponent<MeshRenderer>().enabled = false;
        transform.Find("Body").Find("MultiplayerHands").
                  Find("Multiplayer hand Right").Find("default").
                  GetComponent<MeshRenderer>().enabled = false;
        transform.Find("Body").Find("MultiplayerHands").
                  Find("Multiplayer hand Left").Find("default").
                  GetComponent<MeshRenderer>().enabled = false;

        //Disable multiplayer hands that only other player should see
        mpLeftHand.SetActive(false);
        mpRightHand.SetActive(false);

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        //Initialize the camera settings.
        theCamera = Camera.main;
        originalCameraPosition = theCamera.transform.localPosition;
        mouseLook.Init(transform, theCamera.transform);

        //Initialize private variables for handling footstep sounds.
        stepCycle = 0.0f;
        nextStep = stepCycle / 2.0f;
        runStepLengthen = 0f;

        //Initialize the remaining private variables.
        jumping = false;
        canSprint = true;
        staminaValue = 100.0f;

        pantingSound = transform.Find("SoundEffects").GetComponent<SoundEffects>().panting;
        isPanting = false;

    }
	
	private void Update()
    {

        //If the player is not a local player, don't update the player using this update cycle.
	    if (!isLocalPlayer)
        {
            return;
        }

        RotateView();

        //If the player is not jumping, check whether the player presses the jump button.
        if (!jump)
        {
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        //If the player was not on the ground yet (because of jumping), and he is grounded right now
        //Play the landing sound and dont move down anymore.
        if (!previouslyGrounded && characterController.isGrounded)
        {
            PlayLandingSound();
            moveDirection.y = 0f;
            jumping = false;
        }

        //While the player is grounded keep the vertical movement direction at 0.
        if (!characterController.isGrounded && !jumping && previouslyGrounded)
        {
            moveDirection.y = 0f;
        }

        //Set the previously gorunded variable to the current grounded variable for the next update cycle.
        previouslyGrounded = characterController.isGrounded;

        if (Input.GetKeyDown(KeyCode.P))
        {
            mouseLook.SetCursorLock(!mouseLook.lockCursor);
            isStuck = !mouseLook.lockCursor;
            
        }
    }

    private void FixedUpdate()
    {
        //If the player is not a local player, dont update the player using this update cycle.
        if (!isLocalPlayer)
        {
            return;
        }

        //Make a speed variable, and give it the value that comes out of the GetInput method.
        float speed;
        GetInput(out speed);

        //Move along the camera by multiplying the y input (-1, 0 or 1) to the transform.forward property.
        //Does the same for moving left and right, by multiplying the x input to the transform.right property.
        Vector3 desiredMove = transform.forward * input.y + transform.right * input.x;

        //If the player is stuck, set the move direction to 0.
        if (isStuck)
        {
            moveDirection.x = 0;
            moveDirection.y = 0;
        }
        //If the player is not stuck, 
        //take the desiredMove variable from earlier and multiply it by the speed variable to get the move direction of the player.
        else
        {
            moveDirection.x = desiredMove.x * speed;
            moveDirection.z = desiredMove.z * speed;
        }

        //If the player is grounded stick to the ground.
        if (characterController.isGrounded)
        {
            moveDirection.y = -stickToGroundForce;

            //If the player presses the jump button, move upwards by the jumpSpeed variable and play a jump sound.
            if (jump)
            {
                moveDirection.y = jumpSpeed;
                PlayJumpSound();
                jump = false;
                jumping = true;
            }
        }
        //If the player is not grounded, move downwards by the gravity multiplied by the gravityMultiplier variable.
        else
        {
            moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
        }

        //Checks for the collision flags when the player moves
        collisionFlags = characterController.Move(moveDirection * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        mouseLook.UpdateCursorLock();

        //If the player is moving check if the player is sprinting or walking,
        //If the player is sprinting drain stamina, if the player isn't sprinting regenerate stamina.
        if (input.x != 0 || input.y != 0)
        {
            staminaValue += (isWalking ? staminaRegainSpeed : -1) * Time.deltaTime * staminaDrainPerSecond;
        }
        //If the player isn't moving regenerate stamina.
        else
        {
            staminaValue += staminaRegainSpeed * Time.deltaTime * staminaDrainPerSecond;
        }

        //If the player can sprint and the stamina value is less or equal to 0,
        //The player won't be able to sprint anymore. The stamina can't be lower than 0.
        if (staminaValue <= 0 && canSprint)
        {
            staminaValue = 0;
            canSprint = false;
        }
        //The stamina can't be higher than 100.
        else if (staminaValue > 100) { staminaValue = 100; }

        //If the player can't sprint && the stamina is higher than 50,
        //The player will be able to sprint again.
        if (!canSprint && staminaValue > leastToRun) { canSprint = true; }

        //Set the fill amount of the stamina image to the amount of stamina the player has.
        staminaImage.fillAmount = staminaValue / 100;
    }
    
    public void DisableCursorLock()
    {
        mouseLook.SetCursorLock(true);
        isStuck = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        //dont move the rigidbody if the character is on top of it
        if (collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
    #endregion

    #region Input related methods
    /// <summary>
    /// This method gets the horizontal and vertical input from the player. 
    /// If the player is holding the shift the output will be the running speed, else it will be the walking speed.
    /// </summary>
    /// <param name="speed">the speed of the player</param>
    private void GetInput(out float speed)
    {
        //gets the horizontal and vertical value of the movement input (-1, 0, 1)
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = isWalking;

#if !MOBILE_INPUT
        //If the player can sprint chck whether the player is pressing the shift button.
        if (canSprint)
        {
            isWalking = !Input.GetKey(KeyCode.LeftShift);
        }
        //If the player can't sprint, keep the isWalking variable at true
        else
        {
            isWalking = true;
        }
#endif

        //The speed will be set to walk speed if the player is walking, otherwise the speed will be set to th run speed.
        speed = isWalking ? walkSpeed : runSpeed;

        // checks if the player is out of breath and plays the pantingsound, if the player got his stamina back the panting sound stops.
        if (staminaValue == 100)
        {
            PlayPantingSound(false);
        }
        else if (staminaValue < 1)
        {
            PlayPantingSound(true);
        }



        //If the player is walking backwards, the vertical value will be lower -> the player will move slower when he's going backwards.
        float newVertical = (vertical > 0) ? vertical : vertical * 3 / 5;
        input = new Vector2(horizontal, newVertical);

        //Normalize the input if the squared length is higher than 1
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }
    }
    #endregion

    #region Camera related methods
    /// <summary>
    /// Calls the <see cref="MouseLook.LookRotation(Transform, Transform)"/> method from the <see cref="MouseLook"/> class.
    /// </summary>
    private void RotateView()
    {
        mouseLook.LookRotation(transform, theCamera.transform);
    }

    /// <summary>
    /// Updates the camera position according to the speed the player moves with, so it stays at the same position within the parent.
    /// </summary>
    /// <param name="speed">The speed the player moves with</param>
    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
        {
            newCameraPosition = theCamera.transform.localPosition;
        }
        else
        {
            newCameraPosition = theCamera.transform.localPosition;
            newCameraPosition.y = originalCameraPosition.y;
        }
        theCamera.transform.localPosition = newCameraPosition;
    }
    #endregion

    #region Sound related methods
    /// <summary>
    /// This method plays a sound when the player jumps
    /// </summary>
    private void PlayJumpSound()
    {
        //Only play the sound if this is a local player.
        if (!isLocalPlayer)
        {
            return;
        }

        audioSource.clip = jumpSound;
        audioSource.Play();
    }

    /// <summary>
    /// This method plays a sound when the player lands.
    /// </summary>
    private void PlayLandingSound()
    {
        //Only play the sound if this is a local player.
        if (!isLocalPlayer)
        {
            return;
        }

        audioSource.clip = landSound;
        audioSource.Play();
        nextStep = stepCycle + .5f;
    }


    private void PlayPantingSound(bool play)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        pantingSound.loop = pantingSound;

        if (play && !isPanting)
        {
            pantingSound.Play();
            isPanting = true;
        }
        else if (!play && isPanting)
        {
            pantingSound.Stop();
            isPanting = false;
        }



    }



    /// <summary>
    /// This method progresses the <see cref="stepCycle"/> according to the <paramref name="speed"/> of the player.
    /// The stepcycle handles whether footstep sounds should be played or not.
    /// </summary>
    /// <param name="speed"></param>
    private void ProgressStepCycle(float speed)
    {
        if (characterController.velocity.sqrMagnitude > 0 && (input.x != 0 || input.y != 0))
        {
            stepCycle += (characterController.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen))) *
                            Time.fixedDeltaTime;
        }

        if (!(stepCycle > nextStep))
        {
            return;
        }

        nextStep = stepCycle + stepInterval;

        PlayFootStepAudio();
    }


    /// <summary>
    /// This method plays the audio of the footsteps when the player walks
    /// </summary>
    private void PlayFootStepAudio()
    {
        //Only play the sound if this is a local player
        if (!isLocalPlayer)
        {
            return;
        }

        //If the player is not on the floor , dont play footstep sounds.
        if (!characterController.isGrounded)
        {
            return;
        }

        int n = Random.Range(1, footstepSounds.Length);
        audioSource.clip = footstepSounds[n];
        audioSource.PlayOneShot(audioSource.clip);

        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = audioSource.clip;
    }
    #endregion

    #region Getters & Setters
    public bool IsStuck
    {
        get { return isStuck; }
        set { isStuck = value; }
    }
    #endregion
}
