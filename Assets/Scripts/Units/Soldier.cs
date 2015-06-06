using UnityEngine;
using System.Collections;

public class Soldier : Unit {

    public int strength = 3;

	public Soldier(Tile tile) : base(10, tile, 3)
    {

    }

    public override void Attack(UnitBuilding target)
    {
        //Change object animations

        target.takeDamage(strength);
    }
}
