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
    [SerializeField] private float crouchingRadius;
    private int decibals = 0;

    public static Action<bool> MakeNoiseAction;

    private void OnEnable()
    {
        Crouch.playerCrouch += ManageRadius;
    }

    private void OnDisable()
    {
        Crouch.playerCrouch -= ManageRadius;
    }

    private void ManageRadius(bool isCrouching)
    {
        switch (isCrouching)
        {
            case true:
                soundCollider.radius = crouchingRadius;
                break;
            case false:
                soundCollider.radius = walkingRadius;
                break;
                
        }
    }

    void Start()
    {
        text.text = "Loudness: " + decibals;
        soundCollider.radius = walkingRadius;
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
