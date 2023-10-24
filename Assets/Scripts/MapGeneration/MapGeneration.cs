using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    public Camera cam;
    public GameObject nodePrefab;
    public int numberOfNodes = 10;
    public float nodeSpacing = 2f;
    private List<cell> nodes = new List<cell>();
    public LayerMask whatIsRoom;
    //Nombre Max des cells qu'il y a a la fin, ils vont etre sois des shop ou rest pour faire sure que le joueur puisse se 'reposer' pour le combat du boss
    public int maxEndCells = 2;
    public int currEndCells = 0;

    public int rows = 8;
    public int columns = 4;
    public int offsetX = -3;

    public int maxShop;
    public int maxRest;

    public int currShop;
    public int currRest;


    public enum typeOfRoom {Battle, Shop, Rest, Boss, Starting}
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
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 5f);
            if (Physics.Raycast(ray, out hit, 10f, whatIsRoom)){
                for(int i =0; i < nodes.Count; i++)
                {
                    if(hit.collider.gameObject.GetComponent<RoomInfo>().thisCell.obj == nodes[i].obj)
                    {
                        Debug.Log("We are entering this room : " + nodes[i].room);
                    }
                }
            }
        }

    }

    void GenerateMap()
    {
        GameObject StartNode = Instantiate(nodePrefab, gameObject.transform);
        cell FirstCell = new cell();
        FirstCell.obj = StartNode;
        FirstCell.room = typeOfRoom.Starting;

        nodes.Add(FirstCell);
        for (int j = 1; j < columns; j++) {
            for(int i = 0; i < rows; i++)
            {
                if(j == columns && currEndCells <= maxEndCells)
                {
                    currEndCells++;
                    GameObject finalNode = Instantiate(nodePrefab, new Vector3(transform.position.x + (i * nodeSpacing) + offsetX, transform.position.y, transform.position.z + (j * nodeSpacing)), Quaternion.identity);
                    finalNode.AddComponent<RoomInfo>();
                    cell finalCell = new cell();
                    finalCell.obj = finalNode;
                    finalCell.room = GetRandomEndRoom();
                    finalCell.column = j;
                    finalCell.row = i;
                    finalNode.GetComponent<RoomInfo>().thisCell = finalCell;
                    nodes.Add(finalCell);
                }
                else
                {
                    GameObject Node = Instantiate(nodePrefab, new Vector3(transform.position.x + (i * nodeSpacing) + offsetX, transform.position.y, transform.position.z + (j * nodeSpacing)), Quaternion.identity);
                    Node.AddComponent<RoomInfo>();
                    cell thisCell = new cell();
                    thisCell.obj = Node;
                    thisCell.room = GetAvailableRoom();
                    Node.GetComponent<RoomInfo>().thisCell = thisCell;
                    nodes.Add(thisCell);
                }
            }
        }
        GameObject EndNode = Instantiate(nodePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (columns * nodeSpacing)), Quaternion.identity);
        cell endCell = new cell();
        endCell.obj = EndNode;
        endCell.room = typeOfRoom.Boss;
        nodes.Add(endCell);
    }

    typeOfRoom GetAvailableRoom()
    {
        return typeOfRoom.Battle;
    }

    typeOfRoom GetRandomEndRoom()
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
}
