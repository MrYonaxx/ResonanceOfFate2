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
    public class CharacterEquipementController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        int weaponEquipped = 0;
        List<WeaponData> weapons = new List<WeaponData>(); // Switch to weapon

        private CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
            set { characterStatController = value; }
        }

        int currentWeaponLevel = 0;
        List<CharacterWeaponLevel> weaponLevels = new List<CharacterWeaponLevel>();
        public List<CharacterWeaponLevel> WeaponLevels
        {
            get { return weaponLevels; }
        }

        private List<AttackAimProperty> attackProperties;
        public List<AttackAimProperty> AttackProperties
        {
            get { return attackProperties; }
        }


        private CharacterGrowthController characterGrowthController;
        public CharacterGrowthController CharacterGrowthController
        {
            get { return characterGrowthController; }
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
        // Equipement
        public CharacterEquipementController(List<WeaponData> weaponDatas)
        {
            weapons = weaponDatas;
        }
        public CharacterEquipementController(List<WeaponData> weaponDatas, List<CharacterWeaponLevelData> weaponLevelDatas, CharacterStatController statController, CharacterGrowthController growth)
        {
            weapons = weaponDatas;
            for (int i = 0; i < weaponLevelDatas.Count; i++)
            {
                weaponLevels.Add(new CharacterWeaponLevel(weaponLevelDatas[i]));
            }
            characterStatController = statController;
            characterGrowthController = growth;
            EquipWeapon(0);
            SetNewLevel();
        }

        public void EquipWeapon(int equipementID)
        {
            weaponEquipped = equipementID;
            if (equipementID < weapons.Count)
            {
                EquipWeapon(weapons[weaponEquipped]);
                SetWeaponAttackProperty();
            }
        }
        public void EquipWeapon(WeaponData weaponData)
        {
            characterStatController.AddStat(weaponData.BaseStat, StatModifierType.Flat);
        }
        public void RemoveWeapon(WeaponData weaponData)
        {
            characterStatController.RemoveStat(weaponData.BaseStat, StatModifierType.Flat);
        }

        public AttackData GetWeaponAttackData(int magazineNumber)
        {
            // Apply Properties
            for(int i = 0; i < attackProperties.Count; i++)
            {
                attackProperties[i].AddProperty(magazineNumber, characterStatController);
            }
            AttackData res = new AttackData(weapons[weaponEquipped].AttackProcessor, characterStatController, weapons[weaponEquipped]);
            for (int i = 0; i < attackProperties.Count; i++)
            {
                attackProperties[i].RemoveProperty(magazineNumber, characterStatController);
            }
            return res;
            // Remove properties
        }

        /*public AttackData GetWeaponAttackData(WeaponData weaponData)
        {
            //AttackData res = new AttackData(weaponData.AttackData, characterStatController);
            //return res;
            return null;
        }*/

        public int GetWeaponType()
        {
            return weapons[weaponEquipped].GetWeaponType();
        }

        public WeaponData GetWeapon()
        {
            return weapons[weaponEquipped];
        }



        private void SetWeaponAttackProperty()
        {
            int weaponType = weapons[weaponEquipped].GetWeaponType();
            for (int i = 0; i < weaponLevels.Count; i++)
            {
                if (GetWeaponType() == weaponType)
                {
                    currentWeaponLevel = i;
                    break;
                }
            }
            if (currentWeaponLevel < weaponLevels.Count)
                attackProperties = weaponLevels[currentWeaponLevel].GetAttackProperties();
        }






        public bool GainWeaponExp(int amount)
        {
            // Renvois le nombre de level gagné dans l'arme équipé
            int newLevel = weaponLevels[currentWeaponLevel].AddExperience(amount);
            if(newLevel >= 1) // Levelup
            {
                weaponLevels[currentWeaponLevel].CalculateLevel();
                SetNewLevel();
                SetWeaponAttackProperty();
                return true;
            }
            return false;
        }

        private void SetNewLevel()
        {
            int sumLevel = 0;
            for (int i = 0; i < weaponLevels.Count; i++)
            {
                sumLevel += weaponLevels[i].Level;
            }
            characterStatController.Level = sumLevel;
            characterGrowthController.CalculateStat();
        }

        public int GetWeaponNextExp()
        {
            return weaponLevels[currentWeaponLevel].NextExperience;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace