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
    public class CharacterDamage: MonoBehaviour, IDamageable
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterAnimation characterAnimation;
        [SerializeField]
        CharacterMovement characterMovement;
        [SerializeField]
        ShakeSprite shakeSprite;





        [SerializeField]
        float knockbackTime = 0.5f;
        [SerializeField]
        float recoveryTime = 0.5f;

        [SerializeField]
        string statMass;

        [Title("Body Part")]
        [SerializeField]
        List<BodyPartController> bodyPartControllers;
        public List<BodyPartController> BodyPartControllers
        {
            get { return bodyPartControllers; }
        }

        [Title("Sound (A dégager)")]
        [SerializeField]
        AudioClip airborneClip;


        private CharacterStatController characterStatController;
        public CharacterStatController CharacterStatController
        {
            get { return characterStatController; }
            set { characterStatController = value; }
        }

        [SerializeField]
        private CharacterBodyPartController characterBodyPartController;
        public CharacterBodyPartController CharacterBodyPartController
        {
            get { return characterBodyPartController; }
        }


        float knockbackCurrentTime = 0f;
        float feverTime = 0;

        private bool isKnockback;
        public bool IsKnockback { get { return isKnockback; } }
        private bool isDead;
        public bool IsDead { get { return isDead; } }

        bool airborne = false;
        bool bounce = false;

        private IEnumerator rotationCoroutine;
        private IEnumerator knockbackCoroutine;


        public event HitAction OnHit;
        public event Action OnLaunch;
        public event Action OnSmackdown;
        public event Action OnDead;

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
            characterBodyPartController = new CharacterBodyPartController(characterStatController, bodyPartControllers);
        }

        public void Damage(Vector3 pos, AttackData attackData)
        {
            if (isDead == true)
                return;

            DamageMessage msg = characterBodyPartController.Damage(attackData, characterMovement.CharacterDirection.DirectionTransform);
            //DamageMessage msg = attackData.AttackProcessor.ProcessAttack(attackData.AttackDataStat, characterStatController);
            msg.SetDamagePosition(pos);

            if (OnHit != null) OnHit.Invoke(msg);

            if(characterStatController.Hp <= 0)
            {
                Death(attackData);
                return;
            }
            if(msg.damageRaw + msg.damageScratch > 0)
                shakeSprite.Shake(0.05f, 0.2f);
            if (isKnockback == false && msg.knockback == true)
                Knockback();
            else if (isKnockback == true)
                Knockback();

            if (isKnockback == true && msg.launch == true)
                CheckLaunch(attackData);
        }



        private void Knockback()
        {
            isKnockback = true;
            knockbackCurrentTime = knockbackTime;
            characterAnimation.PlayTrigger("Hit");
            characterAnimation.State = CharacterState.Hit;
            if (characterAnimation.GetMotionSpeed() == 0)
                characterAnimation.SetCharacterMotionSpeed(0.2f);

            if (knockbackCoroutine == null)
            {
                knockbackCoroutine = KnockbackCoroutine();
                StartCoroutine(knockbackCoroutine);
            }
        }

        private void CheckLaunch(AttackData attackData)
        {
            if (characterMovement.IsGrounded() == true && attackData.AttackDataStat.UserPosition.y-1 <= this.transform.position.y)
            {
                if (airborne == false)
                {
                    airborne = true;
                    characterMovement.ResetSpeedY();
                    characterMovement.Jump(5f - characterStatController.GetStat(statMass));
                    if(airborneClip != null)
                        AudioManager.Instance.PlaySound(airborneClip);
                    if (OnLaunch != null) OnLaunch.Invoke();
                }
            }
            else if(airborne == true && bounce == false && attackData.AttackDataStat.UserPosition.y - 1 < this.transform.position.y) // Under the character
            {
                characterMovement.ResetSpeedY();
                characterMovement.Jump(1f);
                if (airborneClip != null)
                    AudioManager.Instance.PlaySound(airborneClip);
            }
            else if (airborne == true && bounce == false && attackData.AttackDataStat.UserPosition.y + 1 >= this.transform.position.y) // Smackdown
            {
                bounce = true;
                if (OnSmackdown != null) OnSmackdown.Invoke();
            }
        }



        private IEnumerator KnockbackCoroutine()
        {
            isKnockback = true;
            while (knockbackCurrentTime > 0)
            {
                if (characterMovement.IsGrounded() == true) // Ground
                {
                    if (airborne == true && bounce == true)
                    {
                        bounce = false;
                        characterMovement.ResetSpeedY();
                        characterMovement.Jump(5f - characterStatController.GetStat(statMass));
                        shakeSprite.Shake(0.05f, 0.2f);
                        //StartRotation();
                    }
                    else if (airborne == true)
                    {
                        knockbackCurrentTime = 0;
                        characterAnimation.State = CharacterState.Idle;
                    }
                    else
                    {
                        knockbackCurrentTime -= (Time.deltaTime * characterAnimation.GetMotionSpeed());
                    }
                }
                else // In air
                {
                    if (bounce == true && characterAnimation.GetMotionSpeed() >= 1)
                    {
                        characterMovement.Jump(-1f);
                    }
                }
                yield return null;
            }
            //StopRotation();
            // Rebond 
            if (airborne == true) 
            {
                characterAnimation.PlayTrigger("Down");
                characterMovement.ResetSpeedY();
                characterMovement.Jump(1.5f);
                while (characterMovement.IsGrounded() == false)
                {
                    yield return null;
                }
                yield return new WaitForSeconds(recoveryTime * 0.8f);
                yield return new WaitForSeconds(recoveryTime * 0.2f);
            }

            // Fin
            airborne = false;
            characterAnimation.PlayTrigger("Idle");
            characterAnimation.State = CharacterState.Idle;
            isKnockback = false;
            knockbackCoroutine = null;
        }



        private void Death(AttackData attackData)
        {
            isDead = true;
            isKnockback = true;
            knockbackCurrentTime = knockbackTime;
            characterAnimation.PlayTrigger("Dead");
            characterAnimation.State = CharacterState.Dead;
            shakeSprite.Shake(0.1f, 0.4f);
            characterAnimation.SetCharacterMotionSpeed(0.2f);
            //characterMovement.CharacterController.enabled = false;

            this.gameObject.layer = 0;
            characterMovement.CharacterController.detectCollisions = false;

            //Vector3 direction = this.transform.position - attackData.AttackDataStat.UserPosition;

            OnDead.Invoke();

            if (knockbackCoroutine != null)
                StopCoroutine(knockbackCoroutine);
            knockbackCoroutine = DeathCoroutine();
            StartCoroutine(knockbackCoroutine);
        }

        private IEnumerator DeathCoroutine()
        {
            float t = 0f;
            characterMovement.Jump(3);
            while( characterMovement.IsGrounded() == false)
            {
                yield return null;
            }
            characterAnimation.PlayTrigger("Down");
            characterMovement.ResetSpeedY();
            characterMovement.Jump(1.5f);
            while (characterMovement.IsGrounded() == false)
            {
                yield return null;
            }
            while (t < 2f)
            {
                this.transform.localScale = Vector3.Lerp(Vector3.one*2, Vector3.zero, t*0.5f);
                t += Time.deltaTime;
                yield return null;
            }
            Destroy(this.gameObject);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace