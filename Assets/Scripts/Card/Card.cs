using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptable cardSO;
    public ICardEffect effect;

    public int currentHealth;
    public int moveCost;

    public TMP_Text attackText;
    public TMP_Text costText;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public GameObject attackTextGO;
    public GameObject costTextGO;
    public GameObject nameTextGO;
    public GameObject descriptionTextGO;
    public GameObject atkIconGO;
    public GameObject costIconGO;

    public Canvas canvasOfArt;
    public Image art;
    public SpriteRenderer cardModel;
    public SpriteRenderer atkIcon;
    public SpriteRenderer costIcon;

    public float moveSpeed = 5f;
    public float rotateSpeed = 540f;

    private Vector3 targetPoint;
    private Quaternion rotationPoint;

    public bool inHand;
    public int handPosition;

    private HandController theHC;

    private bool isSelected;
    private Collider theCol;

    public LayerMask whatIsDesktop;
    public LayerMask whatIsEnemi;

    private bool justPressed = false;


    // Start is called before the first frame update
    void Start()
    {
        SetUpCard();

        theHC = FindObjectOfType<HandController>();
        theCol = GetComponent<Collider>();

        effect = GetComponent<ICardEffect>();
    }

    public void SetUpCard()
    {
        moveCost = cardSO.getCost();
        
        if(cardSO.type == CardScriptable.cardType.Attack)
        {
            int damage = cardSO.getDamage();
            attackText.text = damage.ToString();
        }
        else if( cardSO.type == CardScriptable.cardType.skill)
        {
            if(cardSO.getDamage() < 1)
            {
                atkIconGO.SetActive(false);
                attackText.text = "";
            }
            
        }

        if(cardSO.getCost() < 1)
        {
            costIconGO.SetActive(false);
            costText.text = "";
        }
        else
        {
            costText.text = cardSO.getCost().ToString();
        }

        nameText.text = cardSO.name;
        descriptionText.text = cardSO.getDescription();

        art.sprite = cardSO.Icon;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationPoint, rotateSpeed * Time.deltaTime);

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 100f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, whatIsEnemi) && cardSO.target == CardScriptable.cardTarget.ennemy)
                {
                    if(BattleController.instance.playerMoves >= moveCost)
                    {
                        inHand = false;
                        isSelected = false;
                        theHC.RemoveCardFromHand(this);
                        BattleController.instance.SpendPlayerMoves(moveCost);
                        List<GameObject> enemis = new List<GameObject>();
                        if (cardSO.isMultiTarget)
                        {
                            //make it first
                            enemis.Add(hit.collider.gameObject);
                            foreach (GameObject obj in BattleController.instance.enemis)
                            {
                                if (obj == hit.collider.gameObject) continue;
                                enemis.Add(obj);
                            }
                        }
                        else
                        {
                            enemis.Add(hit.collider.gameObject);
                        }
                        effect.ApplyEffect(enemis, cardSO);
                        Quaternion newRotation = Quaternion.Euler(0, 0, 180);
                        MoveToPoint(BattleController.instance.discardPile.position, newRotation);
                        BattleController.instance.CheckGameCondition();
                        Destroy(gameObject, 5f);
                    }
                    else
                    {
                        ReturnToHand();

                        UIController.instance.ShowMoveWarning();
                    }
                }
                else
                {
                    ReturnToHand();
                }
            }
        }

        justPressed = false;
    }

    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotationToRotateTo)
    {
        targetPoint = pointToMoveTo;
        rotationPoint = rotationToRotateTo;
    }


    private void OnMouseOver()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition] + new Vector3(0f, 1f, .5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.player)
        {
            isSelected = true;
            theCol.enabled = false;

            justPressed = true;

            atkIcon.sortingLayerName = "CardSelectedIcons";
            costIcon.sortingLayerName = "CardSelectedIcons";
            cardModel.sortingLayerName = "CardSelected";
            canvasOfArt.sortingLayerName = "CardSelectedInfo";

            attackTextGO.GetComponent<PushToFront>().SetLayer("CardSelectedInfo");
            costTextGO.GetComponent<PushToFront>().SetLayer("CardSelectedInfo");
            nameTextGO.GetComponent<PushToFront>().SetLayer("CardSelectedInfo");
            descriptionTextGO.GetComponent<PushToFront>().SetLayer("CardSelectedInfo");
        }
    }

    private void ReturnToHand()
    {
        isSelected = false;
        theCol.enabled = true;

        atkIcon.sortingLayerName = "Icons";
        costIcon.sortingLayerName = "Icons";
        cardModel.sortingLayerName = "CardModel";
        canvasOfArt.sortingLayerName = "CardInfo";

        attackTextGO.GetComponent<PushToFront>().SetLayer("CardInfo");
        costTextGO.GetComponent<PushToFront>().SetLayer("CardInfo");
        nameTextGO.GetComponent<PushToFront>().SetLayer("CardInfo");
        descriptionTextGO.GetComponent<PushToFront>().SetLayer("CardInfo");

        MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
    }
}