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
    // Pour faire spawn un ennemi et qu'automatiquement les managers puissent leur donner des instructions
    [CreateAssetMenu(fileName = "GlobalEnemyList", menuName = "System/GlobalEnemyList", order = 1)]
    public class GlobalEnemyList : ScriptableObject
    {
        //!\ S'assurer que Clear la liste au changement de scene /!\
        List<IEnemyListener> enemyListeners = new List<IEnemyListener>();
        public List<IEnemyListener> EnemyListeners
        {
            get { return enemyListeners; }   
        }

        public void AddEnemy(EnemyController enemy)
        {
            for(int i = enemyListeners.Count - 1; i >= 0; i--)
            {
                enemyListeners[i].OnTargetAdd(enemy);
            }
        }

        public void RemoveEnemy(EnemyController enemy)
        {
            for (int i = enemyListeners.Count - 1; i >= 0; i--)
            {
                enemyListeners[i].OnTargetRemove(enemy);
            }
        }

        public void AddListener(IEnemyListener newListener)
        {
            enemyListeners.Add(newListener);
        }

        public void RemoveListener(IEnemyListener newListener)
        {
            enemyListeners.Remove(newListener);
        }

    }

} // #PROJECTNAME# namespace