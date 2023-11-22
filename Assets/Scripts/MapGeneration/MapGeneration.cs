using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration instance;
    public Camera cam;
    public GameObject nodePrefab;
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

    public Material lineMAT;
    public float lineThickness = 10f;
    public float overrideLineThickness = -1f;
    public float EndLineThickness => overrideLineThickness > -1f ? overrideLineThickness : lineThickness;
    public Color startColor = Color.white;
    public Color endColor = Color.black;



    private GameObject currObj;
    private GameObject selection;
    private LineRenderer mainLineRenderer;
    private List<LineRenderer> links = new List<LineRenderer>();
    private bool canGoInRoom = false;
    private List<cell> cells;

    public enum typeOfRoom { Battle, Event,Shop, Rest, Boss, Starting }
    public struct cell
    {
        public GameObject obj;
        public typeOfRoom room;
        public List<cell> neighbors;
        public int row;
        public int column;
        public bool selected;
        public bool isOn;
        public int index;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateMap();
        currObj = cells[0].obj.GetComponentInChildren<RoomInfo>().roomGO;
        ShowLinks();
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0.1f);
        if (Physics.Raycast(ray, out hit, 10f))
        {
            RoomInfo clickedInfo = hit.collider.GetComponent<RoomInfo>();
            if (clickedInfo != null)
            {
                if (currObj.GetComponent<RoomInfo>().thisCell.neighbors.Contains(clickedInfo.thisCell))
                {
                    canGoInRoom = true;
                    //Debug.Log("We hit : " + hit.collider.GetComponentInChildren<RoomInfo>().room);
                    clickedInfo.Hover();
                    selection = clickedInfo.gameObject;
                }
            }
            else
            {
                canGoInRoom = false;
                if (selection == null) return;
                selection.GetComponentInChildren<RoomInfo>().StopHover();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && canGoInRoom)
        {
            if (Physics.Raycast(ray, out hit, 10f))
            {
                RoomInfo clickedInfo = hit.collider.GetComponent<RoomInfo>();
                if (clickedInfo != null)
                {
                    for (int i = 0; i < links.Count; i++)
                    {
                        if (i != clickedInfo.thisCell.index)
                        {
                            Destroy(links[i]);
                        }
                    }
                    currObj = hit.collider.GetComponent<RoomInfo>().gameObject;
                    hit.collider.GetComponent<RoomInfo>().Selected();
                }
            }
        }
    }

    public void ShowLinks()
    {
        links.Clear();
        int myIndex = 0;
        for (int i =0; i < currObj.GetComponent<RoomInfo>().thisCell.neighbors.Count; i++)
        {
            cell thisCell = currObj.GetComponent<RoomInfo>().thisCell.neighbors[i];
            thisCell.index = myIndex;
            cells[i] = thisCell;
            cells[i].obj.GetComponentInChildren<RoomInfo>().thisCell = thisCell;
            currObj.GetComponent<RoomInfo>().thisCell.neighbors[i] = thisCell;
            GameObject linkGO = new GameObject();
            LineRenderer link = linkGO.AddComponent<LineRenderer>();
            link.SetPositions(new Vector3[]{ currObj.transform.position, currObj.GetComponent<RoomInfo>().thisCell.neighbors[i].obj.transform.position});
            link.textureMode = LineTextureMode.Tile;
            link.material = lineMAT;
            link.alignment = LineAlignment.View;
            link.startWidth = lineThickness;
            link.endWidth = EndLineThickness;
            link.startColor = startColor;
            link.endColor = endColor;
            links.Add(link);
            ++myIndex;
        }
    }

    void GenerateMap()
    {
        cells = GenerateGrid();
    }

    typeOfRoom GetAvailableRoom(int currentRow, int totalRow)
    {
        if(currentRow == totalRow - 1)
        {
            int randomNumber = Random.Range(0, 101);
            if(randomNumber <= 50)
            {
                return typeOfRoom.Rest;
            }
            else
            {
                return typeOfRoom.Shop;
            }
        }
        else
        {
            int randomNumber = Random.Range(0, 101);
            if(randomNumber <= 50)
            {
                return typeOfRoom.Battle;
            }
            else if(randomNumber <= 80)
            {
                return typeOfRoom.Event;
            }
            else if(randomNumber <= 90)
            {
                return typeOfRoom.Shop;
            }
            else
            {
                return typeOfRoom.Rest;
            }
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
        FirstCell.selected = true;
        FirstCell.isOn = true;
        StartNode.GetComponentInChildren<RoomInfo>().thisCell = FirstCell;
        StartNode.GetComponentInChildren<RoomInfo>().show();
        nodes.Add(FirstCell);
        for (int y = 1; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {

                GameObject Node = Instantiate(nodePrefab, new Vector3(transform.position.x + (x * nodeSpacing) + offSetX, transform.position.y, transform.position.z + (y * nodeSpacing)), Quaternion.identity);
                cell thisCell = new cell();
                thisCell.obj = Node;
                thisCell.room = GetAvailableRoom(y, (int)gridSize.y);
                thisCell.row = x;
                thisCell.column = y;
                thisCell.neighbors = new List<cell>();
                Node.GetComponentInChildren<RoomInfo>().thisCell = thisCell;
                Node.GetComponentInChildren<RoomInfo>().show();
                nodes.Add(thisCell);
            }
        }

        GameObject EndNode = Instantiate(nodePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (gridSize.y * nodeSpacing)), Quaternion.identity);
        cell endCell = new cell();
        endCell.row = 0;
        endCell.column = (int)gridSize.y;
        endCell.obj = EndNode;
        endCell.room = typeOfRoom.Boss;
        endCell.neighbors = new List<cell>();
        EndNode.GetComponentInChildren<RoomInfo>().thisCell = endCell;
        EndNode.GetComponentInChildren<RoomInfo>().show();
        nodes.Add(endCell);

        //Met les voisins(Haut gauche, Haut milieu, Haut droit)
        for(int i = 0; i < nodes.Count; i++)
        {
            foreach (cell cellB in nodes)
            {
                //Check for the first node
                if(nodes[i].room == typeOfRoom.Starting)
                {
                    foreach(cell cellA in nodes)
                    {
                        if(cellA.column == 1)
                        {
                            
                            nodes[i].neighbors.Add(cellA);
                        }
                        if(nodes[i].neighbors.Count >= gridSize.x)
                        {
                            break;
                        }
                    }
                    break;
                }
                //Check for boss room
                else if(nodes[i].column == gridSize.y - 1)
                {
                    nodes[i].neighbors.Add(nodes[nodes.Count - 1]);
                }
                //Check for the cell above it
                else if((cellB.column == nodes[i].column + 1 && cellB.row == nodes[i].row) ||
                    cellB.column == nodes[i].column + 1 && cellB.row == nodes[i].row + 1 ||
                    cellB.column == nodes[i].column + 1 && cellB.row == nodes[i].row - 1)
                {
                    nodes[i].neighbors.Add(cellB);
                }
            }
        }
        return nodes;
    }
}