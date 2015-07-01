using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : UnitBuilding {

    public Tile curTile;
    public List<Tile> path;
    public List<actions> pathAction;
    public Unit underAttack;

    public enum actions { walk, run};

    //Combat modifiers
    public float speed = 5;
    public float stoneMod = 1;
    public float infantryMod = 1;



    public Unit(int maxHealth, Tile tile, Player owner) : base(maxHealth, owner)
    {
        underAttack = null;
        curTile = tile;
        path = new List<Tile>();
        pathAction = new List<actions>();
        path.Add(curTile);
        pathAction.Add(actions.walk);
    }

    public void Initialise()
    {
        PlaceObjects();
    }

    public abstract void PlaceObjects();


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

                    Color color;

                    if (pathAction[nextTile] == actions.walk)
                        color = Color.blue;
                    else if (pathAction[nextTile] == actions.run)
                        color = Color.green;
                    else
                        color = Color.grey;

                    Debug.DrawLine(start, end, color);
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

    public void RemoveFromPath(Tile targetTile)
    {
        path.RemoveRange((path.IndexOf(targetTile) + 1), path.Count - path.IndexOf(targetTile) - 1);
        return;
    }

    public void AddToPath(Tile targetTile, actions action)
    {
        //If path contains tile, move back to it
        if(path.Contains(targetTile) && path.Count > 1 && targetTile != path[path.Count-1]) //we can remove so long as path is greater than 1 and not last bit in path is selected
        {
            
        }

        if (!path[path.Count - 1].neighbours.ContainsValue(targetTile))
        {
            List<Tile> newPath = ObjectDictionary.getTileMap().findPath(path[path.Count-1], targetTile);
            for (int i = 0; i < newPath.Count; i++ )
            {
                pathAction.Add(action);
            }

            path.AddRange(newPath);
        }
        else
        {
            path.Add(targetTile);
            pathAction.Add(action);
        }        
    }

    public bool checkIfUnderAttack()
    {
        return underAttack != null;
    }

}
