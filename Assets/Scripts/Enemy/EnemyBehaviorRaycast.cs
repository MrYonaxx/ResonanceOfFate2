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
    public class EnemyBehaviorRaycast: EnemyBehavior
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        EnemyNavmeshController navmeshController = null;
        [SerializeField]
        TargetController targetController;

        bool isMoving = false;

        [Title("Parameter")]
        [SerializeField]
        bool aimSpeedFix = true;

        [HorizontalGroup("A1")]
        [SerializeField]
        bool infiniteAimDistance = false;
        [HorizontalGroup("A1")]
        [SerializeField]
        bool dontCareObstruction = false;

        [HorizontalGroup("A2")]
        [SerializeField]
        bool moveIfObstruction = false;
        [HorizontalGroup("A2")]
        [SerializeField]
        bool stopAimIfObstruction = false;

        [Title("Feedback")]
        [SerializeField]
        Bullet feedbackBullet;
        [SerializeField]
        Vector2 intervalBullet;

        float intervalFeedback;

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

        public override Character SelectTarget(Enemy enemy)
        {
            List<Character> targetable = targetController.GetTarget();
            if (targetable.Count == 0)
                return null;
            if (dontCareObstruction == true)
                return targetable[Random.Range(0, targetable.Count)];
            List<int> characterTargetable = new List<int>();
            for (int i = 0; i < targetable.Count; i++)
            {
                int layerMask = (1 << 13) | (1 << 17);
                //RaycastHit hit;
                Vector3 direction = (enemy.CharacterCenter.position - targetable[i].CharacterCenter.position);
                if (!Physics.Raycast(enemy.CharacterCenter.position, direction, direction.magnitude, layerMask))
                {
                    characterTargetable.Add(i);
                }
            }

            if (characterTargetable.Count == 0) // Toutes les unités sont planqués donc on pif
            {
                return targetable[Random.Range(0, targetable.Count)];
            }
            return targetable[characterTargetable[Random.Range(0, characterTargetable.Count)]];
        }

        public override void PauseBehavior()
        {
            isMoving = false;
            navmeshController.StopMove();
        }
        public override void ResumeBehavior()
        {

        }



        public override float UpdateBehavior(Enemy enemy, Character target)
        {
            Vector3 direction = (enemy.CharacterCenter.position - target.CharacterCenter.position);
            // La cible est à portée
            if (direction.magnitude < enemy.CharacterStatController.GetMinAimDistance())
            {
                if (CheckIfObstruction(enemy, direction) == true) // Obstruction je fais un truc
                {
                    if(stopAimIfObstruction == true)
                    {
                        StopMove();
                        return -enemy.CharacterStatController.GetAimSpeed();
                    }
                    // Sinon on fais le comportement (La cible n'est pas à portée)

                }
                else // Pas d'obstruction donc je vise
                {
                    UpdateFeedback(enemy, target);
                    StopMove();
                    return GetAimSpeed(enemy, target);
                }
            }

            if(infiniteAimDistance == true)
            {
                UpdateFeedback(enemy, target);
                StopMove();
                return GetAimSpeed(enemy, target);
            }

            // La cible n'est pas à portée
            navmeshController.MoveToTarget(target.transform);
            if (isMoving == false)
            {
                enemy.CharacterAnimation.PlayTrigger("Move");
                isMoving = true;
            }
            return -enemy.CharacterStatController.GetAimSpeed();
        }

        private bool CheckIfObstruction(Enemy enemy, Vector3 direction)
        {
            if(moveIfObstruction || stopAimIfObstruction)
            {
                int layerMask = (1 << 13) | (1 << 17);
                //RaycastHit hit;
                if (Physics.Raycast(enemy.CharacterCenter.position, -direction, direction.magnitude, layerMask))
                {
                    return true;
                }
            }
            return false;
        }


        private void StopMove()
        {
            if (isMoving == true)
            {
                navmeshController.StopMove();
                isMoving = false;
            }
        }


        private float GetAimSpeed(Enemy enemy, Character target)
        {
            if (aimSpeedFix == true)
                return enemy.CharacterStatController.GetAimSpeed();
            return enemy.CharacterStatController.GetAimSpeed(enemy.transform.position, target.transform.position);
        }



        private void UpdateFeedback(Enemy enemy, Character target)
        {
            if (feedbackBullet == null)
                return;
            if (target == null)
                return;
            if (target.CharacterAnimation.State == CharacterState.Dash || target.CharacterAnimation.State == CharacterState.Jump)
            {
                intervalFeedback -= Time.deltaTime * enemy.CharacterAnimation.GetMotionSpeed();
                if (intervalFeedback <= 0)
                {
                    feedbackBullet.Play(target.CharacterCenter.transform);
                    intervalFeedback = Random.Range(intervalBullet.x, intervalBullet.y);
                }
            }
        }


        #endregion

    } 

} // #PROJECTNAME# namespace