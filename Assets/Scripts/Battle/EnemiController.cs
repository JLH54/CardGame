using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Existe, mais je ne l'utilise pas encore
public class EnemiController : MonoBehaviour
{
    public static EnemiController instance;

    public List<Enemy> enemis = new List<Enemy>();

    public Transform minPos, maxPos;

    public List<Vector3> enemiPositions = new List<Vector3>();

    private Vector3 targetPoint;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            enemis.Add(enemy);
        }
        SetEnemiPositions();
    }

    public void SetEnemiPositions()
    {
        enemiPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;

        if (enemis.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (enemis.Count - 1);
        }

        for (int i = 0; i < enemis.Count; i++)
        {
            enemiPositions.Add(minPos.position + (distanceBetweenPoints * i));

            enemis[i].transform.position = enemiPositions[i];

            enemis[i].isDead = false;
            enemis[i].position = i;
        }
    }

    public void RemoveEnemiFromField(Enemy enemyToRemove)
    {
        if (enemis[enemyToRemove.position] == enemyToRemove)
        {
            enemis.RemoveAt(enemyToRemove.position);
        }
        else
        {
            Debug.Log("Card at position " + enemyToRemove.position + " is not the card being removed from hand");
        }
        SetEnemiPositions();
    }
}
