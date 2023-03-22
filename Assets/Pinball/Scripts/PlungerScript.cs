using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Input;
using UnityEngine.UI;

public class PlungerScript : MonoBehaviour
{
    public float power;
    public float minPower = 0f;
    public float maxPower = 100f;
    public Slider powerSlider;
    private List<Rigidbody> ballList;

    public InputActionAsset actionAsset;
    public string actionName;
    private InputAction plungerAction;

    // Start is called before the first frame update
    void Start()
    {
        powerSlider.minValue = 0f;
        powerSlider.maxValue = maxPower;

        // Get a reference to the flipper action
        plungerAction = actionAsset.FindAction(actionName);
        if (plungerAction == null)
        {
            Debug.LogError("Could not find action " + actionName);
            return;
        }

        // Enable the flipper action
        flipperAction.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        powerSlider.value = power;
        if(ballList.Count > 0)
        {
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballList.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballList.Remove(other.gameObject.GetComponent<Rigidbody>());
            power = minPower;
        }
    }
}
