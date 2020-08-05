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
    public class CharacterMovement: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        GlobalCamera globalCamera;
        [SerializeField]
        CharacterController characterController;
        public CharacterController CharacterController
        {
            get { return characterController; }
        }

        [SerializeField]
        CharacterDirection characterDirection;
        public CharacterDirection CharacterDirection
        {
            get { return characterDirection; }
        }
        [SerializeField]
        CharacterAnimation characterAnimation;
        public CharacterAnimation CharacterAnimation
        {
            get { return characterAnimation; }
        }

        [Title("Parameter")]
        [SerializeField]
        float speed = 2;
        public float Speed
        {
            get { return speed; }
        }

        [SerializeField]
        float gravity = 2;
        [SerializeField]
        float startAcceleration = 1;
        /*[SerializeField]
        float jumpImpulse = 10;*/
        [Title("Rotation")]
        [SerializeField]
        float rotationSpeed = 100;
        public float RotationSpeed
        {
            get { return rotationSpeed; }
        }

        [Title("Animation")]
        [SerializeField]
        AnimationClip startMoveAnimation;
        [SerializeField]
        AnimationClip endMoveAnimation;
        [SerializeField]
        AnimationClip jumpAnimation;


        float speedAcceleration = 1;
        Vector2 previousInput;

        bool canInput = true;
        bool isMoving = false;
        bool isJumping = false;

        bool hasGravity = true;
        public bool HasGravity
        {
            get { return hasGravity; }
            set { hasGravity = value; }
        }


        public bool CanInput
        {
            get { return canInput; }
        }


        float speedY = 0;
        public float SpeedY
        {
            get { return speedY; }
        }


        public event Action OnCollisionWall;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }



        public bool IsGrounded()
        {
            return characterController.isGrounded;
        }

        public Vector3 Velocity()
        {
            return characterController.velocity;
        }

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */


        private void Update()
        {
            ApplyGravity();
        }

        public virtual void ApplyGravity()
        {
            if (hasGravity == false)
                return;
            /*if (characterController.isGrounded == true)
            {
                characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
            }*/
            else if (characterController.isGrounded == false && isJumping == false)
            {
                characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
            }
            else if (characterController.isGrounded == true && isJumping == true)
            {
                speedY = 0;
                isJumping = false;
                characterAnimation.State = CharacterState.Idle;
            }
            else if(isJumping == true)
            {
                speedY -=(6 * Time.deltaTime) * characterAnimation.GetMotionSpeed();
                characterController.Move(new Vector3(0, (speedY * characterAnimation.GetMotionSpeed()) * Time.deltaTime, 0));
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.transform.CompareTag("Wall"))
            {
                if (hit.normal.y <= 0.01f && hit.normal.y >= -0.01f)
                {
                    if (OnCollisionWall != null) OnCollisionWall.Invoke();
                    Debug.Log(gameObject.name + " " + hit.gameObject.name + " " + hit.normal);
                }
            }
            if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player"))
            {
                if(transform.position.y > hit.collider.bounds.center.y)
                {
                    Debug.Log("Atcha");
                    MoveCharacterManual((hit.collider.bounds.size.x * 0.5f) + hit.controller.radius + hit.controller.skinWidth, 0, (hit.collider.bounds.size.z * 0.5f) + hit.controller.radius + hit.controller.skinWidth);
                }
            }
        }

        /// <summary>
        /// Move Character relative to world
        /// </summary>
        /// <param name="directionX"></param>
        /// <param name="directionZ"></param>
        public void MoveCharacterWorld(float directionX, float directionZ, float moveSpeed = -1)
        {
            if (moveSpeed == -1)
                moveSpeed = speed;
            Vector3 move = new Vector3(directionX, 0, directionZ);
            move.Normalize();
            move *= moveSpeed;
            characterController.Move((move * characterAnimation.GetMotionSpeed()) * Time.deltaTime);

            if (move.magnitude > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
                characterDirection.GetDirection().rotation = Quaternion.RotateTowards(characterDirection.GetDirection().rotation, rotation, rotationSpeed * characterAnimation.GetMotionSpeed() * Time.deltaTime);
                characterDirection.GetDirection().eulerAngles = new Vector3(90, characterDirection.GetDirection().localEulerAngles.y, characterDirection.GetDirection().localEulerAngles.z);
                //characterDirection.GetDirection().rotation = rotation;

            }
        }

        /// <summary>
        /// Move Character relative to camera position
        /// </summary>
        public void MoveCharacter(float directionX, float directionZ)
        {
            var forward = globalCamera.Forward();
            var right = globalCamera.Right();
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 move = right * directionX + forward * directionZ;
            if(move.magnitude > 1)
                move.Normalize();
            move *= (speed + (startAcceleration * speedAcceleration));
            speedAcceleration = Mathf.Lerp(speedAcceleration, 0, 0.1f);

            characterController.Move((move * characterAnimation.GetMotionSpeed()) * Time.deltaTime);
            if (move.magnitude != 0)
                SetRotation(move);
        }


        public void Move(float directionX, float directionZ)
        {
            if (canInput == false)
                return;
            if(isMoving == false && (directionX != 0 || directionZ != 0))
            {
                StartupMove();
                previousInput = new Vector2(directionX, directionZ);
            }
            /*else if(isMoving == true && Mathf.Abs(Vector2.Distance(previousInput, new Vector2(directionX, directionZ))) > cancelThreshold)
            {
                EndMove();
            }*/
            else if(isMoving == true && directionX == 0 && directionZ == 0)
            {
                EndMove();
            }
            else if (isMoving == true)
            {
                MoveCharacter(directionX, directionZ);
                previousInput = new Vector2(directionX, directionZ);
            }

        }

        public void StartupMove()
        {
            if (startMoveAnimation != null)
            {
                characterAnimation.PlayAnim(startMoveAnimation.name);
                StartCoroutine(StartMoveCoroutine(startMoveAnimation.length));
            }
        }

        private IEnumerator StartMoveCoroutine(float time)
        {
            canInput = false;
            float t = 0f;
            while (t < time)
            {
                t += (Time.deltaTime * characterAnimation.GetMotionSpeed());
                yield return null;
            }
            isMoving = true;
            canInput = true;
            speedAcceleration = 1;
        }



        public void EndMove()
        {
            if (endMoveAnimation != null)
            {
                characterAnimation.PlayAnim(endMoveAnimation.name);
                StartCoroutine(EndMoveCoroutine(endMoveAnimation.length));
            }
        }

        private IEnumerator EndMoveCoroutine(float time)
        {
            speedAcceleration = 0;
            canInput = false;
            float t = 0f;
            while (t < time)
            {
                MoveCharacter(Mathf.Lerp((speed * previousInput.x) * 0.6f, 0, t / time), Mathf.Lerp((speed * previousInput.y) * 0.6f, 0, (t / time)));
                t += (Time.deltaTime * characterAnimation.GetMotionSpeed());
                /*previousSpeedX = Mathf.Lerp((speed * previousSpeedX) * 0.6f, 0, t / time);
                previousSpeedZ = Mathf.Lerp((speed * previousSpeedZ) * 0.6f, 0, t / time);*/

                yield return null;
            }
            isMoving = false;
            canInput = true;
            previousInput = new Vector2(0, 0);
        }




        public void ResetSpeedY()
        {
            speedY = 0;
        }


        public void Jump(float jumpValue)
        {
            isJumping = true;
            //if(jumpAnimation != null)
            //    characterAnimation.PlayAnim(jumpAnimation.name);
            speedY += jumpValue;
            characterController.Move(new Vector3(0, speedY * characterAnimation.GetMotionSpeed() * Time.deltaTime, 0));
            characterAnimation.State = CharacterState.Jump;
        }

        public void StopJump()
        {
            isJumping = false;
        }



        /// <summary>
        /// Move Character without Time.deltatime
        /// </summary>
        /// <param name="directionX"></param>
        /// <param name="directionZ"></param>
        public void MoveCharacterManual(float speedX, float speedY, float speedZ, bool updateDirection = false)
        {
            Vector3 move = new Vector3(speedX, speedY, speedZ);
            characterController.Move(move);

            if (updateDirection == true && move.magnitude != 0)
            {
                SetRotation(move);
            }
        }

        private void SetRotation(Vector3 move)
        {
            Quaternion rotation = Quaternion.LookRotation(move, Vector3.up);
            characterDirection.GetDirection().rotation = rotation;
            characterDirection.GetDirection().eulerAngles = new Vector3(90, characterDirection.GetDirection().localEulerAngles.y, characterDirection.GetDirection().localEulerAngles.z);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace