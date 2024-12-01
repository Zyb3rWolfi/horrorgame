using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class KeyLogic : MonoBehaviour
{
    [SerializeField] private List<GameObject> allKeys = new List<GameObject>();
    [SerializeField] private int keyAmount;
    [SerializeField] private TextMeshProUGUI deathText;

    private void OnEnable()
    {
        Key.playerPickedUpKey += AddKey;
        Attack.KillPlayer += HandleDeath;
    }

    private void OnDisable()
    {
        Key.playerPickedUpKey -= AddKey;
        Attack.KillPlayer -= HandleDeath;
    }

    private void HandleDeath()
    {
        deathText.gameObject.SetActive(true);;
        Time.timeScale = 0;
    }

    private void AddKey(GameObject key)
    {
        allKeys.Add(key);
        Destroy(key);
        if (keyAmount == allKeys.Count)
        {
            print("Level won!!");
        }
    }
}
