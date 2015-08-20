using UnityEngine;
using System.Collections;

public abstract class Building : UnitBuilding{

    public Tile tile;
    public bool wallConnect = false;

    public Building(int maxHealth, Tile tile, Player owner) : base(maxHealth, owner)
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

        if(wallConnect)
        {
            foreach(Tile t in tile.neighbours.Values)
            {
                if(t.building != null)
                {
                    if(t.building.wallConnect)
                    {
                        WallCon wc = (WallCon)t.building;
                        wc.Update();
                    }
                }
            }
        }

    }

    public override void DeleteObject()
    {
        if(ubObject != null)
        {
            if(ubObject.GetComponent<BoxCollider>())
            {
                if (ObjectDictionary.getDictionary().unitColliders.Contains(ubObject.GetComponent<BoxCollider>()))
                {
                    ObjectDictionary.getDictionary().unitColliders.Remove(ubObject.GetComponent<BoxCollider>());
                }
                
            }
        }
        Object.Destroy(ubObject);
        ubObject = null;
    }
}
