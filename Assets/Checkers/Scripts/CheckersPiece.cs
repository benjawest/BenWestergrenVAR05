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

   

    //// Array of diagonal local coordinates (valid moves)
    //private Vector2Int[] directions = new Vector2Int[]
    //{
    //        new Vector2Int(-1, 1),
    //        new Vector2Int(1, 1),
    //        new Vector2Int(-1, -1),
    //        new Vector2Int(1, -1)
    //};


    //// Returns a list of Vector2Ints that are valid for moving
    //public List<Vector2Int> GetValidMoves(Dictionary<CheckersPiece, Vector2Int> piecePositions, int[,] board)
    //{
    //    List<Vector2Int> validMoves = new List<Vector2Int>();

    //    foreach (Vector2Int direction in directions)
    //    {
    //        if (!isKing && ((isWhite && direction.y == -1) || (!isWhite && direction.y == 1)))
    //            continue;

    //        Vector2Int currentPos = piecePositions[this];
    //        Vector2Int nextPosition = currentPos + direction;

    //        if (checkersGame.IsInsideBoard(nextPosition))
    //        {
    //            CheckersPiece otherPawn = checkersGame.GetPawn(piecePositions, nextPosition);

    //            if (otherPawn == null)
    //                validMoves.Add(nextPosition);
    //            else if (otherPawn.isWhite != isWhite)
    //            {
    //                nextPosition += direction;
    //                if (checkersGame.IsInsideBoard(nextPosition) && checkersGame.GetPawn(piecePositions, nextPosition) == null)
    //                    validMoves.Add(nextPosition);
    //            }
    //        }
    //    }

    //    return validMoves;
    //}
}