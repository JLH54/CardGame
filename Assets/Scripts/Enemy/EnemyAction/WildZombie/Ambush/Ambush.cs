using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : EnemyAction
{
    public override void ApplyAction(List<GameObject> targets)
    {
        Player.instance.TakeDamage(targets[0].GetComponent<Enemy>().damage);
    }
}
