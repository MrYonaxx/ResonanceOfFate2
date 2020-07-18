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
    [CreateAssetMenu(fileName = "CharacterGrowthData", menuName = "Character/StatGrowthData", order = 1)]
    public class CharacterGrowthData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [HideLabel]
        [SerializeField]
        private StatController growthStat;

        public StatController GrowthStat
        {
            get { return growthStat; }
        }


        [Space]
        [Space]
        [Space]
        [Title("Debug")]
        [SerializeField]
        [ValueDropdown("GetStatList")]
        private string statDebug;

        [HorizontalGroup("B")]
        [SerializeField]
        [ReadOnly]
        int[] level; // Pour l'editeur
        [HorizontalGroup("B")]
        [SerializeField]
        float[] value;

        // Debug
        [OnValueChanged("CalculateCurve")]
        [SerializeField]
        int baseValue = 100;

        [Button]
        private void CalculateCurve()
        {
            level = new int[50];
            value = new float[50];
            for (int i = 0; i < 50; i++)
            {
                level[i] = i;
                value[i] = baseValue + growthStat.GetValue(statDebug) * (i-1);
            }
        }

#if UNITY_EDITOR
        private static List<string> GetStatList()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;
        }
#endif

        #endregion


    }

} // #PROJECTNAME# namespace