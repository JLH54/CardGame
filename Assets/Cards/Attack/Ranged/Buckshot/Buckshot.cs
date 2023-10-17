using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buckshot : ICardEffect
{
    public override void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        Player.instance.animator.SetTrigger("Shot");
        targets[0].GetComponent<Enemy>().playAnimation("Hurt");
        targets[0].GetComponent<Enemy>().TakeDamage(card.getDamage());
    }
}
