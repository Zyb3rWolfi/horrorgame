using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform camera;
    private Rigidbody rb;
    private Vector2 moveInput;
    
    public float mouseSense = 100f;

    private float xRotation = 0f;
    private Vector2 input;

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
    
    public void HandleCamera(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        
        float mouseX = input.x * mouseSense * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        
        float mouseY = input.y * mouseSense * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
    }

}
