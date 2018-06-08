using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class MummyStateMachine : MonoBehaviour
{
    public enum States
    {
        Asleep,
        WakingUp,
        Wandering,
        Chasing,
        Attacking,
        Dying
    }

    public States currentState;
    public bool startAwake;

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private Behaviour[] states;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        states = new Behaviour[6];
        states[(int)States.Asleep] = GetComponent<MummySleeping>();
        states[(int)States.WakingUp] = GetComponent<MummyWakingUp>();
        states[(int)States.Wandering] = GetComponent<MummyWandering>();
        states[(int)States.Chasing] = GetComponent<MummyChasing>();
        states[(int)States.Attacking] = GetComponent<MummyAttacking>();
    }

    void Start()
    {
        UpdateCurrentState(ChooseStartStart());
    }

    /// <summary>
    /// Changes the state of the mummy while also updating the animator.
    /// All the states, besides the new state will be disabled.
    /// </summary>
    /// <param name="newState">The state the mummy will change to.</param>
    public void UpdateCurrentState(States newState)
    {
        currentState = newState;
        UpdateAnimator((int)newState);

        for (int i = 0; i < states.Length; i++)
        {
            if (states[i] != null)
            {
                if (i != (int)currentState)
                {
                    states[i].enabled = false;
                }
                else
                {
                    states[i].enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Updates the animator to the given state.
    /// </summary>
    /// <param name="newState">The new state of the mummy.</param>
    private void UpdateAnimator(int newState)
    {
        animator.SetInteger("State", newState);
    }

    /// <summary>
    /// Chooses the state to start at
    /// </summary>
    /// <returns>Start State</returns>
    private States ChooseStartStart()
    {
        States startstate;

        if (!startAwake)
            startstate = States.Asleep;
        else
            startstate = States.WakingUp;

        return startstate;
    }

    public UnityEngine.AI.NavMeshAgent Agent
    {
        get { return agent; }
    }

}
