using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlungerScript : MonoBehaviour
{
    public float power;
    public float minPower = 0f;
    public float maxPower = 100f;
    public float powerIncrement = 10f;
    public Slider powerSlider;
    private List<Rigidbody> ballList;
    // Input from Plunger Action Input
    public float plungerValue;
    public bool ballReady;

    public InputActionAsset actionAsset;
    public string actionName;
    private InputAction plungerAction;

    // Start is called before the first frame update
    void Start()
    {
        powerSlider.minValue = 0f;
        powerSlider.maxValue = maxPower;
        ballList = new List<Rigidbody>();

        // Get a reference to the flipper action
        plungerAction = actionAsset.FindAction(actionName);
        if (plungerAction == null)
        {
            Debug.LogError("Could not find action " + actionName);
            return;
        }

        // Enable the flipper action
        plungerAction.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        // Show power slider when ball is ready to be fired.
        if (ballReady)
        {
            powerSlider.gameObject.SetActive(true);
        }
        else
        {
            powerSlider.gameObject.SetActive(false);
        }
        
        powerSlider.value = power;
        // If there are any balls in the PlungerTrigger
        if(ballList.Count > 0)
        {
            ballReady = true;
           
            // Check if the input action plungerAction has value above 0
            plungerValue = plungerAction.ReadValue<float>();
            if (plungerValue > 0f)
            {
                power += Time.deltaTime * powerIncrement;
                power = Mathf.Clamp(power, minPower, maxPower);
            }
            // If plunger was released, apply force to balls in trigger zone
            else if (plungerValue == 0f && power > minPower)
            {
                // Plunger action was released, apply the stored power to the balls
                foreach (Rigidbody ball in ballList)
                {
                    ball.AddForce(transform.forward * power, ForceMode.Impulse);
                }
                power = minPower;

            }
        }
        // There are no balls in trigger zone
        else
        {
            ballReady = false;
            power = minPower;
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
