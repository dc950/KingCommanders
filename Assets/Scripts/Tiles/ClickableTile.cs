using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickableTile : MonoBehaviour, IPointerClickHandler {

    public Tile tile;
    public StateController sc;

	void Start () {
        sc = ObjectDictionary.getStateController();
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Owned by player " + tile.owner);

        if (sc.state == StateController.states.Placing)
        {
            sc.BuildBuilding(tile);
        }
        else if (sc.state == StateController.states.Commanding)
        {
            sc.TileClicked(tile);
        }
        else if (sc.state == StateController.states.ActionCommanding)
        {
            sc.StopActionCommanding();
        }

        if (sc.state != StateController.states.Commanding && sc.state != StateController.states.ActionCommanding)
        {
            ObjectDictionary.getDictionary().DeactivateFixedUI();
        }
    }
}
