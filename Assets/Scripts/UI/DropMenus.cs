using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class DropMenus : MonoBehaviour {

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform menuPanelBuild;
    [SerializeField] Transform menuPanelRecruit;

    StateController sc;
    
	// Use this for initialization
	void Start () 
    {
        //get sc
        sc = ObjectDictionary.getStateController();

        //Fill Building
        List<string> bNames = new List<string>();
        bNames.AddRange(ObjectDictionary.getDictionary().buildingNames.Keys);
        for(int i = 0; i < bNames.Count; i++)
        {
            GameObject button = (GameObject) Instantiate(buttonPrefab);
            button.GetComponentInChildren<Text>().text = bNames[i];
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(
                () => {sc.PlacingNewBuilding (bNames[index]);}
                );
            button.transform.SetParent(menuPanelBuild);
        }
        
        //Fill recruit
        List<string> uNames = new List<string>();
        uNames.AddRange(ObjectDictionary.getDictionary().unitNames.Keys);
        for (int i = 0; i < uNames.Count; i++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            button.GetComponentInChildren<Text>().text = uNames[i];
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(
                () => { sc.PlaceNewUnit(uNames[index]); }
                );
            button.transform.SetParent(menuPanelRecruit);
        }
	}

    public void RecruitOff()
    {
        sc.setState(StateController.states.Idle);
        sc.currRecruitTile = null;
    }
}
