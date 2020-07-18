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
    public class VariableFloat
    {
        [HideLabel]
        [SerializeField]
        VariableFloatData variableData;

        [HideIf("variableData")]
        [SerializeField]
        private float customValue;

        public float Value
        {
            get { 
                    if (variableData == null)
                    {
                        return customValue;
                    }
                    return variableData.Var;
                }
        }

    } 

} // #PROJECTNAME# namespace