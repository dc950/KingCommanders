using UnityEngine;
using System.Collections;

public class Archer : Unit {


    public float strength = 3;
    public int range = 3;

    public Archer(Tile tile, Player owner) :base(70, tile, owner)
    {
        ubType = UBType.infantry;
        name = "Archer";

        infantryMod = 1.5f;
        stoneMod = 0.25f;
        woodMod = 0.5f;

    }


    public override void PlaceObjects()
    {
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().archer, curTile.getWorldCoords(), Quaternion.identity);
        displayHealthBar();
    }

    public override void Attack(UnitBuilding target)
    {
        float damage = (strength * (getHealth() / getMaxHealth())) * 5;

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
