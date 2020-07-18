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
        [SerializeField]
        Transform muzzle;
        [SerializeField]
        Transform cameraParent;

        [Space]
        //[R (typeof(IAttackCollision))]
        [Sirenix.Serialization.OdinSerialize]
        IAttackCollision attackCollision;
        [SerializeField]
        GameObject muzzleAnimation;

        [Space]
        [SerializeField]
        GlobalFeedbackManager feedbackManager;
        [SerializeField]
        GlobalCamera globalCamera;

        [Title("Parameter")]
        [SerializeField]
        bool stayOnGround = true;
        [SerializeField]
        bool fixDirection = false;
        [SerializeField]
        bool stopTime = true;
        [SerializeField]
        [MinValue(-1)]
        [MaxValue(1)]
        int baseDirection = 1;

        [Title("Debug")]
        [SerializeField]
        Transform targetDebug;

        [SerializeField]
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
            //texture.SetBool("Appear", true);
            globalCamera.ActivateCameraAction(true);
            Transform cam = globalCamera.GetCameraAction();
            cam.SetParent(cameraParent, false);
            cam.localPosition = Vector3.zero;
            cam.localEulerAngles = Vector3.zero;
            TurnToTarget(target);
            //attackCollision.SetAttackData(attackData);
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

        public void CreateBullet()
        {
            muzzle.LookAt(target);
            attackCollision.Play();
            Instantiate(muzzleAnimation, muzzle.transform.position, Quaternion.identity);
        }

        public void AttackActive()
        {
            attackCollision.Play();
        }

        public void EndAttack()
        {
            feedbackManager.SetMotionSpeed(1f);
            globalCamera.ActivateCameraAction(false);
            //texture.SetBool("Appear", false);
            OnEndAction.Invoke();
            //this.gameObject.SetActive(false);
        }

        public void StopAttack()
        {
            anim.enabled = false;
            EndAttack();
        }



        #endregion

    } 

} // #PROJECTNAME# namespace