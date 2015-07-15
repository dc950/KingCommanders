using UnityEngine;
using System.Collections;

public class TowerArrow : MonoBehaviour {

    Transform target;

    float halfX, halfZ;

    public bool damage = false;
    public Tower tower;
    public UnitBuilding targetUB;

    int numberOfPoints = 100;


    // Use this for initialization
    void Start()
    {
        target = targetUB.ubObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null )
        {
            if(targetUB == null)
            {
                return;
            }

            
        }
        //DrawLine();
        if (ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            Move();
        }
    }

    void Move()
    {
        if(target == null)
        {
            DealDamage();
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }
        if(transform.position == target.position)
        {
            DealDamage();
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }


        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 2);
        transform.LookAt(target.position);
    }

    void DealDamage()
    {
        tower.attack(targetUB);
    }
}
