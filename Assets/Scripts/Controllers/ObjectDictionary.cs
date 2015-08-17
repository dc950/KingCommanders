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

    //Towers
    public GameObject tower;
    public GameObject towerWall;

    public GameObject keep;
    public GameObject buildSite;

    //units
    public GameObject soldier;
    public GameObject archer;

    public List<BoxCollider> unitColliders;


    //UIs
    public GameObject ActionUI;
    public GameObject FixedUIImage;


    public GameObject healthBar;

    public GameObject mainCamera;
    public GameObject followCamera;

    public GameObject line;

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
        buildingNames.Add("Mine", 40);
        buildingNames.Add("Tower", 80);

        unitNames = new Dictionary<string, int>();
        //Setup unitNames
        unitNames.Add("Soldier", 40);
        unitNames.Add("Archer", 40);
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
        else if(name == "Tower")
        {
            building = new Tower(tile, player);
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
            soldier.getObject().GetComponent<UnitObj>().unit = soldier;
            //Debug.Log(soldier.getObject().GetComponent<SoldiersObj>().soldier.getHealth());
        }
        if(name == "Archer")
        {
            Unit archer = new Archer(tile, player);
            archer.Initialise();
            archer.getObject().GetComponent<UnitObj>().unit = archer;
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

    public void ActivateFixedUI(UnitBuilding target)
    {
        //set things active
        followCamera.SetActive(true);
        FixedUIImage.SetActive(true);
        FixedUIImage.GetComponent<FixedUI>().Activate(target);

        bool far = false;
        //activate new camera target
        if (target is Building)
            far = true;

        followCamera.GetComponent<CameraFollow>().Activate(target.ubObject.transform, far);
    }

    public void DeactivateFixedUI()
    {
        //Deactivate Follow Camera
        followCamera.GetComponent<CameraFollow>().Deactivate();

        //set things not active
        followCamera.SetActive(false);
        FixedUIImage.SetActive(false);


    }



}
