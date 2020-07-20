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
    public class Character: MonoBehaviour, ITargetable
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        protected Transform characterCenter;
        public Transform CharacterCenter
        {
            get { return characterCenter; }
        }

        [SerializeField]
        protected CharacterData characterData;

        [SerializeField]
        protected CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
            set { characterStatController = value; CharacterDamage.CharacterStatController = value; }
        }

        private Transform targetDirection;
        public Transform TargetDirection
        {
            get { return targetDirection; }
        }


        [Space]
        [SerializeField]
        protected CharacterAnimation characterAnimation;
        public CharacterAnimation CharacterAnimation { get { return characterAnimation; } }

        [SerializeField]
        protected CharacterDirection characterDirection;
        public CharacterDirection CharacterDirection { get { return characterDirection; } }

        [SerializeField]
        protected CharacterMovement characterMovement;
        public CharacterMovement CharacterMovement { get { return characterMovement; } }



        /*[Space]
        [SerializeField]
        CharacterAction characterAction;
        public CharacterAction CharacterAction { get { return characterAction; } }

        [SerializeField]
        CharacterHeroAction characterHeroAction;
        public CharacterHeroAction CharacterHeroAction { get { return characterHeroAction; } }

        [SerializeField]
        CharacterTriAttack characterTriAttack;
        public CharacterTriAttack CharacterTriAttack { get { return characterTriAttack; } }*/

        [Space]
        [SerializeField]
        CharacterDamage characterDamage; 
        public CharacterDamage CharacterDamage { get { return characterDamage; } }

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
        protected void Awake()
        {
            targetDirection = characterDirection.DirectionTransform;
            if (characterData != null)
            {
                characterStatController = new CharacterStatController(characterData);
                CharacterDamage.CharacterStatController = characterStatController;
            }
        }


        #endregion

    } 

} // #PROJECTNAME# namespace