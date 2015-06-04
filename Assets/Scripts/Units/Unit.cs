using UnityEngine;
using System.Collections.Generic;

public abstract class Unit : UnitBuilding {

    public Tile curTile;
    public List<Tile> path;
    public GameObject unitObject;
    public int owner;

    public Unit(int maxHealth, Tile tile, int amount) : base(maxHealth)
    {
        curTile = tile;
        path = new List<Tile>();
    }

    public void Initialise(int turn)
    {
        PlaceObjects();
        owner = turn;
    }

    public void PlaceObjects()
    {
        unitObject = ((GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().soldier, curTile.getWorldCoords(), Quaternion.identity));
    }

    public override void DeleteObject()
    {
        MonoBehaviour.Destroy(unitObject);
    }

    public override void Destroy()
    {
        DeleteObject();
        curTile.units.Remove(this);
    }

    public abstract void Attack();


}
