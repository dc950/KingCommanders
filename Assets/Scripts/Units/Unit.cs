using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : UnitBuilding {

    public float speed = 5;
    public Tile curTile;
    public List<Tile> path;
    public int owner;
    public Unit underAttack;

    public Unit(int maxHealth, Tile tile, int amount) : base(maxHealth)
    {
        underAttack = null;
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
        ObjectDictionary.getDictionary().unitColliders.Remove(ubObject.GetComponent<BoxCollider>());
        MonoBehaviour.Destroy(ubObject);
    }

    public override void Destroy()
    {
        curTile.unit = null;
        DeleteObject();
    }

    public abstract void Attack(UnitBuilding target);


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
            ShowLine();
        }
    }

    public void AddToPath(Tile targetTile)
    {
        //If path contains tile, move back to it
        if(path.Contains(targetTile))
        {
            path.RemoveRange(path.IndexOf(targetTile), path.Count - path.IndexOf(targetTile));
            //for(int i = path.Count; i >= path.IndexOf(targetTile); i-- )
        }

        if (path.Count >= 1)
        {
            if (!path[path.Count - 1].neighbours.ContainsValue(targetTile))
            {
                List<Tile> newPath = ObjectDictionary.getTileMap().findPath(path[path.Count-1], targetTile);
                path.AddRange(newPath);
            }
        }

        //Debug.Log("Adding " + tile.x + "," + tile.y);
        path.Add(targetTile);
    }


}
