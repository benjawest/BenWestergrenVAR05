using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipperScript : MonoBehaviour
{
    public float restPosition = 0f;
    public float pressedPosition = 45f; // Must be inverted maually for right side
    public float hitStrength = 10000f; 
    public float flipperDamper = 150f;

    public HingeJoint hinge;

    public InputActionAsset actionAsset;
    public string actionName;

    private InputAction flipperAction;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useSpring = true;

        // Get a reference to the flipper action
        flipperAction = actionAsset.FindAction(actionName);
        if (flipperAction == null)
        {
            Debug.LogError("Could not find action " + actionName);
            return;
        }

        // Enable the flipper action
        flipperAction.Enable();
    }

    void Update()
    {
        // Assign spring values
        JointSpring spring = new JointSpring();
        spring.spring = hitStrength;
        spring.damper = flipperDamper;

        if (flipperAction.ReadValue<float>() > 0f) // User presses flipperAction
        {
            spring.targetPosition = pressedPosition;
        }
        else // No Input from action
        {
            spring.targetPosition = restPosition;
        }
        hinge.spring = spring;
        hinge.useLimits = true;
    }
}