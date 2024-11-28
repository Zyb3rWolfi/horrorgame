using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSense = 100f;
    private Vector2 currentInput;
    private Vector2 velocity = Vector2.zero;
    public float smoothSpeed = 0.1f;  // Smooth speed for camera rotation
    // Used for smoothing input

    private float xRotation = 0f;
    private Vector2 input;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        currentInput.x = Mathf.SmoothDamp(currentInput.x, input.x, ref velocity.x, smoothSpeed);
        currentInput.y = Mathf.SmoothDamp(currentInput.y, input.y, ref velocity.y, smoothSpeed);
        
        float mouseX = currentInput.x * mouseSense * Time.deltaTime;
        float mouseY = currentInput.y * mouseSense * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        player.Rotate(Vector3.up * mouseX);

    }

    public void LookInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
