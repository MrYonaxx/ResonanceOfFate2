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
    [CreateAssetMenu(fileName = "SOVariableDatabase", menuName = "System/SOVariableDatabase", order = 1)]
    public class GameVariableSODatabase: GameVariableDatabase
    {
        [AssetList(AutoPopulate = true)]
        [SerializeField]
        private List<VariableFloatData> variableDatabase = new List<VariableFloatData>();
        public List<VariableFloatData> VariableDatabase
        {
            get { return variableDatabase; }
        }


        public override void ResetDatabase()
        {
            for (int i = 0; i < variableDatabase.Count; i++)
            {
                variableDatabase[i].Reset();
            }
        }


        public override List<GameVariable> SaveVariables()
        {
            List<GameVariable> res = new List<GameVariable>(variableDatabase.Count);
            for (int i = 0; i < variableDatabase.Count; i++)
            {
                res.Add(new GameVariable(variableDatabase[i].name, variableDatabase[i].Var));
            }
            return res;
        }




        // Plus lent que Load variable mais au cas où des trucs bougent
        public override void LoadVariableSafe(List<GameVariable> savedVariables)
        {
            for (int i = 0; i < savedVariables.Count; i++)
            {
                for (int j = 0; j < variableDatabase.Count; j++)
                {
                    if (variableDatabase[j].name.Equals(savedVariables[i].VariableName))
                    {
                        variableDatabase[j].Var = savedVariables[i].VariableValue;
                    }
                }
            }
        }


        public override void LoadVariable(List<GameVariable> savedVariables)
        {
            if (savedVariables.Count != variableDatabase.Count) //!\ Ne couvre pas le cas ou les variables ont changé d'ordre
            {
                LoadVariableSafe(savedVariables);
                return;
            }

            for (int i = 0; i < savedVariables.Count; i++)
            {
                variableDatabase[i].Var = savedVariables[i].VariableValue;
            }
        }

    } 

} // #PROJECTNAME# namespace