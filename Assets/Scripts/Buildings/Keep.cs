using UnityEngine;
using System.Collections;

public class Keep : Spawner {

    public Keep (Tile tile, Player owner) : base(1000, tile, owner)
    {
        setSpawnTile();
        name = "Keep";
    }
	
	public void spawnUnit(Unit unit)
    {

    }

    public override void PlaceObject()
    {
        int angleNo;
        if(owner.playerNumber==1)
        {
            angleNo = 90;
        }
        else
        {
            angleNo = 270;
        }

        Vector3 angle = new Vector3(0, angleNo, 0);
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().keep, tile.getWorldCoords(), Quaternion.Euler(angle));

        ubObject.GetComponent<KeepObj>().Initialize(this);
    }

	void setSpawnTile()
    {
        if(owner.playerNumber==1) 
        {
            spawnTile = tile.neighbours["E"];
        }
        else
        {
            spawnTile = tile.neighbours["W"];           
        }
        spawnTile.spawnTile = true;
    }
}
