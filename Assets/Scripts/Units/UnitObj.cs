using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class UnitObj : MonoBehaviour, IPointerClickHandler
{

    protected StateController sc;

    public Unit unit;

    public bool running = false;

    public UnitBuilding target;

    protected bool halfway = false;
    public bool moving = false;
    public string facingAngle;

    public List<UnitChar> unitChars;

	// Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
        ObjectDictionary.getDictionary().unitColliders.Add(GetComponent<BoxCollider>());
	}
	
    protected void initialise()
    {
        Start();
    }

	// Update is called once per frame
	protected void DoUpdate () {
        if (sc.state == StateController.states.Attacking)  //attack in progress
        {
            //Debug.Log("Moving: " + moving);
            if (!moving)
            {
                if (unit.path.Count > 2)
                {
                    findNewFacing();
                    setForward();
                }
            }


            if (target != null)
            {
                attack();
                moving = false;
            }
            else
            {
                moving = true;
                move();
            }

            float health = (float)unit.getHealth();
            float maxHealth = (float)unit.getMaxHealth();

            if ((health / maxHealth) * 100 < (unitChars.Count - 1) * 25)//TODO sort this shit
            {
                 //Debug.Log("Health: " + health + ", MaxHealth " + maxHealth + ", HealthDiv: " + health / maxHealth + "Health pct: " + (health / maxHealth) * 100 + ", comp to" + (unitChars.Count - 1) * 25 + "Where unitcount -1 is" + (unitChars.Count - 1));
                //choose random dude to destroy
                int num = Random.Range(0, unitChars.Count - 1);
                //Debug.Log("Going to destroy " + num);
                UnitChar toDestroy = unitChars[num];
                unitChars.Remove(toDestroy);
                toDestroy.Die();
            }

            //unit.ShowLine();
        }
        else if (moving)
        {
            moving = false;
        }

	}

    protected void setForward()
    {
        foreach (UnitChar sc in unitChars)
        {
            sc.facing = facingAngle;
            sc.setNewFormPos();
        }
    }

    protected void findNewFacing()
    {
        bool n, s, w, e;

        //discover if there are neighbours
        n = unit.path[1].neighbours.ContainsKey("N");
        s = unit.path[1].neighbours.ContainsKey("S");
        w = unit.path[1].neighbours.ContainsKey("W");
        e = unit.path[1].neighbours.ContainsKey("E");

        //discover if they have buildings
        if (n)
        {
            if (unit.path[0].neighbours["N"] == unit.path[1])
            {
                facingAngle = "N";
                return;
            }
        }
        if (s)
        {
            if (unit.path[0].neighbours["S"] == unit.path[1])
            {
                facingAngle = "S";
                return;
            }
        }
        if (w)
        {
            if (unit.path[0].neighbours["W"] == unit.path[1])
            {
                facingAngle = "W";
                return;
            }
        }
        if (e)
        {
            if (unit.path[0].neighbours["E"] == unit.path[1])
            {
                facingAngle = "E";
                return;
            }
        }

        Debug.Log("Error: no new facingAngle found");
        facingAngle = null;

    }

    protected bool CheckHalfway()
    {
        float x, y;

        if (unit.path[0].getWorldCoords().x > unit.path[1].getWorldCoords().x)
        {
            x = Mathf.Abs(transform.position.x - unit.path[1].getWorldCoords().x) / Mathf.Abs(unit.path[0].getWorldCoords().x - unit.path[1].getWorldCoords().x);
        }
        else
        {
            x = transform.position.x - unit.path[0].getWorldCoords().x / (unit.path[1].getWorldCoords().x - unit.path[0].getWorldCoords().x);
        }

        if (unit.path[0].getWorldCoords().z > unit.path[1].getWorldCoords().z)
        {
            y = Mathf.Abs(transform.position.z - unit.path[1].getWorldCoords().z) / Mathf.Abs(unit.path[0].getWorldCoords().z - unit.path[1].getWorldCoords().z);
        }
        else
        {
            y = transform.position.z - unit.path[0].getWorldCoords().z / (unit.path[1].getWorldCoords().z - unit.path[0].getWorldCoords().z);
        }




        //Debug.Log("x: " + x + ", y: " + y);

        return x < 0.5 && y < 0.5;
    }

    public abstract void attack();
    void move()
    {
        if (unit.checkIfUnderAttack()) //see if under attack
        {
            //Debug.Log("Under attack...");
            target = unit.underAttack;
            unit.underAttack = null;
        }

        //Check if need to move
        if (unit.path.Count < 2)
        {
            //Debug.Log("Count < 2 (1)");
            moving = false;
            return;
        }
        else if (!moving)
        {
            moving = true;
        }
        //Check if at next tile
        if (!halfway)
        {
            if (CheckHalfway() && unit.path.Count > 1)
            {
                Tile nextTile = unit.path[1];

                //See if the next tile is free
                if (nextTile.building != null || nextTile.unit != null)
                {
                    //Tile is not free: attack or stop - buildings are attacked first
                    if (nextTile.building != null) //building
                    {
                        if (nextTile.building.owner != unit.owner)  //Enemy building
                        {
                            EnemyBuildingCollision(nextTile);
                            return;
                        }
                        else //building is owned by player
                        {

                            //TODO: walk through building if possible, deppending on building type
                        }
                    }
                    else //unit
                    {
                        if (nextTile.unit.owner != unit.owner) //Enemy unit collision
                        {
                            EnemyUnitCollision(nextTile);
                            
                            return;
                        }
                        else //Friendly unit in the way
                        {
                            return;
                        }
                    }
                }

                unit.curTile.unit = null;
                unit.curTile = unit.path[1];
                unit.curTile.unit = unit;
                halfway = true;
            }
        }

        if (atNextPosition())
        {
            unit.path.RemoveAt(0);
            unit.pathAction.RemoveAt(0);
            halfway = false;

            //Check if still need to move
            if (unit.path.Count < 2)
            {
                moving = false;
                return;
            }
            else if (!moving)
            {
                moving = true;
            }

            findNewFacing();
            setForward();
        }

        Vector3 moveTarget = new Vector3(0, 0, 0);


        moveTarget.x = unit.path[1].getWorldCoords().x;
        moveTarget.z = unit.path[1].getWorldCoords().z;

        //transform.LookAt(moveTarget);


        //Debug.Log("Target: "+ unit.path[1].x +","+unit.path[1].y+"      Position: " + target.x + "," + target.y);

        float speed = unit.speed;

        if (unit.pathAction[1] == Unit.actions.run)
        {
            speed *= 3;
            running = true;
        }
        else
        {
            running = false;
        }


        this.transform.position = Vector3.MoveTowards(transform.position, moveTarget, (speed/25)*Time.deltaTime);
    }

    bool atNextPosition()
    {
        if (transform.position.x == unit.path[1].getWorldCoords().x && transform.position.z == unit.path[1].getWorldCoords().z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public abstract void EnemyUnitCollision(Tile tile);
    public abstract void EnemyBuildingCollision(Tile tile);

    public void OnPointerClick(PointerEventData eventData)
    {
        ObjectDictionary.getStateController().UnitClicked(unit);
    }
}
