using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class ActionUI : MonoBehaviour {

    StateController sc;
    Tile tile;
    Unit unit;
    public GameObject btnWalk, btnRun, btnCancel, btnOverwatch, btnOverwatchCancel, btnCatapult;
    List<GameObject> btnList;

    GameObject camera;
    CameraControls cc;

    Vector3 initialScale;
    public float objectScale = 0.1f;

    float animTime = 0.1f;

	// Use this for initialization
	void Start () {
        camera = ObjectDictionary.getDictionary().mainCamera;
        cc = camera.GetComponent<CameraControls>();

        initialScale = transform.localScale;
        Plane plane = new Plane(camera.transform.forward, camera.transform.position);
        float dist = plane.GetDistanceToPoint(transform.position);
        transform.localScale = initialScale * dist * objectScale; 
	}
	
	// Update is called once per frame
    void Update()
    {
        //rotate
        transform.rotation = ObjectDictionary.getDictionary().mainCamera.transform.rotation;
        //Scale
        //transform.localScale = new Vector3(cc.zoomScale, cc.zoomScale, cc.zoomScale);

        Plane plane = new Plane(camera.transform.forward, camera.transform.position);
        float dist = plane.GetDistanceToPoint(transform.position);
        transform.localScale = initialScale * dist * objectScale;

        //Reposition
        Vector3 tPos = tile.getWorldCoords();
        Vector3 cPos = ObjectDictionary.getDictionary().mainCamera.transform.position;
        transform.position = Vector3.MoveTowards(cPos, tPos, Vector3.Distance(cPos, tPos) /2 );
    }
    public void Initialise(Tile tile)
    {
        //get buttons to be in selection
        //put buttons into positions
        //Initialise relevant Buttons

        sc = ObjectDictionary.getStateController();
        btnList = new List<GameObject>();

        this.tile = tile;
        unit = ObjectDictionary.getStateController().unitCommanding;
        Tile lastInPath = unit.path[unit.path.Count - 1];

        //overwatch enabled: all that can be done is disable it...
        if (unit.pathAction[unit.pathAction.Count - 1] == Unit.actions.overwatch)
        {
            btnList.Add(btnOverwatchCancel);
        }
        else
        {
            btnList.Add(btnWalk);
            if (!(unit is Catapult))
            {
                btnList.Add(btnRun);
            }
            else
            {
                //TODO: range and shit
                btnList.Add(btnCatapult);
            }

            //display cancel
            if (unit.path.Contains(tile) && unit.path.Count > 1 && tile != unit.path[unit.path.Count - 1])
            {
                btnList.Add(btnCancel);
            }

            //display overwatch 
            if (unit is Archer && lastInPath != tile && (lastInPath.x == tile.x || lastInPath.y == tile.y))
            {
                btnList.Add(btnOverwatch);
            }
        }

        foreach(GameObject btn in btnList)
        {
            btn.SetActive(true);
            btn.GetComponent<ActionButtonMover>().Initialize();
        }


    }

    void setTargets()
    {

    }

    public void walk()
    {
        sc.AddToPath(tile, Unit.actions.walk);
    }

    public void run()
    {
        sc.AddToPath(tile, Unit.actions.run);
    }

    public void cancel()
    {
        sc.unitCommanding.RemoveFromPath(tile);
        sc.setState(StateController.states.Commanding);
    }

    public void Overwatch()
    {
        sc.AddToPath(tile, Unit.actions.overwatch);
    }

    public void CancelOverwatch()
    {
        sc.unitCommanding.RemoveFromPath(sc.unitCommanding.path[sc.unitCommanding.path.Count-2]);
        sc.setState(StateController.states.Commanding);
    }

    public void Catapult()
    {
        sc.AddToPath(tile, Unit.actions.catapult);
    }


}
