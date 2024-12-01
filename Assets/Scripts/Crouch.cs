using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crouch : MonoBehaviour
{
    public static Action<bool> playerCrouch;
    [SerializeField] private float crouchHeight;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float smoothSpeed;
    private float defaultHeight;
    private float targetHeight;
    private bool isCourching = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultHeight = cameraTransform.localPosition.y;
        targetHeight = defaultHeight; // Initial target height is the default
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = cameraTransform.localPosition;
        newPosition.y = Mathf.Lerp(newPosition.y, targetHeight, Time.deltaTime * smoothSpeed);
        cameraTransform.localPosition = newPosition;
        
    }

    public void CrouchInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCourching = !isCourching;
            targetHeight = isCourching ? defaultHeight - crouchHeight : defaultHeight;
            playerCrouch?.Invoke(isCourching);
            
        }
        
    }
}
