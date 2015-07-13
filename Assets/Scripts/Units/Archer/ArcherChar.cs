using UnityEngine;
using System.Collections;

public class ArcherChar : UnitChar {

    ArcherObj archerObj;
    float deathTime = 5;

    float attackTimerMax = 0.633f;
    float attackTimer;

    GameObject arrow;

	// Use this for initialization
	void Start () {
        base.initialise();
        archerObj = (ArcherObj)unitObj;
        arrow = archerObj.arrow;
	}
	
	// Update is called once per frame
	void Update () {
        base.DoUpdate();

        if (running != archerObj.running)
        {
            running = archerObj.running;
            startMoving();
        }


        if (dead)
        {
            if (deathTime <= 0)
            {
                Destroy();
            }
            if (deathTime <= 0.5)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 0.1f);
            }

            deathTime -= Time.deltaTime;
        }
        if (ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            if (enemy != null)
            {
                if (curTarget != null)
                {
                    if (curTarget.dead)
                    {
                        selectEnemy();
                        return;
                    }
                }
                if (attackTimer <= 0)
                {
                    attackTimer = attackTimerMax;
                    Fire();
                }
                else
                {
                    attackTimer -= Time.deltaTime;
                }

            }
        }
	}

    protected override void startMoving()
    {
        animator.SetBool("Attack", false);

        if (archerObj.running)
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

    protected override void targeted()
    {
        if(enemy != null)
        {
            animator.SetBool("Attack", true);
            transform.LookAt(enemy);
        }
        else
        {
            animator.SetBool("Attack", false);
            selectEnemy();
        }
    }

    void Fire()
    {
        if(dead)
        {
            return;
        }

        GameObject firedArrowObj = (GameObject)Instantiate(arrow, transform.position, Quaternion.identity);
        Arrow firedArrow = firedArrowObj.GetComponent<Arrow>();
        firedArrow.end = enemy.position;

        if (archerObj.unitChars[0] == this)
        {
            firedArrow.damage = true;
            firedArrow.archer = (Archer) archerObj.unit;
            firedArrow.targetUB = archerObj.target;
        }
    }

    public override void Die()
    {
        this.transform.parent = null;

        animator.SetBool("Attack", false);
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Die", true);

        dead = true;

        unitObj.unitChars.Remove(this);
    }
}
