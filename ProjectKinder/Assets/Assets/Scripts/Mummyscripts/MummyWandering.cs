using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class MummyWandering : MummyState
{
    [Tooltip("The time the mummy will wait before choosing a new waypoint to move towards.")]
    [Range(0, 2)] public float waitTime;
    private float timeWaited;
    
    private GameObject[] waypoints;
    private GameObject[] reachableWaypoints;
    private GameObject nextWaypoint;

    #region Built-in Methods
    private void Start()
    {
        timeWaited = 0;

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        reachableWaypoints = new GameObject[0];
        if (waypoints != null && waypoints.Length != 0)
        {
            FindReachableWaypoints();
        }
    }

    private void Update()
    {
        variables.CalculateDistanceToPlayers();
        MoveTowardsWaypoint();
    }
    #endregion

    #region Waypoint Related Methods
    /// <summary>
    /// This method checks for all of the waypoints if they're reachable.
    /// If that's the case the waypoints will be added to a list of the reachable waypoints, which will be used to choose a waypoint to move towards.
    /// </summary>
    private void FindReachableWaypoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            stateMachine.Agent.CalculatePath(waypoints[i].transform.position, path);
            if (path.status != UnityEngine.AI.NavMeshPathStatus.PathPartial && path.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                Array.Resize(ref reachableWaypoints, reachableWaypoints.Length + 1);
                reachableWaypoints[reachableWaypoints.Length - 1] = waypoints[i];
            }
        }
    }

    /// <summary>
    /// This method chooses a random waypoint out of a list of all the waypoints.
    /// </summary>
    private void ChooseNewWaypoint()
    {
        GameObject currentWaypoint = nextWaypoint;

        if (reachableWaypoints != null && reachableWaypoints.Length != 0)
        {
            while (nextWaypoint == currentWaypoint)
            {
                    nextWaypoint = reachableWaypoints[Random.Range(0, reachableWaypoints.Length)] as GameObject;
            }
        }
    }

    /// <summary>
    /// This method calculates a path for the mummy towards a waypoint.
    /// If the agent reaches the waypoint, the agent will wait for a short time and then move towards a newly chosen waypoint.
    /// </summary>
    private void MoveTowardsWaypoint()
    {
        if (nextWaypoint != null)
        {
            stateMachine.Agent.speed = variables.MovementSpeed;

            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            stateMachine.Agent.CalculatePath(nextWaypoint.transform.position, path);
            if (path.status != UnityEngine.AI.NavMeshPathStatus.PathPartial && path.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                stateMachine.Agent.SetPath(path);
            }
            else
            {
                ChooseNewWaypoint();
            }
        }

        if (ReachedWaypoint())
        {
            timeWaited += Time.deltaTime;
            if (timeWaited > waitTime)
            {
                timeWaited = waitTime;
                ChooseNewWaypoint();
            }
        }
    }

    /// <summary>
    /// This method checks whether the agent has reached the waypoint.
    /// </summary>
    /// <returns>true if the waypoint has been reached, returns false otherwise </returns>
    private bool ReachedWaypoint()
    {
        if (!stateMachine.Agent.pathPending)
        {
            if (stateMachine.Agent.remainingDistance <= stateMachine.Agent.stoppingDistance)
            {
                if (!stateMachine.Agent.hasPath || stateMachine.Agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
}