using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


// This script contains the information for an individual card index in the deck
public class CardScript : MonoBehaviour
{
    // Value of this index for calcuating the score
    public int value = 0;

    // Return value of this card index
    public int GetValueOfCard()
    {
        return value;
    }

    // Assign new value for this index
    public void SetValue(int newValue)
    {
        value = newValue;
    }

    // Cards name matches the sprites name, return the string
    public string GetSpriteName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }

    // Assign new card to this index, this just assigns the sprite
    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    // Set up to deal a new hand
    public void ResetCard()
    {
        // Flip card over, assign sprite for back of card, set value to 0
        Sprite back = GameObject.Find("Deck").GetComponent<DeckScript>().GetBackOfCard();
        gameObject.GetComponent<SpriteRenderer>().sprite = back;
        value = 0;
    }
}