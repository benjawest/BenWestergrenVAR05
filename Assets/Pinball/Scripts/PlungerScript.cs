using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlungerScript : MonoBehaviour
{
    // Current Power when plunger input is released
    public float power; 
    public float minPower = 0f;
    public float maxPower = 100f;
    public float powerIncrement = 10f;
    private List<Rigidbody> ballList;
    // Input from Plunger Action Input
    public float plungerValue;
    public InputActionAsset actionAsset;
    private InputAction plungerAction;
    public string actionName;

    public bool ballReady;
    
    // Power Meter
    public GameObject PowerMeterCanvas;
    public RectTransform powerIndicator;
    public float minPosY = -0.4f;
    public float maxPosY = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        PowerMeterCanvas.SetActive(false); // Hide PowerMeter on start
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
            PowerMeterCanvas.SetActive(true);
        }
        else
        {
            PowerMeterCanvas.SetActive(false);
        }

        // If there are any balls in the PlungerTrigger
        if (ballList.Count > 0)
        {
            ballReady = true;
           
            // Check if the input action plungerAction has value
            plungerValue = plungerAction.ReadValue<float>();
            if (plungerValue > 0f)
            {
                power += Time.deltaTime * powerIncrement;
                power = Mathf.Clamp(power, minPower, maxPower);
                //Visually adjust power slider indicator
                UpdatePowerIndicator(power, maxPower);
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
                UpdatePowerIndicator(power, maxPower);
            }
        }
        // There are no balls in trigger zone
        else
        {
            ballReady = false;
            power = minPower;
            UpdatePowerIndicator(power, maxPower);
        }
    }

    public void UpdatePowerIndicator(float power, float maxPower)
    {
        float posY = Mathf.Lerp(minPosY, maxPosY, power / maxPower);
        powerIndicator.anchoredPosition = new Vector2(powerIndicator.anchoredPosition.x, posY);
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
