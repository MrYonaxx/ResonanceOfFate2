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
    [CreateAssetMenu(fileName = "VariableData", menuName = "VariableData", order = 1)]
    public class VariableFloatData : ScriptableObject
    {
        [Title("Variable initial Value")]
        [SerializeField]
        private string variableName;

        [SerializeField]
        private float baseVar;
        public float BaseVar
        {
            get { return baseVar; }
        }

        [Title("Runtime Value")]
        [SerializeField]
        private float var;
        public float Var
        {
            get { return var; }
            set { var = value; }
        }

        [Title("Variable Memo")]
        [SerializeField]
        [TextArea(3, 3)]
        private string memo;
    } 

} // #PROJECTNAME# namespace