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
    public enum StatModifierType
    {
        Flat,
        Multiplier
    }

    [System.Serializable]
    public class StatModifier
    {
        [HorizontalGroup]
        [SerializeField]
        [HideLabel]
        [ValueDropdown("GetStatList")]
        private string statName;
        public string StatName
        {
            get { return statName; }
            set { statName = value; }
        }

        [HorizontalGroup(Width = 80)]
        [HideLabel]
        [SerializeField]
        float statValue;
        public float StatValue
        {
            get { return statValue; }
            set { statValue = value; }
        }

        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        StatModifierType modifierType;
        public StatModifierType ModifierType
        {
            get { return modifierType; }
        }


        public virtual void ApplyModifier()
        {

        }

        public virtual void RemoveModifier()
        {

        }

#if UNITY_EDITOR
        private static List<string> GetStatList()
        {

            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;
            /*for(int i = 0; i < dic.StatDictionary.Count; i++)
            {
                if (customStats2.Count <= i)
                    customStats2.Add(new CustomStat(dic.StatDictionary[i].StatName, dic.StatDictionary[i].StatValue));
                customStats2[i].StatName = dic.StatDictionary[i].StatName;
            }*/

        }
#endif

    }

} // #PROJECTNAME# namespace