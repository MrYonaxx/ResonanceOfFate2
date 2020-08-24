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
    public delegate void FeverAction(AttackController controller);

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

        [HorizontalGroup("Parameter3")]
        [SerializeField]
        bool actionMode = true;

        [Title("Debug")]
        [SerializeField]
        Transform targetDebug;


        AttackData attackData;

        Transform target;
        Animator anim;
        private IEnumerator lookCoroutine;

        //public delegate void EndAction();
        public event Action OnEndAction;
        public event FeverAction OnFeverAction;


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

            if (actionMode == true)
            {
                globalCamera.ActivateCameraAction(true);
                Transform cam = globalCamera.GetCameraAction();
                cam.SetParent(cameraParent, false);
                cam.localPosition = Vector3.zero;
                cam.localEulerAngles = Vector3.zero;
            }

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

        public void StartAnimation()
        {
            if (anim == null)
                anim = GetComponent<Animator>();
            anim.SetTrigger("Start");
            anim.SetInteger("Animation", Random.Range(0, 2));
            globalCamera.ActivateCameraAction(true);
            Transform cam = globalCamera.GetCameraAction();
            cam.SetParent(cameraParent, false);
            cam.localPosition = Vector3.zero;
            cam.localEulerAngles = Vector3.zero;
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


        // Utilisé par les players pour le fever time
        public void EndAttackFever()
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            anim.speed = 0;
            if (OnFeverAction != null)
            {
                OnFeverAction.Invoke(this);
            }
            else
            {
                EndAttack();
            }
        }

        public void SetAnimSpeed(float speed = 1f)
        {
            anim.speed = speed;
        }

        public void EndAttack()
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            anim.speed = 1;
            feedbackManager.SetMotionSpeed(1f);
            if (actionMode == true)
            {
                globalCamera.ActivateCameraAction(false);
                globalCamera.GetCameraAction().SetParent(null, false);
            }
            if(OnEndAction != null) OnEndAction.Invoke();
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



        // Look at enemy (ptet à dégager)
        public void LookAtTarget(float smoothRotation)
        {
            if (lookCoroutine != null)
                StopCoroutine(lookCoroutine);
            lookCoroutine = UpdateLockRotation(smoothRotation);
            StartCoroutine(lookCoroutine);
        }

        private IEnumerator UpdateLockRotation(float smoothRotation)
        {
            Transform pivot = globalCamera.GetCameraAction();
            Vector3 originalRot;
            Vector3 newRot;
            float x;
            float y;
            float z;
            while(true)
            {
                originalRot = pivot.localEulerAngles;
                pivot.LookAt(target.position);
                newRot = pivot.localEulerAngles;
                pivot.localEulerAngles = originalRot;
                x = Mathf.LerpAngle(pivot.localEulerAngles.x, newRot.x, smoothRotation);
                y = Mathf.LerpAngle(pivot.localEulerAngles.y, newRot.y, smoothRotation);
                z = Mathf.LerpAngle(pivot.localEulerAngles.z, newRot.z, smoothRotation);
                pivot.localEulerAngles = new Vector3(x, y, z);
                yield return null;
            }
            /*Vector3 originalRot = pivot.localEulerAngles;
            pivot.LookAt(target.position);
            Vector3 newRot = pivot.localEulerAngles;
            pivot.localEulerAngles = originalRot;
            float x = Mathf.LerpAngle(pivot.localEulerAngles.x, newRot.x, smoothRotation);
            float y = Mathf.LerpAngle(pivot.localEulerAngles.y, newRot.y, smoothRotation);
            float z = Mathf.LerpAngle(pivot.localEulerAngles.z, newRot.z, smoothRotation);
            pivot.localEulerAngles = new Vector3(x, y, z);
            yield return null;*/
        }




        #endregion

    } 

} // #PROJECTNAME# namespace