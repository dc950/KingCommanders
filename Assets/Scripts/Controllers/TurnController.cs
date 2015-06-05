using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurnController : MonoBehaviour {

    //Players
    public int currentTurn = 0;
    public Dictionary<int, Player> players;
    public Text textTurn;
    [SerializeField] int startingMoney;

    //UI
    [SerializeField] GameObject btnEndTurn;
    [SerializeField] GameObject btnStartAttack;
    [SerializeField] GameObject btnBuild;

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
        if (currentTurn < 3)
            textTurn.text = "Player " + currentTurn + "'s turn";
        else if (currentTurn == 3)
            textTurn.text = "Awaiting for attack to commence...";
        else if (currentTurn == 4)
            textTurn.text = "Attacking...";
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
            //hide future plans form player 2
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                if(go.GetComponent<BuildSiteObj>().buildSite.owner == currentTurn)
                    go.GetComponent<BuildSiteObj>().buildSite.hide();
            }

            currentTurn = 2;

            updateText();
        }
        else if (currentTurn==2)
        {
            //Hide buildings
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                if(go.GetComponent<BuildSiteObj>().buildSite.owner == currentTurn)
                    go.GetComponent<BuildSiteObj>().buildSite.hide();
            }

            currentTurn = 3;

            updateText();

            btnEndTurn.SetActive(false);
            btnStartAttack.SetActive(true);
            btnBuild.SetActive(false);

        }
        else if(currentTurn == 3)
        {
            //Show
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                go.GetComponent<BuildSiteObj>().buildSite.show();
            }

            btnStartAttack.SetActive(false);

            

            currentTurn = 4;
            updateText();
        }
        else if(currentTurn == 4)
        {
            btnEndTurn.SetActive(true);
            btnBuild.SetActive(true);

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("BuildSite"))
            {
                go.GetComponent<BuildSiteObj>().buildSite.Build();
            }

            currentTurn = 1;
            updateText();
        }
    } 
}

