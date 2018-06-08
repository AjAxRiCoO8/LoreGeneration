using UnityEngine;
using System.Collections;

public class MummyVariables : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float chasingSpeed;
    [SerializeField]
    private float aggroRange;
    [SerializeField]
    private float attackRange;

    //Player related variables
    [SerializeField]
    private GameObject[] players;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private float[] distanceToPlayers;
    private float closestDistance;
    private int closestPlayer;

    private void Start()
    {
        LoadInPlayerData();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    /// <summary>
    /// This method calculates the distance to the players
    /// </summary>
    public void CalculateDistanceToPlayers()
    {
        if (Players != null && Players.Length > 0)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                DistanceToPlayers[i] = Vector3.Distance(Players[i].transform.position, transform.position);
                if (DistanceToPlayers[i] > AggroRange && DistanceToPlayers[i] != 0)
                {
                    GetComponent<MummyStateMachine>().UpdateCurrentState(MummyStateMachine.States.Wandering);
                    break;
                }
                else if (DistanceToPlayers[i] < AggroRange && DistanceToPlayers[i] > AttackRange)
                {
                    GetComponent<MummyStateMachine>().UpdateCurrentState(MummyStateMachine.States.Chasing);
                    break;
                }
            }
        }
    }

    public void LoadInPlayerData()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        distanceToPlayers = new float[players.Length];
    }

    #region Global Getters
    /// <summary>
    /// Gets the damage of the mummy.
    /// </summary>
    public int Damage
    {
        get { return damage; }
    }

    /// <summary>
    /// Gets the movement speed of the mummy.
    /// </summary>
    public float MovementSpeed
    {
        get { return movementSpeed; }
    }

    /// <summary>
    /// Gets the chasing speed of the mummy.
    /// </summary>
    public float ChasingSpeed
    {
        get { return chasingSpeed; }
    }

    /// <summary>
    /// Gets the range which the mummy will start chasing players
    /// </summary>
    public float AggroRange
    {
        get { return aggroRange; }
    }

    /// <summary>
    /// Gets the range which the mummy will start attacking players.
    /// </summary>
    public float AttackRange
    {
        get { return attackRange; }
    }

    public Vector3 StartingPosition
    {
        get { return startingPosition; }
    }

    public Quaternion StartingRotation
    {
        get { return startingRotation; }
    }
    #endregion  

    #region Player Related Getters & Setters
    /// <summary>
    /// Gets the GameObjects from all players.
    /// </summary>
    public GameObject[] Players
    {
        get { return players; }
    }

    /// <summary>
    /// Gets and sets the distance to the players. 
    /// The distance should only be set in the wandering and chasing state.
    /// </summary>
    public float[] DistanceToPlayers
    {
        get { return distanceToPlayers; }
        set { distanceToPlayers = value; }
    }

    /// <summary>
    /// Gets and sets the closest distance to the players.
    /// The closest distance should only be set in the chasing state.
    /// </summary>
    public float ClosestDistance
    {
        get { return closestDistance; }
        set { closestDistance = value; }
    }

    /// <summary>
    /// Gets and sets the player closest to the mummy.
    /// The closest player should only be set in the chasing state.
    /// </summary>
    public int ClosestPlayer
    {
        get { return closestPlayer; }
        set { closestPlayer = value; }
    }
    #endregion
}
