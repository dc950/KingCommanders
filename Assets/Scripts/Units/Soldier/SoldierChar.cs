using UnityEngine;
using System.Collections.Generic;

public class SoldierChar : MonoBehaviour {

    public Vector3 formationPos;
    public float formationRot;
    float offset;
    Animator animator;
    SoldiersObj soldierObj;

    public string facing;

    float speed = 0.5f;
    Transform enemy;
    bool movingToEnemy = false;
    int number;

    bool walking;
    bool attacking;

	// Use this for initialization
	void Start () 
    {
        formationPos = transform.localPosition;
        formationRot = transform.localRotation.y;
        soldierObj = transform.parent.gameObject.GetComponent<SoldiersObj>();
        animator = this.GetComponent<Animator>();

        soldierObj.soldierChars.Add(this);
        number = soldierObj.soldierChars.IndexOf(this);
        findOffset();


        if (soldierObj.soldier.owner.playerNumber == 1)
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

	// Update is called once per frame
	void Update () {
        if (ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            animator.enabled = true;
            
            if (soldierObj.target != null)
            {
                if (enemy != null)
                {
                    if (movingToEnemy)
                    {
                        moveToEnemy();       
                    }

                }
                else
                {
                    animator.SetBool("Attacking", false);
                    selectEnemy();
                }
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
        if(facing == "N")
        {
            formationPos = new Vector3(offset, 0, 0);
            formationRot = 0;
        }
        if (facing == "E")
        {
            formationPos = new Vector3(0,0,offset);
            formationRot = 90;
        }
        if (facing == "S")
        {
            formationPos = new Vector3(-offset, 0, 0);
            formationRot = 180;
        }
        if (facing == "W")
        {
            formationPos = new Vector3(0, 0,-offset);
            formationRot = 270;
        }
        else
        {
            //Debug.Log("Error: facing does not compute...");
        }

    }

    void moveToCurrentPos()
    {
        if(transform.localPosition != formationPos)
        {
            //Debug.Log("At "+transform.localPosition+" and trying to move to "+formationPos);
            //Debug.DrawLine(transform.position, formationPos+soldierObj.transform.position);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, formationPos, speed*Time.deltaTime);
            //transform.localPosition = formationPos;
            transform.LookAt(formationPos);
            transform.rotation = Quaternion.Euler(0,  formationRot, 0);
        }
        else if(transform.rotation.y != formationRot)
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
        return soldierObj.moving;
    }

    void startMoving()
    {
        Debug.Log("Started Moving");
        animator.SetBool("Walking", true);
    }

    void StopMoving()
    {
        Debug.Log("Stoping movement");
        animator.SetBool("Walking", false);
    }

    void selectEnemy()
    {

        SoldiersObj enemies = soldierObj.target.ubObject.GetComponent<SoldiersObj>();

            //coming from behind
        if(enemies.facingAngle == soldierObj.facingAngle)
        {
            if(enemies.soldierChars.Count > number-1) //there is one for this number
            {
                enemy = enemies.soldierChars[number].transform;
            }
            else //there isnt... go for highest
            {
                enemy = enemies.soldierChars[enemies.soldierChars.Count - 1].transform;
            }
        }
        else
        {
            if(enemies.soldierChars.Count > number) //there is one for this number
            {
                enemy = enemies.soldierChars[enemies.soldierChars.Count - 1 - number].transform;
            }
            else //there isnt... go for highest
            {
                enemy = enemies.soldierChars[enemies.soldierChars.Count - 1].transform;
            }
        }

        movingToEnemy = true;
    }

    void moveToEnemy()
    {
        if(!movingToEnemy)
        {
            return;
        }
        if(enemy == null)
        {
            selectEnemy();
        }

        if(Vector3.Distance(transform.position, enemy.position) >= 0.1)
        {
            //Debug.Log("not close enough! Distance = " + Vector3.Distance(transform.position, enemy.position));
            transform.position = Vector3.MoveTowards(transform.position, enemy.position, 2*speed*Time.deltaTime);
            transform.LookAt(enemy.position);
            return;
        }
        //At enemy
        Debug.Log("Animation set to attacking");
        animator.SetBool("Attacking", true);
        movingToEnemy = false;
    }

    public void Destroy()
    {
        Debug.Log("Destroying");
        Destroy(this);
    }
}
