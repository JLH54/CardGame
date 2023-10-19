using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenziedClaw : EnemyAction
{
    public override void ApplyAction(List<GameObject> targets)
    {
        targets[0].GetComponent<Enemy>().playAnimation("Attack1");
        AudioManager.Instance.PlaySound2D(SoundType.WildZombieAttack1);
        Player.instance.TakeDamage(targets[0].GetComponent<Enemy>().damage);
    }
}
