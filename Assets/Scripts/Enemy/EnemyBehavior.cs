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
    public abstract class EnemyBehavior: MonoBehaviour, IEnemyBehavior
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Data")]
        [SerializeField]
        protected WeaponData attackStats;
        [SerializeField]
        protected AttackController attackController;

        [Title("Parameter")]
        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public WeaponData GetWeaponData()
        {
            return attackStats;
        }
        public AttackController GetAttackController()
        {
            return attackController;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        public virtual Character SelectTarget(Enemy enemy)
        {
            return null;
        }

        public virtual void PauseBehavior()
        {

        }
        public virtual void ResumeBehavior()
        {

        }
        public virtual float UpdateBehavior(Enemy enemy, Character target)
        {
            return 0;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace