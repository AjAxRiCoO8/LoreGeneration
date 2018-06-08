using UnityEngine;
using System.Collections;

public class MummySleeping : MonoBehaviour {

	public void WakeUp()
    {
        if (GetComponent<MummyStateMachine>().currentState == MummyStateMachine.States.Asleep)
            GetComponent<MummyStateMachine>().UpdateCurrentState(MummyStateMachine.States.WakingUp);
    }
}
