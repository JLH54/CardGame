using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    private Character character;

    public static GameInstance instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void characterSelected(Character chosenCharacter)
    {
        character = chosenCharacter;
    }

    public Character GiveCharacterForBattle()
    {
        return character;
    }
}
