using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public int maxMoves = 3;

    public int playerMoves;

    public int startingCardsAmount = 5;

    public enum TurnOrder { player, enemy}

    public TurnOrder currentPhase;

    public int cardsToDrawPerTurn = 5;

    public Transform discardPile;

    public GameObject[] enemiesGO;

    public Enemy[] enemies;

    public float timeToWaitBetweenAttacks = 2f;

    public GameObject endSreen;

    public TMP_Text conditionText;

    public int enemiesDead = 0;

    public int goldEarned = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPlayerMove(maxMoves);
        DeckController.instance.DrawMultipleCards(startingCardsAmount);
        enemiesGO = GameObject.FindGameObjectsWithTag("Enemy");
        enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetAttack();
        }
    }

    public void SpendPlayerMoves(int amountToSpend)
    {
        playerMoves = playerMoves - amountToSpend;
        if(playerMoves < 0)
        {
            playerMoves = 0;
        }
        UIController.instance.SetPlayerMoveText(playerMoves);
    }

    public void AdvanceTurn()
    {
        currentPhase++;
        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length) currentPhase = 0;
        switch (currentPhase)
        {
            case TurnOrder.player:
                CheckGameCondition();
                UIController.instance.endTurnButton.SetActive(true);
                DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);
                break;
            case TurnOrder.enemy:
                Debug.Log("Enemi turn");
                StartCoroutine(EnemyTurnCo());
                CheckGameCondition();
                AdvanceTurn();
                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        AdvanceTurn();
    }

    public void AddPlayerMove(int amount)
    {
        playerMoves += amount;
        if(playerMoves > maxMoves)
        {
            playerMoves = maxMoves;
        }
        UIController.instance.SetPlayerMoveText(playerMoves);
    }

    IEnumerator EnemyTurnCo()
    {
        foreach (Enemy enemy in enemies)
        {
            yield return new WaitForSeconds(timeToWaitBetweenAttacks);
            if (!enemy) continue;
            enemy.Attack();
            enemy.SetAttack();
        }
    }

    public void CheckGameCondition()
    {
        if (Player.instance.currentHealth == 0)
        {
            UIController.instance.gameObject.SetActive(false);
            endSreen.SetActive(true);
            conditionText.text = "You lost.";
        }
        else if(enemiesDead == 3)
        {
            UIController.instance.gameObject.SetActive(false);
            endSreen.SetActive(true);
            conditionText.text = "You won " + goldEarned + " gold coins.";
        }
    }
}
