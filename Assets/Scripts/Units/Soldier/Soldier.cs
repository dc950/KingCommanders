using UnityEngine;
using System.Collections;

public class Soldier : Unit {

    public int strength = 3;

	public Soldier(Tile tile, Player owner) : base(1000, tile, owner)
    {

    }

    public override void PlaceObjects()
    { 
        ubObject = (GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().soldier, curTile.getWorldCoords(), Quaternion.identity);
        displayHealthBar();
    }
            
    public override void Attack(UnitBuilding target)
    {
        //Change object animations

        float damage = (float) strength * ((float)getHealth() / (float)getMaxHealth());
        if(damage < 1)
        {
            damage = 1;
        }

        //Debug.Log("Dealing damage: " + damage);

        target.takeDamage((int)damage);
    }
}
