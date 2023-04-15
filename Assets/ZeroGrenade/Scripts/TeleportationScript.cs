using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class TeleportationScript : MonoBehaviour
{

    public Transform head;
    public Transform hand;

    public bool Teleport;
    public Vector3 TeleportTarget;

    public float raycastDistance = 10f;
    public LayerMask raycastLayer;
    public Color lineColor = Color.white;
    public float lineThickness = 0.02f;
    public Vector3 forwardVector = Vector3.forward;
    public InputActionReference TeleportButtonAction;
    public bool isTeleportButtonPressed = false;
    public float TeleportButtonActionValue;
    public bool teleportAimActive = false;

    private LineRenderer lineRenderer;
    private bool hitSomething = false;
    public GameObject VRRig;

    private void Start()
    {
        TeleportButtonAction.action.Enable();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material.color = lineColor;

    }

    private void Update()
    {
        // Update the TeleportButtonActionValue from TeleportButtonAction
        TeleportButtonActionValue = TeleportButtonAction.action.ReadValue<float>();
        // I[date isTeleportButtonPressed from TeleportButtonActionValue
        isTeleportButtonPressed = TeleportButtonActionValue > 0.5f;

        if (isTeleportButtonPressed)
        {
            teleportAimActive = true;

            // Perform raycast
            Vector3 localForwardVector = transform.TransformDirection(forwardVector);
            Ray ray = new Ray(transform.position, localForwardVector);
            RaycastHit hit;
            hitSomething = Physics.Raycast(ray, out hit, raycastDistance, raycastLayer);

            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            if (hitSomething)
            {
                lineRenderer.SetPosition(1, hit.point);
                TeleportTarget = hit.point;

            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + localForwardVector * raycastDistance);
            }

          

        }
        else if (!isTeleportButtonPressed)
        {
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;

            // If aim is active and the button is released, check if something is hit
            if (teleportAimActive)
            {
                if (hitSomething)
                {
                    Vector3 directionToHead = VRRig.transform.position - head.position;
                    directionToHead.y = 0;

       
                    VRRig.transform.position = TeleportTarget + directionToHead;

                }
            }
            teleportAimActive= false;
        }

        


            
        

    }
}
