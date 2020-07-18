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
    public class BattleEndManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        InputController inputController;
        [SerializeField]
        BattlePartyManager partyManager;
        [SerializeField]
        BattleEnemyManager enemyManager;

        [Space]
        [SerializeField]
        CameraLock cameraLock;
        [SerializeField]
        AimReticle aimReticle;

        [SerializeField]
        Transform finalPosition;

        [SerializeField]
        GlobalFeedbackManager globalFeedback;

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
            enemyManager.OnWin += EndBattle;
        }

        public void EndBattle()
        {
            inputController.StopInput();
            partyManager.StopPlayers();

            aimReticle.StopAim();
            aimReticle.HideAimReticle();
            aimReticle.enabled = false;

            finalPosition.position = enemyManager.GetEnemies()[0].transform.position;
            cameraLock.SetState(0);
            cameraLock.SetFocus(finalPosition);
            cameraLock.LockOn(false);
            cameraLock.SetTarget(null);

            StartCoroutine(EndBattleCoroutine());
        }

        private IEnumerator EndBattleCoroutine()
        {
            globalFeedback.SetMotionSpeed(0.2f);
            yield return new WaitForSeconds(3f);
            globalFeedback.SetMotionSpeed(1f);
            AudioManager.Instance.SwitchToBattle(false);
            cameraLock.SetFocus(partyManager.GetCharacter().CharacterCenter.transform);
        }


        private void OnDestroy()
        {
            enemyManager.OnWin -= EndBattle;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace