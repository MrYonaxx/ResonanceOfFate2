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
    public class CharacterTriAttack: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        CharacterMovement characterMovement;
        [SerializeField]
        CharacterAnimation characterAnimation;
        [SerializeField]
        AfterImageEffect characterAfterImage;
        [SerializeField]
        RotationToCamera characterRotation;

        [Title("Parameter")]
        [SerializeField]
        float speed = 2f;


        [SerializeField]
        Transform debugMarker;
        [SerializeField]
        AudioClip jumpClip;
        [SerializeField]
        AudioClip landClip;

        Transform characterTransform;


        List<Vector3> positions = new List<Vector3>();

        private int idAttacker = 0; // Utilisé pour reconnaitre l'attaquant
        public int IdAttacker
        {
            get { return idAttacker; }
            set { idAttacker = value; }
        }


        float t = 0f; // Utilisé pour dessiner les jauges
        float time = 0;

        float currentDistance = 0;
        float totalDistance = 0;

        Vector3 destination;
        Vector3 direction;
        Vector3 directionNormalize;


        bool isTriAttacking = false;
        public bool IsTriAttacking
        {
            get { return isTriAttacking; }
        }

        bool isJumping = false;
        public bool IsJumping
        {
            get { return isJumping; }
        }

        float timeJump = 0f;
        Vector3 startJump;
        Vector3 endJump;
        Vector3 heightJump;

        Vector3 actualJumpPosition;
        Vector3 previousJumpPosition;


        List<float> intersectionTime = new List<float>();

        public delegate void EndAction(int id);
        public event EndAction OnEndAction;

        public delegate void TimeAction(float T, float totalT);
        public event TimeAction OnTimeChanged;

        public delegate void IntersectionAction();
        public event IntersectionAction OnIntersection;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        private Vector2 PositionToVector2()
        {
            return new Vector2(characterTransform.position.x, characterTransform.position.z);
        }
        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        private void Awake()
        {
            characterTransform = GetComponent<Transform>();
        }

        public void AddTriAttackPosition(Vector3 pos)
        {
            positions.Add(pos);
        }
        public void AddIntersectionTime(float t)
        {
            intersectionTime.Add(t);
        }

        private void CalculateTotalTime()
        {
            currentDistance = 0;
            totalDistance = 0;
            Vector2 startPos = PositionToVector2();
            for (int i = 0; i < positions.Count; i++)
            {
                Vector2 dir = new Vector2(positions[i].x, positions[i].z) - startPos;
                totalDistance += (dir.magnitude);
                startPos = new Vector2(positions[i].x, positions[i].z);
            }
        }

        public void StartTriAttack()
        {
            if (positions.Count <= 0)
                return;
            if(isTriAttacking == false) // FirstCall
            {
                isTriAttacking = true;
                AudioManager.Instance.SwitchToBattle(true);
                CalculateTotalTime();
                characterAnimation.State = CharacterState.Dash;
            }
            isJumping = false;
            destination = positions[0];
            positions.RemoveAt(0);

            direction = destination - characterTransform.position;
            directionNormalize = direction.normalized;
            time = (direction.magnitude / (speed * Time.deltaTime)) / 60f;// Pas framerate machin < ------------------ Il est là le problème
            if(debugMarker != null)
                debugMarker.transform.position = destination;

            currentDistance += (new Vector2(destination.x, destination.z) - PositionToVector2()).magnitude;
            characterAfterImage.StartAfterImage();

            StartCoroutine(TriAttackCoroutine());
        }

        //   U P D A T E
        private IEnumerator TriAttackCoroutine()
        {
            characterMovement.StartupMove();
            t = 0f;
            while (t < time)
            {
                t += (Time.deltaTime * characterAnimation.GetMotionSpeed());
                if (isJumping == true)
                    UpdateJumpTriAttack();
                else
                    characterMovement.MoveCharacterWorld(directionNormalize.x, directionNormalize.z, speed); // Pas framerate machin < ------------------ Il est là le problème
                if (OnTimeChanged != null) OnTimeChanged.Invoke(totalDistance - (currentDistance - (new Vector2(destination.x, destination.z) - PositionToVector2()).magnitude), totalDistance);
                CheckIntersection();
                yield return null;
            }
            EndTriAttack();
        }

        //   E N D
        private void EndTriAttack()
        {
            AudioManager.Instance.PlaySound(landClip);
            characterMovement.CharacterDirection.ShowDirectionSprite();
            characterMovement.StopJump();
            characterMovement.HasGravity = true;
            characterRotation.RotationConstraint(true);
            if (positions.Count != 0) // On repart
            {
                StartTriAttack();
            }
            else // On s'arrête
            {
                isTriAttacking = false;
                characterMovement.EndMove();
                if (OnEndAction != null) OnEndAction.Invoke(idAttacker);
            }
        }







        private void CheckIntersection()
        {
            for(int i = 0; i < intersectionTime.Count; i++)
            {
                if(t / time > intersectionTime[i])
                {
                    if (OnIntersection != null) OnIntersection.Invoke();
                    intersectionTime.RemoveAt(i);
                    i -= 1;
                }
            }
        }





        public void Jump()
        {
            if (isJumping == true)
                return;
            characterRotation.RotationConstraint(false);
            characterAfterImage.EndAfterImage();
            time += 1; // temps additionnel du saut
            isJumping = true;
            timeJump = t;

            // Jump Direction
            direction = destination - characterTransform.position;

            // Jump Bezier point
            startJump = characterTransform.position;
            heightJump = characterTransform.position + (direction * 0.3f) + new Vector3(0,7,0);
            endJump = destination;

            previousJumpPosition = startJump;

            AudioManager.Instance.PlaySound(jumpClip);
            characterAnimation.PlayTrigger("Jump");
            characterMovement.CharacterDirection.HideDirectionSprite();
            characterMovement.Jump(0);
            characterMovement.HasGravity = false;
        }

        private void UpdateJumpTriAttack()
        {
            actualJumpPosition = BezierCurve((t - timeJump) / (time - timeJump), startJump, heightJump, endJump);
            Vector3 jumpMovement = actualJumpPosition - previousJumpPosition;

            characterMovement.MoveCharacterManual(jumpMovement.x,   jumpMovement.y,   jumpMovement.z);
            previousJumpPosition = actualJumpPosition;
        }

        private Vector3 BezierCurve(float time, Vector3 pos0, Vector3 pos1, Vector3 pos2)
        {
            if (time > 1)
                time = 1;
            float a = 1 - time;
            Vector3 bezierPoint = (a * a * pos0) + (2 * a * time * pos1) + (time * time * pos2);
            return bezierPoint;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace