using UnityEngine;
using System.Collections;

public class Tower : WallCon {

    public float damage = 100; //damage per attack
    public float rateOfFire = 1; //number off attacks per second
    public Tower(Tile tile, Player owner) : base(5000, tile, owner)
    {

    }


    public void attack(UnitBuilding target)
    {
        target.takeDamage(damage);
    }


    public override void PlaceObject()
    {
        wallConnect = true;

        Vector3 angle = new Vector3(270, 0, 0);
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().tower, tile.getWorldCoords(), Quaternion.Euler(angle));

        PlaceObject(false);
        ubObject.GetComponent<TowerObj>().Initialise(this);
    }

    protected override void PlaceObject(bool updating)
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
        //build towerWalls
        //***********************************************
        if(n)
        {
            Vector3 angle = new Vector3(270, 90, 0);
            GameObject tWall = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().towerWall, tile.getWorldCoords(), Quaternion.Euler(angle));
            if(!updating)
                UpdateNeighbour("N");

            tWall.transform.SetParent(ubObject.transform);
        }

       if (s)
        {
            Vector3 angle = new Vector3(270, 270, 0);
            GameObject tWall = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().towerWall, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
                UpdateNeighbour("S");

            tWall.transform.SetParent(ubObject.transform);
        }

        if (w)
        {
            Vector3 angle = new Vector3(270, 0, 0);
            GameObject tWall = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().towerWall, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
                UpdateNeighbour("W");

            tWall.transform.SetParent(ubObject.transform);
        }

        if (e)
        {
            Vector3 angle = new Vector3(270, 180, 0);
            GameObject tWall = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().towerWall, tile.getWorldCoords(), Quaternion.Euler(angle));
            if (!updating)
                UpdateNeighbour("E");

            tWall.transform.SetParent(ubObject.transform);
        }


        //displayHealthBar();
    }

    void UpdateNeighbour(string s)
    {
        WallCon w = (WallCon) tile.neighbours[s].building;
        w.Update();
    }
}
