using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class UnitBuilding {

    int health, maxHealth;
    GameObject healthBar;
    protected GameObject ubObject;
    public Player owner;

    public UnitBuilding(int maxHealth, Player owner)
    {
        this.owner = owner;
        this.maxHealth = maxHealth;
        health = maxHealth;
        //Debug.Log("Unit created with " + maxHealth + "max hp and " + health + "current hp");
    }

    public GameObject getObject()
    {
        return ubObject;
    }

    public abstract void Destroy();
    public abstract void DeleteObject();

    //Health
    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(); 
        }

        //Debug.Log("Damage Taken,  Remaining Health: " + health + "/" + maxHealth);

        updateHealthBar();
    }

    public void heal(int healAmmount)
    {
        health += healAmmount;
        if (health > maxHealth)
            health = maxHealth;

        updateHealthBar();
    }

    public void updateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.GetComponent<HealthBar>().HandleHealth();
        }
    }

    public void displayHealthBar()
    {
        Quaternion rot = ObjectDictionary.getDictionary().mainCamera.transform.rotation;

        healthBar = ((GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().healthBar, ubObject.transform.position, rot));
        
        healthBar.GetComponent<HealthBar>().Initialise(this);
        healthBar.transform.SetParent(getObject().transform);

        updateHealthBar();

    }
}
