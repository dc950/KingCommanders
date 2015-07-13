using UnityEngine;
using System.Collections;


public class ArcherObj : UnitObj {

    bool overwatch;
    public GameObject arrow;

	// Use this for initialization
	void Start () {
        base.initialise();
	}
	
	// Update is called once per frame
	void Update () {
        base.DoUpdate();

        if (ObjectDictionary.getStateController().state == StateController.states.Attacking) 
        {
            if (overwatch)
            {
                Overwatch();
            }
        }
       

	}

    void Overwatch()
    {

    }

    public override void attack()
    {
        //if target not in range, remove target
        if (target.getHealth() <= 0)
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
