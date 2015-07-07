using UnityEngine;
using System.Collections;

public class Archer : Unit {


    public float strength = 3;

    public Archer(Tile tile, Player owner) :base(500, tile, owner)
    {
        ubType = UBType.infantry;
    }


    public override void PlaceObjects()
    {
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().archer, curTile.getWorldCoords(), Quaternion.identity);
        displayHealthBar();
    }

    public override void Attack(UnitBuilding target)
    {
        float damage = (strength * (getHealth() / getMaxHealth())) * 10;

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
        }

        if (damage < 1)
        {
            damage = 1;
        }

        target.takeDamage(damage);
    }
}
