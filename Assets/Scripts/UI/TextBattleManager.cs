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
    public class TextBattleManager: MonoBehaviour, IListListener<EnemyController>
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        BattleEnemyManager battleEnemyManager;
        [SerializeField]
        GlobalEnemyList globalEnemyList;

        [Space]
        [SerializeField]
        Animator textLaunch;
        [SerializeField]
        Animator textSmackdown;
        [SerializeField]
        Animator textInterrupt;
        [SerializeField]
        Animator textWin;

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

        private void Awake()
        {
            SubscribeEnemy();
            globalEnemyList.AddListener(this);
        }

        private void OnDestroy()
        {
            globalEnemyList.RemoveListener(this);
        }

        public void OnListAdd(EnemyController enemy)
        {
            enemy.Enemy.CharacterDamage.OnInterrupt += InterruptTrigger;
            enemy.Enemy.CharacterDamage.OnLaunch += LaunchTrigger;
            enemy.Enemy.CharacterDamage.OnSmackdown += SmackdownTrigger;
        }
        public void OnListRemove(EnemyController enemy)
        {
            enemy.Enemy.CharacterDamage.OnInterrupt -= InterruptTrigger;
            enemy.Enemy.CharacterDamage.OnLaunch -= LaunchTrigger;
            enemy.Enemy.CharacterDamage.OnSmackdown -= SmackdownTrigger;
        }

        private void SubscribeEnemy()
        {
            battleEnemyManager.OnWin += WinTrigger;
        }

        public void InterruptTrigger()
        {
            textInterrupt.SetTrigger("Feedback");
        }
        public void LaunchTrigger()
        {
            textLaunch.SetTrigger("Feedback");
        }

        public void SmackdownTrigger()
        {
            textSmackdown.SetTrigger("Feedback");
        }
        public void WinTrigger()
        {
            textWin.SetTrigger("Feedback");
        }


        #endregion

    } 

} // #PROJECTNAME# namespace