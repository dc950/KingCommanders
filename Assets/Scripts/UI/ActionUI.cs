using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ActionUI : MonoBehaviour {

    StateController sc;
    Tile tile;
    Unit unit;
    public GameObject btnWalk, btnRun, btnCancel, btnOverwatch, btnOverwatchCancel;
    List<GameObject> btnList;

   


    float animTime = 0.1f;

	// Use this for initialization
	void Start () {

        

        
	}
	
	// Update is called once per frame
	void Update () {
        /*
	    if(animTime > 0 && btnList != null)
        {
            foreach (GameObject btn in btnList)
            {
                Vector3 center = new Vector3(0, 0, 0);
                Vector3 dif = btn.transform.localPosition - Vector3.MoveTowards(btn.transform.localPosition, center, 200f * Time.deltaTime);
                btn.transform.localPosition += dif;
            }

            animTime -= Time.deltaTime;
        }
         * */
	}

    public void Initialise(Tile tile)
    {
        //get buttons to be in selection
        //put buttons into positions
        //Initialise relevant Buttons

        sc = ObjectDictionary.getStateController();
        btnList = new List<GameObject>();

        this.tile = tile;
        unit = ObjectDictionary.getStateController().unitCommanding;
        Tile lastInPath = unit.path[unit.path.Count - 1];

        //overwatch enabled: all that can be done is disable it...
        if (unit.pathAction[unit.pathAction.Count - 1] == Unit.actions.overwatch)
        {
            btnList.Add(btnOverwatchCancel);
        }
        else
        {
            btnList.Add(btnWalk);
            btnList.Add(btnRun);

            //display cancel
            if (unit.path.Contains(tile) && unit.path.Count > 1 && tile != unit.path[unit.path.Count - 1])
            {
                btnList.Add(btnCancel);
            }

            //display overwatch 
            if (unit is Archer && lastInPath != tile && (lastInPath.x == tile.x || lastInPath.y == tile.y))
            {
                btnList.Add(btnOverwatch);
            }
        }

        foreach(GameObject btn in btnList)
        {
            btn.SetActive(true);
            btn.GetComponent<ActionButtonMover>().Initialize();
        }


    }

    void setTargets()
    {

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

    public void Overwatch()
    {
        sc.AddToPath(tile, Unit.actions.overwatch);
    }

    public void CancelOverwatch()
    {
        sc.unitCommanding.RemoveFromPath(sc.unitCommanding.path[sc.unitCommanding.path.Count-2]);
        sc.setState(StateController.states.Commanding);
    }


}
