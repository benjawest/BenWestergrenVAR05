using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleportation_Control : MonoBehaviour
{
    [SerializeField] InputActionReference Main_Button_Input;
    public GameObject teleportMarker;
    public float teleportDistance = 10f;

    public Transform vrCameraTransform;
    public Transform vrRHandsTransform;
    public Transform vrLHandsTransform;

    void OnEnable()
    {
        Main_Button_Input.action.Enable();
    }

    void OnDisable()
    {
        Main_Button_Input.action.Disable();
    }

    void Update()
    {
        if (Main_Button_Input.action.triggered)
        {
            RaycastHit hit;

            if (Physics.Raycast(vrCameraTransform.position, vrCameraTransform.forward, out hit, teleportDistance))
            {
                teleportMarker.SetActive(true);
                teleportMarker.transform.position = hit.point;
            }
        }

        if (Main_Button_Input.action.triggered)
        {
            transform.position = teleportMarker.transform.position;
            vrRHandsTransform.position = transform.position;
            vrLHandsTransform.position= transform.position;
            teleportMarker.SetActive(false);
        }
    }
}
