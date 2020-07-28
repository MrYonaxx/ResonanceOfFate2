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
    public interface ITargetable
    {
        Transform CharacterCenter
        {
            get;
        }

        CharacterStatController CharacterStatController
        {
            get;
        }

        Transform TargetDirection
        {
            get;
        }

        List<BodyPartController> GetBodyParts();

    } 

} // #PROJECTNAME# namespace