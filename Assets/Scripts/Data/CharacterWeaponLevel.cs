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
    public class CharacterWeaponLevel
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterWeaponLevelData levelData;

        int level;
        public int Level
        {
            get { return level; }
        }

        int nextExperience;
        public int NextExperience
        {
            get { return nextExperience; }
        }

        int totalExperience;
        public int TotalExperience
        {
            get { return totalExperience; }
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
        public CharacterWeaponLevel(CharacterWeaponLevelData data)
        {
            levelData = data;
            level = data.BaseLevel;
            for(int i = 1; i < level; i++)
                totalExperience += levelData.ExperienceCurve[(i - 1)];
            nextExperience = levelData.ExperienceCurve[(level - 1)];
        }
        public CharacterWeaponLevel(CharacterWeaponLevelData data, int totalExp)
        {
            levelData = data;
            totalExperience = totalExp;
            CalculateLevel();
            for (int i = 1; i < level+1; i++)
                nextExperience += levelData.ExperienceCurve[(i - 1)];
            nextExperience -= totalExperience;
            Debug.Log(nextExperience);
        }


        public List<AttackAimProperty> GetAttackProperties()
        {
            return levelData.GetAttackProperties(level);
        }

        public int GetWeaponType()
        {
            return levelData.WeaponType;
        }

        /// <summary>
        /// Return level gained
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int AddExperience(int value)
        {
            int levelGained = 0;
            totalExperience += value;
            nextExperience -= value;
            while(nextExperience < 0)
            {
                levelGained += 1;
                nextExperience += levelData.ExperienceCurve[(level - 1) + levelGained];
            }
            return levelGained;
        }

        // Le procédé permet en théorie de faire des update de la courbe d'exp et de garder le bon niveau
        // En gros si on patch le jeu ça devient pas la zizanie
        public void CalculateLevel()
        {
            int totalExp = totalExperience;
            int finalLevel = 0;
            while(totalExp > 0)
            {
                totalExp -= levelData.ExperienceCurve[finalLevel];
                finalLevel += 1;
            }
            level = finalLevel;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace