using UnityEngine;
using System.Collections.Generic;

public class ObjectDictionary : MonoBehaviour {

    //buildings
    public GameObject mine;

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


    public Dictionary<string, int> buildingNames;
    public Dictionary<string, int> unitNames;

    public void Start()
    {

    }

    public void setUpNames()
    {

        buildingNames = new Dictionary<string,int>();
        //Setup BuildingNames/Costs
        buildingNames.Add("Wall", 20);
        buildingNames.Add("Mine", 50);

        unitNames = new Dictionary<string, int>();
        //Setup unitNames
        unitNames.Add("Soldier", 40);
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
        makeNewBuilding(name, tile, getTurnController().getCurrentPlayer());
    }

    public static void makeNewBuilding(string name, Tile tile, Player player)
    {
        ObjectDictionary od = getDictionary();

        //buy building
        if(name != "Keep")
        {
            if(!player.attemptBuy(od.buildingNames[name]))
            {
                Debug.Log("You cannot afford this!!");
                return;
            }
        }


        Building building;

        //Set up Building objects
        if (name == "Wall")
        {
            building = new Wall(tile, player);
        }
        else if(name == "Keep")
        {
            building = new Keep(tile, player);
            building.Initialise();
            return; //initialise and return because we don't want buildSites for keeps
        }
        else if(name == "Mine")
        {
            building = new Mine(tile, player);
        }
        else
        {
            Debug.LogError("new building name " + name + " not recognised");
            return;
        }

        //set up buildSites
        BuildSite theBuildSite = new BuildSite(building.tile, player);
        theBuildSite.AssignBuilding(building);
        theBuildSite.Initialise();
    }

    public static void makeNewUnit(string name, Tile tile)
    {
        makeNewUnit(name, tile, getTurnController().getCurrentPlayer());
    }


    public static void makeNewUnit(string name, Tile tile, Player player)
    {
        ObjectDictionary od = getDictionary();

        if (!player.attemptBuy(od.unitNames[name]))
        {
            Debug.Log("You cannot afford this!!");
            return;
        }

        if (name == "Soldier")
        {
            Unit soldier = new Soldier(tile, player);
            soldier.Initialise();
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
