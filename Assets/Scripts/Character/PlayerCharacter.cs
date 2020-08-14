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
    public class PlayerCharacter: Character
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Space]
        [SerializeField]
        CharacterAction characterAction;
        public CharacterAction CharacterAction { get { return characterAction; } }

        [SerializeField]
        CharacterHeroAction characterHeroAction;
        public CharacterHeroAction CharacterHeroAction { get { return characterHeroAction; } }

        [SerializeField]
        CharacterTriAttack characterTriAttack;
        public CharacterTriAttack CharacterTriAttack { get { return characterTriAttack; } }

        [SerializeField]
        CharacterEquipementController characterEquipement;
        public CharacterEquipementController CharacterEquipement 
        { 
            get { return characterEquipement; }
            set { characterEquipement = value; characterEquipement.CharacterStatController = characterStatController; }
        }



        //[SerializeField]
        List<IInteraction> interactions = new List<IInteraction>();
        public List<IInteraction> Interactions
        {
            get { return interactions; }
            set { interactions = value; }
        }

        /*[SerializeField]
        CharacterGrowthController characterGrowthController;
        public CharacterGrowthController CharacterGrowthController
        {
            get { return characterGrowthController; }
            set { characterGrowthController = value; characterEquipement.CharacterStatController = characterStatController; }
        }*/

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