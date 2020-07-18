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
    [CreateAssetMenu(fileName = "GlobalTargetable", menuName = "System/GlobalTargetable", order = 1)]
    public class GlobalTargetable: ScriptableObject
    {

        //!\ S'assurer que Clear la liste au changement de scene /!\
        List<ITargetableListener> targetableListeners = new List<ITargetableListener>();

        //!\ S'assurer que Clear la liste au changement de scene /!\
        List<ITargetable> targetsList = new List<ITargetable>();
        public List<ITargetable> TargetsList
        {
            get { return targetsList; }
        }


        public void AddTargetable(ITargetable target)
        {
            targetsList.Add(target);
            for(int i = targetableListeners.Count - 1; i >= 0; i--)
            {
                targetableListeners[i].OnTargetAdd(target);
            }
        }

        public void RemoveTargetable(ITargetable target)
        {
            targetsList.Remove(target);
            for (int i = targetableListeners.Count - 1; i >= 0; i--)
            {
                targetableListeners[i].OnTargetRemove(target);
            }
        }


        public void AddListener(ITargetableListener newListener)
        {
            targetableListeners.Add(newListener);
        }

        public void RemoveListener(ITargetableListener newListener)
        {
            targetableListeners.Remove(newListener);
        }

    } 

} // #PROJECTNAME# namespace