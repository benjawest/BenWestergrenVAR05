using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


// VAR05BEN Race Assignment
// This script runs the entire game.
// Initializes all gameobjects, runs the game logic, and moves the pawns
public class boardGameManager : MonoBehaviour
{
    public int TileCount = 10;
    public float TimeBetweenTurns = 0.2f;
    public int playerIndex1 = 0;
    public int playerIndex2 = 0;
    public int TurnCount = 0;
    public List<string> boardTileTypes;
    public GameObject MudPrefab, GrassPrefab, FinishPrefab;
    public GameObject Pawn1Prefab, Pawn2Prefab;
    private GameObject pawn1, pawn2;
    public TMP_Text turnCountText;
    public TMP_Text outputText;
    public Button startButton;
    private string playerChoice;
    public Toggle pawn1Toggle, pawn2Toggle;
    public GameObject startMenu;
    public GameObject gameResultWin, gameResultLose;


  // Sets up Stat Menu
    void Start()
    {
        startButton.interactable = false;
    
    }


    // Enables Start button if the user has selected a player
    public void EnableStartButton()
    {
        if ((pawn1Toggle.isOn || pawn2Toggle.isOn) && !(pawn1Toggle.isOn && pawn2Toggle.isOn))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }


    // Generate Board Tiles
    // Instantiate Tiles
    // Instantiate Pawns
    // Run Game
    public void StartGame()
    {
        if (pawn1Toggle.isOn)
        {
            playerChoice = "Pawn1";
        }
        else if (pawn2Toggle.isOn)
        {
            playerChoice = "Pawn2";
        }

        startMenu.SetActive(false);

        GenerateBoardTileTypes(TileCount);
        InstantiateTiles();
        InstantiatePawns();

        // Debug: Print content of board game tile types
        Debug.Log("boardTileTypes: ");
        foreach (var tile in boardTileTypes)
        {
            Debug.Log(tile);
        }

        StartCoroutine(Game());


    }


    // Begin The Game
    // While neither player is at the finish, Check if players advance, with a pause between player turns. 
    // Increase turn count
    // Check either player has won
    IEnumerator Game()
    {
        while (playerIndex1 < TileCount - 1 && playerIndex2 < TileCount - 1)
        {
            DidPawnMove(ref playerIndex1, "Player 1");
            yield return new WaitForSeconds(TimeBetweenTurns);
            DidPawnMove(ref playerIndex2, "Player 2");
            yield return new WaitForSeconds(TimeBetweenTurns);
            TurnCount++;
            turnCountText.text = "Turn count: " + TurnCount;
        }
        if (playerIndex1 >= TileCount - 1)
        {
            CheckGameResult();
            outputText.text = "Player 1 won after " + TurnCount + " turns!";
            Debug.Log("Player 1 won after " + TurnCount + " turns!");
        }
        else if (playerIndex2 >= TileCount - 1)
        {
            CheckGameResult();
            Debug.Log("Player 2 won after " + TurnCount + " turns!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    // 
    public void InstantiateTiles()
    {
        for (int i = 0; i < TileCount; i++)
        {
            GameObject newTile1;
            GameObject newTile2;

            if (boardTileTypes[i] == "mud")
            {
                newTile1 = Instantiate(MudPrefab, new Vector3(0, 0, i), Quaternion.identity);
                newTile2 = Instantiate(MudPrefab, new Vector3(1, 0, i), Quaternion.identity);
            }
            else if (boardTileTypes[i] == "grass")
            {
                newTile1 = Instantiate(GrassPrefab, new Vector3(0, 0, i), Quaternion.identity);
                newTile2 = Instantiate(GrassPrefab, new Vector3(1, 0, i), Quaternion.identity);
            }
            else
            {
                newTile1 = Instantiate(FinishPrefab, new Vector3(0, 0, i), Quaternion.identity);
                newTile2 = Instantiate(FinishPrefab, new Vector3(1, 0, i), Quaternion.identity);
            }
        }
    }



    public void InstantiatePawns()
    {
        Vector3 position1 = new Vector3(0.0f, 0.5f, 0f);
        Vector3 position2 = new Vector3(1.0f, 0.5f, 0f);

        pawn1 = Instantiate(Pawn1Prefab, position1, Quaternion.identity);
        pawn2 = Instantiate(Pawn2Prefab, position2, Quaternion.identity);
    }

    // Generates tile types for the board game
    // Takes tileCount as the size of the board
    private void GenerateBoardTileTypes(int tileCount)
    {
        // Clear the list
        boardTileTypes.Clear();

        //Generate the list of tile types
        boardTileTypes = new List<string>();
        for (int i = 0; i < tileCount - 1; i++)
        {
            int tileType = Random.Range(0, 2);
            switch (tileType)
            {
                case 0:
                    boardTileTypes.Add("mud");
                    break;
                case 1:
                    boardTileTypes.Add("grass");
                    break;
            }
        }
        boardTileTypes.Add("finish");
    }

    // Checks if player moves ahead based on current tile type
    void DidPawnMove(ref int playerIndex, string playerName)
    {
        Debug.Log("Turn: " + TurnCount + " " + playerName + " on tile: " + playerIndex);
        if (boardTileTypes[playerIndex] == "grass")
        {
            System.Random rand = new System.Random();
            int randomInt = rand.Next(0, 4);
            if (randomInt == 0)
            {
                playerIndex++;
                outputText.text = playerName + " advanced through the grass";
                Debug.Log(playerName + " moved to tile: " + playerIndex);
                MovePawn(playerName);
            }
        }
        else if (boardTileTypes[playerIndex] == "mud")
        {
            System.Random rand = new System.Random();
            int randomInt = rand.Next(0, 8);
            if (randomInt == 0)
            {
                playerIndex++;
                outputText.text = playerName + " advanced through the mud";
                Debug.Log(playerName + " moved to tile: " + playerIndex);
                MovePawn(playerName);
            }
        }
    }

    // Moves selected players' pawn
    public void MovePawn(string playerName)
    {
        if (playerName == "Player 1")
        {
            pawn1.transform.position += new Vector3(0, 0, 1);
        }
        else if (playerName == "Player 2")
        {
            pawn2.transform.position += new Vector3(0, 0, 1);
        }
    }

    // Checks Game Results
    public void CheckGameResult()
    {
        if (playerChoice == "Pawn1" && pawn1.transform.position.z > pawn2.transform.position.z)
        {
            gameResultWin.SetActive(true);

        }
        else if (playerChoice == "Pawn2" && pawn2.transform.position.z > pawn1.transform.position.z)
        {
            gameResultWin.SetActive(true);
        }
        else
        {
            gameResultLose.SetActive(true);
        }
    }

    // Restart Game
    public void RestartGame()
    {
        StopAllCoroutines();
        playerIndex1 = 0;
        playerIndex2 = 0;
        TurnCount = 0;
        turnCountText.text = "Turn count: 0";
        outputText.text = "";
        gameResultWin.SetActive(false);
        gameResultLose.SetActive(false);
        startMenu.SetActive(true);
        startButton.interactable = false;
        pawn1Toggle.isOn = false;
        pawn2Toggle.isOn = false;
        Destroy(pawn1);
        Destroy(pawn2);
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (var tile in tiles)
        {
            Destroy(tile);
        }
    }



}
