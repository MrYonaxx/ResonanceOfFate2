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
    [CreateAssetMenu(fileName = "AttackProcessorHeal", menuName = "Attack/AttackProcessor_Heal", order = 1)]
    public class AttackProcessorHeal: AAttackProcessor
    {

        [Title("Attack")]
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statHeal;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statHealVariance;

        public override AttackDataStat CreateAttack(CharacterStatController user, WeaponData weaponData)
        {
            AttackDataStat res = new AttackDataStat(weaponData);

            res.Damage = user.GetStat(statHeal);
            res.DamageVariance = user.GetStat(statHealVariance);

            return res;
        }


        public override DamageMessage ProcessAttack(AttackDataStat attack, CharacterStatController target)
        {
            DamageMessage res;
            float variance = attack.Damage * (attack.DamageVariance * 0.01f);
            int rawHeal = (int)(attack.Damage + Random.Range(-variance, variance + 1));

            if (attack.ScratchDamage == true)
            {
                target.Scratch -= rawHeal;
                res = new DamageMessage(attack.AttackType, 0, rawHeal);
            }
            else
            {
                target.Hp += rawHeal;
                res = new DamageMessage(attack.AttackType, rawHeal, 0);
            }
            Debug.Log(target.Scratch);
            return res;
        }

    } 

} // #PROJECTNAME# namespace