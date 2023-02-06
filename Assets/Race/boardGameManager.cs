using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

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
    private Transform[] Pawn1Line, Pawn2Line;





    // Start is called before the first frame update
    void Start()
    {
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

    IEnumerator Game()
    {
        while (playerIndex1 < TileCount - 1 && playerIndex2 < TileCount - 1)
        {
            DidPawnMove(ref playerIndex1, "Player 1");
            yield return new WaitForSeconds(TimeBetweenTurns);
            DidPawnMove(ref playerIndex2, "Player 2");
            yield return new WaitForSeconds(TimeBetweenTurns);
            TurnCount++;
        }
        if (playerIndex1 >= TileCount - 1)
        {
            Debug.Log("Player 1 won after " + TurnCount + " turns!");
        }
        else if (playerIndex2 >= TileCount - 1)
        {
            Debug.Log("Player 2 won after " + TurnCount + " turns!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

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

    void InstantiatePawns()
    {
        Pawn1Line = new Transform[TileCount];
        Pawn2Line = new Transform[TileCount];

        Pawn1Line[0] = Instantiate(Pawn1Prefab, new Vector3(-0.5f, 0, 0), Quaternion.identity).transform;
        Pawn2Line[0] = Instantiate(Pawn2Prefab, new Vector3(0.5f, 0, 0), Quaternion.identity).transform;
    }

    // Generates tile types for the board game
    private void GenerateBoardTileTypes(int tileCount)
    {
        // Clear board
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

    // Checks player moves ahead based on current tile type
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
                Debug.Log(playerName + " moved to tile: " + playerIndex);
            }
        }
        else if (boardTileTypes[playerIndex] == "mud")
        {
            System.Random rand = new System.Random();
            int randomInt = rand.Next(0, 8);
            if (randomInt == 0)
            {
                playerIndex++;
                Debug.Log(playerName + " moved to tile: " + playerIndex);
            }
        }
    }

}
