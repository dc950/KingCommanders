using UnityEngine;
using System.Collections.Generic;

public class Arrow : MonoBehaviour {

    Vector3 start;
    public Vector3 end;

    float halfX, halfZ;

    int numberOfPoints = 100;
    List<Vector3> points;


	// Use this for initialization
	void Start () {
        
        start = transform.position;

        points = new List<Vector3>();

        CreateCurve();
	}

    void CreateCurve()
    {

        for(int i = 0; i < numberOfPoints; i++)
        {
            float x = start.x + (((end.x - start.x)/numberOfPoints)*i);
            
            float z = start.z + (((end.z - start.z)/numberOfPoints)*i);

            float yPoint = (i - (numberOfPoints/2));
            float y = -(yPoint*yPoint) + ((numberOfPoints/2)*(numberOfPoints/2));
            y *= 0.0003f;

            Vector3 nextPoint = new Vector3(x, y, z);
            points.Add(nextPoint);

        }

        string s = "";

        foreach(Vector3 v in points)
        {
            s += "("+ v.x + "," + v.y + ","+ v.z +"), ";
        }

        Debug.Log(s);
    }
	
	// Update is called once per frame
	void Update () {
        //DrawLine();
        Move();
	}

    void Move()
    {
        if(transform.position == points[0])
        {
            points.RemoveAt(0);
            if(points.Count <= 1)
            {
                Destroy(this.gameObject);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, points[0], Time.deltaTime * 2);
        transform.LookAt(points[0]);
    }

    void DrawLine()
    {
        int currentTile = 0;
        int nextTile = 1;

        while (nextTile < points.Count)
        {
            Vector3 start = points[currentTile];
            Vector3 end = points[nextTile];


            Debug.DrawLine(start, end, Color.red);
            currentTile = nextTile;
            nextTile++;
        }
    }


}
