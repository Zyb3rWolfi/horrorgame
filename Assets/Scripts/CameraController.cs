using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float mouseSense = 100f;

    private float xRotation = 0f;
    private Vector2 input;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        float mouseX = input.x * mouseSense * Time.deltaTime;
        float mouseY = input.y * mouseSense * Time.deltaTime;

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
