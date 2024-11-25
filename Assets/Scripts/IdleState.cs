using System;
using System.Collections;
using UnityEngine;

public class IdleState : State
{
    public bool isPlayerInSight;
    public ChasingState chasingState;
    private Coroutine detectPlayerCoroutine;
    
    
    
    public override State RunCurrentState()
    {
        if (isPlayerInSight && !enemyScript.isInLight)
        {
            return chasingState;
        }
        
        if ((transform.position - enemyScript.player.position).magnitude < 3)
        {
            isPlayerInSight = true;
        }
        else
        {
            isPlayerInSight = false;
        }
        
        return this;
    }

    private void Update()
    {
        
    
    }
    
    public override void OnStateEnter()
    {

    }


}
