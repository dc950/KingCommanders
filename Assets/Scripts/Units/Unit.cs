using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : UnitBuilding {

    public Tile curTile;
    public List<Tile> path;
    public List<actions> pathAction;
    public Unit underAttack;

    public enum actions {walk, run, overwatch, catapult};

    //Combat modifiers
    public float speed = 5;
    public float stoneMod = 1;
    public float infantryMod = 1;
    public float woodMod = 1;
    public float critChance = 5;
    public float critMod = 2;

    public GameObject line;
    LineRenderer lineRend;
    List<GameObject> lines;
    bool pathDisplayed = false;

    public Unit(int maxHealth, Tile tile, Player owner) : base(maxHealth, owner)
    {
        underAttack = null;
        curTile = tile;
        path = new List<Tile>();
        pathAction = new List<actions>();
        path.Add(curTile);
        pathAction.Add(actions.walk);

        ObjectDictionary.getTurnController().newUnits.Add(this);
        lines = new List<GameObject>();

    }

    public void Initialise()
    {
        PlaceObjects();
        line = ObjectDictionary.getDictionary().line;
        lineRend = line.GetComponent<LineRenderer>();
    }

    public abstract void PlaceObjects();


    public override void DeleteObject()
    {
        Debug.Log("Deleting object...");
        if (ubObject != null)
        {
            if (ubObject.GetComponent<BoxCollider>())
            {
                ObjectDictionary.getDictionary().unitColliders.Remove(ubObject.GetComponent<BoxCollider>());
            }
        }
        if(getHealth() <= 0)
        {
            //dead - destroy last unitchar...
            if(ubObject != null)
            {
                if(ubObject.GetComponent<UnitObj>().unitChars[0] != null)
                {
                    ubObject.GetComponent<UnitObj>().unitChars[0].Die();
                }
            }
            
        }

        MonoBehaviour.Destroy(ubObject);
    }

    public override void Destroy()
    {
        curTile.unit = null;
        DeleteObject();
    }

    public abstract void Attack(UnitBuilding target);

    public void displayLine()
    {
        pathDisplayed = true;

        for (int i = 0; i < path.Count-1; i++)
        {
            GameObject curLine = (GameObject) MonoBehaviour.Instantiate(line, path[i].getWorldCoords(), Quaternion.identity);
            lines.Add(curLine);


            LineRenderer lr = curLine.GetComponent<LineRenderer>();

            lr.SetPosition(0, path[i].getWorldCoords());
            lr.SetPosition(1, path[i + 1].getWorldCoords());

            Color color;

            if (pathAction[i+1] == actions.walk)
                color = Color.red;
            else if (pathAction[i+1] == actions.run)
                color = Color.green;
            else
                color = Color.grey;

            lr.SetColors(color, color);

        }
    }
    public void hideLine()
    {
        pathDisplayed = false;

        foreach(GameObject go in lines)
        {
            MonoBehaviour.Destroy(go);
            //lines.Remove(go);
            
        }
    }

    public void ShowLine()
    {
        /*
        lineRend.SetVertexCount(path.Count);
        for(int i = 0; i < path.Count; i++)
        {
            lineRend.SetPosition(i, path[i].getWorldCoords());
        } */

        
       if(!pathDisplayed)
           displayLine();
        


        
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

    void RefreshLine()
    {
        hideLine();
        displayLine();
    }

    public void RemoveFromPath(Tile targetTile)
    {

        int indexStart, indexEnd;

        indexStart = path.IndexOf(targetTile) + 1;
        indexEnd = path.Count - path.IndexOf(targetTile) - 1;

        path.RemoveRange(indexStart, indexEnd);
        pathAction.RemoveRange(indexStart, indexEnd);

        //showList();
        RefreshLine();
    }

    void showList()
    {
        string s = "";
        foreach (Tile t in path)
        {
            s += ", (" + t.x + "," +t.y+")";
        }
        s = s.Remove(0, 2);
        Debug.Log(s);
        
        s = "";
        foreach(actions a in pathAction)
        {
            s += ", " + a;
        }
        s = s.Remove(0, 2);
        Debug.Log(s);

        Debug.Log("Length of path: " + path.Count + ", Length of pathAction: " + pathAction.Count);
    }

    public void AddToPath(Tile targetTile, actions action)
    {
        //If path contains tile, move back to it
        if(path.Contains(targetTile) && path.Count > 1 && targetTile != path[path.Count-1]) //we can remove so long as path is greater than 1 and not last bit in path is selected
        {
            
        }

        if (!path[path.Count - 1].neighbours.ContainsValue(targetTile) && (action == actions.run || action == actions.walk))
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

        //showList();
        RefreshLine();
    }

    public bool checkIfUnderAttack()
    {
        return underAttack != null;
    }


    //Hide and show methods for right after spawning - will not work if not exactly on tile
    public void hide()
    {
        ubObject.transform.position = new Vector3(0, 0, -100);
    }
    public void show()
    {
        Vector3 coords =  curTile.getWorldCoords();
        ubObject.transform.position = coords;
    }

}
