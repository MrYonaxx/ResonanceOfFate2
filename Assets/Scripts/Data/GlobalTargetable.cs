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

    [CreateAssetMenu(fileName = "GlobalTargetable", menuName = "System/GlobalTargetable", order = 1)]
    public class GlobalTargetable: ScriptableObject
    {

        //!\ S'assurer que Clear la liste au changement de scene /!\
        List<IListListener<ITargetable>> targetableListeners = new List<IListListener<ITargetable>>();

        //!\ S'assurer que Clear la liste au changement de scene /!\
        List<ITargetable> targetsList = new List<ITargetable>();
        public List<ITargetable> TargetsList
        {
            get { return targetsList; }
        }

        List<ITargetable> targetsListEnemy = new List<ITargetable>();
        public List<ITargetable> TargetsListEnemy
        {
            get { return targetsListEnemy; }
        }




        public void AddTargetable(ITargetable target, bool isEnemy = false)
        {
            targetsList.Add(target);
            for(int i = targetableListeners.Count - 1; i >= 0; i--)
            {
                targetableListeners[i].OnListAdd(target);
            }
            if (isEnemy == true)
                targetsListEnemy.Add(target);
        }

        public void RemoveTargetable(ITargetable target, bool isEnemy = false)
        {
            targetsList.Remove(target);
            for (int i = targetableListeners.Count - 1; i >= 0; i--)
            {
                targetableListeners[i].OnListRemove(target);
            }
            if (isEnemy == true)
                targetsListEnemy.Remove(target);
        }


        public void AddListener(IListListener<ITargetable> newListener)
        {
            targetableListeners.Add(newListener);
        }

        public void RemoveListener(IListListener<ITargetable> newListener)
        {
            targetableListeners.Remove(newListener);
        }

    } 

} // #PROJECTNAME# namespace