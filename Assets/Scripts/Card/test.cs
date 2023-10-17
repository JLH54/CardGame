using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : ICardEffect
{
    public override void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        Player.instance.animator.SetTrigger("Attack1");
        targets[0].GetComponent<Enemy>().playAnimation("Hurt");
        targets[0].GetComponent<Enemy>().TakeDamage(card.getDamage());
    }
}
