using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleportation_Control : MonoBehaviour
{
    [SerializeField] InputActionReference Main_Button_Input;
    public Transform playerTransform;
    public Transform controllerTransform;
    public GameObject teleportMarker;
    private VRInputActions input;



    private void Awake()
    {
        input = GetComponent<VRInputActions>();
    }
    void OnEnable()
    {
        Main_Button_Input.action.performed += TeleportButtonPressed;
        Main_Button_Input.action.canceled += TeleportButtonReleased;
    }

    void OnDisable()
    {
        Main_Button_Input.action.performed -= TeleportButtonPressed;
        Main_Button_Input.action.canceled -= TeleportButtonReleased;
    }

    void TeleportButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Teleport button pressed!");

        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out RaycastHit hit))
        {
            Debug.DrawLine(controllerTransform.position, teleportMarker.transform.position, Color.green, 0.5f);

            teleportMarker.SetActive(true);
            teleportMarker.transform.position = hit.point;
        }
    }

    void TeleportButtonReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Teleport button released!");

        if (teleportMarker.activeSelf)
        {
            playerTransform.position = teleportMarker.transform.position;
            teleportMarker.SetActive(false);
        }
    }
}
