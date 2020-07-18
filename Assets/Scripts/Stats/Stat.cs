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
    public class Stat
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [HorizontalGroup]
        [SerializeField]
        [EnableIf("modifiable")]
        [HideLabel]
        private string statName;
        public string StatName
        {
            get { return statName; }
        }

        [HorizontalGroup(Width = 80)]
        [HideLabel]
        [SerializeField]
        float statValue;
        public float StatValue
        {
            get { return statValue; }
        }

        [HideInInspector]
        public bool modifiable = true;
        public bool Modifiable
        {
            get { return modifiable; }
            set { modifiable = value; }
        }

        float statBaseValue;
        /*public float StatBaseValue
        {
            get { return statBaseValue; }
        }*/
        float statBonusFlat;
        /*public float StatBonusFlat
        {
            get { return statBonusFlat; }
        }*/
        float statBonusMultiplier;
        /*public float StatBonusMultiplier
        {
            get { return statBonusMultiplier; }
        }*/

        #endregion

        public Stat(Stat copy)
        {
            statName = copy.StatName;
            statBaseValue = copy.StatValue;
            statBonusFlat = copy.statBonusFlat;
            statBonusMultiplier = copy.statBonusMultiplier;
            modifiable = copy.Modifiable;
            CalculateFinalValue();
        }

        public Stat(string name, float value, bool modif)
        {
            statName = name;
            statBaseValue = value;
            statBonusFlat = 0;
            statBonusMultiplier = 1;
            modifiable = modif;
            CalculateFinalValue();
        }
        public Stat(string name, float value)
        {
            statName = name;
            statBaseValue = value;
            statBonusFlat = 0;
            statBonusMultiplier = 1;
            modifiable = true;
            CalculateFinalValue();
        }
        public Stat(string name)
        {
            statName = name;
            statBaseValue = 0;
            statBonusFlat = 0;
            statBonusMultiplier = 1;
            modifiable = true;
            CalculateFinalValue();
        }
        public Stat()
        {
            statName = "";
            statBaseValue = 0;
            statBonusFlat = 0;
            statBonusMultiplier = 1;
            modifiable = true;
            CalculateFinalValue();
        }


        public void SetBaseValue(float value)
        {
            statBaseValue = value;
            CalculateFinalValue();
        }
        public void AddBaseValue(float value)
        {
            statBaseValue += value;
            CalculateFinalValue();
        }


        public void AddStatModifier(float value, StatModifierType modifier)
        {
            switch(modifier)
            {
                case StatModifierType.Flat:
                    statBonusFlat += value;
                    break;
                case StatModifierType.Multiplier:
                    statBonusMultiplier += value;
                    break;
            }
            CalculateFinalValue();
        }

        public void RemoveStatModifier(float value, StatModifierType modifier)
        {
            switch (modifier)
            {
                case StatModifierType.Flat:
                    statBonusFlat -= value;
                    break;
                case StatModifierType.Multiplier:
                    statBonusMultiplier -= value;
                    break;
            }
            CalculateFinalValue();
        }

        private void CalculateFinalValue()
        {
            statValue = (statBaseValue + statBonusFlat) * statBonusMultiplier;
        }


        public void DebugFunction()
        {
            statBonusMultiplier = 1;
        }

    } 

} // #PROJECTNAME# namespace