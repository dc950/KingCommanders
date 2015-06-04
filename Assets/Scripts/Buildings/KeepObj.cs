using UnityEngine;
using System.Collections;

public class KeepObj : MonoBehaviour {

    StateController sc;
    public Keep keep;

	// Use this for initialization
	void Start () {
        sc = ObjectDictionary.getStateController();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if (sc.state == StateController.states.Idle)
            sc.StartRecruiting(keep);
    }
}
