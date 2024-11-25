using UnityEngine;
using UnityEngine.InputSystem;

public class TorchScript : MonoBehaviour
{
    [SerializeField] private Light torchLight;
    [SerializeField] private bool isOn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ManageTorchInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isOn = !isOn;
            torchLight.enabled = isOn;
        }
    }
}
