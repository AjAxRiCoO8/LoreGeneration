using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MummyStateMachine))]
[RequireComponent(typeof(MummyVariables))]
public class MummyState : MonoBehaviour
{
    [HideInInspector] public MummyStateMachine stateMachine;
    [HideInInspector] public MummyVariables variables;

    private void Awake()
    {
        stateMachine = GetComponent<MummyStateMachine>();
        variables = GetComponent<MummyVariables>();
    }
}
