using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//La carte qui est dans le jeu
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
    public LayerMask whatIsSelf;

    private bool justPressed = false;
    public float timeToWaitForCardToGoDiscard = 0.5f;

    void Start()
    {
        SetUpCard();

        theHC = FindObjectOfType<HandController>();
        theCol = GetComponent<Collider>();

        effect = GetComponent<ICardEffect>();
    }

    //Met l'information qui vien du SO sur la carte
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
        //Va aller a l'information donner quand on la fait spawner pour qu'elle aye dans la main tout en fesant une rotation pour pas qu'ils ont l'air boguer
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationPoint, rotateSpeed * Time.deltaTime);

        //Si il est selectionner, on fait en sorte qu'il suis la souris
        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Le point qu'il va suivre
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 100f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
            //Quand le joueur clique
            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                //Regard si c'est un enemi
                if (Physics.Raycast(ray, out hit, 100f, whatIsEnemi) && cardSO.target == CardScriptable.cardTarget.ennemy)
                {
                    //Si la carte est un enemi et que ce n'est pas l'enemi le plus proche, revoie la carte dans la main
                    if(hit.collider.gameObject != GetClosestEnemy() && cardSO.isMelee)
                    {
                        ReturnToHand();
                    }
                    else if(BattleController.instance.playerMoves >= moveCost)
                    {
                        //Si on peut jouer la carte on la joue
                        inHand = false;
                        isSelected = false;
                        theHC.RemoveCardFromHand(this);
                        BattleController.instance.SpendPlayerMoves(moveCost);
                        List<GameObject> enemis = new List<GameObject>();
                        //Regarde si la carte peut attaquer plusieurs cible
                        if (cardSO.isMultiTarget)
                        {
                            //Fait en sorte que l'enemi est premier dans la liste
                            enemis.Add(hit.collider.gameObject);
                            foreach (GameObject obj in BattleController.instance.enemiesGO)
                            {
                                if (obj == hit.collider.gameObject) continue;
                                enemis.Add(obj);
                            }
                        }
                        else
                        {
                            //Si c'est juste une cible, donne la cible
                            enemis.Add(hit.collider.gameObject);
                        }
                        //Fait en sorte qu'on peut voir les animation
                        StartCoroutine(PlayCardCo(enemis, cardSO));
                        Quaternion newRotation = Quaternion.Euler(0, 0, 180);
                        MoveToPoint(BattleController.instance.discardPile.position, newRotation);
                        Destroy(gameObject, 5f);
                    }
                    else
                    {
                        //Si la carte coute trop chere, envoi un message disant au joueur qu'elle coute trop chere
                        ReturnToHand();

                        UIController.instance.ShowMoveWarning();
                    }
                }
                else if (Physics.Raycast(ray, out hit, 100f, whatIsSelf) && cardSO.target == CardScriptable.cardTarget.self)
                {
                    //Si c'est un skill qui cible le joueur
                    if (BattleController.instance.playerMoves >= moveCost)
                    {
                        inHand = false;
                        isSelected = false;
                        theHC.RemoveCardFromHand(this);
                        BattleController.instance.SpendPlayerMoves(moveCost);
                        List<GameObject> placeholder = new List<GameObject>();
                        StartCoroutine(PlayCardCo(placeholder, cardSO));
                        Quaternion newRotation = Quaternion.Euler(0, 0, 180);
                        MoveToPoint(BattleController.instance.discardPile.position, newRotation);
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

    //Bouge la carte a ces points(voir la fonction Update())
    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotationToRotateTo)
    {
        targetPoint = pointToMoveTo;
        rotationPoint = rotationToRotateTo;
    }

    //Quand le joueur regarde la carte
    private void OnMouseOver()
    {
        //Le met plus vers l'avant que le joueur met le curseur par-dessus
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition] + new Vector3(0f, 1f, .5f), Quaternion.identity);
        }
    }

    //Quand le joueur ne regarde pus la carte
    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(theHC.cardPositions[handPosition], theHC.minPos.rotation);
        }
    }

    //Quand le joueur a cliquer sur la carte
    private void OnMouseDown()
    {
        //Quand le joeur le selectionne
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

    //Retourne la carte dans la main
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

    //Coroutine pour faire sure de voir les animations
    private IEnumerator PlayCardCo(List<GameObject> enemis,CardScriptable cardSO)
    {
        yield return new WaitForSeconds(timeToWaitForCardToGoDiscard);

        effect.theEffect.ApplyEffect(enemis, cardSO);
    }

    //Retourne l'enenmi le plus proche
    private GameObject GetClosestEnemy()
    {
        for(int i =0; i < BattleController.instance.enemiesGO.Length;i++)
        {
            if(BattleController.instance.enemiesGO[i])
            {
                if (!BattleController.instance.enemiesGO[i].GetComponent<Enemy>().isDead) return BattleController.instance.enemiesGO[i];
            }
        }
        return null;
    }
}