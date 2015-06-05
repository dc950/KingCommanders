using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class UnitBuilding {

    int health, maxHealth;
    GameObject healthBar;
    protected GameObject ubObject;

    public UnitBuilding(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
        
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
            Destroy();

        healthBar.GetComponent<HealthBar>().HandleHealth();
    }

    public void heal(int healAmmount)
    {
        health += healAmmount;
        if (health > maxHealth)
            health = maxHealth;
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
        Vector3 position = new Vector3(0, 0, 0);
       // Vector3 position = ubObject.transform.position;
        //position.y = 1.5f;

        Quaternion rot = ObjectDictionary.getDictionary().camera.transform.rotation;

        healthBar = ((GameObject)MonoBehaviour.Instantiate(ObjectDictionary.getDictionary().healthBar, ubObject.transform.position, rot));

        healthBar.GetComponent<HealthBar>().unitBuilding = this;
        healthBar.transform.SetParent(getObject().transform);
        updateHealthBar();
    }
}
