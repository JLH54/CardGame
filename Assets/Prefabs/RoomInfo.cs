using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo : MonoBehaviour
{
    public GameObject roomGO;
    public int row;
    public int column;
    public MapGeneration.typeOfRoom room;
    public List<MapGeneration.cell> neighbors;
    public MapGeneration.cell thisCell;
    public SpriteRenderer img;
    public GameObject select;
    public bool HaveBeenSelected;
    
    //0=Battle, 1=Event,2=Rest,3=Shop,4=Boss,5=Start
    public List<Sprite> images;

    public void Start()
    {
        HaveBeenSelected = false;
        show();
    }

    public void Update()
    {
        if (thisCell.isOn)
        {
            //Play animation
        }
    }

    public void show()
    {
        row = thisCell.row;
        column = thisCell.column;
        room = thisCell.room;
        neighbors = thisCell.neighbors;
        
        switch (room)
        {
            case MapGeneration.typeOfRoom.Battle:
                img.sprite = images[0];
                break;
            case MapGeneration.typeOfRoom.Event:
                img.sprite = images[1];
                break;
            case MapGeneration.typeOfRoom.Rest:
                img.sprite = images[2];
                break;
            case MapGeneration.typeOfRoom.Shop:
                img.sprite = images[3];
                break;
            case MapGeneration.typeOfRoom.Boss:
                img.sprite = images[4];
                break;
            case MapGeneration.typeOfRoom.Starting:
                img.sprite = images[5];
                break;
        }
    }

    public void Hover()
    {
        select.SetActive(true);
    }

    public void StopHover()
    {
        select.SetActive(false);
    }

    public void Selected()
    {
        switch (room)
        {
            case MapGeneration.typeOfRoom.Battle:
                //SceneManager.LoadScene(0);
                break;
            case MapGeneration.typeOfRoom.Event:
                break;
            case MapGeneration.typeOfRoom.Rest:
                break;
            case MapGeneration.typeOfRoom.Shop:
                break;
            case MapGeneration.typeOfRoom.Boss:
                break;
            case MapGeneration.typeOfRoom.Starting:
                break;
        }

        MapGeneration.instance.ShowLinks();
    }
}
