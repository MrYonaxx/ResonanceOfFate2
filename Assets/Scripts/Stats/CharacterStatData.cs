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
    public class CharacterStatLine
    {
        [Space]
        [Space]
        [HorizontalGroup("Aye")]
        [VerticalGroup("Aye/Left")]
        [SerializeField]
        [HideLabel]
        private string columnName;
        public string ColumnName
        {
            get { return columnName; }
        }


        [VerticalGroup("Aye/Left")]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        private List<Stat> statDictionary = new List<Stat>();
        public List<Stat> StatDictionary
        {
            get { return statDictionary; }
        }



        [Space]
        [Space]
        [HorizontalGroup("Aye")]
        [VerticalGroup("Aye/Center")]
        [SerializeField]
        [HideLabel]
        private string columnNameCenter;
        public string ColumnNameCenter
        {
            get { return columnNameCenter; }
        }

        [VerticalGroup("Aye/Center")]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        private List<Stat> statDictionary2 = new List<Stat>();
        public List<Stat> StatDictionary2
        {
            get { return statDictionary2; }
        }



        [Space]
        [Space]
        [HorizontalGroup("Aye")]
        [VerticalGroup("Aye/Right")]
        [SerializeField]
        [HideLabel]
        private string columnNameRight;
        public string ColumnNameRight
        {
            get { return columnNameRight; }
        }
        [VerticalGroup("Aye/Right")]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        private List<Stat> statDictionary3 = new List<Stat>();
        public List<Stat> StatDictionary3
        {
            get { return statDictionary3; }
        }





        public CharacterStatLine(string columnName1, string columnName2, string columName3)
        {
            columnName = columnName1;
            columnNameCenter = columnName2;
            columnNameRight = columName3;

            statDictionary = new List<Stat>();
            statDictionary2 = new List<Stat>();
            statDictionary3 = new List<Stat>();
        }



    }




    [CreateAssetMenu(fileName = "StatDictionary", menuName = "StatDictionary", order = 1)]
    public class CharacterStatData: ScriptableObject
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        private List<CharacterStatLine> characterStatLines;
        public List<CharacterStatLine> CharacterStatLines
        {
            get { return characterStatLines; }
        }

        [Space]
        [Space]
        [Space]
        [ReadOnly]
        [SerializeField]
        private List<string> statNames = new List<string>();
        public List<string> StatNames
        {
            get { return statNames; }
        }


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        // Plein de truc pas logique mais c'est juste une histoire d'arrangement dans l'editeur
        [OnInspectorGUI]
        private void CreateStatList()
        {
            statNames.Clear();
            for (int i = 0; i < characterStatLines.Count; i++)
            {
                for (int j = 0; j < characterStatLines[i].StatDictionary.Count; j++)
                    statNames.Add(characterStatLines[i].StatDictionary[j].StatName);
                for (int j = 0; j < characterStatLines[i].StatDictionary2.Count; j++)
                    statNames.Add(characterStatLines[i].StatDictionary2[j].StatName);
                for (int j = 0; j < characterStatLines[i].StatDictionary3.Count; j++)
                    statNames.Add(characterStatLines[i].StatDictionary3[j].StatName);
            }
        }

        public Stat GetCustomStat(int index)
        {
            for (int i = 0; i < characterStatLines.Count; i++)
            {
                if (index < characterStatLines[i].StatDictionary.Count)
                    return characterStatLines[i].StatDictionary[index];
                else
                    index -= characterStatLines[i].StatDictionary.Count;


                if (index < characterStatLines[i].StatDictionary2.Count)
                    return characterStatLines[i].StatDictionary2[index];
                else
                    index -= characterStatLines[i].StatDictionary2.Count;

                if (index < characterStatLines[i].StatDictionary3.Count)
                    return characterStatLines[i].StatDictionary3[index];
                else
                    index -= characterStatLines[i].StatDictionary3.Count;
            }
            return null;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace