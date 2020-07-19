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
    // Generic Scriptable Object ?
    // Apparamment faut faire des shenanigan pour que ça marche mais why not

    // Pour faire spawn un ennemi et qu'automatiquement les managers puissent leur donner des instructions
    [CreateAssetMenu(fileName = "GlobalEnemyList", menuName = "System/GlobalEnemyList", order = 1)]
    public class GlobalEnemyList : ScriptableObject
    {
        //!\ S'assurer que Clear la liste au changement de scene /!\
        List<IListListener<EnemyController>> enemyListeners = new List<IListListener<EnemyController>>();
        public List<IListListener<EnemyController>> EnemyListeners
        {
            get { return enemyListeners; }   
        }

        public void AddEnemy(EnemyController enemy)
        {
            for(int i = enemyListeners.Count - 1; i >= 0; i--)
            {
                enemyListeners[i].OnListAdd(enemy);
            }
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            for (int i = enemyListeners.Count - 1; i >= 0; i--)
            {
                enemyListeners[i].OnListRemove(enemy);
            }
        }

        public void AddListener(IListListener<EnemyController> newListener)
        {
            enemyListeners.Add(newListener);
        }

        public void RemoveListener(IListListener<EnemyController> newListener)
        {
            enemyListeners.Remove(newListener);
        }

    }

} // #PROJECTNAME# namespace