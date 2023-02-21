using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;



    // Start is called before the first frame update
    void Start()
    {
        GetCardValues();
    }

    //
    void GetCardValues()
    {
        int num = 0;
        // Loop to assign vlaues to the cards
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            // Count up to the amount of cards, 52
            num %= 13;
            // assign the modulus, unless over 10, then use 10
            if (num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
        currentIndex = 1;
    }

    // Shuffle the deck using the Fisher-Yates algorithm
    public void Shuffle()
    {
        // Shuffles from the bottom of the deck to the top of the deck
        for (int i = cardSprites.Length - 1; i > 0; --i)
        {
            // If i is not 1, generate random number for swap position
            // The random range is creating a percentage of size of the deck, plus one (one is the back of card sprite)
            int j = (i == 1) ? 1 : Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * (i - 1)) + 1;
            // Temp swapping sprite
            Sprite swap = cardSprites[i];
            // Replace current card with random card
            cardSprites[i] = cardSprites[j];
            // Replace random cards' spot with current card
            cardSprites[j] = swap;
            // Swap values
            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
        currentIndex = 1;
    }

    // Takes input from selected card, and assigns it to dealt card
    // Increments Index for Deck
    // Returns value of selected card
    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        Debug.Log("The current card index for dealing is: " + currentIndex);
        currentIndex++;
       
        return cardScript.GetValueOfCard();
       
    }

    // Returns the sprite for the back of card image
    public Sprite GetBackOfCard()
    {
        return cardSprites[0];
    }
}


