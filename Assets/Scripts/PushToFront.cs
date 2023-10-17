using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PushToFront : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetLayer("CardInfo");
    }

    public void SetLayer(string name)
    {
        GetComponent<Renderer>().sortingLayerName = name;
    }
}
