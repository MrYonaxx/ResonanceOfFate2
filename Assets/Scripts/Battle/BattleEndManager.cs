/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

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
        [SerializeField]
        FeverTimeManager feverTimeManager;
        [SerializeField]
        TriAttackManager triAttackManager;

        [Space]
        [SerializeField]
        CameraLock cameraLock;
        [SerializeField]
        AimReticle aimReticle;

        [SerializeField]
        Transform finalPosition;

        [SerializeField]
        GlobalFeedbackManager globalFeedback;

        [SerializeField]
        UnityEvent eventEndBattle;

        [Title("GameOver")]
        [SerializeField]
        GameObject gameOverCanvas;
        [SerializeField]
        GameObject canvas;
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        float smoothRotation = 0.2f;
        [SerializeField]
        float smoothCamera = 0.2f;
        [SerializeField]
        SaveLoadManager saveLoadManager;

        bool gameOver = false;

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
            StartGameOver();
        }

        public void EndBattle()
        {
            inputController.StopInput();
            feverTimeManager.ForceStopFever();
            partyManager.StopPlayers();
            aimReticle.StopAim();
            aimReticle.HideAimReticle();
            aimReticle.SetTarget(null);
            //aimReticle.enabled = false;

            finalPosition.position = enemyManager.GetEnemies()[0].transform.position;
            cameraLock.SetState(0);
            cameraLock.SetFocus(finalPosition);
            cameraLock.LockOn(false);
            cameraLock.SetTarget(null);

            triAttackManager.ForceStopTriAttack();

            StartCoroutine(EndBattleCoroutine());
        }

        private IEnumerator EndBattleCoroutine()
        {
            globalFeedback.SetMotionSpeed(0.2f);
            yield return new WaitForSeconds(3f);
            globalFeedback.SetMotionSpeed(1f);
            AudioManager.Instance.SwitchToBattle(false);
            cameraLock.SetFocus(partyManager.GetCharacter().CharacterCenter.transform);
            inputController.ResumeInput();
            eventEndBattle.Invoke();
        }


        private void OnDestroy()
        {
            enemyManager.OnWin -= EndBattle;
        }






        // Section Game Over (A déplacer dans un script un jour quand j'aurai moins la flemme
        private void StartGameOver()
        {
            for(int i = 0; i < partyManager.GetParty().Count; i++)
            {
                partyManager.GetCharacter(i).CharacterDamage.OnDead += GameOver;
            }
        }

        public void GameOver()
        {
            if (gameOver == true)
                return;
            gameOver = true;
            Time.timeScale = 0.1f;
            inputController.StopInput();
            feverTimeManager.ForceStopFever();
            enemyManager.ResetQueue();
            partyManager.StopPlayers();
            aimReticle.StopAim();
            aimReticle.HideAimReticle();
            aimReticle.SetTarget(null);

            PlayerCharacter p = null;
            for (int i = 0; i < partyManager.GetParty().Count; i++)
            {
                partyManager.GetCharacter(i).CharacterMovement.SetInput(false);
                if (partyManager.GetCharacter(i).CharacterDamage.IsDead == true)
                {
                    p = partyManager.GetCharacter(i);
                    p.CharacterAnimation.PlayTrigger("Hit");
                    break;
                }
            }

            cameraLock.SetState(0);
            cameraLock.SetFocus(p.CharacterCenter);
            cameraLock.LockOn(false);
            cameraLock.SetTarget(null);
            gameOverCanvas.SetActive(true);
            canvas.SetActive(false);
            StartCoroutine(GameOverCoroutine(p));
        }

        private IEnumerator GameOverCoroutine(PlayerCharacter p)
        {
            AudioManager.Instance.StopMusicWithScratch(5);
            Vector3 destination = globalCamera.GetCameraAction().position + (p.CharacterCenter.transform.position - globalCamera.GetCameraAction().position) * 0.5f;
            Vector3 destination2 = globalCamera.GetCameraDefault().position + (p.CharacterCenter.transform.position - globalCamera.GetCameraDefault().position) * 0.5f;
            float t = 0f;
            while(t < 5f)
            {
                t += Time.unscaledDeltaTime;
                globalCamera.GetCameraAction().position = Vector3.Lerp(globalCamera.GetCameraAction().position, destination, smoothCamera);
                globalCamera.GetCameraDefault().position = Vector3.Lerp(globalCamera.GetCameraDefault().position, destination2, smoothCamera);
                UpdateLockRotation(globalCamera.GetCameraAction(), p);
                UpdateLockRotation(globalCamera.GetCameraDefault(), p);
                yield return null;
            }
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(5f);
            Time.timeScale = 1f;
            if (saveLoadManager.LoadPlayerProfile(0) == false) // No save
                SceneManager.LoadScene("TitleScreen");
            else
                saveLoadManager.LoadScene();
        }

        private void UpdateLockRotation(Transform pivot, PlayerCharacter p)
        {
            //Transform pivot = globalCamera.GetCameraAction();
            Vector3 originalRot = pivot.localEulerAngles;
            pivot.LookAt(p.CharacterCenter.position);
            Vector3 newRot = pivot.localEulerAngles;
            pivot.localEulerAngles = originalRot;
            float x = Mathf.LerpAngle(pivot.localEulerAngles.x, newRot.x, smoothRotation);
            float y = Mathf.LerpAngle(pivot.localEulerAngles.y, newRot.y, smoothRotation);
            float z = Mathf.LerpAngle(pivot.localEulerAngles.z, newRot.z, smoothRotation);
            pivot.localEulerAngles = new Vector3(x, y, z);
        }


        #endregion

    } 

} // #PROJECTNAME# namespace