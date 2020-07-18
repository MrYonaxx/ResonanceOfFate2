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
    [CreateAssetMenu(fileName = "PartyData", menuName = "PartyData", order = 1)]
    public class PartyData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        List<CharacterStatController> characterStatControllers = new List<CharacterStatController>();
        public List<CharacterStatController> CharacterStatControllers
        {
            get { return characterStatControllers; }
        }

        [SerializeField]
        List<CharacterEquipementController> characterEquipement = new List<CharacterEquipementController>();
        public List<CharacterEquipementController> CharacterEquipement
        {
            get { return characterEquipement; }
        }

        [SerializeField]
        List<CharacterGrowthController> characterGrowths = new List<CharacterGrowthController>();
        public List<CharacterGrowthController> CharacterGrowths
        {
            get { return characterGrowths; }
        }

        // Playable Characters
        // Stat
        // Weapons
        // Levels

        // Battle Party
        // Stat
        // Weapons
        // Levels

        // Inventory

        // System (Config / options)


        public class PartyInitialized
        {
            public bool Initialized = false;
            public PartyInitialized()
            {
                //Initialized = true;
            }
        }
        PartyInitialized partyInitialized = null;

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

        public void SetInitialize()
        {
            partyInitialized = new PartyInitialized();
        }
        public bool GetInitialize()
        {
            if (partyInitialized == null)
                return false;
            return true;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace