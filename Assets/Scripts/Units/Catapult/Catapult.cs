using UnityEngine;
using System.Collections;

public class Catapult : Unit {

    float strength = 1;
    

	public Catapult(Tile tile, Player owner) : base(100, tile, owner)
    {
        ubType = UBType.wood;
        name = "Catapult";

        speed = 4;

        stoneMod = 4;
        woodMod = 5;
        infantryMod = 0.5f;
    }

    public override void PlaceObjects()
    {
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().catapult, curTile.getWorldCoords(), Quaternion.identity);
        displayHealthBar();
    }

    public override void Attack(UnitBuilding target)
    {
        float damage = (strength * (getHealth() / getMaxHealth())) * 25;

        if (target.ubType == UBType.infantry)
        {
            damage *= infantryMod;
        }
        else if (target.ubType == UBType.stone)
        {
            damage *= stoneMod;
        }
        else if (target.ubType == UBType.wood)
        {
            damage *= woodMod;
        }

        if (damage < 1)
        {
            damage = 1;
        }

        target.takeDamage(damage);
    }
}
