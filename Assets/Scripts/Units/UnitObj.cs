using UnityEngine;
using System.Collections.Generic;

public abstract class UnitObj : MonoBehaviour {

    protected StateController sc;

    public Unit unit;

    public UnitBuilding target;

    protected bool halfway = false;
    public bool moving = false;
    public string facingAngle;

    public List<UnitChar> unitChars;

	// Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
        ObjectDictionary.getDictionary().unitColliders.Add(GetComponent<BoxCollider>());
        List<UnitChar> unitChars = new List<UnitChar>();
	}
	
    protected void initialise()
    {
        Start();
    }

	// Update is called once per frame
	protected void Update () {
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

            if ((health / maxHealth) * 100 < (unitChars.Count - 1) * 25)
            {
                Debug.Log("Health: " + health + ", MaxHealth " + maxHealth + ", HealthDiv: " + health / maxHealth + "Health pct: " + (health / maxHealth) * 100 + ", comp to" + (unitChars.Count - 1) * 25 + "Where unitcount -1 is" + (unitChars.Count - 1));
                //choose random dude to destroy
                int num = Random.Range(0, unitChars.Count - 1);
                Debug.Log("Going to destroy " + num);
                UnitChar toDestroy = unitChars[num];
                unitChars.Remove(toDestroy);
                Destroy(toDestroy.gameObject);
                Destroy(toDestroy);
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
        //take away from smallest number
        //divide by largest
        //????

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
    public abstract void move();


    
}
