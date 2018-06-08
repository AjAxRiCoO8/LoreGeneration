using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PressToPickUp : NetworkBehaviour
{

	[Range (0.5f, 10f)]
	public float viewingDistance;
	public Text interactionText;

    public int treasureCount = 0;
	RaycastHit hit;
	float theDistance;
	Vector3 forward;

	changeShader changeShader;
	changeShader lastFrameChangeShader;

	DoorsToFreedom doorsToFreedom;

	private bool leftWallArtifact = false;
	private bool rightWallArtifact = false;

    public bool MainTreasure = true;


	//Timers
	//private float torchPickUppTimer = 10;

	// Use this for initialization
	void Start ()
	{
	}

    


    void Update ()
	{
		forward = transform.Find ("FirstPersonCharacter").transform.TransformDirection (Vector3.forward) * viewingDistance;
		Debug.DrawRay (transform.position, forward, Color.green);

		if (Physics.Raycast (transform.position, forward, out hit, viewingDistance)) {

			this.lastFrameChangeShader = changeShader;
			this.changeShader = hit.collider.gameObject.GetComponentInChildren<changeShader> ();
			ChangeShader (changeShader);

            Debug.Log("treasureCount " + treasureCount);
            //theDistance = hit.distance;
            //print (theDistance + " " + hit.collider.gameObject.tag);

            switch (hit.collider.tag) {
			case ("MainArtifactRight"):
                    //interactionText.text = "Press 'E' to pick up";
                    if (isLocalPlayer) {
                        if (Input.GetKeyDown(KeyCode.E) && treasureCount == 0)
                        {
                            if (!rightWallArtifact && !leftWallArtifact)
                            {
                                if (!isServer)
                                {
                                    CmdDestroyObject(hit.collider.gameObject);
                                }
                                else {
                                    RpcDestroyObject(hit.collider.gameObject);
                                }
                                rightWallArtifact = true;
                                GetComponent<UIController>().ActivateKey();
                                treasureCount++;
                                //interactionText.text = "";
                            }
                        }
				}
				break;

			case ("MainArtifactLeft"): 

				interactionText.enabled = true;
                    if (isLocalPlayer)
                    {
                        if (Input.GetKeyDown(KeyCode.E) && treasureCount == 0)
                        {
                            if (!leftWallArtifact && !rightWallArtifact)
                            {
                                if (!isServer)
                                {
                                    CmdDestroyObject(hit.collider.gameObject);
                                }
                                else {
                                    RpcDestroyObject(hit.collider.gameObject);
                                }
                                leftWallArtifact = true;
                                GetComponent<UIController>().ActivateKey();
                                treasureCount++;
                            }
                        }
				}
				break;                   

			case ("TorchFuel"):

				interactionText.text = "\"E\" To Charge";
				interactionText.enabled = true;

				if (Input.GetKey (KeyCode.E)) {
                        //torchPickUppTimer = 0;
					GameObject torchFuel = hit.collider.gameObject;
					TorchController torchController = GetComponentInParent<TorchController> ();
					torchController.RefuelTorch (torchFuel);


				}
				break;
			// The cases Rightlock and Leftlock are more like PressToPutDown
			case ("RightLock"):
				if (Input.GetKeyDown (KeyCode.E) && rightWallArtifact) {
					if (doorsToFreedom != null) {
						if (!isServer) {
							CmdCreateArtifact ("right");
						} else {
							RpcCreateArtifact ("right");
						}
						rightWallArtifact = false;
					}
				}
				break;

			case ("LeftLock"):
				if (Input.GetKeyDown (KeyCode.E) && leftWallArtifact) {
					if (doorsToFreedom != null) {
						if (!isServer) {
							CmdCreateArtifact ("left");
						} else {
							RpcCreateArtifact ("left");
						}
						leftWallArtifact = false;
					}
				}
				break;
			case ("Lever"):
				if (Input.GetKeyDown (KeyCode.E))
                    { 
                    LeverAnimation la = hit.collider.gameObject.GetComponent<LeverAnimation>();
                        if (la.isReusable || !la.isReusable && !la.triggered)
                        {
                            Debug.Log(".......");
                            if (!isServer)
                            {
                                CmdTrigger(hit.collider.gameObject);
                            }
                            else
                            {
                                RpcTrigger(hit.collider.gameObject);
                            }
                        }
				}
				break;
			case ("QTELever"):
				GameObject someObject = hit.collider.gameObject;
				if (Input.GetKey (someObject.GetComponent<QTE> ().startKey) && !someObject.GetComponent<QteLever>().StartQTE) {
					someObject.GetComponent<QteLever> ().PullLever (gameObject);
				}
				break;
            case ("HubButton"):
                    if (isLocalPlayer)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            HubButton hb = hit.collider.gameObject.GetComponent<HubButton>();
                            if (hb.canPress && !hb.isResetting)
                            {
                                if (!isServer)
                                {
                                    CmdButtonPressed(hit.collider.gameObject);
                                }
                                else
                                {
                                    RpcButtonPressed(hit.collider.gameObject);
                                }
                            }
                        }
                    }
                break;
			default:
				interactionText.enabled = false;

				if (lastFrameChangeShader != null)
					lastFrameChangeShader.StandartShader ();

				break;
			}
		} else {
			interactionText.enabled = false;

			if (lastFrameChangeShader != null)
				lastFrameChangeShader.StandartShader ();
		}
	}

    [Command]
    public void CmdButtonPressed(GameObject someObject)
    {
        RpcButtonPressed(someObject);
    }

    [ClientRpc]
    public void RpcButtonPressed(GameObject someObject)
    {
        someObject.GetComponent<HubButton>().ButtonPressed();
    }

	[Command]
	public void CmdTrigger (GameObject someObject)
	{
		RpcTrigger (someObject);
	}

	[ClientRpc]
	public void RpcTrigger (GameObject someObject)
	{
		someObject.GetComponent<LeverAnimation> ().Trigger ();
	}

	void ChangeShader (changeShader changeShader)
	{
		if (changeShader != null) {
			changeShader.Outline ();
		}
	}

	[Command]
	public void CmdCreateArtifact(string position)
	{
		RpcCreateArtifact (position);
	}

	[ClientRpc]
	public void RpcCreateArtifact(string position) {
		doorsToFreedom.GetComponent<DoorsToFreedom>().CreateArtifact(position);
	}

	[Command]
	void CmdDestroyObject (GameObject someObject)
	{
		RpcDestroyObject (someObject);
		someObject.SetActive (false);
		//Destroy (someObject);
	}

	[ClientRpc]
	void RpcDestroyObject (GameObject someObject)
	{
		someObject.SetActive (false);
		//Destroy (someObject);
	}
}
