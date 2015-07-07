using UnityEngine;
using System.Collections.Generic;


public class SoldiersObj : UnitObj {

    
    public float curCooldown, cooldown;
    public Soldier soldier;
    
    
    
    // Use this for initialization
	void Start () {
        base.initialise();
        cooldown = 0.005f;
        curCooldown = 0;

        soldier = (Soldier)unit;
 
	}

	// Update is called once per frame
	void Update () {
        base.DoUpdate();
	}

    public override void attack()
    {
        //Debug.Log("attacking: cooldown at "+curCooldown+", t.dt = "+Time.deltaTime);
        if (curCooldown <= 0)
        {
            soldier.Attack(target);
            curCooldown = cooldown;
        }
        else
        {
            curCooldown -= Time.deltaTime;
        }

        if(target.getHealth() <= 0)
        {
            target = null;
        }
    }



    public override void EnemyUnitCollision(Tile tile)
    {
        target = tile.unit;
        tile.unit.underAttack = unit;
    }

    public override void EnemyBuildingCollision(Tile tile)
    {
        target = tile.building;
    }
}
