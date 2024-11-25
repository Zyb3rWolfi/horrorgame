using System;
using System.Collections;
using UnityEngine;

public class ChasingState : State
{
    public AttackState attackState;
    public IdleState idleState;
    public bool isPlayerInSight;
    private float tension = 0f;
    private float tensionIncreaseRate = 10f;
    private float maxTension = 100f;
    private float followSpeed = 5f;
    private Coroutine followPlayerCoroutine = null;

    private void OnEnable()
    {
        EnemyScript.EnemyInLight += RevertToIdleState;
    }

    private void OnDisable()
    {
        EnemyScript.EnemyInLight -= RevertToIdleState;
    }
    
    void RevertToIdleState()
    {
        
        tension = 0;
        Debug.Log("Reverting to idle state");
        if (followPlayerCoroutine != null)
        {
            StopCoroutine(followPlayerCoroutine);
            followPlayerCoroutine = null;
        }
    }
    
    public override State RunCurrentState()
    {
        TeleportBehindPlayer();
        if (followPlayerCoroutine == null)
        {
            followPlayerCoroutine = StartCoroutine(FollowPlayer());
        }
        if (tension < maxTension)
        {
            tension += tensionIncreaseRate * Time.deltaTime;
            if (enemyScript.isInLight)
            {
                TeleportAndHide();
                tension = 0;
                return idleState;
            }
            Debug.Log("Running Chasing State" + tension);
        }
        else
        {
            return attackState;
        }

        return this;
    }
    
    public override void OnStateEnter()
    {
    }
    
    void TeleportAndHide()
    {
        Vector3 playerPosition = enemyScript.player.position;
        Vector3 offset = new Vector3(1, 0, 5);
        Vector3 newPosition = playerPosition + offset;
        this.enemyScript.transform.position = newPosition;
    }
    void TeleportBehindPlayer()
    {
        Vector3 playerPosition = enemyScript.player.position;
        Vector3 offset = new Vector3(1, 0, 5);
        Vector3 newPosition = playerPosition + offset;
        this.enemyScript.transform.position = newPosition;
    }
    

    private IEnumerator FollowPlayer()
    {
        while (tension < maxTension)
        {
            Vector3 direction = (enemyScript.player.position - enemyScript.transform.position).normalized;
            enemyScript.agent.Move(direction * followSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
