using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//P.S: CE N'EST PAS UNE INTERFACE, je suis juste trop paresseux pour le changer partout
//C'est la base pour l'effet des cartes
public class ICardEffect : MonoBehaviour
{
    public ICardEffect theEffect;

    public virtual void ApplyEffect(List<GameObject> targets, CardScriptable card)
    {
        
    }
}
