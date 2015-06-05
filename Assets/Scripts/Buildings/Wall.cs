using UnityEngine;
using System.Collections;

public class Wall : Building {

    public Wall(Tile tile) : base(10, tile)
    {

    }

    public override void PlaceObject()
    {
        wallConnect = true;
        PlaceObject(false);
    }

	private void PlaceObject(bool updating)
    {
        //bools say whether there is a wall in corresponding neighbour tile
        bool n, s, w, e;

        //discover if there are neighbours
        n = tile.neighbours.ContainsKey("N");
        s = tile.neighbours.ContainsKey("S");
        w = tile.neighbours.ContainsKey("W");
        e = tile.neighbours.ContainsKey("E");

        //discover if they have buildings
        if (n)
            n = tile.neighbours["N"].building != null;
        if (s)
            s = tile.neighbours["S"].building != null;
        if (w)
            w = tile.neighbours["W"].building != null;
        if (e)
            e = tile.neighbours["E"].building != null;
       

        //see if buildings are walls
        if(n)
            n = (tile.neighbours["N"].building.wallConnect);
        if(s)
            s = (tile.neighbours["S"].building.wallConnect);
        if(w)
            w = (tile.neighbours["W"].building.wallConnect);
        if(e)
            e = (tile.neighbours["E"].building.wallConnect);

        //Debug.Log("N: " + n + "E: " + e + "S: " + s + "W: " + w);
        
        //***********************************************
        //no wall neighbours - build a single
        //***********************************************
        if (!n && !s && !w && !e)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallSingle, tile.getWorldCoords(), Quaternion.Euler(angle));
        }


        //***********************************************
        //one wall on one side - build an end wall
        //***********************************************
        else if(n && !s && !w && !e)
        {
            Vector3 angle = new Vector3(270, 180, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallEnd, tile.getWorldCoords(), Quaternion.Euler(angle));
            if(!updating)
                UpdateNeighbour("N");
        }

        else if (!n && s && !w && !e)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallEnd, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
                UpdateNeighbour("S");
        }

        else if (!n && !s && w && !e)
        {
            Vector3 angle = new Vector3(270, 90, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallEnd, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
                UpdateNeighbour("W");
        }

        else if (!n && !s && !w && e)
        {
            Vector3 angle = new Vector3(270, 270, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallEnd, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
                UpdateNeighbour("E");
        }




        //***********************************************
        //Two adjacent opposite neighbours have walls : build a straight wall
        //***********************************************
        else if (n && s && !w && !e)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallStraight, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("N");
                UpdateNeighbour("S");
            }
        }
        else if (!n && !s && w && e)
        {
            Vector3 angle = new Vector3(270, 90, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallStraight, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("W");
                UpdateNeighbour("E");
            }
        }


        //***********************************************
        //Two adjacent cornering neighbuors have walls : build a corner
        //***********************************************
        else if (n && !s && !w && e)
        {
            Vector3 angle = new Vector3(270, 270, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallCorner, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("N");
                UpdateNeighbour("E");
            }
        }
        else if (!n && s && !w && e)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallCorner, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("E");
                UpdateNeighbour("S");
            }
        }
        else if (!n && s && w && !e)
        {
            Vector3 angle = new Vector3(270, 90, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallCorner, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("S");
                UpdateNeighbour("W");
            }
        }
        else if (n && !s && w && !e)
        {
            Vector3 angle = new Vector3(270, 180, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallCorner, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("N");
                UpdateNeighbour("W");
            }
        }


        //***********************************************
        //Three adjacent neighbours have walls : build a T
        //***********************************************
        else if (n && s && !w && e)
        {
            Vector3 angle = new Vector3(270, 270, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallT, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("N");
                UpdateNeighbour("E");
                UpdateNeighbour("S");
            }
        }
        else if (!n && s && w && e)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallT, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("E");
                UpdateNeighbour("S");
                UpdateNeighbour("W");
            }
        }
        else if (n && s && w && !e)
        {
            Vector3 angle = new Vector3(270, 90, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallT, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("S");
                UpdateNeighbour("W");
                UpdateNeighbour("N");
            }
        }
        else if (n && !s && w && e)
        {
            Vector3 angle = new Vector3(270, 180, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallT, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
            {
                UpdateNeighbour("W");
                UpdateNeighbour("N");
                UpdateNeighbour("E");
            }
        }


        //***********************************************
        //Four adjacent neighbours have walls : build a Cross
        //***********************************************
        else if (n && s && w && e)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().wallCross, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating) {
                UpdateNeighbour("N");
                UpdateNeighbour("W");
                UpdateNeighbour("S");
                UpdateNeighbour("E");
            }
        }
    }

    public void Update()
    {
        DeleteObject();
        PlaceObject(true);
    }

    void UpdateNeighbour(string s)
    {
        Wall w = (Wall) tile.neighbours[s].building;
        w.Update();
    }


}
