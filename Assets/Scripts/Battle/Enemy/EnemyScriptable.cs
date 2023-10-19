using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pour avoir l'information des enemis
[CreateAssetMenu]
public class EnemyScriptable : ScriptableObject
{
    public int maxHealth;

    public int minHealth;

    public int maxGold;

    public int minGold;

    public Attack[] attacks;

    [System.Serializable]
    public struct Attack {
        public string attack;
        public bool isAttack;
        public int weight;
        public int minAttack, maxAttack;
    }
}
