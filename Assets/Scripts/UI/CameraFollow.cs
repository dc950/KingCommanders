using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    private Transform target;
    private bool active;

    Vector3 StartPos;

    // Use this for initialization
    void Start()
    {
        StartPos = new Vector3(0, 0.25f, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if(target == null)
            {
                //target has died (presumably?) - deactivate
                Deactivate();
                return;
            }

            Rotate();
        }
    }

    public void Activate(Transform target)
    {
        active = true;
        this.target = target;
        GetIntoPosition();
    }

    public void Deactivate()
    {
        active = false;
        target = null;
    }

    public bool GetActive()
    {
        return active;
    }

    void Rotate()
    {
        transform.LookAt(target);
        transform.Translate((Vector3.right / 2) * Time.deltaTime);
    }

    void GetIntoPosition()
    {
        transform.SetParent(target);
        transform.localPosition = StartPos;
    }

}
