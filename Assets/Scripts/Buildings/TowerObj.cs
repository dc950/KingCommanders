using UnityEngine;
using System.Collections.Generic;

public class TowerObj : MonoBehaviour {


    float cooldown;
    Tower tower;

    Unit target = null;

    void Start()
    {

    }

    public void Initialise(Tower tower)
    {
        this.tower = tower;
        cooldown = tower.rateOfFire;
    }

    void Update()
    {
        if(ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            if (target == null)
            {
                Collider[] collisionsArr = Physics.OverlapSphere(transform.position, 2);
                List<Unit> enemyCollisions = new List<Unit>();



                foreach (Collider c in collisionsArr)
                {
                    //Debug.Log("Object in collider...");
                    if (c.gameObject.GetComponent<UnitObj>())
                    {
                        Debug.Log("Unit in collider...");
                        if (c.gameObject.GetComponent<UnitObj>().unit.owner != tower.owner)
                        {
                            Debug.Log("Enemy unit... adding");
                            enemyCollisions.Add(c.gameObject.GetComponent<UnitObj>().unit);
                        }
                    }
                    else
                    {

                    }
                }

                if (enemyCollisions.Count > 0) //if there is an enemy in range
                {
                    target = enemyCollisions[0];
                    Debug.Log("New Target");
                }
            }
            else
            {
                if(target.getHealth() <= 0)
                {
                    target = null;
                }

                if(cooldown < 0)
                {
                    tower.attack(target);
                    cooldown = tower.rateOfFire; 
                    //TODO: do the damage when the arrows hit.
                }
                else
                {
                    cooldown -= Time.deltaTime;
                }
            }
        }
    }

}
