using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public RectTransform healthTransform;
    private float cachedY;
    private float minXValue, maxXValue;
    public UnitBuilding unitBuilding;
    public Image visualHealth;

	// Use this for initialization
    void Start()
    {
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - healthTransform.rect.width;
       // healthTransform = visualHealth.GetComponent<RectTransform>();

        //visualHealth = transform.Find("VisualHealth").gameObject.GetComponent<Image>(); 
    }
    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
	
    public void HandleHealth()
    {
        int health = unitBuilding.getHealth();
        int maxHealth = unitBuilding.getMaxHealth();

        float currentXValue = MapValues(health, 0, maxHealth, minXValue, maxXValue);

        healthTransform.position = new Vector3(currentXValue, cachedY);

        if(unitBuilding.getHealth() > unitBuilding.getMaxHealth()/2)
        {
            visualHealth.color = new Color32((byte)MapValues(health, maxHealth/2,maxHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            visualHealth.color = new Color32(255, (byte)MapValues(health, maxHealth / 2, maxHealth, 0, 255), 0, 255);
        }
    }




	// Update is called once per frame
	void Update () {
    
        
    }
}
