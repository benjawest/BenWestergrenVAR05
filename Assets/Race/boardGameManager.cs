using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardGameManager : MonoBehaviour
{
    public int TileCount = 10;
    public int playerIndex1;
    public int playerIndex2;
    public List<string> boardTileTypes;
    public int currentPlayerIndex;




    // Start is called before the first frame update
    void Start()
    {
        GenerateBoardTileTypes(TileCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // Generates tile types for the board game
    void GenerateBoardTileTypes(int tileCount)
    {
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

        // Debug: Print the content of boardTileTypes
        //Print the contents of boardTileTypes
        foreach (string tileType in boardTileTypes)
        {
            Debug.Log(tileType);
        }
    }


  
}
