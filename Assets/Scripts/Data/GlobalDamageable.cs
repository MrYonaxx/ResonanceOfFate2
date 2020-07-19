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

    [CreateAssetMenu(fileName = "GlobalDamageable", menuName = "System/GlobalDamageable", order = 1)]
    public class GlobalDamageable: ScriptableObject
    {
        List<IListListener<IDamageable>> listeners = new List<IListListener<IDamageable>>();

        public void AddDamageable(IDamageable target)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnListAdd(target);
            }
        }

        public void RemoveDamageable(IDamageable target)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnListRemove(target);
            }
        }


        public void AddListener(IListListener<IDamageable> newListener)
        {
            listeners.Add(newListener);
        }

        public void RemoveListener(IListListener<IDamageable> newListener)
        {
            listeners.Remove(newListener);
        }
    } 

} // #PROJECTNAME# namespace