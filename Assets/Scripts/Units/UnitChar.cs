using UnityEngine;
using System.Collections;

public abstract class UnitChar : MonoBehaviour {

    public Vector3 formationPos;
    public float formationRot;
    float offset;
    protected Animator animator;
    protected UnitObj unitObj;

    public string facing;

    protected float speed = 0.5f;
    protected Transform enemy;

    int number;

    bool walking;
    bool attacking;

	// Use this for initialization
	void Start () {
        formationPos = transform.localPosition;
        formationRot = transform.localRotation.y;
        unitObj = transform.parent.gameObject.GetComponent<UnitObj>();
        animator = this.GetComponent<Animator>();

        unitObj.unitChars.Add(this);
        number = unitObj.unitChars.IndexOf(this);
        findOffset();


        if (unitObj.unit.owner.playerNumber == 1)
        {
            facing = "E";
        }
        else
        {
            facing = "W";
        }
        setNewFormPos();
        instantRepos();
	}
	
    protected void initialise()
    {
        Start();
    }

	// Update is called once per frame
	protected void Update () {
        if (ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            animator.enabled = true;

            if (unitObj.target != null)
            {
                targeted();
            }
            else
            {

                if (!walking && checkWalking())
                {
                    //Debug.Log("aren't walking but should be...");
                    walking = true;
                    startMoving();
                }
                if (walking && !checkWalking())
                {
                    //Debug.Log("walking but shouldnt be...");
                    walking = false;
                    StopMoving();
                }
                animator.SetBool("Attacking", false);

                if (walking)
                {
                    moveToCurrentPos();
                }
            }
        }
        else
        {
            animator.enabled = false;
        }
	}

    void findOffset()
    {
        offset = formationPos.x;
    }

    public void setNewFormPos()
    {
        //Debug.Log("stting new formPos for: " + facing);
        if (facing == "N")
        {
            formationPos = new Vector3(offset, 0, 0);
            formationRot = 0;
        }
        if (facing == "E")
        {
            formationPos = new Vector3(0, 0, offset);
            formationRot = 90;
        }
        if (facing == "S")
        {
            formationPos = new Vector3(-offset, 0, 0);
            formationRot = 180;
        }
        if (facing == "W")
        {
            formationPos = new Vector3(0, 0, -offset);
            formationRot = 270;
        }
        else
        {
            //Debug.Log("Error: facing does not compute...");
        }

    }

    void moveToCurrentPos()
    {
        if (transform.localPosition != formationPos)
        {
            //Debug.Log("At "+transform.localPosition+" and trying to move to "+formationPos);
            //Debug.DrawLine(transform.position, formationPos+unitObj.transform.position);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, formationPos, speed * Time.deltaTime);
            //transform.localPosition = formationPos;
            transform.LookAt(formationPos);
            transform.rotation = Quaternion.Euler(0, formationRot, 0);
        }
        else if (transform.rotation.y != formationRot)
        {
            transform.rotation = Quaternion.Euler(0, formationRot, 0);
        }
    }

    void instantRepos()
    {
        transform.localPosition = formationPos;
        transform.rotation = Quaternion.Euler(0, formationRot, 0);
    }

    bool checkWalking()
    {
        return unitObj.moving;
    }

    void startMoving()
    {
        //Debug.Log("Started Moving");
        animator.SetBool("Walking", true);
    }

    void StopMoving()
    {
        //Debug.Log("Stoping movement");
        animator.SetBool("Walking", false);
    }


    protected void selectEnemy()
    {
        //TODO: fix this!!!


        //If its a unit
        if (unitObj.target is Unit)
        {
            UnitObj enemies = unitObj.target.ubObject.GetComponent<UnitObj>();

            //coming from behind
            if (enemies.facingAngle == unitObj.facingAngle)
            {
                if (enemies.unitChars.Count > number - 1) //there is one for this number
                {
                    enemy = enemies.unitChars[number].transform;
                }
                else //there isnt... go for highest
                {
                    enemy = enemies.unitChars[enemies.unitChars.Count - 1].transform;
                }
            }
            else
            {
                if (enemies.unitChars.Count > number) //there is one for this number
                {
                    enemy = enemies.unitChars[enemies.unitChars.Count - 1 - number].transform;
                }
                else //there isnt... go for highest
                {
                    enemy = enemies.unitChars[enemies.unitChars.Count - 1].transform;
                }
            }
        }
        else //its a building
        {
            enemy = unitObj.target.ubObject.transform;
        }

    }

    public void Destroy()
    {
        Debug.Log("Destroying");
        Destroy(this);
    }

    protected abstract void targeted();
}
