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
    [System.Serializable]
    public class GameVariable
    {
        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        private string variableName;
        public string VariableName
        {
            get { return variableName; }
        }
        [HorizontalGroup]
        [HideLabel]
        [SerializeField]
        private float variableValue;
        public float VariableValue
        {
            get { return variableValue; }
            set { variableValue = value; }
        }

        public GameVariable(string name, float val)
        {
            variableName = name;
            variableValue = val;
        }

    }


    [CreateAssetMenu(fileName = "VariableDatabase", menuName = "System/VariableDatabase", order = 1)]
    public class GameVariableDatabase: ScriptableObject
    {
        [SerializeField]
        private List<GameVariable> gameVariables = new List<GameVariable>();
        public List<GameVariable> GameVariables
        {
            get { return gameVariables; }
        }



        public virtual void ResetDatabase()
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                gameVariables[i].VariableValue = 0;
            }
        }





        public bool CheckExist(string name)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                if (gameVariables[i].VariableName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddGameVariable(string name, float val)
        {
            gameVariables.Add(new GameVariable(name, val));
        }
        public void RemoveVariable(string name)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                if (gameVariables[i].VariableName.Equals(name))
                {
                    gameVariables.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveLastVariable()
        {
            gameVariables.RemoveAt(gameVariables.Count - 1);
        }


        public float GetValue(string variableName)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {     
                if (gameVariables[i].VariableName.Equals(variableName))
                {
                    return gameVariables[i].VariableValue;
                }        
            }
            Debug.LogError("Variable does not exist");
            return -1;
        }

        public void SetValue(string variableName, float newValue)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                if (gameVariables[i].VariableName.Equals(variableName))
                {
                    gameVariables[i].VariableValue = newValue;
                    return;
                }
            }
            Debug.LogError("Variable does not exist");
        }

        public GameVariable GetVariable(string variableName)
        {
            for (int i = 0; i < gameVariables.Count; i++)
            {
                if (gameVariables[i].VariableName.Equals(variableName))
                {
                    return gameVariables[i];
                }
            }
            Debug.LogError("Variable does not exist");
            return null;
        }



        public virtual List<GameVariable> SaveVariables()
        {         
            return gameVariables;
        }


        // Plus lent que Load variable mais au cas où des trucs bougent
        public virtual void LoadVariableSafe(List<GameVariable> savedVariables)
        {
            for (int i = 0; i < savedVariables.Count; i++)
            {
                for (int j = 0; j < gameVariables.Count; j++)
                {
                    if(gameVariables[j].VariableName.Equals(savedVariables[i].VariableName))
                    {
                        gameVariables[j].VariableValue = savedVariables[i].VariableValue;
                    }
                }
            }
        }


        public virtual void LoadVariable(List<GameVariable> savedVariables)
        {
            if (savedVariables.Count != gameVariables.Count) //!\ Ne couvre pas le cas ou les variables ont changé d'ordre
            {
                LoadVariableSafe(savedVariables);
                return;
            }

            for(int i = 0; i < savedVariables.Count; i++)
            {
                gameVariables[i].VariableValue = savedVariables[i].VariableValue;
            }
        }


    } 

} // #PROJECTNAME# namespace