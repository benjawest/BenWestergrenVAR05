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
    public int TileCount = 11;
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
    public TMP_InputField inputField;
    public Camera mainCamera;
    public Transform leader;


    // Sets up Start Menu
    void Start()
    {
        startButton.interactable = false;
        inputField.text = "10";
    
    }


    // Enables Start button if the user has selected a player
    public void EnableStartButton()
    {
        int fieldValue = int.Parse(inputField.text);
        if ((pawn1Toggle.isOn || pawn2Toggle.isOn) && !(pawn1Toggle.isOn && pawn2Toggle.isOn) && fieldValue != 0)
        {
            startButton.interactable = true;
        }
        else if (fieldValue == 0)
        {
            startButton.interactable = false;
        }

        else
        {
            startButton.interactable = false;
        }
    }


    // Assign Tile count from player input, or default to 10
    // Generate Board Tiles
    // Instantiate Tiles
    // Instantiate Pawns
    // Run Game
    public void StartGame()
    {
        
        // Double check that input for race length is valid
        int parsedInput;
        if (int.TryParse(inputField.text, out parsedInput) && parsedInput > 0)
        {
            // Assign player choice to selected pawn
            if (pawn1Toggle.isOn)
            {
                playerChoice = "Pawn1";
            }
            else if (pawn2Toggle.isOn)
            {
                playerChoice = "Pawn2";
            }

            startMenu.SetActive(false);
            AssignTileCount();
            GenerateBoardTileTypes(TileCount);
            InstantiateTiles();
            InstantiatePawns();
            StartCoroutine(Game());
        }
        else
        {
            Debug.Log("Invalid Race Length");
        }
       
    }


    // Checks if input is valid, assigns TileCount from user input
    private void AssignTileCount()
    {
        int parsedInput;
        if (int.TryParse(inputField.text, out parsedInput) && parsedInput > 0)
        {
            TileCount = parsedInput + 1;
        }
        else
        {
            Debug.Log("Invalid Race Length, input must be a natural number. Setting to Default of 10.");
            TileCount = 12;
        }
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
            outputText.text = "Player 2 won after " + TurnCount + " turns!";
            Debug.Log("Player 2 won after " + TurnCount + " turns!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    // Create tiles for race, one for each player
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


    // Create pawns at tile 0
    public void InstantiatePawns()
    {
        Vector3 position1 = new Vector3(0.0f, 0.5f, 0f);
        Vector3 position2 = new Vector3(1.0f, 0.5f, 0f);

        pawn1 = Instantiate(Pawn1Prefab, position1, Quaternion.identity);
        pawn2 = Instantiate(Pawn2Prefab, position2, Quaternion.identity);
        leader = pawn1.transform;
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
                outputText.text = playerName + " moved to tile: " + playerIndex;
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
                outputText.text = playerName + " moved to tile: " + playerIndex;
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
        UpdateCamera();
    }

    // Checks is winning pawn was players' choice
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
        ResetCamera();
    }

    // Move camera to match leading pawn
    private void UpdateCamera()
    {
        // Check if the leader has changed by comparing their positions along the z-axis
        if (pawn1.transform.position.z > pawn2.transform.position.z)
        {
            if (pawn1.transform.position.z > leader.position.z)
            {
                leader = pawn1.transform;
            }
        }
        else
        {
            if (pawn2.transform.position.z > leader.position.z)
            {
                leader = pawn2.transform;
            }
        }

        Vector3 targetCamPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, leader.position.z);
        mainCamera.transform.position = targetCamPos;
    }


    // Move Camera back to origin
    private void ResetCamera()
    {
        mainCamera.transform.position = new Vector3(5f, 2.5f, 0f);
    }




}
