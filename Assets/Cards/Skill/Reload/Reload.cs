using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : ICardEffect
{
    public override void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        AudioManager.Instance.PlaySound2D(SoundType.PlayerReload);
        Player.instance.animator.SetTrigger("Reload");
        BattleController.instance.AddPlayerMove(1);
    }
}
