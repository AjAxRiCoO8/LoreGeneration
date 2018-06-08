using UnityEngine;
using System.Collections;

public class MummyChasing : MummyState
{
    private float[] lengthOfPathToPlayers;

    #region Built-in Methods
    private void Start()
    {
        stateMachine = GetComponent<MummyStateMachine>();
        variables = GetComponent<MummyVariables>();

        lengthOfPathToPlayers = new float[variables.Players.Length];
    }

    private void Update()
    {
        variables.CalculateDistanceToPlayers();
        MoveTowardsPlayer();
    }
    #endregion

    #region Player Related Methods
    /// <summary>
    /// This method takes the path to the closest player and sees if the path is reachable.
    /// If the path is reachable it will move towards the player, otherwise the state will change to the wandering state.
    /// If the player gets withing attack range the state will change to the attacking state.
    /// </summary>
    private void MoveTowardsPlayer()
    {
        GetShortestPath();

        if (variables.ClosestDistance > variables.AttackRange)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            stateMachine.Agent.CalculatePath(variables.Players[variables.ClosestPlayer].transform.position, path);
            if (path.status != UnityEngine.AI.NavMeshPathStatus.PathPartial && path.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                stateMachine.Agent.SetPath(path);
            }
            else
            {
                stateMachine.UpdateCurrentState(MummyStateMachine.States.Wandering);
            }

            stateMachine.Agent.speed = variables.ChasingSpeed;
        }
        else
        {
            if (variables.ClosestDistance != 0)
            {
                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                stateMachine.Agent.CalculatePath(variables.Players[variables.ClosestPlayer].transform.position, path);

                if (path.status != UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    stateMachine.UpdateCurrentState(MummyStateMachine.States.Attacking);
                }
                else
                {
                    stateMachine.UpdateCurrentState(MummyStateMachine.States.Wandering);
                }
            }
        }
    }
    #endregion

    #region Path Related Methods
    /// <summary>
    /// This method gets the total length of the given NavMeshPath
    /// </summary>
    /// <param name="path">the NavMeshPath to get the length from</param>
    /// <returns></returns>
    private float GetPathLength(UnityEngine.AI.NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return 0;

        float totalLength = 0.0f;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            totalLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return totalLength;
    }

    /// <summary>
    /// This method calculates the length of the path towards each player, 
    /// The player with the shortest path will get chased.
    /// If the closest player is outside of the aggro range the state of the mummy will change to the wandering state.
    /// </summary>
    private void GetShortestPath()
    {
        variables.ClosestDistance = variables.AggroRange;
        variables.ClosestPlayer = 0;

        for (int i = 0; i < lengthOfPathToPlayers.Length; i++)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            stateMachine.Agent.CalculatePath(variables.Players[i].transform.position, path);
            lengthOfPathToPlayers[i] = GetPathLength(path);

            if (lengthOfPathToPlayers[i] < variables.ClosestDistance && lengthOfPathToPlayers[i] != 0)
            {
                variables.ClosestDistance = lengthOfPathToPlayers[i];
                variables.ClosestPlayer = i;
            }
        }

        if (variables.ClosestDistance.Equals(variables.AggroRange))
        {
            stateMachine.UpdateCurrentState(MummyStateMachine.States.Wandering);
        }
    }
    #endregion
}
