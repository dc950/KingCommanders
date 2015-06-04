using UnityEngine;
using System.Collections;

public class BuildSite : Building {

    public Building toBuild;
    public bool buildTurn = false;

    public BuildSite(Tile tile) : base(1, tile)
    {

    }

    public override void PlaceObject()
    {
        Vector3 angle = new Vector3(90, 0, 0);
        buildingObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().buildSite, tile.getWorldCoords(), Quaternion.Euler(angle));

        buildingObject.GetComponent<BuildSiteObj>().buildSite = this;
    }

    public void AssignBuilding(Building building)
    {
        toBuild = building;
    }

    public void Advance()
    {
        if(!buildTurn)
        {
            buildTurn = true;
        }
        else
        {
            build();
        }

        //TODO: Update asset to make playing easier (differentiate between what is to be placed and what is already placed)
    }

    public void build()
    {
        if(toBuild==null)
        {
            Debug.LogError("no building assigned");
            return;
        }

        if (buildTurn)
        {
            toBuild.Initialise();
            MonoBehaviour.Destroy(buildingObject);
        }
    }

    public void hide()
    {
        buildingObject.transform.position = new Vector3(0, 0, -100);
    }

    public void show()
    {
        Vector3 coords = tile.getWorldCoords();
        buildingObject.transform.position = coords;
    }
}
