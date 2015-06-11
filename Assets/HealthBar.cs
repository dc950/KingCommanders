using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public RectTransform healthTransform;
    public RectTransform backgroundTransform;
    private float cachedY, cachedZ;
    private float minXValue, maxXValue;
    public UnitBuilding unitBuilding;
    public Image visualHealth;

	// Use this for initialization
    void Start()
    {
       //Ran into problems using this... not beign called first and stuff...
    }

    public void Initialise(UnitBuilding ub)
    {
        unitBuilding = ub;
        HandleHealth();
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        float outcome = (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        return outcome;
    }

    private void setPos()
    {
        cachedY = backgroundTransform.position.y;
        cachedZ = backgroundTransform.position.z;

        maxXValue = backgroundTransform.position.x;
        minXValue = backgroundTransform.position.x - backgroundTransform.rect.width;
    }
	
    public void HandleHealth()
    {
        setPos();

        int health = unitBuilding.getHealth();
        int maxHealth = unitBuilding.getMaxHealth();

        float currentXValue = MapValues(health, 0, maxHealth, minXValue, maxXValue);

        Vector3 pos = new Vector3(currentXValue, cachedY, cachedZ);
        healthTransform.position = pos;

        if(unitBuilding.getHealth() > unitBuilding.getMaxHealth()/2)
        {
            visualHealth.color = new Color32((byte)MapValues(health, maxHealth/2,maxHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            visualHealth.color = new Color32(255, (byte)MapValues(health, 0, maxHealth / 2, 0, 255), 0, 255);
        }
    }




	// Update is called once per frame
	void Update () {

        
    }
}
