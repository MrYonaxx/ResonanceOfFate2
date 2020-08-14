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
    [CreateAssetMenu(fileName = "ArmorData", menuName = "Item/ArmorData", order = 1)]
    public class ArmorData: ItemData
    {
        [SerializeField]
        private List<StatModifier> statModifiers;

        public List<StatModifier> StatModifiers
        {
            get { return statModifiers; }
        }


    } 

} // #PROJECTNAME# namespace