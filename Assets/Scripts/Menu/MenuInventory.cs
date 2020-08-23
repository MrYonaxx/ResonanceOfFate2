/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class MenuInventory: MenuBase
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        ItemDatabase itemDatabase;
        [SerializeField]
        MenuItemListDrawer inventoryListDrawer;

        [SerializeField]
        TextMeshProUGUI textItemDescription;


        List<ItemData> itemsInventory = new List<ItemData>();

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
        protected void Start()
        {
            inventoryListDrawer.OnQuit += CloseMenu;
            inventoryListDrawer.OnSelect += DrawItemPreview;
        }

        public override void OpenMenu()
        {
            base.OpenMenu();
            DrawItemList();
        }
        public override void OpenMenuLate()
        {
            inventoryListDrawer.SetInput(true);
        }

        public override void CloseMenu()
        {
            inventoryListDrawer.SetInput(false);
            base.CloseMenu();

        }

        public void DrawItemList()
        {
            List<string> items = partyData.Inventory;
            itemsInventory = new List<ItemData>();
            for (int i = 0; i < items.Count; i++)
            {
                ItemData item = itemDatabase.GetItemData(items[i]);
                itemsInventory.Add(item);
                inventoryListDrawer.DrawItemList(i, item.ItemIcon, item.ItemName);
            }
            inventoryListDrawer.SetItemCount(items.Count);
        }


        public void DrawItemPreview(int index)
        {
            textItemDescription.text = itemsInventory[index].ItemDescription;
        }

        private void OnDestroy()
        {
            inventoryListDrawer.OnQuit -= CloseMenu;
            inventoryListDrawer.OnSelect -= DrawItemPreview;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace