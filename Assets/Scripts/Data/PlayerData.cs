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
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        private CharacterData characterData;
        public CharacterData CharacterData
        {
            get { return characterData; }
        }
        [SerializeField]
        private CharacterGrowthData characterGrowth;
        public CharacterGrowthData CharacterGrowth
        {
            get { return characterGrowth; }
        }

        [SerializeField]
        List<CharacterWeaponLevelData> weaponLevels;
        public List<CharacterWeaponLevelData> WeaponLevels
        {
            get { return weaponLevels; }
        }

        [SerializeField]
        List<WeaponData> weaponEquipped;
        public List<WeaponData> WeaponEquipped
        {
            get { return weaponEquipped; }
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


        #endregion

    }

} // #PROJECTNAME# namespace