using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
