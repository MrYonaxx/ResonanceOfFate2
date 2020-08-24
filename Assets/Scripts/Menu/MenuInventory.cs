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
            List<string> items = new List<string>(partyData.Inventory.Count);
            for (int i = 0; i < partyData.Inventory.Count; i++)
            {
                items.Add(partyData.Inventory[i]);
            }
            for (int i = 0; i < partyData.CharacterEquipement.Count; i++)
            {
                items.AddRange(partyData.CharacterEquipement[i].GetArmors());
            }
            itemsInventory = new List<ItemData>();
            for (int i = 0; i < items.Count; i++)
            {
                ItemData item = itemDatabase.GetItemData(items[i]);
                itemsInventory.Add(item);
                inventoryListDrawer.DrawItemList(i, item.ItemIcon, item.ItemName);
            }
            inventoryListDrawer.SetItemCount(items.Count);
            if (itemsInventory.Count != 0)
                DrawItemPreview(0);
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