using UnityEngine;
using System.Collections;

public class Mine : Building {

    public int value = 10;

    public Mine(Tile tile, Player owner) : base(100, tile, owner)
    {
        owner.mines.Add(this);
        name = "Mine";
    }

    public int getMoney()
    {
        return value;
    }

    public void increaseValue(int amount)
    {
        value += amount;
    }

    public void decreaseValue(int amount)
    {
        value -= amount;
    }

    public void setValue(int amount)
    {
        value = amount;
    }

    public override void PlaceObject()
    {
        int angle = Random.Range(140, 220);
        ubObject = (GameObject) MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().mine, tile.getWorldCoords(), Quaternion.Euler(new Vector3(0, angle, 0)));
        ubObject.GetComponent<BuildingObj>().Initialize(this);
    }

    public override void Destroy()
    {
        owner.mines.Remove(this);
        base.Destroy();
    }

}
