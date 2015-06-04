using UnityEngine;
using System.Collections;

public abstract class UnitBuilding {

    int health, maxHealth;
    
    public UnitBuilding(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
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
    }

    public void heal(int healAmmount)
    {
        health += healAmmount;
        if (health > maxHealth)
            health = maxHealth;
    }
}
