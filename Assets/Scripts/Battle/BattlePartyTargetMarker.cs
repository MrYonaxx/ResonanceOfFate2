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
    public class BattlePartyTargetMarker : MonoBehaviour, IListListener<EnemyController>
    {

        [SerializeField]
        GlobalEnemyList globalEnemyList;

        [SerializeField]
        BattlePartyManager battlePartyManager;


        private void Awake()
        {
            //enemyHealthBar = new List<EnemyHealthBar>(enemyList.Count);
            globalEnemyList.AddListener(this);
        }
        private void OnDestroy()
        {
            globalEnemyList.RemoveListener(this);
        }

        public void OnListAdd(EnemyController enemy)
        {
            enemy.OnAttackSelected += DrawAttackTarget;
            enemy.OnAttackEnded += DrawAttackUntarget;
            enemy.OnAttackInterrupted += DrawAttackUntarget;
        }
        public void OnListRemove(EnemyController enemy)
        {
            enemy.OnAttackSelected -= DrawAttackTarget;
            enemy.OnAttackEnded -= DrawAttackUntarget;
            enemy.OnAttackInterrupted -= DrawAttackUntarget;
        }

        public void DrawAttackTarget(Character target, bool interrupt) 
        {
            for (int i = 0; i < battlePartyManager.GetParty().Count; i++)
            {
                if(battlePartyManager.GetCharacter(i) == target) // A optimiser peut etre
                {
                    battlePartyManager.PlayerHUDs[i].Target(interrupt);
                }
            }
        }

        public void DrawAttackUntarget(EnemyController enemy, bool interrupt)
        {
            for (int i = 0; i < battlePartyManager.GetParty().Count; i++)
            {
                if (battlePartyManager.GetCharacter(i) == enemy.Target) // A optimiser peut etre
                {
                    battlePartyManager.PlayerHUDs[i].Untarget(interrupt);
                }
            }
        }

    }
}
