using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ActionUI : MonoBehaviour {

    StateController sc;
    Tile tile;
    Unit unit;

	// Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialise(Tile tile)
    {
        this.tile = tile;
        unit = ObjectDictionary.getStateController().unitCommanding;

        //display walk
        //display run


        if (unit.path.Contains(tile) && unit.path.Count > 1 && tile != unit.path[unit.path.Count - 1]) //we can remove so long as path is greater than 1 and not last bit in path is selected
        {
            //display remove
        }

    }


    public void walk()
    {
        Debug.Log("Walking...");
        sc.AddToPath(tile, Unit.actions.walk);
    }

    public void run()
    {
        sc.AddToPath(tile, Unit.actions.run);
    }


}
