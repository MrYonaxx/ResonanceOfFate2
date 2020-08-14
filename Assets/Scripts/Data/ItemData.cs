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

    public class ItemData: ScriptableObject
    {

        [HorizontalGroup("CharacterBasic", Width = 96, PaddingLeft = 10)]
        [HideLabel]
        [PreviewField(ObjectFieldAlignment.Left, Height = 96)]
        [SerializeField]
        Sprite itemIcon;
        public Sprite ItemIcon
        {
            get { return itemIcon; }
        }

        [HorizontalGroup("CharacterBasic", PaddingLeft = 10)]
        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        private string itemName;
        public string ItemName
        {
            get { return itemName; }
        }

        [VerticalGroup("CharacterBasic/Right")]
        [SerializeField]
        [TextArea(2,3)]
        private string itemDescription;
        public string ItemDescription
        {
            get { return itemDescription; }
        }

    } 

} // #PROJECTNAME# namespace