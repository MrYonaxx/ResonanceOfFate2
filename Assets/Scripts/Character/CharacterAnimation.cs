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

    public enum CharacterState
    {
        Idle,
        Hit,
        Jump,
        Dash,
        Dead
    }

    public class CharacterAnimation: MonoBehaviour, IMotionSpeed
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalFeedbackManager globalFeedback;


        bool appear = true;
        Animator animator;
        float characterMotionSpeed = 1;

        private CharacterState state;
        public CharacterState State
        {
            get { return state; }
            set { state = value;
                //animator.SetTrigger();
            }
        }


        private IEnumerator motionSpeedCoroutine;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public float GetMotionSpeed()
        {
            return characterMotionSpeed;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            globalFeedback.AddCharacter(this);
        }

        private void OnDestroy()
        {
            globalFeedback.RemoveCharacter(this);
        }


        public void UserAppear()
        {
            if (appear == false)
            {
                appear = true;
                //user.enabled = true;
                this.transform.position -= new Vector3(0, 5000, 0);
            }
        }
        public void UserDisappear()
        {
            if (appear == true)
            {
                appear = false;
                //user.enabled = false;
                this.transform.position += new Vector3(0, 5000, 0);
            }
        }



        public void PlayAnim(string animationName)
        {
            animator.Play(animationName, 0);
        }
        public void PlayAnimBool(string boolName)
        {
            animator.SetBool(boolName, true);
        }
        public void PlayTrigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        public void PlayAnimBool(string boolName, bool b)
        {
            animator.SetBool(boolName, b);
        }

        public void CreateAnimation(GameObject animation)
        {
            //Instantiate(animation, particlePoint.position, Quaternion.identity);
        }
        /*public T CreateAnimation<T>(T animation)
        {
            return Instantiate(animation, particlePoint.position, Quaternion.identity);
        }*/

        /*public AnimationParticle CreateAnimation(AnimationParticle animation)
        {
            return Instantiate(animation, particlePoint.position, Quaternion.identity);
        }*/


        public void SetCharacterMotionSpeed(float newSpeed, float time = 0)
        {
            characterMotionSpeed = newSpeed;
            animator.speed = characterMotionSpeed;
            /*if (currentAttackController != null)
                currentAttackController.AttackMotionSpeed(newSpeed);*/
            if (time > 0 && this.gameObject.activeInHierarchy == true)
            {
                if (motionSpeedCoroutine != null)
                    StopCoroutine(motionSpeedCoroutine);
                motionSpeedCoroutine = MotionSpeedCoroutine(time);
                StartCoroutine(motionSpeedCoroutine);
            }
        }


        private IEnumerator MotionSpeedCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            characterMotionSpeed = 1;//characterStat.GetMotionSpeed();
            animator.speed = characterMotionSpeed;
            /*if (currentAttackController != null)
                currentAttackController.AttackMotionSpeed(characterMotionSpeed);*/
        }

        #endregion

    } 

} // #PROJECTNAME# namespace