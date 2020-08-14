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
    [CreateAssetMenu(fileName = "PercentageAttackProcessor", menuName = "Attack/PercentageAttackProcessor", order = 1)]
    public class AttackProcessorPercentage: AAttackProcessor
    {
        public override AttackDataStat CreateAttack(CharacterStatController user, WeaponData weaponData)
        {
            AttackDataStat res = new AttackDataStat(weaponData);
            return res;
        }


        public override DamageMessage ProcessAttack(AttackDataStat attack, CharacterStatController target)
        {
            DamageMessage res;
            float finalDamage = target.GetHPMax() / 4f;
            res = new DamageMessage(attack.AttackType, 0, (int)finalDamage);
            target.Scratch += finalDamage;

            if (ProcessKnockback(attack, target) == true)
                res.knockback = true;
            if (ProcessLaunch(attack, target) == true)
                res.launch = true;
            return res;
        }


        private bool ProcessKnockback(AttackDataStat attack, CharacterStatController target)
        {
            return true;
        }

        private bool ProcessLaunch(AttackDataStat attack, CharacterStatController target)
        {
            return true;
        }

    } 

} // #PROJECTNAME# namespace