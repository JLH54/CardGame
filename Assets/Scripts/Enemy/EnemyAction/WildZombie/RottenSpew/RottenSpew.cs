using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenSpew : EnemyAction
{
    public override void ApplyAction(List<GameObject> targets)
    {
        Player.instance.TakeDamage(targets[0].GetComponent<Enemy>().damage);
    }
}
