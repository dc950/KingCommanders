using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class KeepObj : BuildingObj, IPointerClickHandler
{

    StateController sc;
    Keep keep;

	// Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
        ObjectDictionary.getDictionary().unitColliders.Add(GetComponent<BoxCollider>());
	}


    public void Initialize(Keep keep)
    {
        this.keep = keep;
        building = keep;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Display FixedInfo
        base.OnPointerClick(eventData);

        if (sc.state == StateController.states.Idle)
            sc.StartRecruiting(keep);

    }
}
