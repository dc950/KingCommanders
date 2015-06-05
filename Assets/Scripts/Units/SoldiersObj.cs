using UnityEngine;
using System.Collections.Generic;

public class SoldiersObj : MonoBehaviour {

    public Soldier soldier;
    StateController sc;
    
    // Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
	}


	
	// Update is called once per frame
	void Update () {
        if (sc.state == StateController.states.Attacking)
        {
            move();
            soldier.ShowLine();
        }
	}

     void move()
    {
         //Check if need to move
        if(soldier.path.Count < 2)
        {
            //Debug.Log("Count < 2 (1)");
            return;
        }
        //Check if at next tile
        if(atNextPosition())
        {
            //Debug.Log("Removoing path point: " + soldier.path[0].x + "," + soldier.path[0].y);
            soldier.path.RemoveAt(0);
            soldier.curTile = soldier.path[0];
        }
        //Check if stilil need to move
        if (soldier.path.Count < 2)
        {
            //Debug.Log("Count < 2 (2)");
            return;
        }


        Vector3 target = new Vector3(0,0,0);

        target.x = soldier.path[1].getWorldCoords().x;
        target.z = soldier.path[1].getWorldCoords().z;
        //Debug.Log("Target: "+ soldier.path[1].x +","+soldier.path[1].y+"      Position: " + target.x + "," + target.y);
        this.transform.position = Vector3.MoveTowards(transform.position, target, soldier.speed/1000);

    }


    bool atNextPosition()
    {
        if (transform.position.x == soldier.path[1].getWorldCoords().x && transform.position.z == soldier.path[1].getWorldCoords().z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
    void OnMouseDown()
    {
        ObjectDictionary.getStateController().CommandUnit(soldier);
    }
}
