﻿/*****************************************************************
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
    public class MenuEquipment: MenuBase
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Menu Equipement")]
        [SerializeField]
        ControllerConfigurationData control;
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        ItemDatabase itemDatabase;
        [SerializeField]
        MenuItemListDrawer equipementListDrawer;
        [SerializeField]
        MenuItemListDrawer inventoryListDrawer;


        [SerializeField]
        GameObject inventoryObject;

        [Title("UI")]
        [SerializeField]
        TextMeshProUGUI textCharacterName;
        [SerializeField]
        Image imageCharacterFaceOutline;
        [SerializeField]
        Image imageCharacterFace;

        [SerializeField]
        [ValueDropdown("GetStatList")]
        private List<string> stat;
        [SerializeField]
        List<TextMeshProUGUI> textOldStat;
        [SerializeField]
        List<TextMeshProUGUI> textNewStat;

        [SerializeField]
        TextMeshProUGUI textItemDescription;



        StatController currentCharacterStat;
        StatController previewCharacterStat;


        int indexCharacterSelection = 0;
        bool canInput = false;
        List<ItemData> itemsInventory = new List<ItemData>();


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

#if UNITY_EDITOR
        private static List<string> GetStatList()
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterStatData>(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("StatDictionary")[0])).StatNames;
        }
#endif


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        protected void Start()
        {
            // C'est un peu le spaghetti en vérité c'thistoire
            equipementListDrawer.OnValidate += SwitchToInventory;
            equipementListDrawer.OnSelect += DrawEquipementDescription;
            equipementListDrawer.OnQuit += CloseMenu;

            inventoryListDrawer.OnValidate += EquipItem;
            inventoryListDrawer.OnSelect += DrawItemPreview;
            inventoryListDrawer.OnQuit += SwitchToEquipement;
            DrawCharacter();
        }


        public override void OpenMenu()
        {
            base.OpenMenu();
            DrawCharacter();
        }
        public override void OpenMenuLate()
        {
            canInput = true;
        }

        public override void CloseMenu()
        {
            canInput = false;
            base.CloseMenu();

        }

        protected void Update()
        {
            if (canInput == false)
                return;
            if (equipementListDrawer.InputList())
                return;
            else if (Input.GetButtonDown(control.buttonRB))
            {
                indexCharacterSelection += 1;
                if (indexCharacterSelection >= partyData.CharacterStatControllers.Count)
                    indexCharacterSelection = 0;
                DrawCharacter();
                return;
            }
            else if (Input.GetButtonDown(control.buttonLB))
            {
                indexCharacterSelection -= 1;
                if (indexCharacterSelection < 0)
                    indexCharacterSelection = partyData.CharacterStatControllers.Count-1;
                DrawCharacter();
                return;
            }

        }

        public void DrawCharacter()
        {
            textCharacterName.text = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterName;
            imageCharacterFaceOutline.sprite = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterFace;
            imageCharacterFace.sprite = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterFace;
            currentCharacterStat = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            previewCharacterStat = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            DrawCharacterStat();
            DrawEquipement();

        }
        public void DrawCharacterStat()
        {
            for (int i = 0; i < stat.Count; i++)
            {
                textOldStat[i].text = currentCharacterStat.GetValue(stat[i]).ToString();
            }
        }
        public void DrawEquipement()
        {
            ArmorData armor;
            for (int i = 0; i < equipementListDrawer.ListItem.Count; i++)
            {
                armor = partyData.CharacterEquipement[indexCharacterSelection].GetArmor(i);
                if (armor == null)
                {
                    equipementListDrawer.DrawItemList(i, null, "-----");
                }
                else
                {
                    equipementListDrawer.DrawItemList(i, armor.ItemIcon, armor.ItemName);
                }
            }
        }



        // a refaire si je fais un vrai jeu
        public void DrawCharacterStatPreview(ArmorData armorData)
        {
            for (int i = 0; i < stat.Count; i++)
            {
                textNewStat[i].text = previewCharacterStat.GetValue(stat[i]).ToString();
                for (int j = 0; j < armorData.StatModifiers.Count; j++)
                {
                    if (stat[i] == armorData.StatModifiers[j].StatName)
                    {
                        if(armorData.StatModifiers[j].ModifierType == StatModifierType.Flat)
                            textNewStat[i].text = (previewCharacterStat.GetValue(stat[i]) + armorData.StatModifiers[j].StatValue).ToString();
                        else
                            textNewStat[i].text = (previewCharacterStat.GetValue(stat[i]) * armorData.StatModifiers[j].StatValue).ToString();
                        break;
                    }
                }
            }
        }





        public void DrawItemPreview(int index)
        {
            textItemDescription.text = itemsInventory[index].ItemDescription;
            DrawCharacterStatPreview((ArmorData) itemsInventory[index]);
        }

        public void DrawEquipementDescription(int index)
        {
            if (partyData.CharacterEquipement[indexCharacterSelection].GetArmor(index) == null)
                return;
            textItemDescription.text = partyData.CharacterEquipement[indexCharacterSelection].GetArmor(index).ItemDescription;
        }

        public void EquipItem(int index)
        {
            ArmorData removedItem = partyData.CharacterEquipement[indexCharacterSelection].RemoveArmor(equipementListDrawer.IndexSelection);
            if(removedItem != null)
                partyData.Inventory.Add(removedItem.name);
            partyData.CharacterEquipement[indexCharacterSelection].EquipArmor(equipementListDrawer.IndexSelection, (ArmorData)itemsInventory[index]);
            partyData.Inventory.Remove(itemsInventory[index].name);
            SwitchToEquipement();
        }





        public void SwitchToInventory(int index)
        {
            canInput = false;
            inventoryObject.SetActive(true);
            DrawItemList();
            for (int i = 0; i < stat.Count; i++)
            {
                textNewStat[i].gameObject.SetActive(true);
            }
            StartCoroutine(WaitInventoryBuffer(true));

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



        private IEnumerator WaitInventoryBuffer(bool b)
        {
            // On attend une frame sinon unity fait l'update de l'autre menu sur la même frame (En gros ça appuie 2x sur le bouton A alors qu'on appuie qu'une fois)
            yield return null;
            canInput = !b;
            inventoryListDrawer.SetInput(b);
        }

        public void SwitchToEquipement()
        {
            inventoryListDrawer.SetInput(false);
            inventoryObject.SetActive(false);
            StartCoroutine(WaitInventoryBuffer(false));
            for (int i = 0; i < stat.Count; i++)
            {
                textNewStat[i].gameObject.SetActive(false);
            }
            DrawEquipement();
            DrawCharacter();
        }




        private void OnDestroy()
        {
            equipementListDrawer.OnValidate -= SwitchToInventory;
            equipementListDrawer.OnSelect -= DrawEquipementDescription;
            equipementListDrawer.OnQuit -= CloseMenu;

            inventoryListDrawer.OnValidate -= EquipItem;
            inventoryListDrawer.OnSelect -= DrawItemPreview;
            inventoryListDrawer.OnQuit -= SwitchToEquipement;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace