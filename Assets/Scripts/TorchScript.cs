using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TorchScript : MonoBehaviour
{
    [SerializeField] private Light torchLight;
    [SerializeField] private bool isOn = false;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float timeInterval;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float followSpeed;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int charge;
    
    private Vector3 currentVelocity;  // Used to smooth movement using SmoothDamp
    private Vector3 targetPosition; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charge = 100;
        StartCoroutine(ManageCharge());
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = _cameraTransform.position + _cameraTransform.TransformDirection(_offset);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, timeInterval);

        transform.rotation = Quaternion.Lerp(transform.rotation, _cameraTransform.rotation, followSpeed * Time.deltaTime);
    }

    public void ManageTorchInput(InputAction.CallbackContext context)
    {
        if (context.performed && charge != 0)
        {
            isOn = !isOn;
            torchLight.enabled = isOn;
        }
    }

    private IEnumerator ManageCharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeInterval);
            switch (isOn)
            {
                case true:
                    if (charge == 0)
                    {
                        isOn = !isOn;
                        torchLight.enabled = isOn;
                        break;
                    }
                    charge -= 1;
                    break;
                case false:
                    if (charge == 100)
                    {
                        break;
                    }
                    charge += 1;
                    break;
            }
            _text.text = "Charge: " + charge;
            
        }
    }
}
