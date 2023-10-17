using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Character : ScriptableObject
{
    public Sprite icon;
    public List<cardWithEffect> startingDeck;
    public int MaxMoves;
    public int maxHealth;

    [System.Serializable]
    public struct cardWithEffect
    {
        public CardScriptable card;
        public ICardEffect effect;
    }
}
