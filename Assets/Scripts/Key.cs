using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public static Action<GameObject> playerPickedUpKey;

    private void OnMouseDown()
    {
        playerPickedUpKey?.Invoke(this.gameObject);
    }
}
