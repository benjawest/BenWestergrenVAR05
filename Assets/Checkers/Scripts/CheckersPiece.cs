using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class CheckersPiece : MonoBehaviour
{
    //reference to CheckersGame script
    private CheckersGame checkersGame;
    // Booleans to track state
    public bool isKing;
    public bool isWhite;

    void Start()
    {
        // Find the game object with the CheckersGame script attached
        GameObject checkersManagerObject = GameObject.Find("CheckersManager");

        // Get a reference to the CheckersGame script
        checkersGame = checkersManagerObject.GetComponent<CheckersGame>();
    }

    // Position
    public int x, y;

    // Set pawn to new position
    public void SetPosition(int newX, int newY)
    {
        x = newX;
        y = newY;

        transform.position = new Vector3(x, 0, y);
    }

   
    // Set Piece as King
    public void SetKing()
    {
        isKing = true;
        Renderer renderer = this.GetComponentInChildren<Renderer>();
        if (isWhite)
        {
            renderer.material = checkersGame.PlayerAMaterialKing;
        }
        else
        {
            renderer.material = checkersGame.PlayerBMaterialKing;
        }
    }
}