using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected EnemyScript enemyScript;
    
    public void SetEnemyScript(EnemyScript enemyScript)
    {
        this.enemyScript = enemyScript;
    }

    public abstract State RunCurrentState();
    public abstract void OnStateEnter();
}
