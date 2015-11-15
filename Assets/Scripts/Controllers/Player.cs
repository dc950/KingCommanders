using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class Player {

    public int playerNumber;
    int money;
    Text moneyUI;

    //building stats
    public List<Mine> mines;


    //stuff about unlocks/upgrades

    public Player(int number, int money, Text moneyUI)
    {
        playerNumber = number;

        this.money = money;
        this.moneyUI = moneyUI;
        updateMoneyUI();

        mines = new List<Mine>();
    }

    public void turnEnd()
    {
        //give money for mines
        foreach(Mine m in mines)
        {
            addMoney(m.getMoney());
        }

        //and for keep
        addMoney(10);

        updateMoneyUI();
    }

    public void addMoney(int amount)
    {
        money += amount;
        updateMoneyUI();
    }

    public void removeMoney(int amount)
    {
        money -= amount;
        updateMoneyUI();
    }

    public bool canAfford(int cost)
    {
        return (cost <= money);
    }

    public bool attemptBuy(int cost)
    {
        if(canAfford(cost))
        {
            removeMoney(cost);
            return true;
        }
        else
        {
            return false;
        }
    }



    public void updateMoneyUI()
    {
        moneyUI.text = "Gold: "+money;
    }

}
