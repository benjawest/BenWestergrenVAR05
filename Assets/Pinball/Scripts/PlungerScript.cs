using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlungerScript : MonoBehaviour
{
    public SpringJoint springJoint;
    public float maxDistance = 5.0f;
    public float springForce = 5000.0f;
    public InputActionAsset inputActionAsset;
    public string plungerActionName = "Plunger";

    private bool isPressed = false;
    private float distance = 0.0f;
    private InputAction plungerAction;

    void Start()
    {
        springJoint = GetComponent<SpringJoint>(); 
        plungerAction = inputActionAsset.FindAction(plungerActionName);
        plungerAction.Enable();
    }

    void OnDestroy()
    {
        plungerAction.Disable();
    }

    void Update()
    {
        if (plungerAction.ReadValue<float>() > 0 && distance < maxDistance)
        {
            isPressed = true;
            distance += Time.deltaTime * maxDistance;
            transform.localPosition = new Vector3(0, 0, -distance);
            springJoint.spring = springForce;
        }
        else if (isPressed && distance >= maxDistance)
        {
            springJoint.spring = 0.0f;
        }
        else if (isPressed)
        {
            isPressed = false;
            distance = 0.0f;
            springJoint.spring = 0.0f;
            transform.localPosition = Vector3.zero;
        }
    }
}
