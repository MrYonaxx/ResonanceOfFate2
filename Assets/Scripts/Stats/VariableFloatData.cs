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
        /*[SerializeField]
        private string variableName;*/

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
        [TextArea(3, 10)]
        private string memo;

        private List<string> callHistoric;

        public void Reset()
        {
            var = baseVar;
            callHistoric.Clear(); // Idéalement ne pas clear la liste quand faudra debug de manière vénèr et juste empêcher d'ajouter a l'infini 
        }

        public void Set(float value)
        {
            var = value;
            callHistoric.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " : =" + value);
        }

        public void Increment()
        {
            var++;
            callHistoric.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " : +1");
        }

        public void Decrement()
        {
            var--;
            callHistoric.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " : -1");
        }


    } 

} // #PROJECTNAME# namespace