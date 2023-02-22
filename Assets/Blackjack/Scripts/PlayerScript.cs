using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is for BOTH player and dealer
public class PlayerScript : MonoBehaviour
{
    // Get other scripts
    public CardScript cardScript;
    public DeckScript deckScript;

    // Total value of player/dealer's hand
    public int handValue = 0;
    public GameObject[] hand;
    // Index of next card, depending on how many cards are turned over
    public int cardIndex = 0;
    // Tracks if this hand has Aces, and stores the assosiated card script
    List<CardScript> aceList = new List<CardScript>();

    // Add a card to the player/dealer's hand
    // Returns the new value of the hand it was placed into
    public int GetCard()
    {
        // Debug.Log("Hand Size " + hand.Length);
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        Debug.Log("Card Value " + cardValue);
        // Show card on game screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        // Add card value to running total of the hand
        handValue += cardValue;
        // If value is 1, it is an ace
        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }
        // Check if we should use an 11 instead of a 1
        AceCheck();
        // Increment which card in the hand we're interacting with
        Debug.Log("The current card index for this hand is: " + cardIndex);
        cardIndex++;
        return handValue;
    }

    // Search for needed ace conversions, 1 to 11 or vice versa
    // Only checks aces in current hand for selected player
    public void AceCheck()
    {
        // Finds all aces in Conversion list (aceList)
        foreach (CardScript ace in aceList)
        {
            // Decide if Ace should be used as a 1 or 11 for calucating Scores
            // If ace is set as low, check if setting it as high breaks 21. If ace high breaks 21, set ace low
            if (handValue + 10 < 22 && ace.GetValueOfCard() == 1)
            {
                ace.SetValue(11);
                handValue += 10;
            }
            // If ace high breaks 21
            else if (handValue > 21 && ace.GetValueOfCard() == 11)
            {
                // Set ace low
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    // Deal two cards for this player/Dealer
    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    // Hides all exposed cards. Resets all indexes for this player/dealer
    public void ResetHand()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }

}
