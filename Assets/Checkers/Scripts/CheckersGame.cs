using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;

public class CheckersGame : MonoBehaviour
{
    // prefab for checkers pawns and tiles
    public CheckersPiece piecePrefab;
    public GameObject tilePrefab;
    // material for white pawns
    public Material PlayerAMaterial;
    // material for red pawns
    public Material PlayerBMaterial;
    // UI
    public TMP_Text playerScoreA_TMP; public TMP_Text playerScoreB_TMP;
    public TMP_Text playerTurn;

    // Board Dimensions
    int ROWS = 8;
    int COLUMNS = 8;
    // player scores
    int playerScoreA = 0, playerScoreB = 0;
    bool whiteTurn = true;
   
    // Use CheckersPiece scripts (key)  to access their positions (entry)
    private Dictionary<CheckersPiece, Vector2Int> piecePositions;
    // Temporary reference to selected pawn
    private CheckersPiece selectedPiece = null;
    
    // 2D array containing all the tiles of the board
    Tile[,] tiles = new Tile[8, 8];
    // 2d array to display where pieces are on the board --- Deprecate, use pecePositions instead ---
    public int[,] board;
    public bool GameOver = false;



    private void Awake()
    {
        SetupPawns();
        // PrintBoard(board);
        InstantiateBoard();
        
        // Debugging contents of Piece Positions
        Debug.Log("Piece Positions: " + string.Join(", ", piecePositions.Select(kv => kv.Key.name + ": " + kv.Value.ToString()).ToArray()));
    }

    // Instantiates tiles for the board
    public void InstantiateBoard()
    {
        for (int x = 0; x < ROWS; x++)
        {
            for (int z = 0; z < COLUMNS; z++)
            {
                // Instantiate tile
                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.Euler(90, 0, 0)); //Rotation is to fix quads y-axis
                Tile tile = tileObject.GetComponent<Tile>();

                // Set tile color
                Renderer renderer = tileObject.GetComponent<Renderer>();
                if ((x + z) % 2 == 0)
                {
                    renderer.material.color = Color.white;
                }
                else
                {
                    renderer.material.color = Color.black;
                }
                // Set Tile Script variables
                tile.position = new Vector2Int(x, z);


                // Add tile script to the tiles array
                tiles[x, z] = tile;
            }
        }
    }

    // Instantiate pawns on board
    public void SetupPawns()
    {
        // Initialize board array (ground truth)
        board = new int[ROWS, COLUMNS];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = 0;
            }
        }

        // Initialize piecePositions dictionary
        piecePositions = new Dictionary<CheckersPiece, Vector2Int>();

        // Set up PlayerA (white)
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i + j) % 2 == 1)
                {
                    // Instantiate a pawn for player A at row i and column j
                    Vector3 position = new Vector3(j, 0, i); // Changed from new Vector3(1 + j, 0, 1 + i)
                    CheckersPiece pawn = Instantiate(piecePrefab, position, Quaternion.identity);
                    // Set the material of the pawn's child object with a Renderer component to PlayerAMaterial
                    Renderer renderer = pawn.GetComponentInChildren<Renderer>();
                    renderer.material = PlayerAMaterial;
                    pawn.isWhite = true;
                    pawn.isKing = false;
                    pawn.x = j;
                    pawn.y = i;
                    // Update board for playerA positions
                    piecePositions.Add(pawn, new Vector2Int(j, i)); // Add pawn to dictionary
                }
            }
        }

        // Setup PlayerB (red)
        for (int i = 5; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i + j) % 2 == 1)
                {
                    // Instantiate a pawn for player B at row i and column j
                    Vector3 position = new Vector3(j, 0, i); // Changed from new Vector3(1 + j, 0, 1 + i)
                    CheckersPiece pawn = Instantiate(piecePrefab, position, Quaternion.identity);
                    // Set the material of the pawn's child object with a Renderer component to PlayerBMaterial
                    Renderer renderer = pawn.GetComponentInChildren<Renderer>();
                    renderer.material = PlayerBMaterial;
                    pawn.isWhite = false;
                    pawn.isKing = false;
                    pawn.x = j;
                    pawn.y = i;
                    //Update board for playerB positions
                    piecePositions.Add(pawn, new Vector2Int(j, i)); // Add pawn to dictionary
                }
            }
        }
    }

    // Returns Vector2Int for selected pawn script
    public Vector2Int GetPosition(CheckersPiece piece)
    {
        return piecePositions[piece];
    }

    // Returns pawns script for Vector2Int
    public CheckersPiece GetPawn(Vector2Int position)
    {
        foreach (var kvp in piecePositions)
            if (kvp.Value == position)
                return kvp.Key;
        return null;
    }


    public bool IsInsideBoard(Vector2Int position)
    {
        return position.x >= 0 && position.x < board.GetLength(0) && position.y >= 0 && position.y < board.GetLength(1);
    }

    
    private void Update()
    {
        while (!GameOver)
        {
            WasMousePressed();
        }
        
        
    }


    public void WasMousePressed()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Raycasting!
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            bool didWeHitAnything = Physics.Raycast(ray, out RaycastHit hit);

            if (didWeHitAnything == true)
            {
                // If a piece has already been selected -> Move Selection State
                if (selectedPiece != null)
                {
                  
                    // Adjust raycast hit to whole number          
                    int xHit = Mathf.RoundToInt(hit.point.x);
                    int yHit = Mathf.RoundToInt(hit.point.z);

                    Debug.Log($"We hit point: {hit.point} resulting in {xHit}, {yHit}");

                    // Check if the selected piece can move to the selected tile
                    List<Vector2Int> validMoves = GetValidMoves(selectedPiece);
                    Vector2Int destination = new Vector2Int(xHit, yHit);
                    if (validMoves.Contains(destination))
                    {
                        // Move Piece, update piecePositions and script components
                        MovePiece(selectedPiece, destination);
                        ClearHighlights();
                        selectedPiece = null;
                    }
                    else
                    {
                        ClearHighlights();
                        Debug.Log($"Invalid move. {selectedPiece.name} cannot move to {destination}");
                        selectedPiece = null;
                    }
                }
                // No piece has already been selected, so set new selcetion as selectedPiece
                else
                {
                    selectedPiece = hit.collider.gameObject.GetComponent<CheckersPiece>();
                   
                    if (selectedPiece != null)
                    {
                        if ((selectedPiece.isWhite && whiteTurn) || (!selectedPiece.isWhite && !whiteTurn))
                        {
                            HighlightValidMoves(selectedPiece);
                            Debug.Log($"selected piece at {selectedPiece.x}, {selectedPiece.y}");
                        }
                        else
                        {
                            selectedPiece = null;
                        }
                    }
                }

                Debug.DrawRay(hit.point, Vector3.up, Color.cyan, 2);
            }

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 2);
        }
    }


    // Move Piece to new location. Also handles capturing and destroying opponesnts piece, if the move is a capture move
    public void MovePiece(CheckersPiece piece, Vector2Int targetPosition)
    {

        Vector2Int currentPosition = piecePositions[piece];
        int dx = targetPosition.x - currentPosition.x;
        int dy = targetPosition.y - currentPosition.y;

        // Checks if move is two tiles away, treast move as capture move.
        if (Mathf.Abs(dx) == 2 && Mathf.Abs(dy) == 2)
        {
            // Capture move
            int captureX = currentPosition.x + (dx / 2);
            int captureY = currentPosition.y + (dy / 2);
            Vector2Int capturePosition = new Vector2Int(captureX, captureY);

            CheckersPiece capturedPiece = piecePositions.First(x => x.Value == capturePosition).Key;
            piecePositions.Remove(capturedPiece);
            Destroy(capturedPiece.gameObject);

            // Increases current players score for capture.
            if (whiteTurn)
            {
                playerScoreA++;
                playerScoreA_TMP.text = playerScoreA.ToString();
            }
            else
            {
                playerScoreB++;
                playerScoreB_TMP.text = playerScoreB.ToString();
            }
          
            //// Check for multi-capture
            //List<Vector2Int> captureMoves = GetValidMoves(piece);
            //if (captureMoves.Count > 0)
            //{
            //    // Highlight valid capture moves
            //    selectedPiece = piece;
            //    validMoves = captureMoves;
            //    HighlightCells(validMoves);
            //    return;
            //}
        }

        // Move piece
        piecePositions[piece] = targetPosition;
        piece.SetPosition(targetPosition.x, targetPosition.y);

        // Check for promotion
        if ((piece.isWhite && targetPosition.y == ROWS - 1) || (!piece.isWhite && targetPosition.y == 0))
        {
            piece.isKing = true;
        }

        //// Check for game over
        //if (IsGameOver())
        //{
        //    gameOver = true;
        //    Debug.Log("Game Over");
        //}

        // Switch player turn
        selectedPiece = null;
        whiteTurn = !whiteTurn;
        if (whiteTurn)
        {
            playerTurn.text = "White";
        }
        else
        {
            playerTurn.text = "Red";
        }
    }



    public void ClearHighlights()
    {
        // Loop through all the tiles in the 2D array and turn off the highlights
        for (int x = 0; x < ROWS; x++)
        {
            for (int y = 0; y < COLUMNS; y++)
            {
                tiles[x, y].SetHighlight(false);
            }
        }
    }

    // Highlight on the board which tiles are valid for moves
    void HighlightValidMoves(CheckersPiece selectedPawn)
    {
        List<Vector2Int> validMoves = GetValidMoves(selectedPawn);
        foreach (Vector2Int move in validMoves)
        {
            // Debug line, print all entries in ValidMoves
            Debug.Log($"Entries in validMoves: {validMoves.Count}. Contents: {string.Join(", ", validMoves)}");
            // Get the tile at the position of the valid move
            Tile tile = GetTileAtPosition(move);
            // Change the color or material of the tile to highlight it
            tile.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }


    public List<Vector2Int> GetValidMoves(CheckersPiece piece)
    {
        Vector2Int currentPosition = piecePositions[piece];
        int direction = piece.isWhite ? 1 : -1;
        List<Vector2Int> moves = new List<Vector2Int>();
        List<Vector2Int> captureMoves = new List<Vector2Int>();
        Vector2Int[] captureOffsets;



        // Check for capture moves
        if (piece.isKing)
        {
            // If King, check in all directions
           captureOffsets = new Vector2Int[] { new Vector2Int(-1, direction), new Vector2Int(1, direction),
                                                     new Vector2Int(-1, -direction), new Vector2Int(1, -direction)};
        }
        else
        {
            // Else only check in normal "direction"
           captureOffsets = new Vector2Int[] { new Vector2Int(-1, direction), new Vector2Int(1, direction),};
        }
        // Check all directions from captureOffsets
        foreach (Vector2Int offset in captureOffsets)
        {
            Vector2Int capturePos = currentPosition + offset;
            
            Vector2Int captureTarget = currentPosition + (offset * 2);
            if (IsValidCapture(currentPosition, capturePos, captureTarget, piece))
            {
                moves.Add(captureTarget);
            }
        }

        // Check for regular moves
        Vector2Int forwardLeft = currentPosition + new Vector2Int(-1, direction);
        if (IsValidMove(currentPosition, forwardLeft, piece))
        {
            moves.Add(forwardLeft);
        }

        Vector2Int forwardRight = currentPosition + new Vector2Int(1, direction);
        if (IsValidMove(currentPosition, forwardRight, piece))
        {
            moves.Add(forwardRight);
        }

        if (piece.isKing)
        {
            Vector2Int backwardLeft = currentPosition + new Vector2Int(-1, -direction);
            if (IsValidMove(currentPosition, backwardLeft, piece))
            {
                moves.Add(backwardLeft);
            }

            Vector2Int backwardRight = currentPosition + new Vector2Int(1, -direction);
            if (IsValidMove(currentPosition, backwardRight, piece))
            {
                moves.Add(backwardRight);
            }
        }

        if (captureMoves.Count > 0)
        {
            return captureMoves;
        }
        else
        {
            return moves;
        }
    }

    private bool IsValidMove(Vector2Int current, Vector2Int target, CheckersPiece piece)
    {
        if (target.x < 0 || target.x >= ROWS || target.y < 0 || target.y >= COLUMNS)
        {
            return false;
        }

        if (piecePositions.ContainsValue(target))
        {
            return false;
        }

        int dx = target.x - current.x;
        int dy = target.y - current.y;

        if (Mathf.Abs(dx) != 1 || Mathf.Abs(dy) != 1)
        {
            return false;
        }

        if (Mathf.Abs(dx) == 2 && Mathf.Abs(dy) == 2)
        {
            int captureX = current.x + (dx / 2);
            int captureY = current.y + (dy / 2);
            Vector2Int capturePosition = new Vector2Int(captureX, captureY);

            if (!piecePositions.ContainsValue(capturePosition))
            {
                return false;
            }
            CheckersPiece capturedPiece = piecePositions.First(x => x.Value == capturePosition).Key;

            if (capturedPiece.isWhite == piece.isWhite)
            {
                return false;
            }
        }

        return true;
    }

    bool IsValidCapture(Vector2Int currentPosition, Vector2Int capturePos, Vector2Int captureTarget, CheckersPiece piece)
    {
        // Check that the capture target is within the bounds of the board
        if (captureTarget.x < 0 || captureTarget.x >= COLUMNS || captureTarget.y < 0 || captureTarget.y >= ROWS)
        {
            return false;
        }

        // Check that there is an opponent piece to capture
        if (!piecePositions.ContainsValue(capturePos))
        {
            return false;
        }

        // Check that the capture target is not occupied by another piece
        if (piecePositions.ContainsValue(captureTarget))
        {
            return false;
        }

        // Check that the captured piece belongs to the opponent
        CheckersPiece capturedPiece = piecePositions.First(x => x.Value == capturePos).Key;
        if (capturedPiece.isWhite == piece.isWhite)
        {
            return false;
        }

        // Check that the capture is diagonal
        int dx = captureTarget.x - currentPosition.x;
        int dy = captureTarget.y - currentPosition.y;
        if (Mathf.Abs(dx) != 2 || Mathf.Abs(dy) != 2)
        {
            return false;
        }

        return true;
    }




    // Returns the Tile script for the tile at position
    Tile GetTileAtPosition(Vector2Int position)
    {
        
        return tiles[position.x, position.y];
    }

    // Clear board of pawns
    void ClearBoard()
    {
        GameObject[] pawns = GameObject.FindGameObjectsWithTag("Pawn");
        foreach (GameObject pawn in pawns)
        {
            Destroy(pawn);
        }
    }
}