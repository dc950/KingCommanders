using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ActionUI : MonoBehaviour {

    StateController sc;
    Tile tile;
    Unit unit;
    public GameObject btnWalk, btnRun, btnCancel;
    List<GameObject> btnList;

   


    float animTime = 0.1f;

	// Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();

        btnWalk = GameObject.Find("BtnWalk");
        btnRun = GameObject.Find("BtnRun");

        btnList = new List<GameObject>();

        btnList.Add(btnWalk);
        btnList.Add(btnRun);
        btnList.Add(btnCancel);
	}
	
	// Update is called once per frame
	void Update () {
	    if(animTime > 0)
        {
            foreach (GameObject btn in btnList)
            {
                Vector3 center = new Vector3(0, 0, 0);
                Vector3 dif = btn.transform.localPosition - Vector3.MoveTowards(btn.transform.localPosition, center, 200f * Time.deltaTime);
                btn.transform.localPosition += dif;
            }
            
            animTime -= Time.deltaTime;
        }


	}

    public void Initialise(Tile tile)
    {
        this.tile = tile;
        unit = ObjectDictionary.getStateController().unitCommanding;

        //display walk
        //display run


        if ( !(unit.path.Contains(tile) && unit.path.Count > 1 && tile != unit.path[unit.path.Count - 1]) ) //we can remove so long as path is greater than 1 and not last bit in path is selected
        {
            btnCancel.SetActive(false);
        }

    }


    public void walk()
    {
        sc.AddToPath(tile, Unit.actions.walk);
    }

    public void run()
    {
        sc.AddToPath(tile, Unit.actions.run);
    }

    public void cancel()
    {
        sc.unitCommanding.RemoveFromPath(tile);
        sc.setState(StateController.states.Commanding);
    }


}
