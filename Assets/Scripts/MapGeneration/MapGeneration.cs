using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TriangleNet;
//using TriangleNet.Geometry;
using System.Linq;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;
using static MapGeneration;

public class MapGeneration : MonoBehaviour
{
    public Camera cam;
    public GameObject nodePrefab;
    //private List<cell> nodes = new List<cell>();
    public LayerMask whatIsRoom;
    public Vector2 gridSize;
    public float nodeSpacing = 2f;
    public int maxEndCells = 2;
    public int currEndCells = 0;
    public int offSetX = -3;
    public int maxShop;
    public int maxRest;
    public int currRest;
    public int currShop;
    public int numberOfCellsToRemove = 5;

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



    public enum typeOfRoom { Battle, Shop, Rest, Boss, Starting }
    public struct cell
    {
        public GameObject obj;
        public typeOfRoom room;
        public List<cell> neighbors;
        public int row;
        public int column;
    }

    private void Start()
    {
        //startPoint = Vector2.zero;
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.GetComponent<RoomInfo>() != null)
                {
                    Debug.Log("We hit : " + hit.collider.name);
                }
            }
        }
    }

    void GenerateMap()
    {
        //List<Triangle> triangulation = BowyerWatson(points);
        List<cell> cells = GenerateGrid();
        List<cell> path = FindPath(cells[0], cells.Find(cell => cell.room ==typeOfRoom.Boss));

        if (path != null)
        {
            RemoveRandomCells(cells);
        }
        else
        {

        }

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

    List<cell> GenerateGrid()
    {
        List<cell> nodes = new List<cell>();

        List<int> nodesTest = new List<int>();

        GameObject StartNode = Instantiate(nodePrefab, gameObject.transform);
        cell FirstCell = new cell();
        FirstCell.row = 0;
        FirstCell.column = 0;
        FirstCell.obj = StartNode;
        FirstCell.room = typeOfRoom.Starting;
        FirstCell.neighbors = new List<cell>();
        nodes.Add(FirstCell);

        for (int y = 1; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                if (x == gridSize.x && currEndCells <= maxEndCells)
                {
                    currEndCells++;
                    GameObject finalNode = Instantiate(nodePrefab, new Vector3(transform.position.x + (x * nodeSpacing) + offSetX, transform.position.y, transform.position.z + (y * nodeSpacing)), Quaternion.identity);
                    finalNode.AddComponent<RoomInfo>();
                    cell finalCell = new cell();
                    finalCell.obj = finalNode;
                    finalCell.room = GetRandomEndRoom();
                    finalCell.column = x;
                    finalCell.row = y;
                    finalCell.neighbors = new List<cell>();
                    finalNode.GetComponent<RoomInfo>().thisCell = finalCell;
                    nodes.Add(finalCell);
                }
                else
                {
                    GameObject Node = Instantiate(nodePrefab, new Vector3(transform.position.x + (x * nodeSpacing) + offSetX, transform.position.y, transform.position.z + (y * nodeSpacing)), Quaternion.identity);
                    Node.AddComponent<RoomInfo>();
                    cell thisCell = new cell();
                    thisCell.obj = Node;
                    thisCell.room = GetAvailableRoom();
                    thisCell.row = x;
                    thisCell.column = y;
                    thisCell.neighbors = new List<cell>();
                    Node.GetComponent<RoomInfo>().thisCell = thisCell;
                    nodes.Add(thisCell);
                }
            }
        }
        GameObject EndNode = Instantiate(nodePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (gridSize.y * nodeSpacing)), Quaternion.identity);
        cell endCell = new cell();
        endCell.row = (int)gridSize.x;
        endCell.column = (int)gridSize.y;
        endCell.obj = EndNode;
        endCell.room = typeOfRoom.Boss;
        endCell.neighbors = new List<cell>();
        nodes.Add(endCell);

        for(int i = 0; i < nodes.Count; i++)
        {
            List<cell> neighbors = new List<cell>();
            cell currentCell = nodes[i];

            foreach (cell cellB in nodes)
            {
                if (currentCell.row - cellB.row >= -1 && currentCell.column == cellB.column)
                {
                    neighbors.Add(cellB);
                }else if((currentCell.row - cellB.row >= -1) && ((currentCell.column - 1 == cellB.column) || (currentCell.column + 1 == cellB.column)))
                {
                    neighbors.Add(cellB);
                }
            }
            currentCell.neighbors = neighbors;
            nodes[i] = currentCell;
        }
        return nodes;
    }

    public List<cell> FindPath(cell start, cell goal)
    {
        List<cell> openSet = new List<cell>();
        HashSet<cell> closedSet = new HashSet<cell>();
        Dictionary<cell, cell> cameFrom = new Dictionary<cell, cell>();
        Dictionary<cell, float> gScore = new Dictionary<cell, float>();
        Dictionary<cell, float> fScore = new Dictionary<cell, float>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            cell current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fScore[openSet[i]] < fScore[current])
                    current = openSet[i];
            }

            if (current.Equals(goal))
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            //List<cell> neighbors = GetNeighbors(current);
            foreach (cell neighbor in current.neighbors)
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + DistanceBetween(current, neighbor);

                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null;
    }

    private List<cell> GetNeighbors(cell current)
    {
        List<cell> neighbors = new List<cell>();
        foreach (cell cellB in current.neighbors)
        {
            if (Mathf.Abs(current.row - cellB.row) == 1 && current.column == cellB.column)
            {
                neighbors.Add(cellB);
            }
            else if (Mathf.Abs(current.column - cellB.column) == 1 && current.row == cellB.row)
            {
                neighbors.Add(cellB);
            }
        }

        return neighbors;
    }

    private float Heuristic(cell a, cell b)
    {
        return Mathf.Abs(a.row - b.row) + Mathf.Abs(a.column - b.column);
    }

    private float DistanceBetween(cell a, cell b)
    {
        return 1;
    }

    private List<cell> ReconstructPath(Dictionary<cell, cell> cameFrom, cell current)
    {
        List<cell> path = new List<cell>();
        while (cameFrom.ContainsKey(current))
        {
            path.Insert(0, current);
            current = cameFrom[current];
        }
        return path;
    }

    private void RemoveRandomCells(List<cell> cells)
    {
        for(int i = 0; i < numberOfCellsToRemove; i++)
        {
            int randomIndex = Random.Range(1, cells.Count);
            cell cellToRemove = cells[randomIndex];

            RemoveCell(cells, cellToRemove);

            List<cell> path = FindPath(cells[0], cells.Find(cell => cell.room == typeOfRoom.Boss));
            if(path == null)
            {
                RestoreCell(cells,cellToRemove);
            }
        }
    }

    private void RemoveCell(List<cell> cells,cell cellToRemove)
    {
        cells.Remove(cellToRemove);
        Destroy(cellToRemove.obj);
    }

    private void RestoreCell(List<cell> cells,cell cellToRestore)
    {
        cells.Add(cellToRestore);
        Instantiate(nodePrefab, new Vector3(transform.position.x + (cellToRestore.row * nodeSpacing) + offSetX, transform.position.y, transform.position.z + (cellToRestore.column * nodeSpacing)), Quaternion.identity);
    }
}