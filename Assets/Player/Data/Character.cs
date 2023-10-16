using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Character : ScriptableObject
{
    public Sprite icon;
    public List<Card> startingDeck;
    public int MaxMoves;
    public int maxHealth;
}
