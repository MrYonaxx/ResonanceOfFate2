/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ItemButton: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        ItemData itemData;
        public ItemData ItemData
        {
            get { return itemData; }
        }
        [SerializeField]
        Image itemIcon;
        [SerializeField]
        TextMeshProUGUI itemName;

        [SerializeField]
        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get { return rectTransform; }
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
        public void DrawItem(ItemData item)
        {
            itemData = item;
            itemIcon.sprite = item.ItemIcon;
            itemName.text = item.ItemName;
        }

        public void DrawText(Sprite icon, string text)
        {
            itemData = null;
            itemIcon.sprite = icon;
            itemName.text = text;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace