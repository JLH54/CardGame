using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

    public List<cardWithEffect> deck = new List<cardWithEffect>();

    private List<cardWithEffect> activeCards = new List<cardWithEffect>();

    private List<GameObject> cardsInHand = new List<GameObject>();

    public GameObject cardToSpawn;

    public float waitBetweenDrawingCards = 0.25f;

    [System.Serializable]
    public struct cardWithEffect
    {
        public CardScriptable card;
        public ICardEffect effect;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpDeck()
    {
        activeCards.Clear();

        List<cardWithEffect> tempDeck = new List<cardWithEffect>();
        tempDeck.AddRange(deck);
        while(tempDeck.Count > 0)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
        }
    }

    public void DrawCardToHand()
    {
        if(activeCards.Count <= 0)
        {
            SetUpDeck();
        }

        GameObject newCardGO = Instantiate(cardToSpawn, transform.position, transform.rotation);
        ICardEffect cardEffect = newCardGO.AddComponent<ICardEffect>();
        cardEffect = activeCards[0].effect;
        Card newCard = newCardGO.GetComponent<Card>();
        newCard.cardSO = activeCards[0].card;
        newCard.SetUpCard();
        cardsInHand.Add(newCardGO);

        activeCards.RemoveAt(0);

        HandController.instance.AddCardToHand(newCard);
    }

    public void DrawMultipleCards(int amountToDraw)
    {
        StartCoroutine(DrawMultipleCo(amountToDraw));
    }

    IEnumerator DrawMultipleCo(int amountToDraw)
    {
        for(int i =0;i<amountToDraw; i++)
        {
            DrawCardToHand();

            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }
}