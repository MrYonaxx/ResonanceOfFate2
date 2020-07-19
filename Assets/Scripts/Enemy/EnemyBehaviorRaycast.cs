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
            List<int> characterTargetable = new List<int>();
            for (int i = 0; i < targetable.Count; i++)
            {
                int layerMask = (1 << 13) | (1 << 17);
                RaycastHit hit;
                Vector3 direction = (enemy.CharacterCenter.position - targetable[i].CharacterCenter.position);
                if (!Physics.Raycast(enemy.CharacterCenter.position, direction, out hit, direction.magnitude, layerMask))
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
            if (direction.magnitude < enemy.CharacterStatController.GetMinAimDistance())
            {
                if (isMoving == true)
                {
                    navmeshController.StopMove();
                    isMoving = false;
                }
                return GetAimSpeed(enemy, target);
            }
            navmeshController.MoveToTarget(target.transform);
            if (isMoving == false)
            {
                enemy.CharacterAnimation.PlayTrigger("Move");
                isMoving = true;
            }
            return -enemy.CharacterStatController.GetAimSpeed();
        }


        private float GetAimSpeed(Enemy enemy, Character target)
        {
            if (aimSpeedFix == true)
                return enemy.CharacterStatController.GetAimSpeed();
            return enemy.CharacterStatController.GetAimSpeed(enemy.transform.position, target.transform.position);
        }

        #endregion

    } 

} // #PROJECTNAME# namespace