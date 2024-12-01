using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float crouchSpeed;
    [Header("Player Checks")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isCrouching;
    [Header("Camera Settings")]
    [SerializeField] private Transform camera;
    [SerializeField] private float smoothSpeed;
    
    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 _currentInput;  
    
    public float mouseSense = 100f;

    private float xRotation = 0f;
    private Vector2 input;

    private void OnEnable()
    {
        Crouch.playerCrouch += ManageCrouchSpeed;
    }

    private void OnDisable()
    {
        Crouch.playerCrouch -= ManageCrouchSpeed;
    }

    private void ManageCrouchSpeed(bool isCrouching)
    {
        if (isCrouching)
        {
            moveSpeed -= crouchSpeed;
        }
        else
        {
            moveSpeed += crouchSpeed;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.MovePosition(transform.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
        
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        
        moveInput = context.ReadValue<Vector2>();
        
    }

    private void KillPlayer()
    {
        Destroy(this.gameObject);
    }

    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGroundedCheck())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private bool IsGroundedCheck()
    {
            
         return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    
    /*
    public void HandleCamera(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        
        float mouseX = input.x * mouseSense * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        
        float mouseY = input.y * mouseSense * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
    }*/
    

}
