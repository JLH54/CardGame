using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardScriptable : ScriptableObject
{
    //Nom de la carte
    public string Cardname;
    //est-elle ameliorer
    public bool isUpgraded;

    public bool isMultiTarget;
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
