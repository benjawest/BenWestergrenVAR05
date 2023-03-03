using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckersGame : MonoBehaviour
{
    public CheckersPiece piecePrefab;
    public Material PlayerAMaterial;
    public Material PlayerBMaterial;
    int ROWS = 8;
    int COLUMNS = 8;
    public int[,] board;
    private Dictionary<CheckersPiece, Vector2Int> piecePositions;
    private CheckersPiece selectedPiece = null;
    public GameObject tilePrefab;
    Tile[,] tiles = new Tile[8, 8];

    private void Awake()
    {
        SetupPawns();
        PrintBoard(board);
        InstantiateBoard();
    }


    public void BoardManager(int[,] board)
    {
        this.board = board;
        piecePositions = new Dictionary<CheckersPiece, Vector2Int>();
    }


    public void InstantiateBoard()
    {
        for (int x = 0; x < ROWS; x++)
        {
            for (int z = 0; z < COLUMNS; z++)
            {
                // Instantiate tile
                GameObject tileObject = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.Euler(90, 0, 0));
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

                // Add tile to the tiles array
                tiles[x, z] = tile;
            }
        }
    }

    // Instantiate pawns on board
    public void SetupPawns()
    {

        // Initialize board arry (ground truth)
        board = new int[ROWS, COLUMNS];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = 0;
            }
        }

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

                    // Update board for playerA positions
                    board[i, j] = 1;
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

                    //Update board for playerB positions
                    board[i, j] = -1;
                }
            }
        
        }
    }


    public Vector2Int GetPosition(CheckersPiece piece)
    {
        return piecePositions[piece];
    }

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
 
    // 
    public void PrintBoard(int[,] board)
    {
        string output = "";
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                output += board[i, j] + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
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

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Raycasting!
            // Ray ray = new Ray(transform.position, transform.forward);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            bool didWeHitAnything = Physics.Raycast(ray, out RaycastHit hit);

            if (didWeHitAnything == true)
            {
                // Are we placing a piece?
                if (selectedPiece != null)
                {
                    // What tile did we hit on the board?             
                    int xHit = Mathf.RoundToInt(hit.point.x);
                    int yHit = Mathf.RoundToInt(hit.point.z);

                    Debug.Log($"We hit point: {hit.point} resulting in {xHit}, {yHit}");

                    selectedPiece.SetPosition(xHit, yHit);

                    selectedPiece = null;
                }
                // ...or are we selecting a piece?
                else
                {
                    selectedPiece = hit.collider.gameObject.GetComponent<CheckersPiece>();

                    if (selectedPiece != null)
                    {
                        HighlightValidMoves(selectedPiece);
                        Debug.Log($"selected piece at {selectedPiece.x}, {selectedPiece.y}");
                    }
                }

                Debug.DrawRay(hit.point, Vector3.up, Color.cyan, 2);
            }

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 2);
        }
    }

    void HighlightValidMoves(CheckersPiece selectedPawn)
    {
        List<Vector2Int> validMoves = selectedPawn.GetValidMoves(board);
        foreach (Vector2Int move in validMoves)
        {
            // Get the tile at the position of the valid move
           Tile tile = GetTileAtPosition(move);
            // Change the color or material of the tile to highlight it
            tile.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    Tile GetTileAtPosition(Vector2Int position)
    {
        // Assuming your tiles are stored in a 2D array called tiles
        return tiles[position.x, position.y];
    }
}