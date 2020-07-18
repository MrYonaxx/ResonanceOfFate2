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
    public class DestroyAnimation: MonoBehaviour
    {
        [SerializeField]
        GameObject specificObject;


        public void Destroy()
        {
            Destroy(this.gameObject);
        }
        public void DestroySpecific()
        {
            Destroy(specificObject);
        }
    } 

} // #PROJECTNAME# namespace