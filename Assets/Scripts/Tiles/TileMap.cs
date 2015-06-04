using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour 
{
	public Tile[,] tiles;
	public TileType[] tileTypes;
	public GameObject mainCamera;
	
	public int mapSizeX, mapSizeY;


	// Use this for initialization
	void Start () {
		PlaceCamera();
		CreateGrid();
        setNeightbours();
        

        ObjectDictionary.getTurnController().SetupPlayers();
        ObjectDictionary.getDictionary().setUpNames();
        PlaceKeeps();

	}
	
	void CreateGrid() {
		
		tiles = new Tile[mapSizeX, mapSizeY];
		

        //Place tiles
		for(int x = 0; x < mapSizeX; x++) {
			for(int y = 0; y < mapSizeY; y++) {
				tiles[x,y] = new Tile(x, y, tileTypes[0], this);
			}
		}

        //Set ownership
        //TODO: Maybe more advanced here for bigger NML on bigger maps?
        int botMid = 0;
        int topMid = 0;

        if(mapSizeX % 2 == 0)
        {
            topMid = (int)mapSizeX/2;
            botMid = topMid - 1;
        }
        else 
        {
            topMid = Mathf.CeilToInt(mapSizeX / 2);
            botMid = topMid;
        }

        for (int x = 0; x < botMid; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y].owner = 1;
            }
        }


        for (int x = topMid+1; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y].owner = 2;
            }
        }
	}

    void setNeightbours()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y].neighbours = new Dictionary<string, Tile>();

                //North tile
                if(x < mapSizeX-1)
                {
                    tiles[x, y].neighbours.Add("E", tiles[x + 1, y]);
                }
                //South tile
                if (x > 0)
                {
                    tiles[x, y].neighbours.Add("W", tiles[x - 1, y]);
                }
                //West tile
                if (y > 0)
                {
                    tiles[x, y].neighbours.Add("S", tiles[x, y-1]);
                }
                //East tile
                if (y < mapSizeY-1)
                {
                    tiles[x, y].neighbours.Add("N", tiles[x, y + 1]);
                }
            }
        }
    }

	void PlaceCamera()
	{
		float x, y;
		
		//set values like this or wierd rounding errors occur
		x = mapSizeX-1;
		x = x/2;
		y = -2;
		
		mainCamera.transform.position = new Vector3(x, 10, y);
		
	}

	public static Vector3 CoordsToWorld(int x, int y)
	{
		return new Vector3(x, 0, y);
	}

	// Update is called once per frame
	void Update () {
	
	}

    public Tile getStartPos(Player player)
    {
        if (player.playerNumber == 1)
        {
            return tiles[0, (int)Mathf.Floor(mapSizeY / 2)];
        }
        else
        {
            return tiles[mapSizeX - 1, (int)Mathf.Floor(mapSizeY / 2)];
        }
    }

    public Tile getStartPos(int playerNo)
    {
        TurnController tc = ObjectDictionary.getTurnController();
        Player player = tc.players[playerNo];
        return getStartPos(player);
    }

    void PlaceKeeps()
    {
        TurnController tc = ObjectDictionary.getTurnController();
        Player player1 = tc.players[1];
        Player player2 = tc.players[2];

        Tile tile1 = getStartPos(player1);
        Tile tile2 = getStartPos(player2);

        ObjectDictionary.makeNewBuilding("Keep", tile1, player1);
        ObjectDictionary.makeNewBuilding("Keep", tile2, player2);
    }
}
