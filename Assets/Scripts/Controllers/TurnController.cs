using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurnController : MonoBehaviour {

    //Players
    int currentTurn = 0;
    public Dictionary<int, Player> players;
    public Text textTurn;
    [SerializeField] int startingMoney;

    public void Start()
    {
        //set turn to player 1
        currentTurn = 1;
    }

    public void SetupPlayers()
    {
        players = new Dictionary<int, Player>();
        players.Add(1, new Player(1, startingMoney));
        players.Add(2, new Player(2, startingMoney));
    }

    //***************************
    //Turn
    //***************************
    public int getTurn()
    {
        return currentTurn;
    }

    public void updateText()
    {
        textTurn.text = "Player " + currentTurn + "'s turn";
    }

    public void NextTurn()
    {
        if(ObjectDictionary.getStateController().state != StateController.states.Idle)
        {
            return;
        }

        //THIS IS THE METHOD FOR LOCAL 2 PLAYER - Move it accordingly and change it for later methods(ai, online etc.)
        if (currentTurn == 1)
        {
            Debug.Log("Hiding p1 changes");
            //hide future plans form player 2
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                if (!go.GetComponent<BuildSiteObj>().buildSite.buildTurn)
                {
                    go.GetComponent<BuildSiteObj>().buildSite.hide();
                }
            }

            currentTurn = 2;
        }
        else if (currentTurn==2)
        {
            //TODO: ENABLE ATTACK MODE

            Debug.Log("Showing buildings");
            //Build buildings
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                go.GetComponent<BuildSiteObj>().buildSite.show();
            }

            Debug.Log("Advancing buildings");
            //Advance BuildSites
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                go.GetComponent<BuildSiteObj>().buildSite.Advance();
            }

            //TODO: Commence attack button

            currentTurn = 1;

        }

        

        updateText();
    }
}
