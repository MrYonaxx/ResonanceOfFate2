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
    [CreateAssetMenu(fileName = "AttackProperty", menuName = "AttackProperty/BaseAttackProperty", order = 1)]
    public class AttackAimProperty: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title(" Data")]
        //[InfoBox("# = value, ? = chance")]
        [SerializeField]
        string propertyText;

        [SerializeField]
        [MinValue(0)]
        [MaxValue(100)]
        int activationChance = 100;

        [SerializeField]
        bool percentage = true;

        [SerializeField]
        List<StatModifier> statModifiers = new List<StatModifier>();

        [Space]
        [Space]
        [Title(" Value")]
        [HorizontalGroup]
        [SerializeField]
        int[] charge;

        [Space]
        [Space]
        [Title("")]
        [HorizontalGroup]
        [SerializeField]
        float[] multiplier;

        [Space]
        [Space]
        [Title(" Generation")]
        [HorizontalGroup("MaxCharge")]
        [SerializeField]
        bool infinite = true;
        [Space]
        [Space]
        [Title(" ")]
        [HorizontalGroup("MaxCharge")]
        [HideIf("infinite")]
        [SerializeField]
        int maxLevel = -1;

        [SerializeField]
        float startMultiplier = 1;
        [HorizontalGroup("Charge")]
        [SerializeField]
        int chargeStep = 1;
        [HorizontalGroup("Charge")]
        [SerializeField]
        float addMultiplier = 0;

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

        public bool CanAddProperty(int magazineNumber)
        {
            if (magazineNumber < charge[0])
                return false;
            return true;
        }
        public string DrawProperty(int magazineNumber)
        {
            int index = 0;
            for (int i = 0; i < charge.Length; i++)
            {
                if (magazineNumber < charge[i])
                    return propertyText.Replace("#", multiplier[index].ToString());
                else
                    index = i;
            }
            return null;
        }

        public string GetLabel()
        {
            return propertyText;
        }

        public float GetLabelValue(int magazineNumber)
        {
            int index = 0;
            for (int i = 0; i < charge.Length; i++)
            {
                if (magazineNumber < charge[i] && statModifiers.Count == 1)
                    return multiplier[index] + statModifiers[0].StatValue;
                else if (magazineNumber < charge[i])
                    return multiplier[index];
                else
                    index = i;
            }
            return 0;
        }

        public float GetMultiplier(int magazineNumber)
        {
            int index = 0;
            for (int i = 0; i < charge.Length; i++)
            {
                if (magazineNumber < charge[i])
                    return multiplier[index];
                else
                    index = i;
            }
            return 0;
        }

        // Add Character stat for the attack
        public void AddProperty(int magazineNumber, CharacterStatController character)
        {
            if(CanAddProperty(magazineNumber))
            {
                float multiplier = GetMultiplier(magazineNumber);
                for(int i = 0; i < statModifiers.Count; i++)
                    character.AddStat(statModifiers[i].StatName, statModifiers[i].StatValue * multiplier, statModifiers[i].ModifierType);
                //character.StatController.AddStat(new Stat(statModifiers[i].StatName, statModifiers[i].StatValue * multiplier), statModifiers[i].ModifierType);
            }
        }

        // Remove the proerty after the attack was generated
        public void RemoveProperty(int magazineNumber, CharacterStatController character)
        {
            if (CanAddProperty(magazineNumber))
            {
                float multiplier = GetMultiplier(magazineNumber);
                for (int i = 0; i < statModifiers.Count; i++)
                    character.RemoveStat(statModifiers[i].StatName, statModifiers[i].StatValue * multiplier, statModifiers[i].ModifierType);
                //character.StatController.RemoveStat(new Stat(statModifiers[i].StatName, statModifiers[i].StatValue * multiplier), statModifiers[i].ModifierType);
            }
        }



        [Button]
        public void GenerateMultiplierCurve()
        {
            int max = maxLevel;
            if (infinite == true)
                max = 50;
            max = max / chargeStep;
            charge = new int[max];
            multiplier = new float[max];
            for (int i = 0; i < max; i++)
            {
                charge[i] = chargeStep * (i+1);
                multiplier[i] = startMultiplier + (addMultiplier * (i));
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace