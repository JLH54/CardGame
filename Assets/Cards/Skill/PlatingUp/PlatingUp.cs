using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatingUp : ICardEffect
{
    public int NonupgradedArmor = 4;
    public int upgradedArmor = 6;

    public override void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        AudioManager.Instance.PlaySound2D(SoundType.ArmorUp);
        Player.instance.animator.SetTrigger("PlateUp");
        if (card.isUpgraded)
        {
            Player.instance.GiveArmor(upgradedArmor);
        }
        else
        {
            Player.instance.GiveArmor(NonupgradedArmor);
        }
    }
}
