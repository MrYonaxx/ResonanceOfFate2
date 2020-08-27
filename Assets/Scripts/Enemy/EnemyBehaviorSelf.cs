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
    public class EnemyBehaviorSelf: EnemyBehavior
    {

        public override Character SelectTarget(Enemy enemy)
        {
            return enemy;
        }

        public override float UpdateBehavior(Enemy enemy, Character target, out bool interrupt)
        {
            interrupt = false;
            return enemy.CharacterStatController.GetAimSpeed();
        }

    } 

} // #PROJECTNAME# namespace
                            