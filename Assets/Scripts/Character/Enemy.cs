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
    public class Enemy: Character
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterAction characterAction;
        public CharacterAction CharacterAction { get { return characterAction; } }

        [SerializeField]
        Transform healthBar;
        public Transform HealthBar
        {
            get { return healthBar; }
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

        public override List<BodyPartController> GetBodyParts()
        {
            //Debug.Log(CharacterDamage.CharacterBodyPartController.BodyPart[0]);
            if (CharacterDamage.CharacterBodyPartController == null)
                return CharacterDamage.BodyPartControllers;
            return CharacterDamage.CharacterBodyPartController.BodyPart;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace