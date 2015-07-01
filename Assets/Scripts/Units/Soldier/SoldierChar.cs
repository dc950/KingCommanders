using UnityEngine;
using System.Collections.Generic;

public class SoldierChar : UnitChar {

    //SoldiersObj soldierObj;
    bool movingToEnemy = false;
    

	// Use this for initialization
	void Start () 
    {
        base.initialise();
        //soldierObj = (SoldiersObj) unitObj;
	}

	// Update is called once per frame
	void Update () {
        base.Update();
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
        //Debug.Log("Animation set to attacking");
        animator.SetBool("Attacking", true);
        movingToEnemy = false;
    }

    protected override void targeted()
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

            movingToEnemy = true;
        }
    }

    
}
