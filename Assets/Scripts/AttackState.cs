using UnityEngine;

public class AttackState : State
{
    public override State RunCurrentState()
    {
        Debug.Log("Running Attack State");
        AttackPlayer();
        return this;
    }
    
    public override void OnStateEnter()
    {
    }

    void AttackPlayer()
    {
        // Move towards the player
        Vector3 playerPosition = enemyScript.player.position;
        Vector3 direction = (playerPosition - enemyScript.transform.position).normalized;
        enemyScript.agent.Move(direction * 5 * Time.deltaTime);
        // Attack logic below
        
        if (Vector3.Distance(enemyScript.transform.position, playerPosition) < 1)
        {
            Debug.Log("Attacking player");
        }
    }
}
