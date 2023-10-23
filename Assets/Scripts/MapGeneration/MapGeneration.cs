using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    public GameObject nodePrefab;
    public int numberOfNodes = 10;
    public float nodeSpacing = 2f;
    private List<GameObject> nodes = new List<GameObject>();

    public int rows = 8;
    public int columns = 4;

    private void Start()
    {
        GenerateMap();
    }

    //void GenerateMap()
    //{
    //    Vector3 position = Vector3.zero;

    //    for (int i = 0; i < numberOfNodes; i++)
    //    {
    //        GameObject node = Instantiate(nodePrefab, position, Quaternion.identity);
    //        node.transform.SetParent(transform);

    //        position.x += nodeSpacing;

    //        if(Random.Range(0f,1f) < 0.2f && i < numberOfNodes - 1)
    //        {
    //            int branches = Random.Range(1, 4);
    //            for (int b = 0; b < branches; b++)
    //            {
    //                position.y += nodeSpacing;
    //                GameObject branchNode = Instantiate(nodePrefab, position, Quaternion.identity);
    //                branchNode.transform.SetParent(transform);
    //            }
    //        }
    //    }
    //}

    void GenerateMap()
    {
        GameObject StartNode = Instantiate(nodePrefab, gameObject.transform);
        nodes.Add(StartNode);
        for (int j = 1; j < columns; j++) {
            for(int i = 0; i < rows; i++)
            {
                GameObject Node = Instantiate(nodePrefab, new Vector3(transform.position.x + (i * nodeSpacing), transform.position.y, transform.position.z + (j * nodeSpacing)), Quaternion.identity);
                nodes.Add(Node);
            }
        }
        GameObject EndNode = Instantiate(nodePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (columns * nodeSpacing)), Quaternion.identity);
        nodes.Add(EndNode);
    }
}
