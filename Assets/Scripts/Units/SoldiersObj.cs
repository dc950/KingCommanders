using UnityEngine;
using System.Collections;

public class SoldiersObj : MonoBehaviour {

    public Soldier soldier;
    
    // Use this for initialization
	void Start () {
	
	}


	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        ObjectDictionary.getStateController().CommandUnit(soldier);
    }
}
