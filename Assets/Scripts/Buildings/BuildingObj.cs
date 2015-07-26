using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BuildingObj : MonoBehaviour, IPointerClickHandler
{

    protected Building building;

    // Use this for initialization
    void Start()
    {

    }

    public Building GetBuilding()
    {
        return building;
    }
    public void Initialize(Building building)
    {
        this.building = building;
        ObjectDictionary.getDictionary().unitColliders.Add(GetComponent<BoxCollider>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        //Display FixedInfo
        ObjectDictionary.getStateController().UnitBuildingClicked(building);
    }
}
