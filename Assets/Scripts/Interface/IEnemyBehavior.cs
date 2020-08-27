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
    public interface IEnemyBehavior
    {
        WeaponData GetWeaponData();
        AttackController GetAttackController();



        Character SelectTarget(Enemy enemy);

        void PauseBehavior();
        void ResumeBehavior();


        float UpdateBehavior(Enemy enemy, Character target, out bool interrupt);

        float UpdatePausedBehavior(Enemy enemy, Character target);

        bool EndBehavior(Enemy enemy, Character target);

    } 

} // #PROJECTNAME# namespace