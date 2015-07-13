using UnityEngine;
using System.Collections.Generic;

public class Arrow : MonoBehaviour {

    Vector3 start;
    public Vector3 end;

    float halfX, halfZ;

    public bool damage = false;
    public Archer archer;
    public UnitBuilding targetUB;

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
    }
	
	// Update is called once per frame
	void Update () {
        //DrawLine();
        if (ObjectDictionary.getStateController().state == StateController.states.Attacking)
        {
            Move();
        }
	}

    void Move()
    {
        if (points.Count <= 1)
        {
            if (damage)
            {
                DealDamage();
            }

            Destroy(this.gameObject);
        }

        if(transform.position == points[0])
        {
            points.RemoveAt(0);

        }

        transform.position = Vector3.MoveTowards(transform.position, points[0], Time.deltaTime * 2);
        transform.LookAt(points[0]);
    }

    void DealDamage()
    {
        if (targetUB != null)
        {
            archer.Attack(targetUB);
        }
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
