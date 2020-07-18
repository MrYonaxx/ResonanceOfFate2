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
    public class StatController
    {
        // Liste de Stat generic mais c'est de la merde
        // Certe on réécrit rien, mais toute les opérations prennent 1000x plus de temps en complexité en temps.
        [InfoBox("Pour l'editeur")]
        [SerializeField]
        [HideLabel]
        public List<CharacterStatLine> StatEditor;

        [SerializeField]
        [HideLabel]
        public List<Stat> Stats = new List<Stat>();

        [HideInInspector]
        public bool manual = false;

        [OnInspectorGUI]
        //[Button]
        private void OnInspectorGUI()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                return;
            }

            if (statModel == null)
                statModel = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0]));

            if (manual == true) // Whacky as f
            {
                manual = false; 
                return;
            }
            else
                manual = true;

            Stats = new List<Stat>();
            for (int i = 0; i < StatEditor.Count; i++)
            {
                for (int j = 0; j < StatEditor[i].StatDictionary.Count; j++)
                    Stats.Add(StatEditor[i].StatDictionary[j]);
                for (int j = 0; j < StatEditor[i].StatDictionary2.Count; j++)
                    Stats.Add(StatEditor[i].StatDictionary2[j]);
                for (int j = 0; j < StatEditor[i].StatDictionary3.Count; j++)
                    Stats.Add(StatEditor[i].StatDictionary3[j]);
            }

            for (int i = 0; i < statModel.StatNames.Count; i++)
            {
                if(Stats.Count <= i)
                    Stats.Add(new Stat(statModel.GetCustomStat(i).StatName, statModel.GetCustomStat(i).StatValue, false));
                // Check si le dictionnaire à une stat avec le même nom
                if (!Stats[i].StatName.Equals(statModel.StatNames[i]))
                {
                    CheckStatMoving(Stats, i); // L'ancienne stat n'existe plus donc on change le nom
                }
            }
            for (int i = statModel.StatNames.Count; i < Stats.Count; i++)
            {
                CheckStatAfter(Stats, i);
                Stats[i].Modifiable = true;
            }

            SliceStat(Stats);
#endif
        }

        private void CheckStatMoving(List<Stat> list, int iPrev)
        {
            Stat tmp;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StatName.Equals(statModel.StatNames[iPrev]))
                {
                    // Interchange les valeurs
                    tmp = new Stat(list[iPrev].StatName, list[iPrev].StatValue, false);
                    list[iPrev] = new Stat(list[i].StatName, list[i].StatValue, false);
                    list[i] = tmp;
                    list[i].Modifiable = false;
                    return;
                }
            }
            list.Add(new Stat(statModel.GetCustomStat(iPrev).StatName, list[iPrev].StatValue));
            tmp = new Stat(list[iPrev].StatName, list[iPrev].StatValue, false);
            list[iPrev] = new Stat(list[list.Count-1].StatName, list[list.Count - 1].StatValue, false);
            list[list.Count - 1] = tmp;
            list[list.Count - 1].Modifiable = true;
            return;
        }

        private void CheckStatAfter(List<Stat> list, int iPrev)
        {
            for (int i = 0; i < statModel.StatNames.Count; i++)
            {
                if (list[iPrev].StatName.Equals(statModel.StatNames[i]))
                {
                    // Interchange les valeurs
                    Stat tmp = new Stat(list[iPrev].StatName, list[iPrev].StatValue, false);
                    list[iPrev] = new Stat(list[i].StatName, list[i].StatValue, false);
                    list[i] = tmp;
                    return;
                }
            }
        }



        private void SliceStat(List<Stat> list)
        {
            int index = 0;
            StatEditor.Clear();
            for (int i = 0; i < statModel.CharacterStatLines.Count; i++)
            {
                if (StatEditor.Count <= i)
                    StatEditor.Add(new CharacterStatLine(statModel.CharacterStatLines[i].ColumnName, statModel.CharacterStatLines[i].ColumnNameCenter, statModel.CharacterStatLines[i].ColumnNameRight));

                // C'est dégueulasse mais tu dois t'en foutre sinon ton éditeur ressemble a rien
                for (int j = 0; j < statModel.CharacterStatLines[i].StatDictionary.Count; j++)
                {
                    if (StatEditor[i].StatDictionary.Count <= j)
                        StatEditor[i].StatDictionary.Add(list[index]);
                    else
                        StatEditor[i].StatDictionary[j] = list[index];
                    index += 1;
                }

                for (int j = 0; j < statModel.CharacterStatLines[i].StatDictionary2.Count; j++)
                {
                    if (StatEditor[i].StatDictionary2.Count <= j)
                        StatEditor[i].StatDictionary2.Add(list[index]);
                    else
                        StatEditor[i].StatDictionary2[j] = list[index];
                    index += 1;
                }

                for (int j = 0; j < statModel.CharacterStatLines[i].StatDictionary3.Count; j++)
                {
                    if (StatEditor[i].StatDictionary3.Count <= j)
                        StatEditor[i].StatDictionary3.Add(list[index]);
                    else
                        StatEditor[i].StatDictionary3[j] = list[index];
                    index += 1;
                }
            }

            while(index < list.Count)
            {
                StatEditor[StatEditor.Count-1].StatDictionary3.Add(list[index]);
                index += 1;
            }
        }


        [Space]
        [Space]
        [Space]
        [SerializeField]
        [HideLabel]
        public CharacterStatData statModel;




        public StatController()
        {

        }

        public StatController(StatController stat)
        {
            Stats.Clear();
            if (stat.Stats == null)
            {
                for (int i = 0; i < stat.StatEditor.Count; i++)
                {
                    for (int j = 0; j < stat.StatEditor[i].StatDictionary.Count; j++)
                        Stats.Add(new Stat(stat.StatEditor[i].StatDictionary[j]));
                    for (int j = 0; j < stat.StatEditor[i].StatDictionary2.Count; j++)
                        Stats.Add(new Stat(stat.StatEditor[i].StatDictionary2[j]));
                    for (int j = 0; j < stat.StatEditor[i].StatDictionary3.Count; j++)
                        Stats.Add(new Stat(stat.StatEditor[i].StatDictionary3[j]));
                }
            }
            else
            {
                for (int i = 0; i < stat.Stats.Count; i++)
                {
                    Stats.Add(new Stat(stat.Stats[i]));
                }
            }
        }




        public float GetValue(string variableName)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].StatName.Equals(variableName))
                {
                    return Stats[i].StatValue;
                }
            }
            return 0;
        }

        // à utiliser quand on est sur que les données ne vont plus bouger n'importe comment (on perd les références avec les int mais c'est 100x plus rapide)
        public float GetValue(int id)
        {
            return Stats[id].StatValue;
        }




        public void Add(StatController stat, StatModifierType modifierType)
        {
            for(int i = 0; i < Stats.Count; i++)
            {
                Stats[i].AddStatModifier(stat.Stats[i].StatValue, modifierType);
            }
        }

        public void Remove(StatController stat, StatModifierType modifierType)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                Stats[i].RemoveStatModifier(stat.Stats[i].StatValue, modifierType);
            }
        }



        public void AddStat(Stat stat, StatModifierType modifierType)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].StatName.Equals(stat.StatName))
                {
                    Stats[i].AddStatModifier(stat.StatValue, modifierType);
                    return;
                }
            }
        }

        public void RemoveStat(Stat stat, StatModifierType modifierType)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].StatName.Equals(stat.StatName))
                {
                    Stats[i].RemoveStatModifier(stat.StatValue, modifierType);
                    return;
                }
            }
        }


        public void SetStatBase(StatController stat)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                Stats[i].SetBaseValue(stat.Stats[i].StatValue);
            }
        }

        // Principalement utilisé pour calculer les stats du personnage en fonction de son level
        public void AddStatBase(StatController stat, int level = 1)
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                Stats[i].AddBaseValue(stat.Stats[i].StatValue * level);
            }
        }


        /*[Button]
        public void DebugFunction()
        {
            for (int i = 0; i < Stats.Count; i++)
            {
                Stats[i].DebugFunction();
            }
        }*/

    }

} // #PROJECTNAME# namespace