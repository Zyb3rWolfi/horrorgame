using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State currentState;
    public EnemyScript enemyScript;
    
    
    
    private void Start()
    {
        currentState.SetEnemyScript(enemyScript);

    }

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }
    
    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();
        
        if (nextState != null)
        {
            SwitchToNewState(nextState);
        }
    }
    
    private void SwitchToNewState(State newState)
    {
        Debug.Log("Switching to new state");
        currentState = newState;
        currentState.OnStateEnter();
        currentState.SetEnemyScript(enemyScript);
    }
}
