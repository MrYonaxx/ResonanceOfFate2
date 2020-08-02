/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class CineEvent: MonoBehaviour
    {
        [SerializeField]
        UnityEvent eventCine;

        private void OnEnable()
        {
            eventCine.Invoke();
        }
    } 

} // #PROJECTNAME# namespace