using UnityEngine;
using System.Collections;

public class CatapultObj : UnitObj {

    public GameObject rock;
    Catapult catapult;

    public Tile tileTarget;

	// Use this for initialization
	void Start () {
        base.initialise();
        catapult = (Catapult)unit;
	}
	
	// Update is called once per frame
	void Update () {
        base.DoUpdate();
	}

    public override void attack()
    {

    }

    public override void EnemyUnitCollision(Tile tile)
    {
        target = tile.unit;
    }

    public override void EnemyBuildingCollision(Tile tile)
    {
        target = tile.building;
    }
}
