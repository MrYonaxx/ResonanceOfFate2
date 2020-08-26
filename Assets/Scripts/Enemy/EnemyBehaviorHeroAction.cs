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
        CharacterHeroAction heroActionManager = null;
        [SerializeField]
        TriAttackManager triAttackManager = null;

        [SerializeField]
        TargetController targetController = null;
        [SerializeField]
        bool targetNearest = false;

        Vector3 destination;

        public override Character SelectTarget(Enemy enemy)
        {
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
                Debug.Log("J'ai hit un mur");
                count += 1;
            }
            Debug.Log("c dead");
            return false;

        }


        public override void PauseBehavior()
        {
            heroActionManager.ShowCursor(true);
            //isMoving = false;
            //navmeshController.StopMove();
        }
        public override void ResumeBehavior()
        {
            heroActionManager.ShowCursor(false);
            //triAttackManager.StartTriAttack()
        }

        public override void InterruptBehavior()
        {
            heroActionManager.ShowCursor(true);
            //isMoving = false;
            navmeshController.StopMove();
        }

        public override float UpdateBehavior(Enemy enemy, Character target)
        {
            //navmeshController.MoveToTarget(destination);
            return enemy.CharacterStatController.GetAimSpeed();
            /*Vector3 direction = (enemy.CharacterCenter.position - target.CharacterCenter.position);
            if (direction.magnitude < enemy.CharacterStatController.GetMinAimDistance())
            {
                if (isMoving == true)
                {
                    navmeshController.StopMove();
                    isMoving = false;
                }
                return enemy.CharacterStatController.GetAimSpeed();
            }
            navmeshController.MoveToTarget(destination);
            if (isMoving == false)
            {
                enemy.CharacterAnimation.PlayTrigger("Move");
                isMoving = true;
            }
            return -enemy.CharacterStatController.GetAimSpeed();*/
        }

    } 

} // #PROJECTNAME# namespace