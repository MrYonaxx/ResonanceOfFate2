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
    [CreateAssetMenu(fileName = "WeaponLevelData", menuName = "Character/WeaponLevelData", order = 1)]
    public class CharacterWeaponLevelData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [ValueDropdown("SelectAttackType")]
        [SerializeField]
        private int weaponType;
        public int WeaponType
        {
            get { return weaponType; }
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

        [HorizontalGroup("A")]
        [MinValue(1)]
        [MaxValue("maxLevel")]
        [SerializeField]
        int baseLevel = 1;
        public int BaseLevel
        {
            get { return baseLevel; }
        }
        [HorizontalGroup("A")]
        [SerializeField]
        int maxLevel = 50;



        [Space]
        [Title("Skills")]
        [HorizontalGroup]
        [SerializeField]
        int[] level;

        [Space]
        [Title(" ")]
        [HorizontalGroup]
        [SerializeField]
        AttackAimProperty[] skills;


        [Space]
        [Space]
        [Space]
        [Title("Experience Curve")]
        [HorizontalGroup("B")]
        [SerializeField]
        [ReadOnly]
        int[] experienceLevel; // Pour l'editeur
        [Space]
        [Space]
        [Space]
        [Title(" ")]
        [HorizontalGroup("B")]
        [SerializeField]
        int[] experienceCurve;
        public int[] ExperienceCurve
        {
            get { return experienceCurve; }
        }

        // Debug
        [OnValueChanged("CalculateCurve")]
        [SerializeField]
        int baseValue = 100;
        [OnValueChanged("CalculateCurve")]
        [SerializeField]
        float multiplier = 1.1f;

        private void CalculateCurve()
        {
            experienceLevel = new int[maxLevel];
            experienceCurve = new int[maxLevel];
            int previousValue = baseValue;
            for (int i = 0; i < maxLevel; i++)
            {
                experienceLevel[i] = i + 1;
                experienceCurve[i] = (int) (previousValue * multiplier);
                previousValue = experienceCurve[i];
            }
        }

        #endregion

        public List<AttackAimProperty> GetAttackProperties(int lv)
        {
            List<AttackAimProperty> res = new List<AttackAimProperty>();
            for (int i = 0; i < level.Length; i++)
            {
                if (level[i] <= lv)
                    res.Add(skills[i]);
            }
            return res;
        }

    }

} // #PROJECTNAME# namespace