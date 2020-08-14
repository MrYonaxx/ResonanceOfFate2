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
        List<ArmorData> armors = new List<ArmorData>(); // armor equipped

        private CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
            set { characterStatController = value; }
        }

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


        int currentWeaponLevel = 0;
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
        /*public CharacterEquipementController(List<WeaponData> weaponDatas)
        {
            weapons = weaponDatas;
        }*/
        /*public CharacterEquipementController(List<WeaponData> weaponDatas, List<CharacterWeaponLevelData> weaponLevelDatas, CharacterStatController statController, CharacterGrowthController growth)
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
        }*/

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

        // Bon ça devient n'importe quoi, à refactor
        public CharacterEquipementController(List<WeaponData> weaponDatas, List<ArmorData> armorDatas, List<CharacterWeaponLevelData> weaponLevelDatas, List<int> totalExp, CharacterStatController statController, CharacterGrowthController growth)
        {
            weapons = weaponDatas;
            armors = armorDatas;
            for (int i = 0; i < weaponLevelDatas.Count; i++)
            {
                weaponLevels.Add(new CharacterWeaponLevel(weaponLevelDatas[i], totalExp[i]));
            }
            characterStatController = statController;
            characterGrowthController = growth;
            EquipWeapon(0);
            EquipArmors();
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






        public void EquipArmors()
        {
            for (int i = 0; i < armors.Count; i++)
            {
                EquipArmor(i, armors[i]);
            }
        }
        public void EquipArmor(int index, ArmorData newArmor)
        {
            if (armors.Count <= index)
                armors.Add(newArmor);
            else
                armors[index] = newArmor;
            for (int i = 0; i < armors[index].StatModifiers.Count; i++)
            {
                characterStatController.StatController.AddStat(new Stat(armors[index].StatModifiers[i].StatName, armors[index].StatModifiers[i].StatValue), armors[index].StatModifiers[i].ModifierType);
            }
        }
        public ArmorData RemoveArmor(int index)
        {
            if (index >= armors.Count)
                return null;
            if (armors[index] == null)
                return null;
            for (int i = 0; i < armors[index].StatModifiers.Count; i++)
            {
                characterStatController.StatController.RemoveStat(new Stat(armors[index].StatModifiers[i].StatName, armors[index].StatModifiers[i].StatValue), armors[index].StatModifiers[i].ModifierType);
            }
            ArmorData res = armors[index];
            armors[index] = null;
            return res;
        }

        public ArmorData GetArmor(int index)
        {
            if (index >= armors.Count)
                return null;
            return armors[index];
        }

        public List<string> GetArmors()
        {
            List<string> res = new List<string>(armors.Count);
            for (int i = 0; i < armors.Count; i++)
            {
                res.Add(armors[i].name);
            }
            return res;
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
        public List<int> GetWeaponTotalExp()
        {
            List<int> res = new List<int>(weaponLevels.Count);
            for (int i = 0; i < weaponLevels.Count; i++)
            {
                res.Add(weaponLevels[i].TotalExperience);
            }
            return res;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace