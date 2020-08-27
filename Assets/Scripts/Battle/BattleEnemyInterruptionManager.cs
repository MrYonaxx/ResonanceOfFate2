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

            inputController.EnemyAttack();
            battleEnemyManager.InterruptionAttack();
            StartCoroutine(InterruptionFeedback(attack.GetAttack()));
        }

        public void EnemyInterruptionEnd()
        {
            aimReticle.ShowAimReticle();
            inputController.ResumeInput();
            inputController.CancelAim();
        }


        private IEnumerator InterruptionFeedback(AttackController attack)
        {
            animatorInterruption.SetTrigger("Feedback");
            animatorCanvasAction.speed = 0.2f;
            attack.SetAnimSpeed(0.2f);
            yield return new WaitForSeconds(0.5f);
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