/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public delegate void Action();
    public delegate void HitAction(DamageMessage damageMessage);

    public interface IDamageable
    {
        void Damage(Vector3 pos, AttackData attackData);

        event HitAction OnHit;
        event Action OnDead;
    } 

} // #PROJECTNAME# namespace