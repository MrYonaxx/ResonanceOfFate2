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
    public class ElementalResistance 
    {
        [SerializeField]
        [HorizontalGroup("ElementalResistance")]
        [HideLabel]
        [ValueDropdown("SelectCardElement")]
        private string element;
        public string Element
        {
            get { return element; }
        }

        [SerializeField]
        [HideLabel]
        [HorizontalGroup("ElementalResistance")]
        private float resistance;
        public float Resistance
        {
            get { return resistance; }
        }


        private static IEnumerable SelectCardElement()
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<TypeDictionary>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("ElementalType")[0]))
                .GetAllTypeName();
#else
            return null;
#endif
        }

    }

    [System.Serializable]
    public class StatusResistance
    {
        /*[SerializeField]
        [HorizontalGroup("StatusResistance")]
        [HideLabel]
        public StatusEffectData statusData;
        public StatusEffectData StatusData
        {
            get { return statusData; }
        }*/

        [HideLabel]
        [SerializeField]
        [HorizontalGroup("StatusResistance")]
        private float resistance;
        public float Resistance
        {
            get { return resistance; }
        }
    }







    [System.Serializable]
    public class CharacterStat
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [HorizontalGroup("HP", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [HideLabel]
        [Title("HP Max")]
        [SerializeField]
        public float HpMax = 1;


        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Statistiques")]
        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float Attack = 1;

        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float Defense = 1;

        [VerticalGroup("Stat/Left")]
        [SerializeField]
        public float MotionSpeed = 1;


        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Weapon")]
        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float AimSpeed;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float AimAcceleration;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float MinAimDistance;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float MaxAimDistance;

        [VerticalGroup("Stat/Center")]
        [SerializeField]
        public float MaxMagazine;


        [HorizontalGroup("Stat", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Character")]
        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float Speed = 0;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float Jump = 0;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float Gravity = 0;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float GravityMax = 0;

        [VerticalGroup("Stat/Right")]
        [SerializeField]
        public float Mass = 0;




        [HorizontalGroup("Stat2", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Attack")]
        [VerticalGroup("Stat2/Left")]
        [SerializeField]
        public float Damage = 0;

        [VerticalGroup("Stat2/Left")]
        [SerializeField]
        public float KnockbackDamage = 0;

        [VerticalGroup("Stat2/Left")]
        [SerializeField]
        public float LaunchDamage = 0;

        [VerticalGroup("Stat2/Left")]
        [SerializeField]
        public float StunDamage = 0;

        [VerticalGroup("Stat2/Left")]
        [SerializeField]
        public float Accuracy = 0;




        [HorizontalGroup("Stat2", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Variance (%)")]
        [VerticalGroup("Stat2/Left2")]
        [SerializeField]
        public float DamageVariance = 0;

        [VerticalGroup("Stat2/Left2")]
        [SerializeField]
        public float KnockbackDamageVariance = 0;

        [VerticalGroup("Stat2/Left2")]
        [SerializeField]
        public float LaunchDamageVariance = 0;

        [VerticalGroup("Stat2/Left2")]
        [SerializeField]
        public float StunDamageVariance = 0;

        [VerticalGroup("Stat2/Left2")]
        [SerializeField]
        public float AccuracyVariance = 0;




        [HorizontalGroup("Stat2", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Defense")]
        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float KnockbackTime = 0;

        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float KnockbackResistance = 0;

        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float StunTime = 0;

        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float StunResistance = 0;

        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float LaunchImpulse = 0;

        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float LaunchResistance = 0;

        [VerticalGroup("Stat2/Center")]
        [SerializeField]
        public float RecoveryTime = 0;


        /*[HorizontalGroup("Stat2", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        [Title("Other")]
        [VerticalGroup("Stat2/Right")]
        [SerializeField]
        public float AilmentRate = 0;
        [VerticalGroup("Stat2/Right")]
        [SerializeField]
        public float PropertyActivationRate = 0;*/

        //[HorizontalGroup("Stat2", LabelWidth = 150, PaddingLeft = 10, PaddingRight = 10)]
        //[Title("Custom Stats")]
        /*[VerticalGroup("Stat2/Right2")]
        [SerializeField]
        [HideLabel]
        AttackDataStat attackDataStat;*/


        /*[SerializeField]
        [HideLabel]
        List<CustomStat> customStats = new List<CustomStat>();*/



        //[SerializeField]
        //[HideLabel]
        //public List<CharacterStatLine> Stat;


        [Title("Attack")]
        [HorizontalGroup("CharacterAttack")]
        [SerializeField]
        public List<ElementalResistance> ElementalAttack = new List<ElementalResistance>();

        [Title("Resistance")]
        [HorizontalGroup("CharacterAttack")]
        [SerializeField]
        public List<ElementalResistance> ElementalResistances = new List<ElementalResistance>();


        [HorizontalGroup("CharacterResistance")]
        [SerializeField]
        public List<StatusResistance> StatusAttack = new List<StatusResistance>();

        [HorizontalGroup("CharacterResistance")]
        [SerializeField]
        public List<StatusResistance> StatusResistances = new List<StatusResistance>();





        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public CharacterStat()
        {

        }

        // A utiliser si CharacterStat est utilisé comme pourcentage
        public CharacterStat(int i)
        {
            HpMax = i;
            Attack = i;
            Defense = i;
            MotionSpeed = i;

            AimSpeed = i;
            AimAcceleration = i;
            MinAimDistance = i;
            MaxAimDistance = i;
            MaxMagazine = i;

            Speed = i;
            Jump = i;
            Gravity = i;
            GravityMax = i;
            Mass = i;

            Damage = i;
            KnockbackDamage = i;
            LaunchDamage = i;
            StunDamage = i;
            Accuracy = i;

            DamageVariance = i;
            KnockbackDamageVariance = i;
            LaunchDamageVariance = i;
            StunDamageVariance = i;
            AccuracyVariance = i;


            KnockbackTime = i;
        }


        public CharacterStat(CharacterStat characterStat)
        {
            HpMax = characterStat.HpMax;

            MotionSpeed = characterStat.MotionSpeed;
            Attack = characterStat.Attack;
            Defense = characterStat.Defense;

            AimSpeed = characterStat.AimSpeed;
            AimAcceleration = characterStat.AimAcceleration;
            MinAimDistance = characterStat.MinAimDistance;
            MaxAimDistance = characterStat.MaxAimDistance;
            MaxMagazine = characterStat.MaxMagazine;

            KnockbackTime = characterStat.KnockbackTime;

            Speed = characterStat.Speed;
            Jump = characterStat.Jump;
            Gravity = characterStat.Gravity;
            GravityMax = characterStat.GravityMax;
            Mass = characterStat.Mass;

            Damage = characterStat.Damage;
            KnockbackDamage = characterStat.KnockbackDamage;
            LaunchDamage = characterStat.LaunchDamage;
            StunDamage = characterStat.StunDamage;
            Accuracy = characterStat.Accuracy;

            DamageVariance = characterStat.DamageVariance;
            KnockbackDamageVariance = characterStat.KnockbackDamageVariance;
            LaunchDamageVariance = characterStat.LaunchDamageVariance;
            StunDamageVariance = characterStat.StunDamageVariance;
            AccuracyVariance = characterStat.AccuracyVariance;
        }



        public void Add(CharacterStat characterStat, float multiplier = 1)
        {
            HpMax += characterStat.HpMax * multiplier;

            MotionSpeed += characterStat.MotionSpeed * multiplier;
            Attack += characterStat.Attack * multiplier;
            Defense += characterStat.Defense * multiplier;


            AimSpeed += characterStat.AimSpeed * multiplier;
            AimAcceleration += characterStat.AimAcceleration * multiplier;
            MinAimDistance += characterStat.MinAimDistance * multiplier;
            MaxAimDistance += characterStat.MaxAimDistance * multiplier;
            MaxMagazine += characterStat.MaxMagazine * multiplier;


            Speed += characterStat.Speed * multiplier;
            Jump += characterStat.Jump * multiplier;
            Gravity += characterStat.Gravity * multiplier;
            GravityMax += characterStat.GravityMax * multiplier;
            Mass += characterStat.Mass * multiplier;

            Damage += characterStat.Damage * multiplier;
            KnockbackDamage += characterStat.KnockbackDamage * multiplier;
            LaunchDamage += characterStat.LaunchDamage * multiplier;
            StunDamage += characterStat.StunDamage * multiplier;
            Accuracy += characterStat.Accuracy * multiplier;

            DamageVariance += characterStat.DamageVariance * multiplier;
            KnockbackDamageVariance += characterStat.KnockbackDamageVariance * multiplier;
            LaunchDamageVariance += characterStat.LaunchDamageVariance * multiplier;
            StunDamageVariance += characterStat.StunDamageVariance * multiplier;
            AccuracyVariance += characterStat.AccuracyVariance * multiplier;

            KnockbackTime += characterStat.KnockbackTime * multiplier;
        }

        public void Remove(CharacterStat characterStat)
        {
            Add(characterStat, -1);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace