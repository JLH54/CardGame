using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : ICardEffect
{
    public override void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        Player.instance.animator.SetTrigger("Shot");

        targets[0].GetComponent<Enemy>().playAnimation("Hurt");
        targets[0].GetComponent<Enemy>().TakeDamage(card.getDamage());

        for(int i =0; i < targets.Count; i++)
        {
            if (targets[i].GetComponent<Enemy>().position == targets[0].GetComponent<Enemy>().position) continue;
            if (targets[i].GetComponent<Enemy>().position == targets[0].GetComponent<Enemy>().position + 1)
            {
                if (targets[i] != null)
                {
                    targets[i].GetComponent<Enemy>().playAnimation("Hurt");
                    targets[i].GetComponent<Enemy>().TakeDamage(card.getDamage());
                }
            }
        }

    }
}
