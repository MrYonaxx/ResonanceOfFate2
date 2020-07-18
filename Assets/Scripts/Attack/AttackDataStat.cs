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
    [System.Serializable]
    public class AttackDataStat
    {

        [ValueDropdown("SelectAttackType")]
        [HorizontalGroup]
        [SerializeField]
        [HideLabel]
        private int attackType;
        public int AttackType
        {
            get { return attackType; }
            set { attackType = value; }
        }

        private static IEnumerable SelectAttackType()
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<TypeDictionary>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("WeaponType")[0]))
                .GetAllTypeIndex();

#else
            return null;
#endif
        }

        [HorizontalGroup]
        [SerializeField]
        private bool scratchDamage;
        public bool ScratchDamage
        {
            get { return scratchDamage; }
            set { scratchDamage = value; }
        }

        [Title("Value")]
        [HorizontalGroup("AttackData")]
        [VerticalGroup("AttackData/Left")]
        [SerializeField]
        private float damage = 0;
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        [Title("Variance (%)")]
        [VerticalGroup("AttackData/Right")]
        [SerializeField]
        private float damageVariance = 0;
        public float DamageVariance
        {
            get { return damageVariance; }
            set { damageVariance = value; }
        }


        [VerticalGroup("AttackData/Left")]
        [SerializeField]
        private float knockbackDamage = 0;
        public float KnockbackDamage
        {
            get { return knockbackDamage; }
            set { knockbackDamage = value; }
        }
        [VerticalGroup("AttackData/Right")]
        [SerializeField]
        private float knockbackDamageVariance = 0;
        public float KnockbackDamageVariance
        {
            get { return knockbackDamageVariance; }
            set { knockbackDamageVariance = value; }
        }

        [VerticalGroup("AttackData/Left")]
        [SerializeField]
        private float launchDamage = 0;
        public float LaunchDamage
        {
            get { return launchDamage; }
            set { launchDamage = value; }
        }
        [VerticalGroup("AttackData/Right")]
        [SerializeField]
        private float launchDamageVariance = 0;
        public float LaunchDamageVariance
        {
            get { return launchDamageVariance; }
            set { launchDamageVariance = value; }
        }

        [VerticalGroup("AttackData/Left")]
        [SerializeField]
        private float stunDamage = 0;
        public float StunDamage
        {
            get { return stunDamage; }
            set { stunDamage = value; }
        }
        [VerticalGroup("AttackData/Right")]
        [SerializeField]
        private float stunDamageVariance = 0;
        public float StunDamageVariance
        {
            get { return stunDamageVariance; }
            set { stunDamageVariance = value; }
        }

        [VerticalGroup("AttackData/Left")]
        [SerializeField]
        private float accuracy = 0;
        public float Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }
        [VerticalGroup("AttackData/Right")]
        [SerializeField]
        private float accuracyVariance = 0;
        public float AccuracyVariance
        {
            get { return accuracyVariance; }
            set { accuracyVariance = value; }
        }

        [Title("Element")]
        [HorizontalGroup("AttackElement")]
        [SerializeField]
        public List<ElementalResistance> AttackElement = new List<ElementalResistance>();

        [Title("Status Chance")]
        [HorizontalGroup("AttackElement")]
        [SerializeField]
        public List<StatusResistance> AttackStatus = new List<StatusResistance>();


        private Vector3 userPosition;
        public Vector3 UserPosition
        {
            get { return userPosition; }
            set { userPosition = value; }
        }


        public AttackDataStat(WeaponData weaponData)
        {
            attackType = weaponData.AttackType;
            scratchDamage = weaponData.ScratchDamage;
        }

        public void Add(AttackDataStat attackDataStat)
        {
            damage += attackDataStat.Damage;
            damageVariance += attackDataStat.DamageVariance;

            knockbackDamage += attackDataStat.KnockbackDamage;
            knockbackDamageVariance += attackDataStat.KnockbackDamageVariance;

            launchDamage += attackDataStat.LaunchDamage;
            launchDamageVariance += attackDataStat.LaunchDamageVariance;

            stunDamage += attackDataStat.StunDamage;
            stunDamageVariance += attackDataStat.StunDamageVariance;

            accuracy += attackDataStat.Accuracy;
            accuracyVariance += attackDataStat.AccuracyVariance;
        }

    } 

} // #PROJECTNAME# namespace