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


    [CreateAssetMenu(fileName = "BaseAttackProcessor", menuName = "Attack/BaseAttackProcessor", order = 1)]
    public class AttackProcessor : AAttackProcessor
    {
        [Title("Attack")]
        [SerializeField] 
        [ValueDropdown("GetStatList")]
        private string statDamage;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statDamageVariance;

        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statKnockbackDamage;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statKnockbackDamageVariance;

        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statLaunchDamage;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statLaunchDamageVariance;

        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statAccuracy;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statAccuracyVariance;



        [Title("Defense")]
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statDefense;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statKnockbackResistance;
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statLaunchResistance;



        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public override AttackDataStat CreateAttack(CharacterStatController user, WeaponData weaponData)
        {
            AttackDataStat res = new AttackDataStat(weaponData);
            res.Damage = user.GetStat(statDamage);
            res.DamageVariance = user.GetStat(statDamageVariance);

            res.KnockbackDamage = user.GetStat(statKnockbackDamage);
            res.KnockbackDamageVariance = user.GetStat(statKnockbackDamageVariance);

            res.LaunchDamage = user.GetStat(statLaunchDamage);
            res.LaunchDamageVariance = user.GetStat(statLaunchDamageVariance);

            res.Accuracy = user.GetStat(statAccuracy);
            res.AccuracyVariance = user.GetStat(statAccuracyVariance);
            return res;
        }


        public override DamageMessage ProcessAttack(AttackDataStat attack, CharacterStatController target)
        {
            DamageMessage res;
            int finalDamage = 0;
            float variance = attack.Damage * (attack.DamageVariance * 0.01f);
            int rawDamage = (int) (attack.Damage + Random.Range(-variance, variance+1));
            rawDamage = (int)(rawDamage * target.GetStat(statDefense));

            if (attack.ScratchDamage == true)
            {
                finalDamage = rawDamage;
                res = new DamageMessage(attack.AttackType, 0, finalDamage);
                // Exp
                if (target.Scratch + finalDamage > target.Hp)
                    res.exp = (int)(target.Hp - target.Scratch);
                else
                    res.exp = finalDamage;
                // Exp
                target.Scratch += finalDamage;
            }
            else
            {
                finalDamage = rawDamage + (int)target.Scratch;
                res = new DamageMessage(attack.AttackType, rawDamage, (int)target.Scratch);
                // Exp
                if (target.Hp - finalDamage < 0)
                    res.exp = (int)target.Hp;
                else
                    res.exp = finalDamage;
                // Exp
                target.Hp -= finalDamage;
                target.Scratch = 0;
            }
            if (ProcessKnockback(attack, target) == true)
                res.knockback = true;
            if (ProcessLaunch(attack, target) == true)
                res.launch = true;
            return res;
        }


        private bool ProcessKnockback(AttackDataStat attack, CharacterStatController target)
        {
            float variance = attack.KnockbackDamage * (attack.KnockbackDamageVariance * 0.01f);
            int rawDamage = (int)(attack.KnockbackDamage + Random.Range(-variance, variance));
            rawDamage -= (int) target.GetStat(statKnockbackResistance);
            return (rawDamage > 0);
        }

        private bool ProcessLaunch(AttackDataStat attack, CharacterStatController target)
        {
            float variance = attack.LaunchDamage * (attack.LaunchDamageVariance * 0.01f);
            int rawDamage = (int)(attack.LaunchDamage + Random.Range(-variance, variance));
            rawDamage -= (int)target.GetStat(statLaunchResistance);
            return (rawDamage > 0);
        }


        private void ApplyStatus()
        {

        }

        #endregion

    } 

} // #PROJECTNAME# namespace