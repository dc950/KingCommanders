using UnityEngine;
using System.Collections;

public class Soldier : Unit {

    public int strength = 3;

	public Soldier(Tile tile, Player owner) : base(1000, tile, owner)
    {

    }

    public override void Attack(UnitBuilding target)
    {
        //Change object animations

        target.takeDamage(strength);
    }
}
