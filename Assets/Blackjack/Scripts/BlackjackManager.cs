using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class BlackjackManager : MonoBehaviour
{
    // Interactable Buttons
    public Button dealButton;
    public Button hitButton;
    public Button standButton;
    public TMP_Text standButtonText;
    // Tracks if stand has been clicked// 0 = stand state, 1 = call state
    private int standClicks = 0;
    // Output for playerScore & Dealer Score
    public TMP_Text playerScoreText;
    public TMP_Text dealerScoreText;
    public TMP_Text mainText;
    // Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;
    // Hides Dealers' first card from player view
    public GameObject hideCard;
    // Array of Dealers' Photos
    public Sprite[] dealerSprites;
    // Index for Rounds won, and index for dealerSprites[]
    public int roundsWon = 0;
    public GameObject dealerAvatar;



    // Start is called before the first frame update
    void Start()
    {
        // Hide extra cards on start
        playerScript.ResetHand();
        dealerScript.ResetHand();
        hideCard.GetComponent<Renderer>().enabled = false;

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
        hideCard.GetComponent<Renderer>().enabled = true;
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
        // Get the text component of the stand button, "Stand State"
        TMP_Text buttonText = standButton.GetComponentInChildren<TMP_Text>();
        buttonText.SetText("Stand");
        // Check for 21s on the first round, lets Dealer take a hit to see if they tie
        if (playerScript.handValue > 20)
        {
            HitDealer();
            RoundOver();
        }
        // Checks if Dealer hits 21 on initial dealing
        else if (dealerScript.handValue > 20) RoundOver();

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

    // If stand has not been clicked this round, enter "call" state, call HitDealer()
    // If in call state, End round (user clicks call)
    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standButtonText.text = "Call";
    }

    // Checks if Daeler wants to take a hit or stand, when the player Stands
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
            PlayerWinsRound();
            
        }
        //Check for tie
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "You tied with the Dealer, Dealer Wins";
        }
        // This is a failsafe incase a logic flow was missed
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

    private void PlayerWinsRound()
    {
        // Set the dealer sprite based on the number of rounds won
        // -2 negates the first and last sprites
        if (roundsWon < dealerSprites.Length - 2)
        {
            roundsWon++;
            SpriteRenderer dealerSpriteRenderer = dealerAvatar.GetComponent<SpriteRenderer>();
            dealerSpriteRenderer.sprite = dealerSprites[roundsWon];
        }
        // If this is the winning final round, print ending message
        else if( roundsWon == dealerSprites.Length - 2)
        {
            roundsWon++;
            SpriteRenderer dealerSpriteRenderer = dealerAvatar.GetComponent<SpriteRenderer>();
            dealerSpriteRenderer.sprite = dealerSprites[roundsWon];
            mainText.text = "You've Finished the Game!";
        }
        // Does nothing right now, just continues to count rounds won after winning
        else
        {
            roundsWon++;
        }
    }
}
