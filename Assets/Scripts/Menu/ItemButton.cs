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
        /*ItemData itemData;
        public ItemData ItemData
        {
            get { return itemData; }
        }*/
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
        /*public void DrawItem(ItemData item)
        {
            itemData = item;
            itemIcon.sprite = item.ItemIcon;
            itemName.text = item.ItemName;
        }*/

        public string GetText()
        {
            return itemName.text;
        }

        public void DrawText(Sprite icon, string text)
        {
            itemIcon.sprite = icon;
            if (icon == null)
                itemIcon.enabled = false;
            else
                itemIcon.enabled = true;
            itemName.text = text;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace