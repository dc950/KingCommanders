using UnityEngine;
using System.Collections;

public class SoldierChar : MonoBehaviour {

    public Vector3 formationPos;
    public float formationRot;
    float offset;
    Animator animator;
    SoldiersObj soldierObj;

    public string facing;

    float speed = 0.5f;

    bool walking;

	// Use this for initialization
	void Start () 
    {
        formationPos = transform.localPosition;
        formationRot = transform.localRotation.y;
        soldierObj = transform.parent.gameObject.GetComponent<SoldiersObj>();
        animator = this.GetComponent<Animator>();

        soldierObj.soldierChars.Add(this);
        findOffset();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!walking && checkWalking())
        {
            walking = true;
            startMoving();
        }
        if(walking && !checkWalking())
        {
            walking = false;
            StopMoving();
        }


        if(walking)
        {
            moveToCurrentPos();
        }

	}

    void findOffset()
    {
        offset = formationPos.x;
    }

    public void setNewFormPos()
    {
        Debug.Log("stting new formPos for: " + facing);
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

    }

    void moveToCurrentPos()
    {
        if(transform.localPosition != formationPos)
        {
            //Debug.Log("At "+transform.localPosition+" and trying to move to "+formationPos);
            Debug.DrawLine(transform.position, formationPos+soldierObj.transform.position);
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
}
