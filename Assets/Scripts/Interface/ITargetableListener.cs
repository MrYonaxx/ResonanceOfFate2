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
    public interface ITargetableListener
    {
        void OnTargetAdd(ITargetable newTarget);
        void OnTargetRemove(ITargetable newTarget);
    } 

} // #PROJECTNAME# namespace