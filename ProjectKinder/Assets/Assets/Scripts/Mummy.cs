using System.Collections;
using UnityEngine;

/// <summary>
/// This class handles all logic behind the behaviour of the mummy. 
/// 
/// @author Kay van der Lans
/// </summary>
public class Mummy : MonoBehaviour
{
    public enum States
    {
        Idle,
        Wandering,
        Chasing,
        Attacking
    }

    [SerializeField] private States currentState;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackTimer;
    [SerializeField] private float aggroRange;

    private GameObject[] waypoints;
    private GameObject[] targets;
    private bool[] targetInRange;
    private float[] distances;
    private float[] lengthOfPaths;

    private GameObject nextWaypoint;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    
    private bool canAttack;
    private bool wait;
    private float waypointWaitTime;
    private float closestDistance;
    private int closestTarget;

    private void Awake()
    {
        waypointWaitTime = 2.0f;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = movementSpeed;

        canAttack = true;
        wait = true;

        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(GetPlayers(2));

        ChangeState(States.Wandering);

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        ChooseNewWaypoint();
    }
 
    private void Update()
    {

        if (currentState.Equals(States.Wandering)) { MoveTowardsWaypoint(); }
        else if (currentState.Equals(States.Chasing)) { MoveTowardsPlayer(); }
        else { agent.SetDestination(transform.position); }

        if (currentState.Equals(States.Attacking))
        {
            if (canAttack)
            {
                Attack(targets[closestTarget], attackDamage);
                ChangeState(States.Chasing);
            }
            else
            {
                if (wait)
                {
                    StartCoroutine(Wait(attackTimer));
                    wait = false;
                }
            }
        }
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

        canAttack = !canAttack;
        ChangeState(States.Chasing);
        wait = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// This method changes the current state of the mummy to another one. The animation state will be changed accordingly.
    /// </summary>
    /// <param name="state">The new state of the mummy</param>
    private void ChangeState(States state)
    {
        currentState = state;

        if (currentState.Equals(States.Wandering) || currentState.Equals(States.Chasing))
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
        }
    }

    /// <summary>
    /// Attacks the target player with the given damage
    /// </summary>
    /// <param name="target">the target to attack</param>
    /// <param name="damage">the damage of the attack</param>
    private void Attack(GameObject target, int damage)
    {
        HealthController playerHealth = target.GetComponent<HealthController>();

        if (Vector3.Distance(transform.position, target.transform.position) < attackRange + 2)
        {
            playerHealth.TakeDamage(damage);
        }

        canAttack = false;
    }

    /// <summary>
    /// This method chooses a random waypoint out of a list of all the waypoints.
    /// </summary>
    private void ChooseNewWaypoint()
    {
        GameObject currentWaypoint = nextWaypoint;
        ArrayList possibleWaypoints = new ArrayList();

        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                agent.CalculatePath(waypoints[i].transform.position, path);
                if (path.status != UnityEngine.AI.NavMeshPathStatus.PathPartial && path.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                {
                    possibleWaypoints.Add(waypoints[i]);
                }
            }

            while (nextWaypoint == currentWaypoint)
            {
                nextWaypoint = possibleWaypoints[Random.Range(0, possibleWaypoints.Count)] as GameObject;
            }
        }
    }

    /// <summary>
    /// This method sets the destination of the mummy to the next waypoint.
    /// If the agent reaches the waypoint, a new waypoint will be chosen for the mummy to move towards.
    /// </summary>
    private void MoveTowardsWaypoint()
    {
        if (nextWaypoint != null)
        {
            agent.speed = movementSpeed;

            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            agent.CalculatePath(nextWaypoint.transform.position, path);
            if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial || path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                ChooseNewWaypoint();
            }
            else
            {
                agent.SetPath(path);
            }

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        anim.SetInteger("State", 0);

                        waypointWaitTime -= Time.deltaTime;
                        if (waypointWaitTime < 0)
                        {
                            waypointWaitTime = 2.0f;
                            ChooseNewWaypoint();
                            anim.SetInteger("State", 1);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Move towards the closest player
    /// </summary>
    private void MoveTowardsPlayer()
    {
        if (closestDistance > attackRange)
        {
            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            agent.CalculatePath(targets[closestTarget].transform.position, path);
            if (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial || path.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                ChangeState(States.Wandering);
            }
            else
            {
                agent.SetPath(path);
            }

            agent.speed = movementSpeed + ((aggroRange / 4.5f) / closestDistance);
        }
        else
        {
            if (closestDistance != 0)
            {
                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                agent.CalculatePath(targets[closestTarget].transform.position, path);
                
                if (path.status != UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    ChangeState(States.Attacking);
                }
                else
                {
                    ChangeState(States.Wandering);
                }
            }
        }
    }

    /// <summary>
    /// If there is at least one target close to the mummy, the shortest path will get calculated.
    /// This method is used to find the closest target to the mummy.
    /// </summary>
    private void GetShortestPath()
    {
        closestDistance = aggroRange;
        closestTarget = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targetInRange[i])
            {
                UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
                agent.CalculatePath(targets[i].transform.position, path);
                lengthOfPaths[i] = GetPathLength(path);

                if (lengthOfPaths[i] < closestDistance && lengthOfPaths[i] != 0)
                {
                    closestDistance = lengthOfPaths[i];
                    closestTarget = i;
                }
            }
        }

        if (closestDistance.Equals(aggroRange))
        {
            ChangeState(States.Wandering);
        }
        else if (currentState.Equals(States.Wandering) && !closestDistance.Equals(aggroRange))
        {
            ChangeState(States.Chasing);
        }
    }

    /// <summary>
    /// This method calculates the distance to the given player object
    /// </summary>
    /// <param name="player">The GameObject to get the distance from</param>
    /// <returns>The distance towards the give player</returns>
    private float GetDistanceToPlayer(GameObject player)
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

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
    /// This interface instantiates an amount of arrays based on the amount of players that joined
    /// </summary>
    /// <param name="waitTime">the time to wait before the players get added</param>
    /// <returns></returns>
    private IEnumerator GetPlayers(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        targets = GameObject.FindGameObjectsWithTag("Player");
        targetInRange = new bool[targets.Length];
        distances = new float[targets.Length];
        lengthOfPaths = new float[targets.Length];

        StartCoroutine(GetPlayersInRange(0.5f));
    }

    /// <summary>
    /// This interface gets the players that are in the aggro range.
    /// If there is one or more players within the aggro range, the <see cref="GetShortestPath"/>
    /// will be called. 
    /// </summary>
    /// <param name="nextCheck">This parameter decides how many seconds it takes to do the next check</param>
    /// <returns></returns>
    private IEnumerator GetPlayersInRange(float nextCheck)
    {
        bool targetClose = false;

        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                distances[i] = GetDistanceToPlayer(targets[i]);

                if (distances[i] < aggroRange)
                {
                    targetInRange[i] = true;
                } 
                else
                {
                    targetInRange[i] = false;
                }
                
                if (targetInRange[i])
                {
                    targetClose = true;
                }
                else
                {
                    closestDistance = aggroRange;
                }
            }
        }

        yield return new WaitForSeconds(nextCheck);

        StartCoroutine(GetPlayersInRange(nextCheck));

        if (targetClose)
        {
            GetShortestPath();
        }
    }
}
