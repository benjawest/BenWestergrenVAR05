using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BowlingManager : MonoBehaviour
{   
    public GameObject ballPrefab; // Prefabs
    public GameObject pinPrefab;

    public Transform ballSpawnPosition; // Position to spawn ball

    public Transform pinParent; // Parent containing transform information for pins, used when spawning
    public List<PinManager> pins; // List of Pin Manager scripts, these contain state for isFallenOver

    private GameObject currentBall; // Reference to spawned Ball
    private BallManager ballManager; // Reference to ballMananger script for currentBall

    public TMP_Text outputText; // Main output text to player
    public TMP_Text throwCounterText; // Balls remaining text
    public Button throwButton;
    public Button newBallButton;
    
    public int ballsRemaining; // Player attempts left for this round
    public int ballMax = 2;

    // Sets up scene for first throw
    private void Start()
    {
        pins = new List<PinManager>();
        ballsRemaining = ballMax;
        SpawnPins();
        SpawnBall();
        outputText.text = "Welcome to Bolf";
        newBallButton.interactable = false;
        throwButton.interactable = true;
    }

    public void LaunchBall()
    {
        // Update remaining balls
        --ballsRemaining;
        throwCounterText.text = "Balls: " + ballsRemaining;

        // Launch Ball
        if (ballManager != null)
        {
            ballManager.LaunchBall();
        }
        // Disable Launch button
        throwButton.interactable = false;
        if(ballsRemaining > 0) // If balls left, show New Ball button
        {
            newBallButton.interactable = true;
        }
    }

    // Spawns a new ball for player to take their second shot
    public void SecondThrow()
    {
        if (currentBall != null)
        {
            Destroy(currentBall); //Destroy Current Ball
        }
        SpawnBall();
        outputText.text = "Second Throw";
        newBallButton.interactable = false;
        throwButton.interactable = true;
    }

    // Resets scene for next round
    public void NextRound()
    {
        DestroyAllPins();
        if(currentBall != null)
        {
            Destroy(currentBall); //Destroy Current Ball
        }
        SpawnBall();
        SpawnPins();
        ballsRemaining = ballMax;
        throwCounterText.text = "Balls: " + ballsRemaining;
        outputText.text = "New Round";
        newBallButton.interactable = false;
        throwButton.interactable = true;
    }


    public void SpawnBall()
    {
        currentBall = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
        ballManager = currentBall.GetComponent<BallManager>();
    }

    private void SpawnPins()
    {
        foreach (Transform pinTransform in pinParent)
        {
            GameObject pinObject = Instantiate(pinPrefab, pinTransform.position, pinTransform.rotation);
            PinManager pin = pinObject.GetComponent<PinManager>();
            pin.upVector = pinTransform.up;
            pins.Add(pin);
        }
    }

    // Destroys Pins game objects and clears pins<>
    public void DestroyAllPins()
    {
        foreach (PinManager pin in pins)
        {
            Destroy(pin.gameObject);
        }
        pins.Clear();
    }

    // Resets score, sets score to the number of pins fallen over in pins<>
    // Pins will call this function when their state changes to fallen over
    public void UpdateScore()
    {
        int score = 0;
        foreach (PinManager pin in pins)
        {
            if (pin.hasFallenOver)
            {
                score++;
            }
        }

        if(score == 10)
        {
            outputText.text = "Strike!";
        }
        else if (score != 0)
        {
            outputText.text = "Score: " + score;
        }
        else if (score == 0)
        {
            outputText.text = "Gutter!";
        }
        
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    }
}
