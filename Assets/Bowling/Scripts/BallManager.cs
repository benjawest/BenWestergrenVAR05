using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallManager : MonoBehaviour
{
    public float launchSpeed = 10f; // The speed at which the ball is launched
    private Rigidbody rb; // The ball's Rigidbody component
    public GameObject arrow; // Aiming Arrow

    // Variables for Ball//Arrow rotation for aiming
    public float rotationSpeed = 10f;
    public float rotationAmplitude = 45f;
    private float rotationTimer = 0f;
    public float rotationY;

    private Coroutine aimCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component on Start
        aimCoroutine = StartCoroutine(StartBallAimRotation());
    }

    void Update()
    {
        
        


        // Launch ball if F is pressed
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            StopCoroutine(aimCoroutine);
            LaunchBall(); // Call the LaunchBall method when F is pressed
        }
    }

    IEnumerator StartBallAimRotation()
    {
        while (true)
        {
            // Update rotation timer based on time elapsed
            rotationTimer += Time.deltaTime;

            // Calculate rotation angles based on sine wave
            rotationY = Mathf.Sin(rotationTimer * rotationSpeed) * rotationAmplitude;

            // Set rotation of ball and arrow around Y-axis
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            yield return null;
        }
    }

    void LaunchBall()
    {
        Vector3 launchDirection = transform.forward; // Get the forward direction of the ball
        launchDirection.y = 0f; // Set the y component to 0 to keep the ball on the ground
        launchDirection.Normalize(); // Normalize the vector to make its magnitude 1
        rb.AddForce(launchDirection * launchSpeed, ForceMode.Impulse); // Apply the launch force to the ball

        // Calculate and print launch angle
        Debug.Log("Ball launched at angle: " + rotationY);

        // Hide arrow when ball is launched
        arrow.SetActive(false);
    }
}
