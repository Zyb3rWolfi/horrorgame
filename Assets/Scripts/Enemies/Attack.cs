using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public static Action KillPlayer;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            KillPlayer?.Invoke();
        }
    }
}
