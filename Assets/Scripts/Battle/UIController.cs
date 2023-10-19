using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Le UI du joueur pendant le combat
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
    //Montre le nombre de move que le joueur a
    public void SetPlayerMoveText(int moveAmount)
    {
        playerMoveText.text = "Move : " + moveAmount;
    }
    //Montre que le joueur n'a pas assez de move
    public void ShowMoveWarning()
    {
        moveWarning.SetActive(true);
        moveWarningCounter = moveWarningTime;
    }
    //Quand le joueur clique sur le boutton pour finir son tour
    public void EndPlayerButton()
    {
        //Doit aller en premier aussi non sa bug la premiere carte qui sort
        HandController.instance.EmptyHand();
        BattleController.instance.EndPlayerTurn();
    }
}
