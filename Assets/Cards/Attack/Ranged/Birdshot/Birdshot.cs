using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdshot : ICardEffect
{
    public override void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        AudioManager.Instance.PlaySound2D(SoundType.PlayerShot);
        Player.instance.animator.SetTrigger("Shot");
        for (int i =0;i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                targets[i].GetComponent<Enemy>().TakeDamage(card.getDamage());
            }
        }
    }
}
