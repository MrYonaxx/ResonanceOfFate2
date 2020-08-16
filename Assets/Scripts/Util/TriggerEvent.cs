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
    public class TriggerEvent: MonoBehaviour
    {
        [SerializeField]
        UnityEvent triggerEvent;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                triggerEvent.Invoke();
                this.gameObject.SetActive(false);
            }
        }

    } 

} // #PROJECTNAME# namespace