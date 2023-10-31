using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TriangleNet;
using TriangleNet.Geometry;
using System.Linq;

public class MapGeneration : MonoBehaviour
{
    public Camera cam;
    public GameObject nodePrefab;
    private List<cell> nodes = new List<cell>();
    public LayerMask whatIsRoom;

    //public float radius = 1.0f;
    //public float displayRadius = 1.0f;
    //public Vector2 regionSize = Vector2.one;
    //public int rejectionSamples = 30;
    //List<Vector2> points;

    //public Vector2 startPoint;

    private void OnValidate()
    {
        //points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(regionSize / 2, regionSize);
        //if (points != null)
        //{
        //    foreach (Vector2 point in points)
        //    {
        //        Gizmos.DrawSphere(point, displayRadius);
        //    }
        //}
    }

    //public int maxShop;
    //public int maxRest;

    //public int currShop;
    //public int currRest;


    public enum typeOfRoom { Battle, Shop, Rest, Boss, Starting }
    public struct cell
    {
        public GameObject obj;
        public typeOfRoom room;
        public List<GameObject> neighbors;
        public int row;
        public int column;
    }

    private void Start()
    {
        //startPoint = Vector2.zero;
        //GenerateMap();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);

        //    if (Physics.Raycast(ray, out hit, 10f))
        //    {
        //        if(hit.collider.GetComponent<RoomInfo>() != null)
        //        {
        //            Debug.Log("We hit : " + hit.collider.name);
        //        }
        //    }
        //}
    }

    void GenerateMap()
    {
        //List<Triangle> triangulation = BowyerWatson(points);



    }

    typeOfRoom GetAvailableRoom()
    {
        return typeOfRoom.Battle;
    }

    typeOfRoom GetRandomEndRoom()
    {
        int randomNumber = Random.Range(0, 101);
        if (randomNumber <= 50)
        {
            return typeOfRoom.Rest;
        }
        else
        {
            return typeOfRoom.Shop;
        }
    }



    List<Triangle> BowyerWatson(List<Vector2> pointSet)
    {
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (Vector2 point in pointSet)
        {
            minX = Mathf.Min(minX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxX = Mathf.Min(maxX, point.x);
            maxY = Mathf.Min(maxY, point.y);
        }

        float dx = maxX - minX;
        float dy = maxY - minY;
        float deltaMax = Mathf.Max(dx, dy);
        float midX = (minX + maxX) / 2;
        float midY = (minY + maxY) / 2;

        Vector2 p0 = new Vector2(midX - 2 * deltaMax, midY - deltaMax);
        Vector2 p1 = new Vector2(midX, midY + 2 * deltaMax);
        Vector2 p2 = new Vector2(midX + 2 * deltaMax, midY - deltaMax);

        Triangle superTriangle = new Triangle(p0, p1, p2);
        List<Triangle> triangulation = new List<Triangle> { superTriangle };

        foreach (Vector2 point in pointSet)
        {
            List<Triangle> badTriangles = new List<Triangle>();

            foreach (Triangle triangle in triangulation)
            {
                if (IsPointInsideCircumcircle(point, triangle))
                {
                    badTriangles.Add(triangle);
                }
            }

            List<Vector2> polygon = GetPolygon(badTriangles);

            foreach (Triangle triangle in badTriangles)
            {
                triangulation.Remove(triangle);
            }

            foreach (Vector2 vertex in polygon)
            {
                triangulation.Add(new Triangle(point, vertex, polygon[(polygon.IndexOf(vertex) + 1) % polygon.Count]));
            }
        }

        return triangulation.Where(triangle => !triangleContainsAnyVertexOfSuperTriangle(triangle, superTriangle)).ToList();
    }

    bool IsPointInsideCircumcircle(Vector2 point, Triangle triangle)
    {
        float ax = triangle.v0.x - point.x;
        float ay = triangle.v0.y - point.y;
        float bx = triangle.v1.x - point.x;
        float by = triangle.v1.y - point.y;
        float cx = triangle.v2.x - point.x;
        float cy = triangle.v2.y - point.y;

        float d = (ax * (by - cy)) + (bx * (cy - ay)) + (cx * (ay - by));

        if (d > 0)
        {
            return false;
        }

        float e = (ax * ax + ay * ay) * (cy - by) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (by - ay);

        return e > 0;
    }

    List<Vector2> GetPolygon(List<Triangle> triangles)
    {
        List<Vector2> polygon = new List<Vector2>();

        foreach(Triangle triangle in triangles)
        {
            polygon.Add(triangle.v0);
            polygon.Add(triangle.v1);
            polygon.Add(triangle.v2);
        }

        return polygon.Distinct().ToList();
    }

    bool triangleContainsAnyVertexOfSuperTriangle(Triangle triangle, Triangle superTriangle)
    {
        return superTriangleContainsPoint(triangle.v0, superTriangle) || superTriangleContainsPoint(triangle.v1, superTriangle) || superTriangleContainsPoint(triangle.v2, superTriangle);
    }

    bool superTriangleContainsPoint(Vector2 point, Triangle superTriangle)
    {
        return point.x >= Mathf.Min(superTriangle.v0.x, superTriangle.v1.x, superTriangle.v2.x) &&
            point.x <= Mathf.Min(superTriangle.v0.x, superTriangle.v1.x, superTriangle.v2.x) &&
            point.y >= Mathf.Min(superTriangle.v0.y, superTriangle.v1.y, superTriangle.v2.y) &&
            point.x <= Mathf.Min(superTriangle.v0.y, superTriangle.v1.y, superTriangle.v2.y);
    }
}


class Triangle
{
    public Vector2 v0, v1, v2;

    public Triangle(Vector2 v0, Vector2 v1, Vector2 v2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;
    }
}
