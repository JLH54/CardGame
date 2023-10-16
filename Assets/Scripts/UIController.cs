using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TMP_Text playerMoveText;

    public GameObject moveWarning;

    public float moveWarningTime;

    private float moveWarningCounter;

    public GameObject endTurnButton;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moveWarningCounter > 0)
        {
            moveWarningCounter -= Time.deltaTime;
            if(moveWarningCounter <= 0)
            {
                moveWarning.SetActive(false);
            }
        }
    }

    public void SetPlayerMoveText(int moveAmount)
    {
        playerMoveText.text = "Move : " + moveAmount;
    }

    public void ShowMoveWarning()
    {
        moveWarning.SetActive(true);
        moveWarningCounter = moveWarningTime;
    }

    public void EndPlayerButton()
    {
        //Doit aller en premier aussi non sa bug la premiere carte qui sort
        HandController.instance.EmptyHand();
        BattleController.instance.EndPlayerTurn();
    }
}
