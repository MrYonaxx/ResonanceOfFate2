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
    public class BattleEnemyInterruptionManager: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        BattleEnemyManager battleEnemyManager;

        [SerializeField]
        InputController inputController;
        [SerializeField]
        AimReticle aimReticle;
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        TriAttackManager triAttackManager;
        [SerializeField]
        CameraLock cameraLock;

        [Title("UI")]
        [SerializeField]
        Animator animatorCanvasAction;
        [SerializeField]
        Animator animatorInterruption;

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
            battleEnemyManager.OnEnemyInterruption += EnemyInterruption;
            battleEnemyManager.OnEndAttackInterruption += EnemyInterruptionEnd;
        }



        public void EnemyInterruption(IAttackPerformer attack)
        {
            if (triAttackManager.IsTriAttacking) // On Tri attack l'enemi ne peut pas interrompre
            {
                return;
            }
            if (globalCamera.CameraAction().enabled == true)  // On est dans une attaque, l'ennemi ne peut pas interrompre
            {
                return;
            }

            aimReticle.PauseAim();
            inputController.EnemyAttack();
            battleEnemyManager.InterruptionAttack();
            StartCoroutine(InterruptionFeedback(attack.GetAttack()));
        }

        public void EnemyInterruptionEnd()
        {
            aimReticle.ResumeAim();
            aimReticle.ShowAimReticle();
            inputController.ResumeInput();
            //inputController.CancelAim();
        }


        private IEnumerator InterruptionFeedback(AttackController attack)
        {
            //cameraLock.SetTarget(attack.transform);
            //cameraLock.LockOn(false);
            //cameraLock.SetState(0);
            animatorInterruption.SetTrigger("Feedback");
            /*animatorCanvasAction.speed = 0f;
            attack.SetAnimSpeed(0);
            globalCamera.ActivateCameraAction(false);
            yield return new WaitForSeconds(0.4f);*/

            animatorCanvasAction.speed = 0.1f;
            attack.SetAnimSpeed(0.1f);
            globalCamera.ActivateCameraAction(true);
            yield return new WaitForSeconds(1f);

            animatorCanvasAction.speed = 1;
            float t = 0f;
            float animSpeed = 0;
            while(t < 1f)
            {
                t += (Time.deltaTime * 2);
                animSpeed = Mathf.Lerp(0.2f, 1f, t);
                attack.SetAnimSpeed(animSpeed);
            }
            attack.SetAnimSpeed(1f);
        }



        #endregion

    }

} // #PROJECTNAME# namespace