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
    public class AttackData
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */


        // Logique de l'attaque
        [SerializeField]
        AAttackProcessor attackProcessor;
        public AAttackProcessor AttackProcessor
        {
            get { return attackProcessor; }
        }


        //[Title("Debug")]
        //[BoxGroup]
        [SerializeField]
        private AttackDataStat attackDataStat;
        public AttackDataStat AttackDataStat
        {
            get { return attackDataStat; }
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

        public AttackData(AAttackProcessor attackProcess, CharacterStatController user, WeaponData weaponData)
        {
            attackProcessor = attackProcess;
            attackDataStat = attackProcessor.CreateAttack(user, weaponData);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace