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
        float heroActiontargetRange = 3;
        [SerializeField]
        float heroActionLength = 5;

        [SerializeField]
        EnemyHeroAction heroActionManager = null;
        [SerializeField]
        AfterImageEffect afterImageEffect = null;
        [SerializeField]
        TriAttackManager triAttackManager = null;

        [SerializeField]
        TargetController targetController = null;
        [SerializeField]
        bool targetNearest = false;

        Vector3 destination;
        bool isMoving = false;

        public override Character SelectTarget(Enemy enemy)
        {
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
            int layerMask = (1 << 13) | (1 << 17);
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
            canInterruptPlayer = true;
            afterImageEffect.EndAfterImage();
            heroActionManager.SetCursor(destination);
            heroActionManager.ShowCursor(true);

            isMoving = false;
            navmeshController.StopMove();
        }

        public override void ResumeBehavior()
        {
            heroActionManager.Desactivate();
            afterImageEffect.StartAfterImage();
        }

        public override void InterruptBehavior()
        {
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
            heroActionManager.SetCursor(destination);
            navmeshController.MoveToTarget(destination);
            if (isMoving == false)
            {
                enemy.CharacterAnimation.PlayTrigger("Move");
                isMoving = true;
            }
            if (Vector3.Distance(enemy.transform.position, destination) <= 1.2f)
            {
                InterruptBehavior();
                interrupt = true;
            }
            return enemy.CharacterStatController.GetAimSpeed();
        }

        public override float UpdatePausedBehavior(Enemy enemy, Character target)
        {
            //canInterruptPlayer = !heroActionManager.CheckPlayerIntersections();
            heroActionManager.CheckPlayerIntersections();
            return 0;
        }

        public override bool EndBehavior(Enemy enemy, Character target)
        {
            afterImageEffect.EndAfterImage();
            return false;
        }

    } 

} // #PROJECTNAME# namespace