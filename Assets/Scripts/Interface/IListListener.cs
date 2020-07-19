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
    public interface IListListener <T>
    {
        void OnListAdd(T newTarget);
        void OnListRemove(T newTarget);
    } 

} // #PROJECTNAME# namespace