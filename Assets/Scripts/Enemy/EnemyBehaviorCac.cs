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
    public class EnemyBehaviorCac: EnemyBehavior
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Space]
        [SerializeField]
        EnemyNavmeshController navmeshController = null;
        [SerializeField]
        TargetController targetController = null;
        [SerializeField]
        bool targetNearest = false;


        bool isMoving = false;

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
                return targetable[nearestIndex];
            }
            else
            {
                return targetable[Random.Range(0, targetable.Count)];
            }
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
            /*if(target.CharacterDamage.IsDead == true) // placebo
            {
                target = SelectTarget(enemy);
                return -9999;
            }*/
            Vector3 direction = (enemy.CharacterCenter.position - target.CharacterCenter.position);
            if (direction.magnitude < enemy.CharacterStatController.GetMinAimDistance())
            {
                if (isMoving == true)
                {
                    navmeshController.StopMove();
                    isMoving = false;
                }
                return enemy.CharacterStatController.GetAimSpeed();
            }
            navmeshController.MoveToTarget(target.transform);
            if (isMoving == false)
            {
                enemy.CharacterAnimation.PlayTrigger("Move");
                isMoving = true;
            }
            return -enemy.CharacterStatController.GetAimSpeed();
        }


        #endregion

    } 

} // #PROJECTNAME# namespace