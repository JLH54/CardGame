using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : EnemyAction
{
    public override void ApplyAction(List<GameObject> targets)
    {
        targets[0].GetComponent<Enemy>().playAnimation("Attack2");
        AudioManager.Instance.PlaySound2D(SoundType.WildZombieAttack1);
        Player.instance.TakeDamage(targets[0].GetComponent<Enemy>().damage);
    }
}
