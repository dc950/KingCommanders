using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : UnitBuilding {

    public float speed = 5;
    public Tile curTile;
    public List<Tile> path;
    public int owner;

    public Unit(int maxHealth, Tile tile, int amount) : base(maxHealth)
    {
        curTile = tile;
        path = new List<Tile>();
        path.Add(curTile);
    }

    public void Initialise(int turn)
    {
        PlaceObjects();
        owner = turn; 
    }

    public void PlaceObjects()
    {
        ubObject = ((GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().soldier, curTile.getWorldCoords(), Quaternion.identity));
        displayHealthBar();
    }

    public override void DeleteObject()
    {
        MonoBehaviour.Destroy(ubObject);
    }

    public override void Destroy()
    {
        DeleteObject();
        curTile.units.Remove(this);
    }

    public abstract void Attack();


    public void ShowLine()
    {
        if (path != null)
        {
            if (path.Count != 0)
            {
                int currentTile = 0;
                int nextTile = 1;

                while (nextTile < path.Count)
                {
                    Vector3 start = TileMap.CoordsToWorld(path[currentTile].x, path[currentTile].y);
                    Vector3 end = TileMap.CoordsToWorld(path[nextTile].x, path[nextTile].y);

                    Debug.DrawLine(start, end, Color.blue);
                    currentTile = nextTile;
                    nextTile++;
                }
            }
        }
    }

    void Update()
    {
        if (ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            Debug.Log("gogog");
            ShowLine();
        }
    }
}
