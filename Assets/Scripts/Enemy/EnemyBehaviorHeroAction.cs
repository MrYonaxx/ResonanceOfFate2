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
    public class EnemyBehaviorHeroAction: EnemyBehavior
    {
        [SerializeField]
        EnemyNavmeshController navmeshController = null;

        [SerializeField]
        EnemyHeroAction heroActionManager = null;
        [SerializeField]
        AfterImageEffect afterImageEffect = null;

        [Title("Parameter")]
        [SerializeField]
        float heroActiontargetRange = 3;
        [SerializeField]
        float heroActionLength = 5;
        [SerializeField]
        bool canJump = false;

        [Title("Jump Parameter")]
        [ShowIf("canJump")]
        [SerializeField]
        float jumpTimeRatio = 0.2f;
        [ShowIf("canJump")]
        [SerializeField]
        bool autoJump = false;

        [Title("Targeting")]
        [SerializeField]
        TargetController targetController = null;
        [SerializeField]
        bool targetNearest = false;
        [SerializeField]
        Transform[] customPositions;

        Vector3 destination;
        bool isMoving = false;
        bool isJumping = false;
        bool canStart = false; // Une action ne peut pas commencer si le jeu n'est pas en mode pause 

        int layerMask = (1 << 13) | (1 << 17);

        public override Character SelectTarget(Enemy enemy)
        {
            canStart = false;
            canInterruptPlayer = true;
            List<Character> targetable = targetController.GetTarget();
            if (targetable.Count == 0)
                return null;
            if (targetNearest == true)
            {
                float distance = 0;
                float minDistance = 999;
                int nearestIndex = 0;
                for (int i = 0; i < targetable.Count; i++)
                {
                    distance = Vector3.Distance(targetable[i].transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestIndex = i;
                    }

                }
                if(SelectDestination(enemy, targetable[nearestIndex].CharacterCenter))
                    return targetable[nearestIndex];
            }
            else
            {
                int rand = Random.Range(0, targetable.Count);
                if (SelectDestination(enemy, targetable[rand].CharacterCenter) == true)
                    return targetable[rand];
            }
            return null;
        }

        private bool SelectDestination(Enemy enemy, Transform target)
        {
            //int layerMask = (1 << 13) | (1 << 17);
            if(customPositions.Length != 0)
            {
                // Eventuellement rajouter un test raycast pour choisir la bonne cover
                destination = customPositions[Random.Range(0, customPositions.Length)].position;
                heroActionManager.SetCursor(destination);
                heroActionManager.ShowCursor(true);
                return true;
            }
            int count = 0;
            while (count < 10)
            {
                Vector3 direction = target.position + new Vector3(Random.Range(-heroActiontargetRange, heroActiontargetRange), 0, Random.Range(-heroActiontargetRange, heroActiontargetRange));
                direction = new Vector3(direction.x - enemy.CharacterCenter.position.x, 0, direction.z - enemy.CharacterCenter.position.z).normalized;
                destination = enemy.CharacterCenter.position + (direction * heroActionLength);
                if (!Physics.Raycast(enemy.CharacterCenter.position, direction, heroActionLength+1, layerMask))
                {
                    heroActionManager.SetCursor(destination);
                    heroActionManager.ShowCursor(true);
                    return true;
                    // Le déplacement est valide il n'y a pas d'obstruction
                }
                count += 1;
            }
            return false;

        }


        public override void PauseBehavior()
        {
            if (isJumping == true)
                return;

            canInterruptPlayer = true;
            afterImageEffect.EndAfterImage();
            heroActionManager.SetCursor(destination);
            heroActionManager.ShowCursor(true);

            isMoving = false;
            navmeshController.StopMove();
        }

        public override void ResumeBehavior()
        {
            if (isJumping == true)
                return;

            heroActionManager.Desactivate();
            afterImageEffect.StartAfterImage();
        }

        public override void InterruptBehavior()
        {
            if (isJumping == true)
            {
                isJumping = false;
                saveEnemy.CharacterMovement.StopJump();
                saveEnemy.CharacterMovement.EndMove();
                saveEnemy.CharacterMovement.HasGravity = true;
            }

            canInterruptPlayer = true;
            afterImageEffect.EndAfterImage();
            heroActionManager.Desactivate();
            heroActionManager.ShowCursor(false);

            isMoving = false;
            navmeshController.StopMove();
        }



        public override float UpdateBehavior(Enemy enemy, Character target, out bool interrupt)
        {
            interrupt = false;
            if (canStart == false)
                return 0;
            if (Vector3.Distance(enemy.transform.position, destination) <= 1.2f && isJumping == false)
            {
                InterruptBehavior();
                interrupt = true;
                return 0;
            }


            // Section Jump
            if (canJump == true) 
            {
                if(autoJump == true)
                {
                    Jump(enemy);
                }
                if (isJumping == true)
                {
                    UpdateJump(enemy, target);
                    return enemy.CharacterStatController.GetAimSpeed();
                }
                else
                {
                    CheckIfNeedJump(enemy);
                    if (isJumping == true)
                        return 0;
                }
            }
            //


            // Section normal
            heroActionManager.SetCursor(destination);
            navmeshController.MoveToTarget(destination);
            if (isMoving == false)
            {
                enemy.CharacterAnimation.PlayTrigger("Move");
                isMoving = true;
            }
            return enemy.CharacterStatController.GetAimSpeed();
            //
        }

        public override float UpdatePausedBehavior(Enemy enemy, Character target)
        {
            canStart = true;
            if (isJumping == true)
            {
                UpdateJump(enemy, target);
                return enemy.CharacterStatController.GetAimSpeed();
            }
            else
            {
                heroActionManager.CheckPlayerIntersections();
                return 0;
            }
        }

        public override bool EndBehavior(Enemy enemy, Character target)
        {
            afterImageEffect.EndAfterImage();
            return false;
        }





        // ================================
        Vector3 startJump;
        Vector3 heightJump;
        Vector3 endJump;
        Vector3 actualJumpPosition;
        Vector3 previousJumpPosition;
        float t = 0f;
        float timeJump = 0f;
        Enemy saveEnemy; // ça c'est nul

        private void CheckIfNeedJump(Enemy enemy)
        {
            Vector3 direction = (destination - enemy.transform.position).normalized;
            //Debug.DrawRay(enemy.CharacterCenter.position, direction - new Vector3(0, 1f, 0), Color.yellow, 2f);
            if (!Physics.Raycast(enemy.CharacterCenter.position, direction - new Vector3(0, 1f, 0), 2f, layerMask))
            {
                heroActionManager.ShowCursor(true);
                Jump(enemy);
                // Le déplacement est valide il n'y a pas d'obstruction
            }
        }

        // Quasi un Ctrl-C de Character Tri attack donc peut etre a refactor
        public void Jump(Enemy enemy)
        {
            if (isJumping == true)
                return;
            saveEnemy = enemy;
            if (isMoving == true)
            {
                isMoving = false;
                navmeshController.StopMove();
            }
            //afterImageEffect.EndAfterImage();
            isJumping = true;


            // Jump Direction
            Vector3 direction = destination - enemy.transform.position;
            t = 0f;
            timeJump = Mathf.Max(1, direction.magnitude * jumpTimeRatio);

            // Jump Bezier point
            startJump = enemy.transform.position;
            heightJump = enemy.transform.position + (direction * 0.3f) + new Vector3(0, 7, 0);
            endJump = destination;

            previousJumpPosition = startJump;

            enemy.CharacterAnimation.PlayTrigger("Jump");
            //enemy.CcharacterMovement.CharacterDirection.HideDirectionSprite();
            enemy.CharacterMovement.Jump(0);
            enemy.CharacterMovement.HasGravity = false;
        }




        private void UpdateJump(Enemy enemy, Character target)
        {
            heroActionManager.SetCursor(destination);
            t += Time.deltaTime * enemy.CharacterAnimation.GetMotionSpeed();
            actualJumpPosition = BezierCurve(t / timeJump, startJump, heightJump, endJump);
            Vector3 jumpMovement = actualJumpPosition - previousJumpPosition;

            enemy.CharacterMovement.MoveCharacterManual(jumpMovement.x, jumpMovement.y, jumpMovement.z);
            previousJumpPosition = actualJumpPosition;

            if(t >= timeJump)
            {
                isJumping = false;
                enemy.CharacterMovement.StopJump();
                enemy.CharacterMovement.EndMove();
                saveEnemy.CharacterMovement.HasGravity = true;
                // Interrupt
            }
        }

        private Vector3 BezierCurve(float time, Vector3 pos0, Vector3 pos1, Vector3 pos2)
        {
            if (time > 1)
                time = 1;
            float a = 1 - time;
            Vector3 bezierPoint = (a * a * pos0) + (2 * a * time * pos1) + (time * time * pos2);
            return bezierPoint;
        }




    } 

} // #PROJECTNAME# namespace