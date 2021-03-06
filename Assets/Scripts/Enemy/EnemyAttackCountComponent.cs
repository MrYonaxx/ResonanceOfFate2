﻿/*****************************************************************
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
    public class AttackCountEvent
    {
        [HorizontalGroup]
        [SerializeField]
        public int maxAttackCount = 3;

        [HorizontalGroup]
        [SerializeField]
        public int phaseNumber = 0;
    }

    public class EnemyAttackCountComponent: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        EnemyController enemyController;
        [SerializeField]
        AttackCountEvent[] attackCountEvents;

        [SerializeField]
        bool destroyAfterOneCycle = false;
        [SerializeField]
        bool cycleOnInterruption = false;

        int attackCount = 0;

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
        private void Start()
        {
            enemyController.Enemy.CharacterAction.OnEndAction += AddAttackCount;
            if(cycleOnInterruption == true)
                enemyController.OnAttackInterrupted += AddAttackCountInterruption;
        }

        private void AddAttackCount()
        {
            attackCount += 1;
            for (int i = 0; i < attackCountEvents.Length; i++)
            {
                if (attackCount == attackCountEvents[i].maxAttackCount)
                {
                    enemyController.ChangePhase(attackCountEvents[i].phaseNumber);
                    if (i == attackCountEvents.Length - 1)
                    {
                        attackCount = 0;
                        if (destroyAfterOneCycle == true)
                            Destroy(this.gameObject);
                    }
                }
            }
        }

        private void AddAttackCountInterruption(EnemyController enemyController, bool interruption)
        {
            AddAttackCount();
        }


        public void StopCount()
        {
            // Si y'a d'autre trucs sur l'objet a changer
            Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
            enemyController.Enemy.CharacterAction.OnEndAction -= AddAttackCount;
            if (cycleOnInterruption == true)
                enemyController.OnAttackInterrupted -= AddAttackCountInterruption;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace