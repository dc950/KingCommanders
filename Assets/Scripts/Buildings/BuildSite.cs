using UnityEngine;
using System.Collections;

public class BuildSite : Building {

    public Building toBuild;

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

    public void Build()
    {
        if (toBuild == null)
        {
            Debug.LogError("no building assigned");
            return;
        }

        //Make sure no enemies are nearby or on this tile or something

        toBuild.Initialise();
        MonoBehaviour.Destroy(buildingObject);
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
