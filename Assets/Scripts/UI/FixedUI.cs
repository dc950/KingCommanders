using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FixedUI : MonoBehaviour
{

    UnitBuilding target;
    bool active;
    public Text health;
    public Text name;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if(target == null)
            {
                Deactivate();
            }
            health.text = target.getHealth() + "/" + target.getMaxHealth();
        }
    }

    public void Activate(UnitBuilding target)
    {
        this.gameObject.SetActive(true);
        this.GetComponentInChildren<HealthBarFixed>().Initialise(target);
        this.target = target;
        active = true;
        name.text = target.GetName();
    }

    public void Deactivate()
    {
        target = null;
        active = false;
        name.text = "Name";
        health.text = "Health";
        this.gameObject.SetActive(false);
    }
}
