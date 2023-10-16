using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;

    public List<Card> heldCards = new List<Card>();

    public Transform minPos, maxPos;

    public List<Vector3> cardPositions = new List<Vector3>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionInHand();
    }

    public void SetCardPositionInHand()
    {
        cardPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;

        if(heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }

        for(int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPos.position + (distanceBetweenPoints * i));

            heldCards[i].MoveToPoint(cardPositions[i], minPos.rotation);

            heldCards[i].inHand = true;
            heldCards[i].handPosition = i;
        }
    }


    public void RemoveCardFromHand(Card cardToRemove)
    {
        if(heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        }
        else
        {
            Debug.Log("Card at position " + cardToRemove.handPosition + " is not the card being removed from hand");
        }
        SetCardPositionInHand();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        if(heldCards.Count >= 5)
        {
            return;
        }
        heldCards.Add(cardToAdd);
        SetCardPositionInHand();
    }

    public void EmptyHand()
    {
        foreach(Card heldCard in heldCards)
        {
            heldCard.inHand = false;
            heldCard.MoveToPoint(BattleController.instance.discardPile.position, heldCard.transform.rotation);
        }
        heldCards.Clear();
    }
}
