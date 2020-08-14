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
    public class MenuEquipment: MenuItemListDrawer
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Menu Equipement")]
        [SerializeField]
        PartyData partyData;
        [SerializeField]
        MenuItemListDrawer inventoryListDrawer;
        [SerializeField]
        GameObject inventoryObject;

        [Title("UI")]
        [SerializeField]
        TextMeshProUGUI textCharacterName;

        [SerializeField]
        [ValueDropdown("GetStatList")]
        private List<string> stat;
        [SerializeField]
        List<TextMeshProUGUI> textOldStat;
        [SerializeField]
        List<TextMeshProUGUI> textNewStat;

        StatController currentCharacter;
        StatController previewCharacter;

        int indexCharacterSelection = 0;

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

        protected override void Start()
        {
            base.Start();
            inventoryListDrawer.OnValidate += EquipItem;
            inventoryListDrawer.OnSelect += DrawItemPreview;
            inventoryListDrawer.OnQuit += SwitchToEquipement;
            DrawCharacter();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetButtonDown(control.buttonRB))
            {
                indexCharacterSelection += 1;
                if (indexCharacterSelection >= partyData.CharacterStatControllers.Count)
                    indexCharacterSelection = 0;
                DrawCharacter();
            }
            else if (Input.GetButtonDown(control.buttonLB))
            {
                indexCharacterSelection -= 1;
                if (indexCharacterSelection < 0)
                    indexCharacterSelection = partyData.CharacterStatControllers.Count-1;
                DrawCharacter();
            }

        }

        public override void Validate()
        {
            SwitchToInventory();
        }

        public override void Quit()
        {
        }



        public void DrawCharacter()
        {
            canInput = true;
            textCharacterName.text = partyData.CharacterStatControllers[indexCharacterSelection].CharacterData.CharacterName;
            currentCharacter = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            previewCharacter = partyData.CharacterStatControllers[indexCharacterSelection].StatController;
            DrawEquipement();
            DrawCharacterStat();
        }
        public void DrawEquipement()
        {
            ArmorData armor;
            for (int i = 0; i < listItem.Count; i++)
            {
                armor = partyData.CharacterEquipement[indexCharacterSelection].GetArmor(i);
                if (armor == null)
                {
                    listItem[i].DrawText(null, "-----");
                }
                else
                {
                    listItem[i].DrawItem(armor);
                }
            }
        }

        public void DrawCharacterStat()
        {

        }

        public void DrawCharacterStatPreview()
        {

        }

        public void DrawItemPreview(ItemData item)
        {

        }

        public void EquipItem(ItemData item)
        {
            ArmorData removedItem = partyData.CharacterEquipement[indexCharacterSelection].RemoveArmor(indexSelection);
            if(removedItem != null)
                partyData.Inventory.Add(removedItem.name);
            partyData.CharacterEquipement[indexCharacterSelection].EquipArmor(indexSelection, (ArmorData) item);
            partyData.Inventory.Remove(item.name);
            SwitchToEquipement();
        }


        public void SwitchToInventory()
        {
            canInput = false;
            inventoryObject.SetActive(true);
            inventoryListDrawer.DrawList(partyData.Inventory);
            StartCoroutine(WaitBuffer(true));

        }

        private IEnumerator WaitBuffer(bool b)
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
            StartCoroutine(WaitBuffer(false));
            DrawEquipement();
        }




        private void OnDestroy()
        {
            inventoryListDrawer.OnValidate -= EquipItem;
            inventoryListDrawer.OnSelect -= DrawItemPreview;
            inventoryListDrawer.OnQuit -= SwitchToEquipement;
        }

        #endregion

    } 

} // #PROJECTNAME# namespace