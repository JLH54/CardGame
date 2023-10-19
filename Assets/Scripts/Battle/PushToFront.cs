using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Fait en sorte que la sorting layer du texte(TMPRO) peut etre changer 
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
