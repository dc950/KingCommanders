using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

    //Variables

    //public Variables
    public float moveSpeed;
    public float rotateSpeed;
    public float zoomSpeed;

    //Clamps
    public float maxLeft, maxRight, maxUp, maxDown;
    public float maxZoomIn, maxZoomOut;
    public float maxPitchUp, maxPitchDown;


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
        cameraBoom.transform.Translate(Vector3.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,Space.World);
        cameraBoom.transform.Translate(Vector3.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime, Space.World);

        //Zoom TODO - set target zoom and smooth out separately (Also change angle?)
        transform.Translate(Vector3.forward * zoomSpeed * Input.GetAxis("Zoom") * Time.deltaTime, Space.Self);
        //cam.fieldOfView -= (zoomSpeed * Input.GetAxis("Zoom") * Time.deltaTime);

        //Rotation
        if(Input.GetButton("Rotate"))
        {
            float x = rotateSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
            float y = rotateSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            cameraBoom.transform.Rotate(0, x, 0, Space.World);
            cameraBoom.transform.Rotate(y, 0, 0, Space.Self);
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
