using UnityEngine;
using System.Collections.Generic;


public class ArcherObj : UnitObj {

    public bool overwatch;
    public GameObject arrow;
    Archer archer;
    

	// Use this for initialization
	void Start () {
        base.initialise();
        archer = (Archer)unit;
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
        if(target != null)
        {
            //check if target is still in valid pos
            return;
        }

        //check each tile for an enemy
        Tile tar = unit.path[1];
        Tile cur = unit.path[0];
        int range = archer.range;
        List<Tile> tiles = new List<Tile>();
        string dir = "";

        if(tar.y > cur.y)
        {
            dir = "N";  
        }
        else if(tar.y < cur.y)
        {
            dir = "S";
        }
        else if(tar.x > cur.x)
        {
            dir = "E";
        }
        else if(tar.x < cur.x)
        {
            dir = "W";
        }
        else
        {
            Debug.LogError("overwatch direction not detected.  This shouldn't happen...");
        }



        tiles.Add(cur.neighbours[dir]);
        for (int i = 1; i < range; i++)
        {
            tiles.Add(tiles[tiles.Count - 1].neighbours[dir]);
        }

        string s = "";
        foreach(Tile t in tiles)
        {
            s += "("+t.x+","+t.y+"), ";
        }
        Debug.Log("Tiles: "+s);
        foreach(Tile t in tiles)
        {
            if(t.unit != null)
            {
                if(t.unit.owner != ObjectDictionary.getTurnController().getCurrentPlayer())
                {
                    target = t.unit;
                    Debug.Log("Enemy detected...");
                    break;
                }
            }
        }






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
