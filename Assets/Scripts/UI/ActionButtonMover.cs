using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionButtonMover : MonoBehaviour
{

    Vector3 target;
    bool inPosition = false;
    Image image;
    Color c;

    // Use this for initialization
    void Start()
    {
        image = this.GetComponent<Button>().GetComponent<Image>();
        c = image.color;
        c.a = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inPosition && target != null)
        {
            Move();
            if (Mathf.Abs(transform.localPosition.x - target.x) < 0.5f && Mathf.Abs(transform.localPosition.y - target.y) < 0.5f)
            {
                inPosition = true;
                //CHEEKY FIX FOR WIERD ASS FADE BUG
                Image image = this.GetComponent<Button>().GetComponent<Image>();
                Color c = image.color;
                c.a = 1f;
                image.color = c;
            }
        }

         

    }

    void Move()
    {
        float x = transform.localPosition.x;
        float y = transform.localPosition.y;

        x += (target.x - x) * 0.1f * 100 * Time.deltaTime;
        y += (target.y - y) * 0.1f * 100 * Time.deltaTime;

        transform.localPosition = new Vector3(x,y,0);

        if (c.a < 1)
        {
            c.a += (1 - c.a) * 0.15f * 100 * Time.deltaTime;
            image.color = c;
        }
        
    }


    public void Initialize()
    {
        target = transform.localPosition * 1.5f;
    }
}
