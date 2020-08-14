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
    public enum Logic
    {
        Equal,
        Greater,
        Lower,
        GreaterEqual,
        LowerEqual,
        NotEqual,
    }

    [System.Serializable]
    public class ConditionVariableFloat
    {
        [HideLabel]
        [SerializeField]
        [HorizontalGroup]
        VariableFloatData variable;

        [HideLabel]
        [SerializeField]
        [HorizontalGroup]
        Logic logic;

        [HideLabel]
        [SerializeField]
        [HorizontalGroup]
        float conditionValue;

        public bool CheckCondition()
        {
            switch(logic)
            {
                case Logic.Equal:
                    return (variable.Var == conditionValue);
                case Logic.Greater:
                    return (variable.Var > conditionValue);
                case Logic.Lower:
                    return (variable.Var < conditionValue);
                case Logic.GreaterEqual:
                    return (variable.Var >= conditionValue);
                case Logic.LowerEqual:
                    return (variable.Var <= conditionValue);
                case Logic.NotEqual:
                    return (variable.Var != conditionValue);

            }
            return false;
        }
    }

    [System.Serializable]
    public class ConditionEventFloat
    {
        [SerializeField]
        [HideLabel]
        ConditionVariableFloat conditionVariable;
        [SerializeField]
        UnityEvent unityEvent;

        public bool CheckCondition()
        {
            if(conditionVariable.CheckCondition() == true)
            {
                unityEvent.Invoke();
                return true;
            }
            return false;
        }
    }

    public class ConditionEvent: MonoBehaviour
    {
        [SerializeField]
        List<ConditionEventFloat> conditions = new List<ConditionEventFloat>();



        private void Awake()
        {
            CheckConditions();
        }

        public void CheckConditions()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                /*if (conditions[i].CheckCondition() == true)
                {
                }*/
                conditions[i].CheckCondition();
            }
        }

    } 

} // #PROJECTNAME# namespace