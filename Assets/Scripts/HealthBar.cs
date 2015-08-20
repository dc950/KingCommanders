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

    public bool fixedUI = false;

    GameObject camera;

    Vector3 initialScale;
    public float objectScale = 0.1f;

	// Use this for initialization
    void Start()
    {
        if (!fixedUI)
        {
            camera = ObjectDictionary.getDictionary().mainCamera;
            initialScale = transform.localScale;
            Plane plane = new Plane(camera.transform.forward, camera.transform.position);
            float dist = plane.GetDistanceToPoint(transform.position);
            transform.localScale = initialScale * dist * objectScale;
        }
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
        cachedY = backgroundTransform.localPosition.y;
        cachedZ = backgroundTransform.localPosition.z;

        maxXValue = backgroundTransform.localPosition.x;
        minXValue = backgroundTransform.localPosition.x - backgroundTransform.rect.width;

    }
	
    public void HandleHealth()
    {
        setPos();

        float health = unitBuilding.getHealth();
        float maxHealth = unitBuilding.getMaxHealth();

        float currentXValue = MapValues(health, 0, maxHealth, minXValue, maxXValue);
        if (fixedUI)
            Debug.Log("currX: " + currentXValue+", MinX: "+minXValue+", maxX: "+maxXValue+", health: "+health+"/"+ maxHealth);
        Vector3 pos = new Vector3(currentXValue, cachedY, cachedZ);
        healthTransform.localPosition = pos;

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

        if(fixedUI)
        {
            if (unitBuilding != null)
            {
                HandleHealth();
            }
        }

        if (!fixedUI)
        {
            //rotate
            transform.rotation = ObjectDictionary.getDictionary().mainCamera.transform.rotation;
            //Scale
            //transform.localScale = new Vector3(cc.zoomScale, cc.zoomScale, cc.zoomScale);

            Plane plane = new Plane(camera.transform.forward, camera.transform.position);
            float dist = plane.GetDistanceToPoint(transform.position);
            transform.localScale = initialScale * dist * objectScale;
        }
        
    }
}
