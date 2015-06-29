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
        base.Update();
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

     public override void move()
    {

         if(soldier.checkIfUnderAttack()) //see if under attack
         {
             //Debug.Log("Under attack...");
             target = soldier.underAttack;
             soldier.underAttack = null;
         }

         //Check if need to move
        if(soldier.path.Count < 2)
        {
            //Debug.Log("Count < 2 (1)");
            moving = false;
            return;
        }
        else if(!moving)
        {
            moving = true;
        }
        //Check if at next tile
        if (!halfway)
        {
            if (CheckHalfway() && soldier.path.Count > 1)
            {
                //Debug.Log("At half way");

                Tile nextTile = soldier.path[1];

                //See if the next tile is free
                if (nextTile.building != null || nextTile.unit != null)
                {
                    //Tile is not free: attack or stop - buildings are attacked first
                    if (nextTile.building != null) //building
                    {
                        if (nextTile.building.owner != soldier.owner)  //Enemy building, attack!
                        {
                            //Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n but enemy building at ("+nextTile.x+","+nextTile.y+")");
                            target = nextTile.building;
                            return;
                        }
                        else //building is owned by player
                        {
                            //Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n Friendly building at (" + nextTile.x + "," + nextTile.y + ")");

                            //TODO: walk through building if possible, deppending on building type
                        }
                    }
                    else //unit
                    {
                        if (nextTile.unit.owner != soldier.owner)
                        {
                            //Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n but enemy unit at (" + nextTile.x + "," + nextTile.y + "), attacking!");

                            target = nextTile.unit;
                            nextTile.unit.underAttack = soldier;
                            return;
                        }
                        else
                        {
                            //Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n but friendly unit at (" + nextTile.x + "," + nextTile.y + ")");

                            string s = "";

                            foreach(Tile t in soldier.path)
                            {
                                s += "("+t.x+","+t.y+")"+", ";
;                            }

                            //Debug.Log("Path is " + s + " and curTile is "+ soldier.curTile);
                            return;
                        }
                    }
                }

                soldier.curTile.unit = null;
                soldier.curTile = soldier.path[1];
                soldier.curTile.unit = soldier;
                halfway = true;

                //Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n path is free! curTile = ("+soldier.curTile.x+","+soldier.curTile.y+") , path[0] = "+soldier.path[0].x+","+soldier.path[0].y);

            }
        }
        if(atNextPosition())
        {
            //Debug.Log("Removoing path point: " + soldier.path[0].x + "," + soldier.path[0].y);

            

            soldier.path.RemoveAt(0);
            halfway = false;
            
            //Check if still need to move
            if (soldier.path.Count < 2)
            {
                //Debug.Log("Count < 2 (2)");
                moving = false;
                return;
            }
            else if(!moving)
            {
                moving = true;
            }

            findNewFacing();
            setForward();

            //Debug.Log("At next pos for unit owned by player " + soldier.owner + "\n curTile = (" + soldier.curTile.x + "," + soldier.curTile.y + ") , path[1] = " + soldier.path[1].x + "," + soldier.path[1].y);
        }
        
        Vector3 moveTarget = new Vector3(0,0,0);
        

        moveTarget.x = soldier.path[1].getWorldCoords().x;
        moveTarget.z = soldier.path[1].getWorldCoords().z;

        //transform.LookAt(moveTarget);


        //Debug.Log("Target: "+ soldier.path[1].x +","+soldier.path[1].y+"      Position: " + target.x + "," + target.y);
        this.transform.position = Vector3.MoveTowards(transform.position, moveTarget, soldier.speed / 1000);
    }




    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
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
