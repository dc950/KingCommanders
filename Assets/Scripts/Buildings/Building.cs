using UnityEngine;
using System.Collections;

public abstract class Building : UnitBuilding{

    public Tile tile;
    public GameObject buildingObject;
    public bool wallConnect = false;

    public Building(int maxHealth, Tile tile) : base(maxHealth)
    {
        this.tile = tile;
        
        //PlaceObject(); --for some wierd reason, instantiated objects that are originally called from constructor won't actually spawn in... called in Initialise() instead
    }

    public abstract void PlaceObject();

    public void Initialise()
    {
        if (tile.building != null)
        {
            tile.building.Destroy();
        }
        tile.building = this;
        PlaceObject();
    }

    public override void Destroy()
    {
        DeleteObject();
        tile.building = null;
    }

    public override void DeleteObject()
    {
        Object.Destroy(buildingObject);
        buildingObject = null;
    }
}
