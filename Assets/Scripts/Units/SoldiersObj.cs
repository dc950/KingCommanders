using UnityEngine;
using System.Collections.Generic;

public class SoldiersObj : MonoBehaviour {

    public Soldier soldier;
    StateController sc;

    public UnitBuilding target;
    public float curCooldown, cooldown;
    bool halfway = false;

    
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

     void move()
    {
         if(soldier.underAttack != null) //nothing in the way but under attack from blocking unit (e.g. soldiers) - fight back
         {
             Debug.Log("Under attack...");
             target = soldier.underAttack;
             soldier.underAttack = null;
         }

         //Check if need to move
        if(soldier.path.Count < 2)
        {
            //Debug.Log("Count < 2 (1)");
            return;
        }
        //Check if at next tile
        if (!halfway)
        {
            if (CheckHalfway() && soldier.path.Count > 1)
            {
                Debug.Log("At half way");

                Tile nextTile = soldier.path[1];

                //See if the next tile is free
                if (nextTile.building != null || nextTile.unit != null)
                {
                    //Tile is not free: attack or stop - buildings are attacked first
                    if (nextTile.building != null) //building
                    {
                        if (nextTile.building.owner != soldier.owner)  //Enemy building, attack!
                        {
                            Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n but enemy building at ("+nextTile.x+","+nextTile.y+")");
                            target = nextTile.building;
                            return;
                        }
                        else //building is owned by player
                        {
                            Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n Friendly building at (" + nextTile.x + "," + nextTile.y + ")");
                            //TODO: walk through building if possible, deppending on building type
                        }
                    }
                    else //unit
                    {
                        if (nextTile.unit.owner != soldier.owner)
                        {
                            Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n but enemy unit at (" + nextTile.x + "," + nextTile.y + "), attacking!");

                            target = nextTile.unit;
                            nextTile.unit.underAttack = soldier;
                            return;
                        }
                        else
                        {
                            Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n but friendly unit at (" + nextTile.x + "," + nextTile.y + ")");

                            string s = "";

                            foreach(Tile t in soldier.path)
                            {
                                s += "("+t.x+","+t.y+")"+", ";
;                            }

                            Debug.Log("Path is " + s + " and curTile is "+ soldier.curTile);
                            return;
                        }
                    }
                }

                soldier.curTile.unit = null;
                soldier.curTile = soldier.path[1];
                soldier.curTile.unit = soldier;
                halfway = true;

                Debug.Log("At halfway for unit owned by player " + soldier.owner + "\n path is free! curTile = ("+soldier.curTile.x+","+soldier.curTile.y+") , path[0] = "+soldier.path[0].x+","+soldier.path[0].y);

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
                return;
            }

            Debug.Log("At next pos for unit owned by player " + soldier.owner + "\n curTile = (" + soldier.curTile.x + "," + soldier.curTile.y + ") , path[1] = " + soldier.path[1].x + "," + soldier.path[1].y);
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

    bool CheckHalfway()
    {
        //take away from smallest number
        //divide by largest
        //????

        float x, y;

        if(soldier.path[0].getWorldCoords().x > soldier.path[1].getWorldCoords().x)
        {
            x = Mathf.Abs(transform.position.x - soldier.path[1].getWorldCoords().x) / Mathf.Abs(soldier.path[0].getWorldCoords().x - soldier.path[1].getWorldCoords().x);
        }
        else
        {
            x = transform.position.x - soldier.path[0].getWorldCoords().x / (soldier.path[1].getWorldCoords().x - soldier.path[0].getWorldCoords().x);
        }

        if (soldier.path[0].getWorldCoords().z > soldier.path[1].getWorldCoords().z)
        {
            y = Mathf.Abs(transform.position.z - soldier.path[1].getWorldCoords().z) / Mathf.Abs(soldier.path[0].getWorldCoords().z - soldier.path[1].getWorldCoords().z);
        }
        else
        {
            y = transform.position.z - soldier.path[0].getWorldCoords().z / (soldier.path[1].getWorldCoords().z - soldier.path[0].getWorldCoords().z);
        }




        //Debug.Log("x: " + x + ", y: " + y);

        return x < 0.5 && y < 0.5;
    }
   
    void OnMouseDown()
    {
        ObjectDictionary.getStateController().UnitClicked(soldier);
    }




}
