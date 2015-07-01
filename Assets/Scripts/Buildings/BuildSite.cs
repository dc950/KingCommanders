using UnityEngine;
using System.Collections;

public class BuildSite : Building {

    public Building toBuild;

    public BuildSite(Tile tile, Player owner) : base(1, tile, owner)
    {

    }

    public override void PlaceObject()
    {
        Vector3 angle = new Vector3(90, 0, 0);
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().buildSite, tile.getWorldCoords(), Quaternion.Euler(angle));

        ubObject.GetComponent<BuildSiteObj>().buildSite = this;
    }

    public void AssignBuilding(Building building)
    {
        toBuild = building;
    }

    public void Build()
    {
        if (toBuild == null)
        {
            Debug.LogError("no building assigned");
            return;
        }

        if(tile.unit != null)
        {
            Debug.Log("Cannot build: unit in the way!");
            return;
        }
        else
        {
            foreach(Tile t in tile.neighbours.Values)
            {
                if(t.unit != null)
                {
                    if(t.unit.owner != owner)
                    {
                        Debug.Log("Cannot build! Enemy nearby!");
                        return;
                    }
                }
            }
        }

        //Make sure no enemies are nearby or on this tile or something

        toBuild.Initialise();
        MonoBehaviour.Destroy(ubObject);
    }


    public void hide()
    {
        ubObject.transform.position = new Vector3(0, 0, -100);
    }

    public void show()
    {
        Vector3 coords = tile.getWorldCoords();
        ubObject.transform.position = coords;
    }
}
