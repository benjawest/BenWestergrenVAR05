using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental.GraphView;

public class BlackjackManager : MonoBehaviour
{
    // Interactable Buttons
    public Button dealButton;
    public Button hitButton;
    public Button standButton;
    public TMP_Text standButtonText;

    // Tracks if stand has been clicked twice
    private int standClicks = 0;

    // Output for playerScore & Dealer Score
    public TMP_Text playerScoreText;
    public TMP_Text dealerScoreText;
    public TMP_Text mainText;


    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    public GameObject hideCard;


    // Start is called before the first frame update
    void Start()
    {
        // Add on click listeners to the buttons
        // Hide Hit and Set 
        dealButton.onClick.AddListener(() => DealClicked());
        hitButton.onClick.AddListener(() => HitClicked());
        hitButton.gameObject.SetActive(false);
        standButton.onClick.AddListener(() => StandClicked());
        standButton.gameObject.SetActive(false);

    }

    private void DealClicked()
    {
        // Reset Round
        playerScript.ResetHand();
        dealerScript.ResetHand();
        
        // Clear mainText
        mainText.text = "";
        // Hide dealer score
        dealerScoreText.gameObject.SetActive(false);
        //Shuffle Deck
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        // Reset round, hide text, prep for new hand
        playerScript.StartHand();
        dealerScript.StartHand();
        //update the score
        playerScoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // Enable hidden card for dealer
        hideCard.GetComponent<Renderer>().enabled = true;
        // Adjust button visibility
        dealButton.gameObject.SetActive(false);
        standButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(true);
        // Get the text component of the stand button
        TMP_Text buttonText = standButton.GetComponentInChildren<TMP_Text>();
        buttonText.SetText("Stand");

    }

    private void HitClicked()
    {
        // Check that there is still room on the table
        if (playerScript.cardIndex <= 9)
        {
            playerScript.GetCard();
            playerScoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standButtonText.text = "Call";
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }

    // Check to see if game is over, and if so: who is the winner
    void RoundOver()
    {
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        // If stand has been clicked less than twice, no 21s or busts, quit function
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // All bust, Round Over
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
        }
        //Check for tie
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "You tied with the Dealer";
        }
        else
        {
            roundOver = false;
        }


        // Reset UI
        if (roundOver)
        {
            hitButton.gameObject.SetActive(false);
            standButton.gameObject.SetActive(false);
            dealButton.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            standClicks = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
