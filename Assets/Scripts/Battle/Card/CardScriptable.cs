using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SO pour pouvoir creer une carte
[CreateAssetMenu]
public class CardScriptable : ScriptableObject
{
    //Nom de la carte
    public string Cardname;
    //est-elle ameliorer
    public bool isUpgraded;
    //Le damage de la carte(meme si elle ne fait pas de damage faut quand meme l'avoir)
    public CardDamage damage;
    //Si elle peut cibler plusieurs
    public bool isMultiTarget;
    //Si elle est physique(par physique je veut dire qu'elle doit cibler la premier cible le plus proche)
    public bool isMelee;
    //Description de ce qu'elle fait
    public CardDescription description;
    //Son cout pour les actions
    public CardCost cost;
    //Pour la visualiser
    public Sprite Icon;
    //Son type
    public cardType type;
    public enum cardType { Attack, skill }
    //Elle vise qui?
    public cardTarget target;
    public enum cardTarget { self, ennemy }

    public int getCost()
    {
        return isUpgraded ? cost.upgradedCard : cost.baseCard;
    }

    public string getDescription()
    {
        return isUpgraded ? description.upgradedCard : description.baseCard;
    }

    public int getDamage()
    {
        return isUpgraded ? damage.upgradedCard : damage.baseCard;
    }
}

[System.Serializable]
public struct CardDescription
{
    public string baseCard;
    public string upgradedCard;
}

[System.Serializable]
public struct CardCost
{
    public int baseCard;
    public int upgradedCard;
}

[System.Serializable]
public struct CardDamage
{
    public int baseCard;
    public int upgradedCard;
}
