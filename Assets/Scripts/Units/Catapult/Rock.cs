using UnityEngine;
using System.Collections.Generic;

public class Rock : MonoBehaviour
{

    Vector3 start;
    public Vector3 end;

    float halfX, halfZ;

    public bool damage = false;
    public Catapult catapult;
    public Tile targetTile;

    int numberOfPoints = 100;
    List<Vector3> points;

    public List<GameObject> rocks;
    GameObject go;

    float velocity = 7;

    // Use this for initialization
    void Start()
    {

        start = transform.position;

        points = new List<Vector3>();
        PlaceObject();
        SetTrajectory();

        end = targetTile.getWorldCoords();


    }

    void SetTrajectory()
    {
        transform.LookAt(end);
        Rigidbody rb = GetComponent<Rigidbody>();

        float angle = findAngle(velocity, end);

        if(angle == null)
        {
            Destroy(transform.parent);
            Destroy(gameObject);
            Destroy(this);
            return;
        }

        float x = velocity * Mathf.Cos(angle);
        float y = velocity * Mathf.Sin(angle);

        Debug.Log("Force x " + x + "Force y: " + y);

        Vector3 force = new Vector3(x, y, 0);

        rb.velocity = force;
    }

    void CreateCurve()
    {
        if (end != null)
        {
            end += new Vector3(0, 0.39f, 0);
        }
        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = start.x + (((end.x - start.x) / numberOfPoints) * i);

            float z = start.z + (((end.z - start.z) / numberOfPoints) * i);

            float yPoint = (i - (numberOfPoints / 2));
            float y = -(yPoint * yPoint) + ((numberOfPoints / 2) * (numberOfPoints / 2));
            y *= 0.0006f;

            Vector3 nextPoint = new Vector3(x, y, z);
            points.Add(nextPoint);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(AtPosition())
        {
            Debug.Log("At position");
            DealDamage();
            Destroy(this);
        }
    }

    void DealDamage()
    {
        if (targetTile != null)
        {
            if(targetTile.building!= null)
            {
                catapult.Attack(targetTile.building);
            }
            else if (targetTile.unit != null)
            {
                catapult.Attack(targetTile.unit);
            }
            
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

    void PlaceObject()
    {
        int i = Random.Range(0, rocks.Count - 1);

        go = (GameObject)Instantiate(rocks[i], transform.position, Quaternion.identity);
        go.transform.SetParent(transform);
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    float findAngle(float velocity, Vector3 target)
    {
        //v is velocity
        float v = velocity;
        //y is height
        float y = target.y - transform.position.y;
        //x is range
        float x = Vector3.Distance(transform.position, target);
        //g is gravity
        float g = -Physics.gravity.y;

        Debug.Log("v: " + v + ", y: " + y + ", x: " + x + ", g: " + g);

        float root = (v*v*v*v) - (g * ((g*x*x)+(2*y*v*v)));

        if(root <= 0)
        {
            Debug.Log("Too far...");
            return -1000;
        }

        float sqrt = Mathf.Sqrt(root);

        float atanP = ((v * v) + sqrt) / (g * x);
        float atanN = ((v * v) - sqrt) / (g * x);

        Debug.Log("Atan+: "+atanP+", atanN: "+atanN);
        float posAngle = Mathf.Atan(atanP);
        float negAngle = Mathf.Atan(atanN);

        Debug.Log("OrigRoot: "+root+"Root: "+sqrt+ ", Pos: " + posAngle + ", Neg: " + negAngle);

        Debug.Log("Pos Angle is " + posAngle * Mathf.Rad2Deg+", neg angle is"+negAngle * Mathf.Rad2Deg);

        return posAngle;
    }

    bool AtPosition()
    {
        if(Vector3.Distance(transform.position, end) <= 1 || transform.position.y <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


