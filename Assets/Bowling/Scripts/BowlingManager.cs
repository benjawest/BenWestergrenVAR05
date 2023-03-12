using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class BowlingManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform ballSpawnPosition;

    public GameObject pinPrefab;
    public Transform pinParent;
    public List<PinManager> pins;

    private GameObject currentBall;
    private BallManager ballManager;

    public TMP_Text outputText;

    private void Start()
    {
        pins = new List<PinManager>();
        SpawnPins();
        SpawnBall();
        outputText.text = "Press space to launch the ball!";
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (ballManager != null && !ballManager.hasLaunched )
            {
                ballManager.LaunchBall();
                outputText.text = "Nice Shot!";
            }
        }
        else if (Keyboard.current.rKey.wasPressedThisFrame && currentBall != null)
        {
            NextRound();
        }

    }

    public void NextRound()
    {
        DestroyAllPins();
        if(currentBall != null)
        {
            Destroy(currentBall); //Destroy Current Ball
        }
        SpawnBall();
        SpawnPins();
        outputText.text = "Press space to launch the ball!";
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

    public void DestroyAllPins()
    {
        foreach (PinManager pin in pins)
        {
            Destroy(pin.gameObject);
        }
        pins.Clear();
    }

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

        if(score== 10)
        {
            outputText.text = "Strike!\n\nPress the \"R\" key to try again";
        }
        if (score != 0)
        {
            outputText.text = "Score: " + score + "\n\nPress the \"R\" key to try again";
        }
        else if (score == 0)
        {
            outputText.text = "Gutter!\n\nPress the \"R\" key to try again";
        }
        
    }
}
