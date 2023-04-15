using System;
using TMPro;
using UnityEngine;

public class GrenadeGameManager : MonoBehaviour
{
    public int score = 0;
    public int scoreGoal = 10;
    public float countdownTime = 60.0f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreGoalText;
    public TextMeshProUGUI countdownText;

    private float timeLeft;
    private bool gameStarted = false;
    public GameObject StartMenu;
    public GameObject GameplayUI;
    public GameObject RighthandObject;
    private MenuRaycaster menuRaycaster;

    public GameObject WinScreen;
    public GameObject LoseScreen;


    void Start()
    {
        GameplayUI.SetActive(false);
        UpdateScoreUI();
        UpdateCountdownUI();

        // Get the script MenuRaycaster on rightHandObject
        menuRaycaster = RighthandObject.GetComponent<MenuRaycaster>();
    }

    void Update()
    {
        if (gameStarted)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f || score >= scoreGoal)
            {
                gameStarted = false;
                if(score >= scoreGoal)
                {
                    onGameWin();
                }
                else
                {
                    OnGameLose();
                }
            }
            else
            {
                UpdateCountdownUI();
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        scoreGoalText.text = "Goal: " + scoreGoal.ToString();
    }

    void UpdateCountdownUI()
    {
        var timeSpan = TimeSpan.FromSeconds(timeLeft);
        countdownText.text = timeSpan.ToString(@"mm\:ss");
    }

    public void StartGame()
    {
        timeLeft = countdownTime;
        gameStarted = true;
        StartMenu.SetActive(false);
        GameplayUI.SetActive(true);
        menuRaycaster.ClearLineRenderer();
        menuRaycaster.enabled = false;
    }

    private void OnGameLose()
    {
        LoseScreen.SetActive(true);
        menuRaycaster.enabled = true;
    }

    private void onGameWin()
    {
        WinScreen.SetActive(true);
        menuRaycaster.enabled = true;


    }
}
