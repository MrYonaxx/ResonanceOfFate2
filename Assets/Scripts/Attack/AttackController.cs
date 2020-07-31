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
using UnityEditor;

namespace VoiceActing
{
    public class AttackController: SerializedMonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        AttackDataStat bonusAttackData;

        [Space]
        /*[SerializeField]
        Transform muzzle;*/

        [Space]
        [Sirenix.Serialization.OdinSerialize]
        IAttackCollision attackCollision;
        /*[SerializeField]
        GameObject muzzleAnimation;*/

        [SerializeField]
        GlobalFeedbackManager feedbackManager;
        [SerializeField]
        GlobalCamera globalCamera;

        [Title("Camera")]
        [SerializeField]
        Transform cameraParent;
        [SerializeField] // Transform a placé sur la target (Pour les effets de camera nécessitant la position du joueur)
        Transform cameraLockTransform;
        [SerializeField]
        Transform[] cameraSecondaryParent;

        [Title("Parameter")]
        [SerializeField]
        [MinValue(-1)]
        [MaxValue(1)]
        int baseDirection = 1;

        [HorizontalGroup("Parameter1")]
        [SerializeField]
        bool stayOnGround = true;
        [HorizontalGroup("Parameter1")]
        [SerializeField]
        bool fixDirection = false;

        [HorizontalGroup("Parameter2")]
        [SerializeField]
        bool stopTime = true;
        [HorizontalGroup("Parameter2")]
        [SerializeField] // Si false, il faut placer le cameraLockTransform via les scripts (utile pour les AoE par exemple qui ne vise pas un personnage en particulier)
        bool manualTarget = false;


        [Title("Debug")]
        [SerializeField]
        Transform targetDebug;


        AttackData attackData;

        Transform target;
        Animator anim;

        public delegate void EndAction();
        public event EndAction OnEndAction;



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

        [Button]
        private void TargetDebug()
        {
            TurnToTarget(targetDebug);
        }

        public void SetAttackData(AttackData data)
        {
            attackData = data;
            attackData.AttackDataStat.Add(bonusAttackData);
            attackData.AttackDataStat.UserPosition = this.transform.position;
            attackCollision.SetAttackData(attackData);
        }

        public void CreateAttack(Transform newTarget)
        {
            if(anim == null)
                anim = GetComponent<Animator>();
            anim.SetTrigger("Start");
            anim.SetInteger("Animation", Random.Range(0, 2));
            if(stopTime == true)
                feedbackManager.SetMotionSpeed(0f);
            target = newTarget;
            globalCamera.ActivateCameraAction(true);
            Transform cam = globalCamera.GetCameraAction();
            cam.SetParent(cameraParent, false);
            cam.localPosition = Vector3.zero;
            cam.localEulerAngles = Vector3.zero;

            if (cameraLockTransform != null)
            {
                if (manualTarget == false)
                    cameraLockTransform.position = newTarget.position;
                Vector3 lockPreviousTransform = cameraLockTransform.position;
                TurnToTarget(cameraLockTransform);
                cameraLockTransform.position = lockPreviousTransform;
            }
            else
            {
                TurnToTarget(newTarget);
            }
        }

        public void SetDirection(int direction)
        {
            if (fixDirection == true)
                return;
            this.transform.localScale = new Vector3(direction * baseDirection, this.transform.localScale.y, this.transform.localScale.z);
            if (direction != baseDirection)
            {
                this.transform.Rotate(new Vector3(0, 180, 0));
            }
        }

        public void TurnToTarget(Transform targ)
        {
            this.transform.LookAt(targ, Vector3.up);
            this.transform.Rotate(new Vector3(0,90,0));
            if(stayOnGround == true)
                this.transform.localEulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            else
                this.transform.localEulerAngles = new Vector3(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        }

        // Appelé par les anim, le nom est une erreur mais supprimer casserai quelques anims
        public void CreateBullet()
        {
            attackCollision.Play(target);
        }

        public void AttackActive()
        {
            attackCollision.Play(target);
        }

        public void EndAttack()
        {
            feedbackManager.SetMotionSpeed(1f);
            globalCamera.ActivateCameraAction(false);
            globalCamera.GetCameraAction().SetParent(null, false);
            OnEndAction.Invoke();
        }

        public void StopAttack()
        {
            anim.enabled = false;
            EndAttack();
        }


        public void SetSecondaryCamera(int camNb)
        {
            Transform cam = globalCamera.GetCameraAction();
            cam.SetParent(cameraSecondaryParent[camNb], false);
            cam.localPosition = Vector3.zero;
            cam.localEulerAngles = Vector3.zero;
            
        }

        public void SetLockPosition(Vector3 position)
        {
            cameraLockTransform.position = position;
        }


        #endregion

    } 

} // #PROJECTNAME# namespace