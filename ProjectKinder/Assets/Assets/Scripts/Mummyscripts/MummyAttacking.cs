using UnityEngine;
using System.Collections;

public class MummyAttacking : MummyState
{
    /// <summary>
    /// This method damages the closest player if the player is still within attacking range when this method gets called.
    /// </summary>
    /// <param name="damage">the damage of the attack</param>
    public void Attack()
    {
        stateMachine.Agent.speed = 0;
        HealthController playerHealth = variables.Players[variables.ClosestPlayer].GetComponent<HealthController>();

        if (playerHealth.currentHealth > 0)
        {
            transform.LookAt(variables.Players[variables.ClosestPlayer].transform);

            if (Vector3.Distance(transform.position, variables.Players[variables.ClosestPlayer].transform.position) < variables.AttackRange + 1)
            {
                playerHealth.TakeDamage(variables.Damage);
            }

            variables.CalculateDistanceToPlayers();
        }
    }
}
