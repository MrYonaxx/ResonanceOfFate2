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
    [CreateAssetMenu(fileName = "AttackProcessorWeaponRaw", menuName = "Attack/AttackProcessorWeaponRaw", order = 1)]
    public class AttackProcessorRaw: AAttackProcessor
    {
        [Title("Weapon Data ONLY")]
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statDamage;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statDamageVariance;

        [SerializeField]
        private Vector3 positionOffset;

        public override AttackDataStat CreateAttack(CharacterStatController user, WeaponData weaponData)
        {
            AttackDataStat res = new AttackDataStat(weaponData);

            res.Damage = weaponData.BaseStat.GetValue(statDamage);
            res.DamageVariance = weaponData.BaseStat.GetValue(statDamageVariance);
            res.UserPosition += positionOffset;
            return res;
        }


        public override DamageMessage ProcessAttack(AttackDataStat attack, CharacterStatController target)
        {
            DamageMessage res;
            float variance = attack.Damage * (attack.DamageVariance * 0.01f);
            int damage = (int)(attack.Damage + Random.Range(-variance, variance + 1));

            if (attack.ScratchDamage == true)
            {
                target.Scratch -= damage;
                res = new DamageMessage(attack.AttackType, 0, damage);
            }
            else
            {
                target.Hp -= damage;
                res = new DamageMessage(attack.AttackType, damage, 0);
            }
            res.knockback = ProcessKnockback(attack, target);
            res.launch = ProcessLaunch(attack, target);
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