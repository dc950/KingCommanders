using UnityEngine;
using System.Collections.Generic;

public class ObjectDictionary : MonoBehaviour {

    //walls
	public GameObject wallStraight;
    public GameObject wallCorner;
    public GameObject wallEnd;
    public GameObject wallSingle;
    public GameObject wallT;
    public GameObject wallCross;
    public GameObject keep;
    public GameObject buildSite;

    //units
    public GameObject soldier;

    public List<BoxCollider> unitColliders;


    public GameObject healthBar;

    public GameObject mainCamera;


    public List<string> buildingNames;
    public List<string> unitNames;

    public void Start()
    {
       
    }

    public void setUpNames()
    {
        //Setup BuildingNames
        buildingNames.Add("Wall");

        //Setup unitNames
        unitNames.Add("Soldier");
    }

    public static TurnController getTurnController()
    {
        return getDictionary().GetComponent<TurnController>();
        //getComponent<TurnController>();
    }

    
    public static void makeNewBuilding(string name, Tile tile)
    {
        if(name == "Keep")
        {
            Debug.Log("Error: No player for keep");
            return;
        }
        makeNewBuilding(name, tile, null);
    }

    public static void makeNewBuilding(string name, Tile tile, Player player)
    {
        ObjectDictionary od = getDictionary();

        Building building;

        //Set up Building objects
        if (name == od.buildingNames[0])
        {
            building = new Wall(tile);
        }
        else if(name == "Keep")
        {
            building = new Keep(tile, player);
            building.Initialise();
            return; //initialise and return because we don't want buildSites for keeps
        }
        else
        {
            Debug.LogError("new building name " + name + " not recognised");
            return;
        }

        building.owner = getTurnController().currentTurn;

        //set up buildSites
        BuildSite theBuildSite = new BuildSite(building.tile);
        theBuildSite.AssignBuilding(building);
        theBuildSite.Initialise();
        theBuildSite.owner = getTurnController().currentTurn;
    }

    public static void makeNewUnit(string name, Tile tile)
    {
        ObjectDictionary od = getDictionary();
        if (name == od.unitNames[0])
        {
            Unit soldier = new Soldier(tile);
            soldier.Initialise(getTurnController().getTurn());
            soldier.getObject().GetComponent<SoldiersObj>().soldier = (Soldier)soldier;
        }
    }

    public static ObjectDictionary getDictionary() 
    {
        return GameObject.Find("TileMap").GetComponent<ObjectDictionary>();
    }

    public static TileMap getTileMap()
    {
        return getDictionary().GetComponent<TileMap>();
    }

    public static StateController getStateController()
    {
        return getDictionary().GetComponent<StateController>();
    }


    public void setColliders(bool state)
    {
        foreach(BoxCollider bc in unitColliders)
        {
            bc.enabled = state;
        }
    }
}
