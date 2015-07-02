using UnityEngine;
using System.Collections;

public class Soldier : Unit {

    public float strength = 3;

	public Soldier(Tile tile, Player owner) : base(1000, tile, owner)
    {
        ubType = UBType.infantry;
    }

    public override void PlaceObjects()
    { 
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().soldier, curTile.getWorldCoords(), Quaternion.identity);
        displayHealthBar();
    }
            
    public override void Attack(UnitBuilding target)
    {


        float damage = strength * (getHealth() / getMaxHealth());

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

        if(Random.Range(0,100) <= critChance)
        {
            damage *= critMod;
            Debug.Log("Critical hit!");
        }



        //Debug.Log("Player " + owner.playerNumber + " giving damage: " + damage);
        target.takeDamage((int)damage);
    }
}
