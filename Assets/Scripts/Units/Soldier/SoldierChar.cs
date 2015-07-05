using UnityEngine;
using System.Collections.Generic;

public class SoldierChar : UnitChar {

    //SoldiersObj soldierObj;
    bool movingToEnemy = false;
    bool running = false;
    SoldiersObj soldierObj;

    float deathTime = 2;
    

	// Use this for initialization
	void Start () 
    {
        base.initialise();
        soldierObj = (SoldiersObj)unitObj;
        //soldierObj = (SoldiersObj) unitObj;
	}

	// Update is called once per frame
	void Update () {
        base.DoUpdate();

        if(running != soldierObj.running)
        {
            running = soldierObj.running;
            startMoving();
        }

        if(dead)
        {
            if(deathTime <= 0)
            {
                Destroy();
            }
            if(deathTime <= 0.5)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 0.1f);
            }

            deathTime -= Time.deltaTime;
            Debug.Log("DeathTime: " + deathTime);
        }

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
        animator.SetBool("Attack 01", true);
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
            animator.SetBool("Attack 01", false);
            selectEnemy();

            movingToEnemy = true;
        }
    }

    protected override void startMoving()
    {
        if (soldierObj.running)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
            animator.SetBool("Walk", true);
        }
    }

    protected override void StopMoving()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
    }

    public override void Die()
    {
        Debug.Log("Dying...");

        animator.SetBool("Attack 01", false);
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Die", true);

        dead = true;

        unitObj.unitChars.Remove(this);
    }

    
}
