using UnityEngine;
using System.Collections;

public class CatapultChar : UnitChar {

    CatapultObj catapultObj;
    bool loaded = false;
    bool firing = false;
    bool fired = false;
    float maxCooldown = 3;
    float cooldown, loadTime;
    float maxLoadTime = 3;
    float maxFireWait = 0.25f;
    float maxFireTime = 3;
    float fireWait, fireTime;

    Vector3 rockSpawn;
    GameObject rock;

	// Use this for initialization
	void Start () {
        base.initialise();
        catapultObj = (CatapultObj)unitObj;
        loadTime = maxLoadTime;
        fireWait = maxFireWait;
        fireTime = maxFireTime;

        rock = catapultObj.rock;

        rockSpawn = new Vector3(0, 0.39f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        base.DoUpdate();

        if (ObjectDictionary.getStateController().state == StateController.states.Attacking &&  catapultObj.tileTarget != null)
        {
            //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).nameHash);
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
            }
            else
            {
                if (!loaded)
                {
                    if(loadTime >= 3)
                    {
                        load();
                    }
                    if(loadTime <= 0)
                    {
                        loaded = true;
                        stopLoad();
                    }
                    loadTime -= Time.deltaTime;
                }
                if (loaded)
                {
                    if (fireWait > 0)
                    {
                        fireWait -= Time.deltaTime;
                    }
                    else
                    {
                        if (fireTime >= maxFireTime)
                        {
                            fire();
                            fireTime -= Time.deltaTime;
                            
                        }
                        else if(fireTime <= 0.1f && !fired)
                        {
                            spawnRock();
                            fired = true;
                        }
                        else if(fireTime <= 0)
                        {
                            stopFire();
                            loaded = false;
                            cooldown = maxCooldown;
                            loadTime = maxLoadTime;
                            fireWait = maxFireWait;
                            fireTime = maxFireTime;
                            fired = false;
                        }
                        else
                        {
                            fireTime -= Time.deltaTime;
                        }

                        
                    }
                }
            }
            
            
        }

	
	}

    private void spawnRock()
    {
        Vector3 pos = transform.position + rockSpawn;
        GameObject go = (GameObject)Instantiate(rock, pos, Quaternion.identity);
        go.GetComponent<Rock>().targetTile = catapultObj.tileTarget;
        go.GetComponent<Rock>().catapult = catapultObj.catapult;
        //Debug.Log("Rock spawned...");
    }

    protected override void startMoving()
    {
        animator.SetBool("Move", true);
        //Debug.Log("Moving true");
    }

    protected override void StopMoving()
    {
        animator.SetBool("Move", false);
        //Debug.Log("Moving false");
    }

    protected override void targeted()
    {
        if(!loaded)
        {
            load();
        }
        else if(cooldown <= 0)
        {
            fire();
        }
    }

    void load()
    {
        //Debug.Log("Loading");
        animator.SetBool("Load", true);
        //animator.SetBool("Load", false);
    }

    void stopLoad()
    {
        //Debug.Log("Stopping Load");
        animator.SetBool("Load", false);
    }

    void fire()
    {
        //Debug.Log("Firing");
        animator.SetBool("Fire", true);
    }

    void stopFire()
    {
        //Debug.Log("Stopping firing");
        animator.SetBool("Fire", false);
    }


    public override void Die()
    {
        Destroy(this.gameObject);
    }
}
