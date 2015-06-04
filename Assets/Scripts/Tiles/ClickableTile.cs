using UnityEngine;
using System.Collections;

public class ClickableTile : MonoBehaviour {

    public Tile tile;
    public StateController sc;

	void Start () {
        sc = ObjectDictionary.getStateController();
	}

    void OnMouseDown()
    {
        //Debug.Log("Owned by player " + tile.owner);

        if (sc.state == StateController.states.Placing)
        {
            sc.BuildBuilding(tile);
        }
        if (sc.state == StateController.states.Commanding)
        {
            sc.AddToPath(tile);
        }
    }
}
