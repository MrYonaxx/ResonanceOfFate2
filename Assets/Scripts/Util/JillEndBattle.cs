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
    public class JillEndBattle: MonoBehaviour
    {
        [SerializeField]
        EnemyController jill;
        [SerializeField]
        EnemyController chris;

        [SerializeField]
        UnityEngine.Events.UnityEvent unityEvent;


        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Start()
        {
            jill.Enemy.CharacterDamage.OnDead += EndBattle;
            chris.Enemy.CharacterDamage.OnDead += EndBattle;
        }

        public void EndBattle()
        {
            unityEvent.Invoke();
        }
        #endregion
    }

} // #PROJECTNAME# namespace