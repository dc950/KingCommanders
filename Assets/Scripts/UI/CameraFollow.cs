using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    private Transform target;
    private bool active;
    private bool far;

    Vector3 StartPosClose, StartPosFar;

    // Use this for initialization
    void Start()
    {
        StartPosClose = new Vector3(0, 0.25f, -1);
        StartPosFar = new Vector3(0, 2, 4);
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

    public void Activate(Transform target, bool far)
    {
        active = true;
        this.target = target;
        this.far = far;
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
        transform.Translate((Vector3.right / 10) * Time.deltaTime);
    }

    void GetIntoPosition()
    {
        transform.SetParent(target);
        if (!far)
            transform.localPosition = StartPosClose;
        else
            transform.localPosition = StartPosFar;
    }

}
