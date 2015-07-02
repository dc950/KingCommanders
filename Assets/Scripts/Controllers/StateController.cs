using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StateController : MonoBehaviour {

    //UI Stuff
    Text stateText;
    [SerializeField] GameObject btnStop;
    [SerializeField] GameObject btnRecruit;
    [SerializeField] GameObject pnlRecruit;

    GameObject actionUI;

    //States
    public enum states { Idle, Placing, Recruiting, Commanding, ActionCommanding, Attacking };
    public states state;
    TurnController tc;
    //Building stuff
    private string bToPlace;
    private bool contBuild = false;

    //Recruiting Stuff
    public Tile currRecruitTile; //gets activated when a spawner is clicked on so when unit is selected, it can be placed here.
    //Unit stuff
    public Unit unitCommanding;

    //Time
    public float turnTime = 5;
    float currentTime = 0;

	// Use this for initialization
	void Start () {
        //get UIs
        state = states.Idle;
        stateText = GameObject.Find("State").GetComponent<Text>();
        currentTime = turnTime;

        tc = ObjectDictionary.getTurnController();       
	}

    public void StartAttack()
    {
        tc.NextTurn();
        setState(states.Attacking);
    }

    void EndAttack()
    {
        currentTime = turnTime;
        setState(states.Idle);

        tc.NextTurn();
    }
    

    public void setState(states newState)
    {


        if (state == states.ActionCommanding && newState != states.ActionCommanding)
        {
            Destroy(actionUI);
            actionUI = null;
        }



        if (newState == states.Placing)
        {
            stateText.text = newState + " " + bToPlace;
            btnStop.SetActive(true);
        }
        else if (newState == states.Idle)
        {
            stateText.text = newState + "";
            btnStop.SetActive(false);

            ObjectDictionary.getDictionary().setColliders(true);
        }
        else if (newState == states.Recruiting)
        {
            stateText.text = newState + "";
            btnStop.SetActive(false);
        }
        else if (newState == states.Commanding)
        {
            stateText.text = newState + "";
            btnStop.SetActive(true);
            ObjectDictionary.getDictionary().setColliders(false);
        }
        else if (newState == states.ActionCommanding)
        {
            stateText.text = newState + "";
        }

        state = newState;
    }

    //***************************
    //Placing Buildings
    //***************************
    //A new building is going to be placed - the usere has selected to build but not chosen where to yet
    public void PlacingNewBuilding(string name)
    {
        bToPlace = name;
        if (bToPlace == null)
        {
            Debug.Log("Error: building to Place is null");
            return;
        }

        setState(states.Placing);

        //If the object is a wall set variable for continous building
        if (name == "Wall")
        {
            contBuild = true;
        }


    }

    //A tile has been clicked while placing - build the tile
    public void BuildBuilding(Tile tile)
    {
        if (state != states.Placing)
        {
            Debug.Log("Error: trying to build but not Placing");
            return;
        }

        if (bToPlace == null)
        {
            Debug.Log("Error: trying to build but no building chosen");
            return;
        }

        if (tile.owner != tc.getTurn())
        {
            Debug.Log("Current player does not own this tile!");
            return;
        }

        if(tile.building == null && tile.unit == null && !tile.spawnTile)
            ObjectDictionary.makeNewBuilding(bToPlace, tile);


        if (!contBuild)
        {
            setState(states.Idle);
            bToPlace = null;
        }
    }

    //stops from placing/
    public void Stop()
    {
        setState(states.Idle);
        bToPlace = null;
        unitCommanding = null;
    }

    //***************************
    //Placing Units
    //***************************

    //Unit is to be chosen
    public void StartRecruiting(Spawner spawner)
    {
        if(spawner.tile.owner != tc.getTurn())
        {
            Debug.Log("Not current players keep!");
            return;
        }

        btnRecruit.SetActive(true);
        pnlRecruit.SetActive(true);
        currRecruitTile = spawner.spawnTile;
        setState(states.Recruiting);
    }

    //Unit is to be placed
    public void PlaceNewUnit(string name)
    {
        ObjectDictionary.makeNewUnit(name, currRecruitTile);
    }

    //***************************
    //Commanding Units
    //***************************

    //A valid Unit has been clicked on: enter Commanding mode
    public void UnitClicked(Unit unit)
    {
        if (unit.owner == tc.getCurrentPlayer())
        {
            setState(states.Commanding);
            unitCommanding = unit;
        }
    }

    public void TileClicked(Tile tile)
    {
        Vector3 pos = tile.getWorldCoords();

        pos = Vector3.MoveTowards(pos, ObjectDictionary.getDictionary().mainCamera.transform.position, 2);
        actionUI = (GameObject)GameObject.Instantiate(ObjectDictionary.getDictionary().ActionUI, pos, ObjectDictionary.getDictionary().mainCamera.transform.rotation);
        actionUI.GetComponent<ActionUI>().Initialise(tile);

        setState(states.ActionCommanding);
    }

    public void StopActionCommanding()
    {
        setState(states.Commanding);
    }

    public void AddToPath(Tile tile, Unit.actions action)
    {
        if(state == states.ActionCommanding)
        {
            setState(states.Commanding);
        }


        if (unitCommanding == null)
        {
            Debug.Log("Error: unitCommadning = null");
            return;
        }

        unitCommanding.AddToPath(tile, action);

    }



    public void Update()
    {

        //Draw Line for unitCommanding

        if (unitCommanding != null)
        {
            unitCommanding.ShowLine();

            /*
            int currentTile = 0;
            int nextTile = 1;

            while (nextTile < unitCommanding.path.Count)
            {
                Vector3 start = TileMap.CoordsToWorld(unitCommanding.path[currentTile].x, unitCommanding.path[currentTile].y);
                Vector3 end = TileMap.CoordsToWorld(unitCommanding.path[nextTile].x, unitCommanding.path[nextTile].y);

                Debug.DrawLine(start, end, Color.blue);
                currentTile = nextTile;
                nextTile++;
            }

            //Do first bit as well
            Vector3 start2 = TileMap.CoordsToWorld(unitCommanding.curTile.x, unitCommanding.curTile.y);
            Vector3 end2 = TileMap.CoordsToWorld(unitCommanding.path[0].x, unitCommanding.path[0].y);

            Debug.DrawLine(start2, end2, Color.blue);
                */
        }

        //Count attack
        if (state == states.Attacking)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                EndAttack();
                stateText.text = "Idle"; 
            }
            else
            {
                stateText.text = "Attacking: " + currentTime.ToString("F1");
            }
        }
    }
            
}
