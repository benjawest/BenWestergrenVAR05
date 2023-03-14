using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public Vector3 upVector = Vector3.up; // The up vector of the pin, publicly exposed for convenience
    public bool hasFallenOver = false; // Flag to indicate if the pin has fallen over

    private Quaternion initialRotation; // The initial rotation of the pin
    public BowlingManager bowlingManager;

    private void Start()
    {
        // Find the game object with the BowlingManager script
        GameObject gameManagerObject = GameObject.Find("BowlingManager");

        // Get the BowlingManager script component on the game object
        bowlingManager = gameManagerObject.GetComponent<BowlingManager>();

        initialRotation = transform.rotation;

    }

    private void Update()
    {
        // Calculate the current up vector of the pin
        Vector3 currentUpVector = transform.rotation * Vector3.up;

        // Calculate the angle between the current up vector and the initial up vector
        float angle = Vector3.Angle(upVector, currentUpVector);

        // If the angle is greater than 18 degrees, set hasFallenOver to true
        if (angle > 18f || transform.position.y < 0)
        {
            hasFallenOver = true;
            bowlingManager.UpdateScore();
        }
       
    }
}