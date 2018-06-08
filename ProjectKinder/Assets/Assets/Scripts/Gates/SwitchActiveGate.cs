using UnityEngine;
using System.Collections;

public class SwitchActiveGate : MonoBehaviour {

    public GameObject plateForward;
    public GameObject plateBackward;

    public SwitchableGate[] gates;

    public int activeOne;

    public bool plateDownForward, plateDownBackward;

    // Use this for initialization
    void Start () {
        gates[activeOne].doorActive = true;
    }
	
	// Update is called once per frame
	void Update () {
        ChangeActive();
    }

    public void ChangeActive()
    {
        if (plateForward.gameObject.GetComponent<PressurePlate>().triggered && !plateDownForward)
        {
            plateDownForward = true;
            activeGateSwitch(1);

        }

        if (plateBackward.gameObject.GetComponent<PressurePlate>().triggered && !plateDownBackward)
        {
            plateDownBackward = true;
            activeGateSwitch(-1);
        }

        if (!plateForward.gameObject.GetComponent<PressurePlate>().triggered && plateDownForward)
        {
            plateDownForward = false;
        }

        if (!plateBackward.gameObject.GetComponent<PressurePlate>().triggered && plateDownBackward)
        {
            plateDownBackward = false;
        }

    }

    /// <summary>
    /// set the next or the previous gate on active. 
    /// </summary>
    /// <param name="plusOrMin"> +1 or -1 depending if you want to move forward or backwards in the array </param>
    public void activeGateSwitch(int plusOrMin)
    {
        if (activeOne + plusOrMin < gates.Length && activeOne + plusOrMin >= 0)
        {
            gates[activeOne].doorActive = false;
            activeOne += plusOrMin;
            Debug.Log(activeOne);
            gates[activeOne].doorActive = true;

        }
    }
}
