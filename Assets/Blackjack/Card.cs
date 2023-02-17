using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardName;
    public int value;
    public Suit suit;

    public enum Suit { Hearts, Spades};

    public void Initialize(string cardName, int value, Suit suit)
    {

    }
}
