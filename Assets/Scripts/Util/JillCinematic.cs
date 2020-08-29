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
    public class JillCinematic: MonoBehaviour, IAttackPerformer
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        EnemyController jill;
        [SerializeField]
        EnemyController chris;
        [SerializeField]
        float threshold = 2000f;

        [SerializeField]
        AttackController attackController;
        [SerializeField]
        BattleEnemyManager enemyManager;
        [SerializeField]
        GlobalCamera globalCamera;

        [SerializeField]
        CameraLock cameraLock;


        [SerializeField]
        Transform cameraFocus;
        [SerializeField]
        Transform jillPosition;

        [SerializeField]
        UnityEngine.Events.UnityEvent unityEvent;


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public AttackController GetAttack()
        {
            return attackController;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        private void Start()
        {
            jill.Enemy.CharacterStatController.OnHPChanged += CheckHP;
            chris.Enemy.CharacterStatController.OnHPChanged += CheckHP;
            chris.Enemy.CharacterStatController.OnScratchChanged += CheckScratch;
            jill.Enemy.CharacterStatController.OnScratchChanged += CheckScratch;
        }

        public void CheckHP(float a, float b)
        {
            if (((jill.Enemy.CharacterStatController.Hp - jill.Enemy.CharacterStatController.Scratch) + (chris.Enemy.CharacterStatController.Hp - chris.Enemy.CharacterStatController.Scratch)) / (jill.Enemy.CharacterStatController.GetHPMax() + chris.Enemy.CharacterStatController.GetHPMax()) <= threshold)
            {
                jill.Enemy.CharacterStatController.OnHPChanged -= CheckHP;
                chris.Enemy.CharacterStatController.OnHPChanged -= CheckHP;
                chris.Enemy.CharacterStatController.OnScratchChanged -= CheckScratch;
                jill.Enemy.CharacterStatController.OnScratchChanged -= CheckScratch;
                enemyManager.ResetQueue();
                enemyManager.QueueAttack(this, false);
            }

        }

        public void CheckScratch(float a, float b)
        {
            CheckHP(a,b);
        }

        public void PerformAction()
        {
            enemyManager.ShowHealthBars(false);
            attackController.OnEndAction += EndCinematic;
            attackController.OnEndAction += enemyManager.ResolveAction;
            attackController.StartAnimation();
        }

        public void EndCinematic()
        {
            unityEvent.Invoke();
            enemyManager.ResetQueue();
            jill.InterruptBehavior();
            chris.InterruptBehavior();
            enemyManager.ShowHealthBars(true);
            cameraLock.transform.SetParent(null);
            cameraLock.enabled = true;
            Destroy(this.gameObject);
        }

        public void PlaceEnemyPhase2()
        {
            globalCamera.ActivateCameraAction(false);
            globalCamera.GetCameraAction().SetParent(null, false);
            jill.Enemy.CharacterMovement.SetPosition(jillPosition.position);
            cameraLock.transform.position = cameraFocus.position;
            cameraLock.SetFocus(cameraFocus);
            cameraLock.SetCameraAxis(0, 0);
            cameraLock.enabled = false;
            cameraLock.transform.SetParent(cameraFocus);

        }


        #endregion

    } 

} // #PROJECTNAME# namespace