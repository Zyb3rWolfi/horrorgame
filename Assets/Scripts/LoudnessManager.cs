using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoudnessManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private SphereCollider soundCollider;
    [SerializeField] private float walkingRadius;
    private int decibals = 0;

    public static Action<bool> MakeNoiseAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = "Loudness: " + decibals;
        soundCollider.radius = walkingRadius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeNoise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MakeNoiseAction?.Invoke(true);
        }
        else
        {
            MakeNoiseAction?.Invoke(false);
        }
    }


}
