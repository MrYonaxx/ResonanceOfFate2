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
        TypeDictionary dictionary;

        [SerializeField]
        ItemButton weapon;
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
        Image[] imageCharacterFaceOutline;
        [SerializeField]
        Image[] imageCharacterFace;


        [SerializeField]
        [ValueDropdown("GetStatList")]
        private List<string> stat;
        [SerializeField]
        List<TextMeshProUGUI> textOldStat;
        [SerializeField]
        List<TextMeshProUGUI> textNewStat;
        [SerializeField]
        List<float> statMultiplier; // Pour donner des valeurs "propres" et pas de 1,2 chelou

        [SerializeField]
        GameObject panelDescription;
        [SerializeField]
        TextMeshProUGUI textItemDescription;

        [Title("Feedback (Note faire plusieurs canvas par menu)")]
        [SerializeField]
        Animator animatorFeedback;
        [SerializeField]
        Animator animatorCharacterRotation;

        [Title("Sound")]
        [SerializeField]
        AudioClip soundSwitchCharacter;

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
            else if (Input.GetButtonDown(control.buttonY))
            {
                RemoveItem();
                DrawCharacter();
                return;
            }
            else if (Input.GetButtonDown(control.buttonRB))
            {
                AudioManager.Instance.PlaySound(soundSwitchCharacter);
                indexCharacterSelection += 1;
                if (indexCharacterSelection >= partyData.CharacterStatControllers.Count)
                    indexCharacterSelection = 0;
                animatorFeedback.SetTrigger("Feedback");
                animatorCharacterRotation.SetTrigger("RotateRight");
                DrawCharacter();
                return;
            }
            else if (Input.GetButtonDown(control.buttonLB))
            {
                AudioManager.Instance.PlaySound(soundSwitchCharacter);
                indexCharacterSelection -= 1;
                if (indexCharacterSelection < 0)
                    indexCharacterSelection = partyData.CharacterStatControllers.Count-1;
                animatorFeedback.SetTrigger("Feedback");
                animatorCharacterRotation.SetTrigger("RotateLeft");
                DrawCharacter();
                return;
            }

        }

        public void DrawCharacter()
        {
            textCharacterName.text = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterName;

            currentCharacterStat = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            previewCharacterStat = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            int index = indexCharacterSelection;
            for (int i = 0; i < imageCharacterFaceOutline.Length; i++)
            {
                imageCharacterFace[i].sprite = partyData.CharacterStatControllers[index].CharacterData.CharacterFace;
                imageCharacterFaceOutline[i].sprite = partyData.CharacterStatControllers[index].CharacterData.CharacterFace;
                index += 1;
                index = index % partyData.CharacterStatControllers.Count;
            }
            DrawCharacterStat();
            DrawEquipement();


            weapon.DrawText(dictionary.GetSpriteIcon(partyData.CharacterEquipement[indexCharacterSelection].GetWeaponType()), partyData.CharacterEquipement[indexCharacterSelection].GetWeapon().WeaponName);

        }
        public void DrawCharacterStat()
        {
            for (int i = 0; i < stat.Count; i++)
            {
                textOldStat[i].text = ((int)(currentCharacterStat.GetValue(stat[i]) * statMultiplier[i])).ToString();
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
            DrawEquipementDescription(equipementListDrawer.IndexSelection);
        }



        // a refaire si je fais un vrai jeu
        public void DrawCharacterStatPreview(ArmorData armorData)
        {
            for (int i = 0; i < stat.Count; i++)
            {
                textNewStat[i].text = ((int)(previewCharacterStat.GetValue(stat[i]) * statMultiplier[i])).ToString();
                textNewStat[i].color = Color.white;
                for (int j = 0; j < armorData.StatModifiers.Count; j++)
                {
                    if (stat[i] == armorData.StatModifiers[j].StatName)
                    {
                        textNewStat[i].color = Color.yellow;
                        if (armorData.StatModifiers[j].ModifierType == StatModifierType.Flat)
                            textNewStat[i].text = ((int)((previewCharacterStat.GetValue(stat[i]) + armorData.StatModifiers[j].StatValue) * statMultiplier[i])).ToString();
                        else
                            textNewStat[i].text = ((int)((previewCharacterStat.GetValue(stat[i]) * armorData.StatModifiers[j].StatValue) * statMultiplier[i])).ToString();
                        break;
                    }
                }
            }
        }





        public void DrawItemPreview(int index)
        {
            panelDescription.SetActive(true);
            textItemDescription.text = itemsInventory[index].ItemDescription;
            DrawCharacterStatPreview((ArmorData) itemsInventory[index]);
        }

        public void DrawEquipementDescription(int index)
        {
            if (partyData.CharacterEquipement[indexCharacterSelection].GetArmor(index) == null)
            {
                panelDescription.SetActive(false);
                textItemDescription.text = "";
                return;
            }
            panelDescription.SetActive(true);
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

        public void RemoveItem()
        {
            ArmorData removedItem = partyData.CharacterEquipement[indexCharacterSelection].RemoveArmor(equipementListDrawer.IndexSelection);
            if (removedItem != null)
                partyData.Inventory.Add(removedItem.name);
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
            if (itemsInventory.Count != 0)
                DrawItemPreview(0);
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