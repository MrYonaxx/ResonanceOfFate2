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
    public class EnemyBehaviorAoE: EnemyBehavior
    {
        [Space]
        [SerializeField]
        TargetController targetController = null;
        [SerializeField]
        float targetMinDistance = 2f;

        [SerializeField]
        GameObject aoeIndicator;
        [SerializeField]
        AnimationClip animAim;

        public override Character SelectTarget(Enemy enemy)
        {
            List<Character> targetable = targetController.GetTarget();
            float distance = 0;
            for (int i = targetable.Count - 1; i >= 0; i--)
            {
                distance = Vector3.Distance(targetable[i].transform.position, enemy.transform.position);
                if (distance < targetMinDistance)
                {
                    targetable.RemoveAt(i);
                }
            }
            if (targetable.Count == 0)
                return null;
            int rand = Random.Range(0, targetable.Count);
            aoeIndicator.SetActive(true);
            aoeIndicator.transform.position = targetable[rand].transform.position + new Vector3(0,0.01f,0);
            attackController.SetLockPosition(targetable[rand].transform.position);
            enemy.CharacterAnimation.PlayAnim(animAim.name);
            return targetable[rand];
        }

        public override void PauseBehavior()
        {
        }
        public override void ResumeBehavior()
        {

        }

        public override void InterruptBehavior()
        {
            aoeIndicator.SetActive(false);
        }

        public override float UpdateBehavior(Enemy enemy, Character target)
        {
            return enemy.CharacterStatController.GetAimSpeed();
        }

    } 

} // #PROJECTNAME# namespace