using UnityEngine;
using System.Collections.Generic;

public class Tile {

	public int x,y;
	public TileType tileType;
	public TileMap map;
	GameObject tileObj;
    public Building building;
    public Unit unit;
    public Dictionary<string, Tile> neighbours;
    public bool spawnTile = false;
    public int owner;

	public Tile(int x, int y, TileType tileType, TileMap map)
	{
		this.x = x;
		this.y = y;
		this.tileType = tileType;
		this.map = map;
		PlaceObject();
        building = null;
        unit = null;
	}

    public List<Tile> getAllNeighbours()
    {
        List<Tile> items = new List<Tile>();
        items.AddRange(neighbours.Values);
        return items;
    }

    public Vector3 getWorldCoords()
    {
        return TileMap.CoordsToWorld(x, y);
    }
	
	public void PlaceObject()
	{
        GameObject tileObj = (GameObject)MonoBehaviour.Instantiate(tileType.visualPrefab, getWorldCoords(), Quaternion.identity);
        ClickableTile ct = tileObj.GetComponent<ClickableTile>();
        ct.tile = this;
	}
}
