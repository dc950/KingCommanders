using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

    //Variables

    //public Variables
    public float moveSpeed;
    public float rotateSpeed;
    public float zoomSpeed;
    public float boomHeight;

    //Clamps
    public float maxLeft, maxRight, maxUp, maxDown;
    public float maxZoomDist, minZoomDist;
    public float maxPitchUp, maxPitchDown;

    public float zoomScale;
    public float dist;

    public GameObject cameraBoom;
    
    Camera cam;

    Transform center;

    //Repositioning/////
    float reposSpeed;
    Transform reposTrans;
    ////////////////////

	void Start () 
    {
        reposSpeed = moveSpeed * 2;
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(reposTrans != null)
        {
            Reposition();
            return;
        }

        FreeCam();
        
	}

    void FreeCam()
    {
        //Translation
        cameraBoom.transform.Translate(Vector3.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,Space.Self);
        cameraBoom.transform.Translate(Vector3.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime, Space.Self);

        float x = Mathf.Clamp(cameraBoom.transform.position.x, maxLeft, maxRight);
        float z = Mathf.Clamp(cameraBoom.transform.position.z, maxDown, maxUp);

        cameraBoom.transform.position = new Vector3(x, boomHeight, z);

        //Zoom
        //TODO - set target zoom and smooth out separately (Also change angle?)
        float zoom = Input.GetAxis("Zoom");
        dist = Vector3.Distance(this.transform.localPosition, new Vector3(0, 0, 0));

        if(zoom > 0)
        {
            if(dist < minZoomDist )
            {
                zoom = 0;
            }
        }
        else if(zoom < 0)
        {
            if (dist > maxZoomDist)
            {
                zoom = 0;
            }
        }


        transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(0,0,0), zoom * zoomSpeed * Time.deltaTime);
        
        
        zoomScale = dist / (maxZoomDist - minZoomDist);


        
        //transform.Translate(Vector3.forward * zoomSpeed * Input.GetAxis("Zoom") * Time.deltaTime, Space.Self);
        //cam.fieldOfView -= (zoomSpeed * Input.GetAxis("Zoom") * Time.deltaTime);


        //Rotation
        if(Input.GetButton("Rotate"))
        {
            x = rotateSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
            float y = -rotateSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            cameraBoom.transform.Rotate(0, x, 0, Space.World);

            //Debug.Log("Rotation Rad2Deg: " + cameraBoom.transform.rotation.x + ", x")

            float xRot = cameraBoom.transform.rotation.eulerAngles.x;

            if (y > 0)
            {

                if (xRot > maxPitchUp)
                {
                    y = 0;
                }
            }
            else if (y < 0)
            {
                if (xRot < maxPitchDown)
                {
                    y = 0;
                }
            }
            
            cameraBoom.transform.Rotate(y, 0, 0, Space.Self);


            //x = Mathf.Clamp(cameraBoom.transform.rotation.x, maxPitchDown, maxPitchUp);
        }



    }


    public void Recenter()
    {
        MoveToPos(center);
    }

    public void SetCenter(Transform center)
    {
        this.center = center;
    }

    public void MoveToPos(Transform pos)
    {
        reposTrans = pos;
    }

    void Reposition()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        x += (reposTrans.position.x - x) * 0.1f * 100 * Time.deltaTime;
        y += (reposTrans.position.y - y) * 0.1f * 100 * Time.deltaTime;
        z += (reposTrans.position.z - z) * 0.1f * 100 * Time.deltaTime;

        transform.position = new Vector3(x, y, z);

        if(Vector3.Distance(transform.position, reposTrans.position) <= 0.1f)
        {
            reposTrans = null;
        }

    }


}
