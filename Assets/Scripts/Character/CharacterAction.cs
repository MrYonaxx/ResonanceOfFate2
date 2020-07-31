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
    public class CharacterAction: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        SpriteRenderer user;
        [SerializeField]
        CharacterDirection direction;
        [SerializeField]
        CharacterAnimation characterAnimation;

        [Space]
        [SerializeField]
        AttackController attackControllerIdle;
        [SerializeField]
        AttackController attackControllerDash;
        [SerializeField]
        AttackController attackControllerDashForward;
        [SerializeField]
        AttackController attackControllerJump;

        AttackController currentAttack = null;

        public delegate void EndAction();
        public event EndAction OnEndAction;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public bool isAttacking()
        {
            return (currentAttack != null);
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        public void StartShoot(AttackData attackData, Transform target)
        {
            switch (characterAnimation.State)
            {
                case CharacterState.Idle:
                    Action(attackData, attackControllerIdle, target);
                    break;
                case CharacterState.Dash:
                    if(CalculateAngle(target) < 15f)
                        Action(attackData, attackControllerDashForward, target);
                    else
                        Action(attackData, attackControllerDash, target);
                    break;
                case CharacterState.Jump:
                    Action(attackData, attackControllerJump, target);
                    break;
            }
        }

        private void Action(AttackController attack, Transform target)
        {
            //user.gameObject.SetActive(false);
            user.enabled = false;
            user.transform.position += new Vector3(0, 5000, 0);
            attack.gameObject.SetActive(true);
            attack.CreateAttack(target);
            attack.SetDirection(direction.GetDirectionInt());
            currentAttack = attack;
            currentAttack.OnEndAction += ActionEnd;
        }

        public void Action(AttackData data, AttackController attack, Transform target)
        {
            Action(attack, target);
            currentAttack.SetAttackData(data);
        }

        public void ActionEnd()
        {
            //user.gameObject.SetActive(true);
            user.enabled = true;
            user.transform.position -= new Vector3(0, 5000, 0);
            currentAttack.OnEndAction -= ActionEnd;
            currentAttack = null;
            if(OnEndAction != null) OnEndAction.Invoke();
        }


        public void ForceStopAction()
        {
            if(currentAttack != null)
                currentAttack.StopAttack();
        }


        private float CalculateAngle(Transform target)
        {
            Vector2 directionCharacter = new Vector2(-direction.GetDirection().up.x, -direction.GetDirection().up.z);
            Vector2 directionTarget = new Vector2(transform.position.x, transform.position.z) - new Vector2(target.position.x, target.position.z);
            return Vector2.Angle(directionCharacter, directionTarget);
        }

        private void OnDestroy()
        {
            if(currentAttack != null)
                currentAttack.OnEndAction -= ActionEnd;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace