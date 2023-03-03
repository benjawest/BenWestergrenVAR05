using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class CheckersPiece : MonoBehaviour
{
    //reference to CheckersGame script
    private CheckersGame checkersGame;

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

    public bool isKing;
    public bool isWhite;

    private Vector2Int[] directions = new Vector2Int[]
    {
            new Vector2Int(-1, 1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, -1)
    };

    public List<Vector2Int> GetValidMoves(int[,] board)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();

        foreach (Vector2Int direction in directions)
        {
            if (!isKing && ((isWhite && direction.y == -1) || (!isWhite && direction.y == 1)))
                continue;

            Vector2Int nextPosition = checkersGame.GetPosition(this) + direction;
            if (checkersGame.IsInsideBoard(nextPosition))
            {
                CheckersPiece otherPawn = checkersGame.GetPawn(nextPosition);
                if (otherPawn == null)
                    validMoves.Add(nextPosition);
                else if (otherPawn.isWhite != isWhite)
                {
                    nextPosition += direction;
                    if (checkersGame.IsInsideBoard(nextPosition) && checkersGame.GetPawn(nextPosition) == null)
                        validMoves.Add(nextPosition);
                }
            }
        }

        return validMoves;
    }
}