using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPooler : MonoBehaviour
{
    [SerializeField]
    private GameObject pooledObjectPrefab;
    [SerializeField]
    private int pooledAmount = 2;
    [SerializeField]
    private bool willGrow = true;

    private List<GameObject> pooledObjects;
    private List<AudioSourceElement> audioSourceElements;

    private void Awake()
    {
        InitializePooledObjects();
    }

    private void InitializePooledObjects()
    {
        pooledObjects = new List<GameObject>();
        audioSourceElements = new List<AudioSourceElement>();

        for (int i = 0; i < pooledAmount; i++)
        {
            InstantiatePooledObject();
        }
    }

    private AudioSourceElement InstantiatePooledObject()
    {
        GameObject obj = Instantiate(pooledObjectPrefab, gameObject.transform);

        var audioSourceElement = obj.GetComponent<AudioSourceElement>();
        audioSourceElements.Add(audioSourceElement);
        obj.SetActive(false);
        pooledObjects.Add(obj);

        return audioSourceElement;
    }
    public AudioSourceElement GetAvailablePooledObject()
    {
        for (int i = 0; i < audioSourceElements.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return audioSourceElements[i];
            }
        }
        if (willGrow)
        {
            return InstantiatePooledObject();
        }

        return null;
    }
}
