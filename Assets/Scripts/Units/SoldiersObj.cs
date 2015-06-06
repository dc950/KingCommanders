using UnityEngine;
using System.Collections.Generic;

public class SoldiersObj : MonoBehaviour {

    public Soldier soldier;
    StateController sc;

    public UnitBuilding target;
    public float curCooldown, cooldown;

    
    // Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
        cooldown = 0.01f;
        curCooldown = 0;
        ObjectDictionary.getDictionary().unitColliders.Add(GetComponent<BoxCollider>());
	}

	// Update is called once per frame
	void Update () {
        if (sc.state == StateController.states.Attacking)
        {
            if(target != null)
            {
                attack();
            }
            else
            {
                move();
            }
            
            //soldier.ShowLine();
        }
	}

    void attack()
    {
        Debug.Log("attacking: cooldown at "+curCooldown+", t.dt = "+Time.deltaTime);
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

     void move()
    {
         if(soldier.underAttack != null) //nothing in the way but under attack from blocking unit (e.g. soldiers) - fight back
         {
             target = soldier.underAttack;
         }

         //Check if need to move
        if(soldier.path.Count < 2)
        {
            //Debug.Log("Count < 2 (1)");
            return;
        }
        //Check if at next tile
        if(atNextPosition())
        {
            //Debug.Log("Removoing path point: " + soldier.path[0].x + "," + soldier.path[0].y);
            soldier.curTile.unit = null;
            soldier.path.RemoveAt(0);
            soldier.curTile = soldier.path[0];
            soldier.curTile.unit = soldier;
        }
        //Check if still need to move
        if (soldier.path.Count < 2)
        {
            //Debug.Log("Count < 2 (2)");
            return;
        }

        Tile nextTile = soldier.path[1];

        //See if the next tile is free
        if(nextTile.building != null || nextTile.unit != null)
        {
            //Tile is not free: attack or stop - buildings are attacked first
            if (soldier.path[1].building != null) //building
            {
                if (nextTile.building.owner != soldier.owner)  //Enemy building, attack!
                {
                    target = nextTile.building;
                    return;
                }
                else //building is owned by player
                {
                    //TODO: walk through building if possible, deppending on building type
                }
            }
            else //unit
            {
                if(nextTile.unit.owner != soldier.owner) 
                {
                    target = nextTile.unit;
                    return;
                }
                else
                {
                    return;
                }
            }

         
        }

        Vector3 moveTarget = new Vector3(0,0,0);

        moveTarget.x = soldier.path[1].getWorldCoords().x;
        moveTarget.z = soldier.path[1].getWorldCoords().z;
        //Debug.Log("Target: "+ soldier.path[1].x +","+soldier.path[1].y+"      Position: " + target.x + "," + target.y);
        this.transform.position = Vector3.MoveTowards(transform.position, moveTarget, soldier.speed / 1000);
    }


    bool atNextPosition()
    {
        if (transform.position.x == soldier.path[1].getWorldCoords().x && transform.position.z == soldier.path[1].getWorldCoords().z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
    void OnMouseDown()
    {
        ObjectDictionary.getStateController().UnitClicked(soldier);
    }




}
