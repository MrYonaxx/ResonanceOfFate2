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
    [CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData", order = 1)]
    public class WeaponData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        private string weaponName;
        public string WeaponName
        {
            get { return weaponName; }
        }

        [Title("Weapon Attack Data")]
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

        [Title("")]
        [HorizontalGroup]
        [SerializeField]
        private bool scratchDamage;
        public bool ScratchDamage
        {
            get { return scratchDamage; }
            set { scratchDamage = value; }
        }


        [HideLabel]
        [SerializeField]
        private AAttackProcessor attackProcessor;
        public AAttackProcessor AttackProcessor
        {
            get { return attackProcessor; }
        }

        [Space]
        [Title("Weapon Stat Data")]
        [HideLabel]
        [SerializeField]
        private StatController baseStat;
        public StatController BaseStat
        {
            get { return baseStat; }
        }
        //[SerializeField]
        //List<StatModifier> statModifiers = new List<StatModifier>();


        #endregion


        public int GetWeaponType()
        {
            return attackType;
        }

    } 

} // #PROJECTNAME# namespace