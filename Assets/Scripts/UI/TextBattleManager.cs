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
    public class TextBattleManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        BattleEnemyManager battleEnemyManager;
        [SerializeField]
        Animator textLaunch;
        [SerializeField]
        Animator textSmackdown;
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

        private void Start()
        {
            SubscribeEnemy();
        }

        private void SubscribeEnemy()
        {
            List<EnemyController> enemies = battleEnemyManager.GetEnemies();
            for (int i = 0;i < enemies.Count; i++)
            {
                enemies[i].Enemy.CharacterDamage.OnLaunch += LaunchTrigger;
                enemies[i].Enemy.CharacterDamage.OnSmackdown += SmackdownTrigger;
                battleEnemyManager.OnWin += WinTrigger;
            }
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



        private void OnDestroy()
        {
            List<EnemyController> enemies = battleEnemyManager.GetEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Enemy.CharacterDamage.OnLaunch -= LaunchTrigger;
                enemies[i].Enemy.CharacterDamage.OnSmackdown -= SmackdownTrigger;
            }
        }

        #endregion

    } 

} // #PROJECTNAME# namespace