/***************************************************************
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
    public class CharacterGrowthController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        private CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
        }

        private CharacterGrowthData characterGrowth;
        public CharacterGrowthData CharacterGrowth
        {
            get { return characterGrowth; }
        }


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
        public CharacterGrowthController(CharacterGrowthData growthData, CharacterStatController stat)
        {
            characterStatController = stat;
            characterGrowth = growthData;
            CalculateStat();
        }


        public void CalculateStat()
        {
            int level = characterStatController.Level-1;

            // Base
            characterStatController.StatController.SetStatBase(new StatController(characterStatController.CharacterData.StatController));
            // Growth Value
            characterStatController.StatController.AddStatBase(characterGrowth.GrowthStat, level);

            characterStatController.Hp += (int) (characterGrowth.GrowthStat.GetValue("HPMax") * level);
        }

        public int GetStatAtLevel(string valueName, int level)
        {
            float baseValue = characterStatController.CharacterData.StatController.GetValue(valueName);
            float growthValue = characterGrowth.GrowthStat.GetValue(valueName) * (level - 1);
            return (int) (baseValue + growthValue);
        }

            #endregion

        } 

} // #PROJECTNAME# namespace